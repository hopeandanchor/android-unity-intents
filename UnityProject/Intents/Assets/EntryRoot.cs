using UnityEngine;
using UnityEngine.UI;

public class EntryRoot : MonoBehaviour
{
	[SerializeField] private Text sessionIdDisplay;
	[SerializeField] private Text genericMessageDisplay;


	#if UNITY_ANDROID
//	[DllImport("__Internal")]
//	extern static public string getMessage();


	void Start () 
	{
		Debug.Log("Getting Intent...");
		AndroidJavaClass pluginClass = new AndroidJavaClass("digital.haa.plugin.MainActivity");
		Debug.Log("pluginClass : " + pluginClass);
		genericMessageDisplay.text = pluginClass.CallStatic<string>("getMessage");
		sessionIdDisplay.text = "*" + pluginClass.CallStatic<string>("GetSessionId") + "*";
	}

//	private string RetrieveIntentExtra(string extraMethod)
//	{
//		AndroidJavaClass unityPlayerClass = new AndroidJavaClass("ai.pupil.spectrum.vmax.UnityIntentsBridge");
//		AndroidJavaObject activityObject = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
//
//		return activityObject.Call<string>(extraMethod);
//
//	}
	#endif

}
