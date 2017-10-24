using Android.Widget;
using Android.OS;
using Android.Content;
using System;
using System.Web;
using System.Runtime.Serialization.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.IO;
using Android.Support.V7.App;


namespace Test
{

	[Android.App.Activity(Label = "Login", MainLauncher = true, Icon = "@mipmap/icon")]
	public class LoginActivity : AppCompatActivity
	{

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_login);

			// Get our button from the layout resource,
			// and attach an event to it
			Button btnLogin = FindViewById<Button>(Resource.Id.login);
			TextView btnRegister = FindViewById<TextView>(Resource.Id.register);

			EditText email = FindViewById<EditText>(Resource.Id.email);
			EditText password = FindViewById<EditText>(Resource.Id.password);

			btnLogin.Click += delegate
			{
				if (email.Text != "" && password.Text != "")
				{
					HttpClient client = new HttpClient();
					client.BaseAddress = new Uri("https://apicapitalsante.herokuapp.com/");
					client.DefaultRequestHeaders
					 .Accept
					 .Add(new MediaTypeWithQualityHeaderValue("application/json")); //ACCEPT header

					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/authenticate/login");
					request.Content = new StringContent("{\"email\":\"" + email.Text + "\",\"password\":\"" + password.Text + "\"}",
					 Encoding.UTF8,
					 "application/json"); //CONTENT-TYPE header

					client.SendAsync(request)
					 .ContinueWith(async responseTask =>
					 {
						 var result = await responseTask.Result.Content.ReadAsStringAsync();
						 var serializer = new DataContractJsonSerializer(typeof(APIResponse));

						 var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
						 var data = (APIResponse)serializer.ReadObject(ms);
						 Console.WriteLine("Token: {0}, Success: {1}, Message: {2}, Email: {3}", data.success, data.success, data.message, email);
						 if (data.success == false)
						 {
							 Toast toast = Toast.MakeText(ApplicationContext, data.message, ToastLength.Long);
							 toast.Show();
							 AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
							 alertDialog.SetTitle("Error");
							 alertDialog.SetMessage(data.message);
							 alertDialog.SetNeutralButton("Ok", delegate
							 {
								 alertDialog.Dispose();
							 });
							 alertDialog.Show();
						 }
						 else
						 {
							 var intent = new Intent(this, typeof(SecondActivity));
							 intent.PutExtra("email", email.Text);
							 intent.PutExtra("password", password.Text);
							 intent.PutExtra("token", data.token);
							 StartActivity(intent);
						 }
					 });
				}
				else
				{
					AlertDialog.Builder alertDialog = new AlertDialog.Builder(this);
					alertDialog.SetTitle("Alert");
					alertDialog.SetMessage("Email and Password Are Required");
					alertDialog.SetNeutralButton("Ok", delegate
					{
						alertDialog.Dispose();

					});
					alertDialog.Show();
				}
			};

			btnRegister.Click += delegate
			{
				var intent = new Intent(this, typeof(RegisterActivity));
				StartActivity(intent);
			};

		}


	}
}