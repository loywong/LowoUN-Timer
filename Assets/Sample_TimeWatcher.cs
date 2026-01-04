using LowoUN.Util;
using UnityEngine;

public class Sample_TimeWatcher : MonoBehaviour {
	[Header ("Test1_毫秒延迟_不循环")][SerializeField] private uint milliseconds_1 = 5000;
	[Header ("Test2_毫秒延迟_循环")][SerializeField] private uint milliseconds_2 = 3000;
	[Header ("Test3_毫秒延迟_循环")][SerializeField] private uint milliseconds_3 = 1000;
	[Header ("Test3_毫秒延迟_有限次数循环")][SerializeField] private uint numOfTimes = 5;
	[Header ("Test4+5_指定时间点_24小时制")][SerializeField] private string dateTime_specificTime = "13:00:00";
	[Header ("Test_帧延迟")][SerializeField] private uint intervalFrames = 100;

	private void OnTest1 () {
		Debug.Log ($"[test1] run {milliseconds_1/1000}'s later once");
	}

	private void OnTest2 () {
		Debug.Log ($"[test2] run every {milliseconds_2/1000}'s");
	}

	private void OnTest3 () {
		Debug.Log ($"[test3] run every {milliseconds_3/1000}'s for {numOfTimes} timers");
	}

	private void OnTest4 () {
		Debug.Log ($"[test4] run at {dateTime_specificTime}");
	}

	private void OnTest5 () {
		Debug.Log ($"[test5] run at {dateTime_specificTime} everyday!");
	}

	private void OnTest6 () {
		Debug.Log ($"[test6] run at eveny {3} target frames!");
	}
	private void OnTest7 () {
		Debug.Log ($"[test6] run at eveny {3} target frames!");
	}
	private void OnTest8 () {
		Debug.Log ($"[test6] run at eveny {3} target frames!");
	}

	void OnGUI () {
		GUI.skin.button.fontSize = 20;
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;

		if (GUI.Button (new Rect (30, 5, 360, 40), "AddWatcher_millisecond > Once"))
			TimeWatcher.Instance.AddWatcher_Once ("test1", milliseconds_1, OnTest1);
		if (GUI.Button (new Rect (30, 55, 360, 40), "AddWatcher_millisecond > Loop"))
			TimeWatcher.Instance.AddWatcher_Loop ("test2", milliseconds_2, OnTest2);
		if (GUI.Button (new Rect (30, 105, 360, 40), "AddWatcher_millisecond > Multi"))
			TimeWatcher.Instance.AddWatcher_Multi ("test3", milliseconds_3, numOfTimes, OnTest3);

		if (GUI.Button (new Rect (30, 205, 360, 40), "AddWatcher_DateTime > NotLoop"))
			TimeWatcher.Instance.AddWatcher_DateTime ("test4", dateTime_specificTime, false, OnTest4);
		if (GUI.Button (new Rect (30, 255, 360, 40), "AddWatcher_DateTime > Loop"))
			TimeWatcher.Instance.AddWatcher_DateTime ("test5", dateTime_specificTime, true, OnTest5);

		if (GUI.Button (new Rect (30, 355, 360, 40), "AddWatcher_Frame > Once"))
			TimeWatcher.Instance.AddWatcher_Frame_Once ("test6", intervalFrames, OnTest6);
		if (GUI.Button (new Rect (30, 405, 360, 40), "AddWatcher_Frame > Loop"))
			TimeWatcher.Instance.AddWatcher_Frame_Loop ("test7", intervalFrames, OnTest7);
		// if (GUI.Button (new Rect (30, 455, 360, 40), "AddWatcher_Frame > Multi"))
		// 	TimeWatcher.Instance.AddWatcher_Frame_Multi ("test8", 300, OnTest8);

		if (GUI.Button (new Rect (400, 5, 360, 40), "RemoveWatcher Once"))
			TimeWatcher.Instance.RemoveWatcher ("test1");
		if (GUI.Button (new Rect (400, 55, 360, 40), "RemoveWatcher Loop"))
			TimeWatcher.Instance.RemoveWatcher ("test2");
		if (GUI.Button (new Rect (400, 105, 360, 40), "RemoveWatcher Multi"))
			TimeWatcher.Instance.RemoveWatcher ("test3");

		if (GUI.Button (new Rect (400, 205, 360, 40), "RemoveWatcher NotLoop"))
			TimeWatcher.Instance.RemoveWatcher ("test4");
		if (GUI.Button (new Rect (400, 255, 360, 40), "RemoveWatcher Loop"))
			TimeWatcher.Instance.RemoveWatcher ("test5");

		if (GUI.Button (new Rect (400, 355, 360, 40), "RemoveWatcher Once"))
			TimeWatcher.Instance.RemoveWatcher_Frame ("test6");
		if (GUI.Button (new Rect (400, 405, 360, 40), "RemoveWatcher Loop"))
			TimeWatcher.Instance.RemoveWatcher_Frame ("test7");
		// if (GUI.Button (new Rect (400, 455, 360, 40), "RemoveWatcher Test08"))
		// 	TimeWatcher.Instance.RemoveWatcher ("test8");
	}
}