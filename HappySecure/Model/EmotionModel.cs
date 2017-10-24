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

namespace AppEmotion.Model
{
	public class EmotionModel
	{
		public FaceRectangle faceRectangle { get; set; }
		public Scores scores { get; set; }
	}

	public class Scores
	{
		public double anger { get; set; }
		public double contempt { get; set; }
		public double disgust { get; set; }
		public double fear { get; set; }
		public double happiness { get; set; }
		public double sadness { get; set; }
		public double surprise { get; set; }
		public double neutral { get; set; }

	}

	public class FaceRectangle
	{
		public int left { get; set; }
		public int top { get; set; }
		public int width { get; set; }
		public int height { get; set; }

	}
}