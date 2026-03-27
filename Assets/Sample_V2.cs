using LowoUN.Util;
using UnityEngine;

public class Sample_V2 : MonoBehaviour {

    long cachetimer1;
    long looptimer1;

    void Start () {
        TimeMgr.Self.Awake ();

        Time.timeScale = 1;
        TimeMgr.Self.SetWork ();

        TimeMgr.Self.StartTimer (2f, () => { Debug.Log ("execute action - Sample_V2 StartTimer"); }, () => { return true; });

        cachetimer1 = TimeMgr.Self.StartTimer (0.5f, () => { Debug.Log ("execute action - Sample_V2 StartTimer"); });
        TimeMgr.Self.StartTimer (
            0.2f,
            () => {
                Debug.Log ("execute action - Sample_V2 StopTimer");
                TimeMgr.Self.StopTimer (cachetimer1);
            },
            () => { return true; }
        );

        looptimer1 = TimeMgr.Self.StartTimer_Loop (1f, () => { Debug.Log ("execute action - StartTimer_Loop"); });
        // TimeMgr.Self.PauseObj(looptimer1);

        TimeMgr.Self.StartTimer_Multi (1f, 3, () => { Debug.LogError ("execute action - Sample_V2 StartTimer_Multi"); });
    }

    void Update () {
        TimeMgr.Self.Update ();
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 36;
        GUI.backgroundColor = Color.green;
        if (looptimer1 > 0) {
            if (GUI.Button (new Rect (10, 10, 450, 60), "Pause loop timer")) { TimeMgr.Self.PauseObj (looptimer1); }
            if (GUI.Button (new Rect (10, 80, 450, 60), "Resume loop timer")) { TimeMgr.Self.ResumeObj (looptimer1); }
            if (GUI.Button (new Rect (10, 150, 450, 60), "Resume loop timer")) { TimeMgr.Self.StopTimer (looptimer1); }
        }
    }
}