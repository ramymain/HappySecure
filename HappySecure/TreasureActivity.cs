
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

namespace Test
{
    [Activity(Label = "Treasure")]
    public class TreasureActivity : Activity
    {
        public ImageView background;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_treasure);
            // Create your application here
        }
    }
}
