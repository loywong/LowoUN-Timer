using LowoUN.Util;
using UnityEngine;

public class Sample_V2 : MonoBehaviour {

    long cachetimer1;

    void Start () {
        TimeMgr.Self.Awake();

        Time.timeScale = 1;
        TimeMgr.Self.SetWork();

        cachetimer1 = TimeMgr.Self.StartTimer (0.1f, () => { Debug.Log ("execute action - Sample_V2 case1"); });
        TimeMgr.Self.StopTimer(cachetimer1);

        TimeMgr.Self.StartTimer (2f, () => { Debug.Log ("execute action - Sample_V2 case2"); }, () => { return true; });
    }

    void Update () {
        TimeMgr.Self.Update();
    }
}