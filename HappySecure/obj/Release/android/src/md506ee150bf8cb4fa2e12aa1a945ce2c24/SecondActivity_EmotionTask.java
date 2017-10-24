package md506ee150bf8cb4fa2e12aa1a945ce2c24;


public class SecondActivity_EmotionTask
	extends android.os.AsyncTask
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_doInBackground:([Ljava/lang/Object;)Ljava/lang/Object;:GetDoInBackground_arrayLjava_lang_Object_Handler\n" +
			"n_onPreExecute:()V:GetOnPreExecuteHandler\n" +
			"n_onProgressUpdate:([Ljava/lang/Object;)V:GetOnProgressUpdate_arrayLjava_lang_Object_Handler\n" +
			"n_onPostExecute:(Ljava/lang/Object;)V:GetOnPostExecute_Ljava_lang_Object_Handler\n" +
			"";
		mono.android.Runtime.register ("Test.SecondActivity+EmotionTask, Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", SecondActivity_EmotionTask.class, __md_methods);
	}


	public SecondActivity_EmotionTask () throws java.lang.Throwable
	{
		super ();
		if (getClass () == SecondActivity_EmotionTask.class)
			mono.android.TypeManager.Activate ("Test.SecondActivity+EmotionTask, Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public SecondActivity_EmotionTask (md506ee150bf8cb4fa2e12aa1a945ce2c24.SecondActivity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == SecondActivity_EmotionTask.class)
			mono.android.TypeManager.Activate ("Test.SecondActivity+EmotionTask, Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Test.SecondActivity, Test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public java.lang.Object doInBackground (java.lang.Object[] p0)
	{
		return n_doInBackground (p0);
	}

	private native java.lang.Object n_doInBackground (java.lang.Object[] p0);


	public void onPreExecute ()
	{
		n_onPreExecute ();
	}

	private native void n_onPreExecute ();


	public void onProgressUpdate (java.lang.Object[] p0)
	{
		n_onProgressUpdate (p0);
	}

	private native void n_onProgressUpdate (java.lang.Object[] p0);


	public void onPostExecute (java.lang.Object p0)
	{
		n_onPostExecute (p0);
	}

	private native void n_onPostExecute (java.lang.Object p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
