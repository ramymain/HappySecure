using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using AppEmotion.Model;

namespace AppEmotion.Helper
{
	public class ImageHelper
	{
		public static Bitmap DrawRectOnBitmap(Bitmap mBitmap, FaceRectangle faceRectangle, string status)
		{
			Bitmap bitmap = mBitmap.Copy(Bitmap.Config.Argb8888, true);
			Canvas canvas = new Canvas(bitmap);

			Paint paint = new Paint();
			paint.AntiAlias = true;
			paint.SetStyle(Paint.Style.Stroke);
			paint.Color = Color.White;
			paint.StrokeWidth = 12;

			canvas.DrawRect(faceRectangle.left,
				faceRectangle.top,
				faceRectangle.left + faceRectangle.width,
				faceRectangle.top + faceRectangle.height,
				paint);

			int cX = faceRectangle.left + faceRectangle.width;
			int cY = faceRectangle.top + faceRectangle.height;

			DrawTextBelowRect(canvas, 100, cX / 2 + cX / 5, cY + 100, Color.White, status);

			return bitmap;

		}

		private static void DrawTextBelowRect(Canvas canvas, int textSize, int cX, int cY, Color color, string status)
		{
			Paint paint = new Paint();
			paint.AntiAlias = true;
			paint.SetStyle(Paint.Style.Fill);
			paint.Color = color;
			paint.TextSize = textSize;

			canvas.DrawText(status, cX, cY, paint);
		}
	}
}