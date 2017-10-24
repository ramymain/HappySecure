using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Content.PM;
using Android.Support.V7.App;
using Android.Provider;
using Android.Graphics.Drawables;


using Com.Microsoft.Projectoxford.Emotion;

using AppEmotion.Model;
using Newtonsoft.Json;
using AppEmotion.Helper;


namespace Test
{
	[Activity(Label = "HappySecure")]
	public class SecondActivity : AppCompatActivity
	{
		public EmotionServiceRestClient emotionRestClient = new EmotionServiceRestClient("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX");
		public ImageView imageView;
		public TextView emotionTV;
		public Button BtnNext;
		public Bitmap mBitmap;
		public TextView textView;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.Second);

			mBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.smile);
			imageView = FindViewById<ImageView>(Resource.Id.imageView);
			emotionTV = FindViewById<TextView>(Resource.Id.emotionTV);
			BtnNext = FindViewById<Button>(Resource.Id.BtnNext);
			BtnNext.Click += displayTreasure;
			imageView.SetImageBitmap(mBitmap);
			textView = FindViewById<TextView>(Resource.Id.textView);
			if (IsThereAnAppToTakePictures())
			{
				CreateDirectoryForPictures();

				Button button = FindViewById<Button>(Resource.Id.PictureButton);
				imageView = FindViewById<ImageView>(Resource.Id.imageView);
				button.Click += TakeAPicture;
			}


			Button btnProcess = FindViewById<Button>(Resource.Id.BtnEmotion);
			btnProcess.Click += delegate
			{
				imageView.BuildDrawingCache();
				mBitmap = ((BitmapDrawable)imageView.Drawable).Bitmap;
				byte[] bitmapData;
				using (var stream = new MemoryStream())
				{
					mBitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
					bitmapData = stream.ToArray();

				}

				Stream inputStream = new MemoryStream(bitmapData);
				new EmotionTask(this).Execute(inputStream);
			};
		}

		private void displayTreasure(object sender, System.EventArgs eventArgs)
		{
			var intent = new Intent(this, typeof(TreasureActivity));
			StartActivity(intent);
		}


		private void CreateDirectoryForPictures()
		{
			App._dir = new Java.IO.File(
			 Android.OS.Environment.GetExternalStoragePublicDirectory(
			  Android.OS.Environment.DirectoryPictures), "CameraAppDemo");
			if (!App._dir.Exists())
			{
				App._dir.Mkdirs();
			}
		}

		private bool IsThereAnAppToTakePictures()
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			IList<ResolveInfo> availableActivities =
			 PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
			return availableActivities != null && availableActivities.Count > 0;
		}

		private void TakeAPicture(object sender, System.EventArgs eventArgs)
		{
			Intent intent = new Intent(MediaStore.ActionImageCapture);
			App._file = new Java.IO.File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(App._file));
			StartActivityForResult(intent, 0);
		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			// Make it available in the gallery

			Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
			Android.Net.Uri contentUri = Android.Net.Uri.FromFile(App._file);
			mediaScanIntent.SetData(contentUri);
			SendBroadcast(mediaScanIntent);

			// Display in ImageView. We will resize the bitmap to fit the display.
			// Loading the full sized image will consume to much memory
			// and cause the application to crash.

			int height = Resources.DisplayMetrics.HeightPixels;
			int width = imageView.Width;
			App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
			if (App.bitmap != null)
			{
				var matrix = new Matrix();
				matrix.PostRotate(270);
				Bitmap bitmapp = Bitmap.CreateBitmap(App.bitmap, 0, 0, App.bitmap.Width, App.bitmap.Height, matrix, true);

				imageView.SetImageBitmap(bitmapp);
				App.bitmap = null;
			}

			// Dispose of the Java side bitmap.
			GC.Collect();
		}
	}

	public static class BitmapHelpers
	{
		public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
		{
			// First we get the the dimensions of the file on disk
			BitmapFactory.Options options = new BitmapFactory.Options
			{
				InJustDecodeBounds = true
			};
			BitmapFactory.DecodeFile(fileName, options);

			// Next we calculate the ratio that we need to resize the image by
			// in order to fit the requested dimensions.
			int outHeight = options.OutHeight;
			int outWidth = options.OutWidth;
			int inSampleSize = 1;

			if (outHeight > height || outWidth > width)
			{
				inSampleSize = outWidth > outHeight ? outHeight / height : outWidth / width;
			}

			// Now we will load the image and have BitmapFactory resize it for us.
			options.InSampleSize = inSampleSize;
			options.InJustDecodeBounds = false;
			Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

			return resizedBitmap;
		}
	}

	public static class App
	{
		public static Java.IO.File _file;
		public static Java.IO.File _dir;
		public static Bitmap bitmap;
	}

	class EmotionTask : AsyncTask<Stream, string, string>
	{
		private SecondActivity secondActivity;
		private ProgressDialog mDialog = new ProgressDialog(Application.Context);

		public EmotionTask(SecondActivity secondActivity)
		{
			this.secondActivity = secondActivity;
		}

		protected override string RunInBackground(params Stream[] @params)
		{
			try
			{
				PublishProgress("Recognizing...");
				var result = secondActivity.emotionRestClient.RecognizeImage(@params[0]);
				var list = new List<EmotionModel>();
				foreach (var item in result)
				{
					EmotionModel emotionModel = new EmotionModel();

					Com.Microsoft.Projectoxford.Emotion.Contract.FaceRectangle faceRect = item.FaceRectangle;
					emotionModel.faceRectangle = new FaceRectangle();
					emotionModel.faceRectangle.left = faceRect.Left;
					emotionModel.faceRectangle.top = faceRect.Top;
					emotionModel.faceRectangle.width = faceRect.Width;
					emotionModel.faceRectangle.height = faceRect.Height;

					Com.Microsoft.Projectoxford.Emotion.Contract.Scores scores = item.Scores;
					emotionModel.scores = new Scores();
					emotionModel.scores.anger = scores.Anger;
					emotionModel.scores.happiness = scores.Happiness;
					emotionModel.scores.sadness = scores.Sadness;
					emotionModel.scores.fear = scores.Fear;
					emotionModel.scores.neutral = scores.Neutral;
					emotionModel.scores.surprise = scores.Surprise;
					emotionModel.scores.disgust = scores.Disgust;

					list.Add(emotionModel);
				}
				string strResult = JsonConvert.SerializeObject(list);
				return strResult;
			}
			catch (Exception ex)
			{
				System.Console.Write(ex);
				return null;
			}
		}

		protected override void OnPreExecute()
		{
			mDialog.Window.SetType(Android.Views.WindowManagerTypes.SystemAlert);
			mDialog.Show();
		}

		protected override void OnProgressUpdate(params string[] values)
		{
			mDialog.SetMessage(values[0]);
		}

		protected override void OnPostExecute(string result)
		{
			mDialog.Dismiss();
			var list = JsonConvert.DeserializeObject<List<EmotionModel>>(result);
			foreach (var item in list)
			{
				String status = GetEmo(item);

				secondActivity.emotionTV.Text = status;
				secondActivity.imageView.SetImageBitmap(ImageHelper.DrawRectOnBitmap(secondActivity.mBitmap, item.faceRectangle, status));
				if (status == "Happy")
				{
					secondActivity.BtnNext.Visibility = ViewStates.Visible;
				}
				else
				{
					secondActivity.BtnNext.Visibility = ViewStates.Gone;
				}
			}
		}

		private string GetEmo(EmotionModel item)
		{
			List<double> list = new List<double>();
			Scores scores = item.scores;
			list.Add(scores.happiness);
			list.Add(scores.anger);
			list.Add(scores.sadness);
			list.Add(scores.neutral);
			list.Add(scores.contempt);
			list.Add(scores.disgust);
			list.Add(scores.fear);
			list.Add(scores.surprise);

			var listSorted = list.OrderBy(i => i).ToList();
			double maxElementList = listSorted[listSorted.Count - 1];
			if (maxElementList == scores.anger)
				return "Anger";
			else if (maxElementList == scores.happiness)
			{
				return "Happy";
			}
			else if (maxElementList == scores.sadness)
				return "Sadness";
			else if (maxElementList == scores.neutral)
				return "Neutral";
			else if (maxElementList == scores.contempt)
				return "Contempt";
			else if (maxElementList == scores.disgust)
				return "Disgust";
			else if (maxElementList == scores.fear)
				return "Fear";
			else if (maxElementList == scores.surprise)
				return "Surprise";
			else
				return "Neutral";
		}


	}

}