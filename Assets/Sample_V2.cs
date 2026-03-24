using LowoUN.Util;
using UnityEngine;

public class Sample_V2 : MonoBehaviour {

    long cachetimer1;
    long looptimer1;

    void Start () {
        TimeMgr.Self.Awake ();

        Time.timeScale = 1;
        TimeMgr.Self.SetWork ();

        cachetimer1 = TimeMgr.Self.StartTimer (0.5f, () => { Debug.Log ("execute action - Sample_V2 case1"); });
        TimeMgr.Self.StartTimer (
            0.2f,
            () => {
                Debug.Log ("execute action - Sample_V2 case2");
                TimeMgr.Self.StopTimer (cachetimer1);
            },
            () => { return true; }
        );

        TimeMgr.Self.StartTimer (2f, () => { Debug.Log ("execute action - Sample_V2 case3"); }, () => { return true; });

        looptimer1 = TimeMgr.Self.StartTimer_Loop (1f, () => { Debug.Log ("execute action - loop event"); });
        // TimeMgr.Self.PauseObj(looptimer1);
    }

    void Update () {
        TimeMgr.Self.Update ();
    }

    void OnGUI () {
        GUI.skin.button.fontSize = 36;
        GUI.backgroundColor = Color.green;
        if(looptimer1>0) {
            if (GUI.Button (new Rect (10, 10, 450, 60), "Pause loop timer")) {TimeMgr.Self.PauseObj(looptimer1);}
            if (GUI.Button (new Rect (10, 80, 450, 60), "Resume loop timer")) {TimeMgr.Self.ResumeObj(looptimer1);}
            if (GUI.Button (new Rect (10, 150, 450, 60), "Resume loop timer")) {TimeMgr.Self.StopTimer(looptimer1);}
        }
    }
}