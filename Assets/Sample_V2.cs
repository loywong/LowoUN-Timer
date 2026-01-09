using LowoUN.Util;
using UnityEngine;

public class Sample_V2 : MonoBehaviour {

    long cachetimer1;

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
    }

    void Update () {
        TimeMgr.Self.Update ();
    }
}