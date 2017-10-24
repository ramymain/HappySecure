using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Support.V7.App;

namespace Test
{
    [Activity(Label = "Register")]
    public class RegisterActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_register);

			TextView btnLogin = FindViewById<TextView>(Resource.Id.login);
			Button btnRegister = FindViewById<Button>(Resource.Id.register);

			EditText firstName = FindViewById<EditText>(Resource.Id.firstName);
            EditText lastName = FindViewById<EditText>(Resource.Id.lastName);
			EditText email = FindViewById<EditText>(Resource.Id.email);
			EditText password = FindViewById<EditText>(Resource.Id.password);
			EditText confPassword = FindViewById<EditText>(Resource.Id.confPassword);

			btnRegister.Click += delegate
			{
				if (firstName.Text != "" && lastName.Text != "" && email.Text != "" && password.Text != "" && confPassword.Text != "" && password.Text == confPassword.Text)
				{
					HttpClient client = new HttpClient();
					client.BaseAddress = new Uri("https://apicapitalsante.herokuapp.com/");
					client.DefaultRequestHeaders
						  .Accept
						  .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header

					HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "api/authenticate/signup");
                    request.Content = new StringContent("{\"email\":\"" + email.Text + "\",\"password\":\"" + password.Text + "\",\"firstname\":\"" + firstName.Text + "\",\"lastname\":\"" + lastName.Text + "\"}",
														Encoding.UTF8,
														"application/json");//CONTENT-TYPE header

					client.SendAsync(request)
						  .ContinueWith(async responseTask =>
                          {
                          var result = await responseTask.Result.Content.ReadAsStringAsync();
                          var serializer = new DataContractJsonSerializer(typeof(APIResponse));

                          var ms = new MemoryStream(Encoding.UTF8.GetBytes(result));
                          var data = (APIResponse)serializer.ReadObject(ms);
							  if (data.success == false)
							  {
                                  Toast toast = Toast.MakeText(ApplicationContext, data.message, ToastLength.Long);
                                  toast.Show();
							  }
                              else {
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
				}
			};

			btnLogin.Click += delegate
			{
				var intent = new Intent(this, typeof(LoginActivity));
				StartActivity(intent);
			};
        }
    }
}
