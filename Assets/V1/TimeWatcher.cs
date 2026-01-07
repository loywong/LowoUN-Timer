using System;
using System.Collections.Generic;
using UnityEngine;

namespace LowoUN.Util {
    public class TimeWatcher : ManagerMono<TimeWatcher> {
        private Dictionary<string, WatchObj> name2watch = new Dictionary<string, WatchObj> ();
        private Dictionary<string, Action> name2action = new Dictionary<string, Action> ();
        private Queue<Action> cbqueue = new Queue<Action> ();

        private Dictionary<string, WatchObj_Frame> name2watch_frame = new Dictionary<string, WatchObj_Frame> ();
        private List<string> name2watch_frame_toremove = new List<string> ();

        public void AddWatcher_DelayMsecond (string name, uint mseconds, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, false, callback);
        }
        public void AddWatcher_Once (string name, uint mseconds, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, false, callback);
        }
        public void AddWatcher_Loop (string name, uint mseconds, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, true, callback);
        }
        public void AddWatcher_Multi (string name, uint mseconds, uint times, Action callback, bool isTimeScaleAff = true) {
            AddWatcher (name, mseconds, times, callback);
        }

        public void AddWatcher_DateTime (string name, string dateTime_specificTime, bool loop, Action callback, bool isTimeScaleAff = true) {
            var timeString = DateTime.Now.Date.ToShortDateString () + " " + dateTime_specificTime;
            Debug.Log ($"timeString {timeString}");
            AddWatcher ("test4", DateTime.Parse (timeString), loop, callback);
        }

        public void AddWatcher_DelayFrame (string name, uint frames, Action callback, bool isTimeScaleAff = true) {
            AddWatcher_Frame (name, frames, false, callback);
        }
        public void AddWatcher_Frame_Once (string name, uint frames, Action callback, bool isTimeScaleAff = true) {
            AddWatcher_Frame (name, frames, false, callback);
        }
        public void AddWatcher_Frame_Loop (string name, uint frames, Action callback, bool isTimeScaleAff = true) {
            AddWatcher_Frame (name, frames, true, callback);
        }
        // public void AddWatcher_Frame_Multi (string name, uint frames, Action callback, bool isTimeScaleAff = true) {
        //     AddWatcher_Frame (name, frames, false, callback);
        // }
        private void AddWatcher_Frame (string name, uint frames, bool loop, Action callback, bool isTimeScaleAff = true) {
            if (name2watch_frame.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of frame delay event has exist!");
                return;
            }
            name2watch_frame[name] = new WatchObj_Frame (name, frames, loop);
        }

        /// <summary>
        /// add delay event
        /// </summary>
        /// <param name="name">uniqueue name for delay event</param>
        /// <param name="mseconds">milliseconds for delay</param>
        /// <param name="loop">repeat or not</param>
        private void AddWatcher (string name, uint mseconds, bool loop, Action callback, bool isTimeScaleAff = true) {
            if (mseconds <= 0) {
                Debug.LogWarning ("watch time can not be less or equal 0");
                return;
            }

            if (name2watch.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of timer has exist!");
                return;
            }

            uint ms = isTimeScaleAff ? (uint) ((float) mseconds * (1f / Time.timeScale)) : mseconds;
            if (ms <= 0)
                return;

            name2watch[name] = new WatchObj (name, ms, loop);
            name2action[name] = callback;
            name2watch[name].Start ();
        }

        // times: repeat for fixed number of times
        private void AddWatcher (string name, uint mseconds, uint times, Action callback, bool isTimeScaleAff = true) {
            if (times <= 1) {
                Debug.LogWarning ("times must be more then 1");
                return;
            }

            if (mseconds <= 0) {
                Debug.LogWarning ("watch time can not be less or equal 0");
                return;
            }

            if (name2watch.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of timer has exist!");
                return;
            }

            uint ms = isTimeScaleAff ? (uint) ((float) mseconds * (1f / Time.timeScale)) : mseconds;
            if (ms <= 0)
                return;

            name2watch[name] = new WatchObj (name, ms, times);
            name2action[name] = callback;
            name2watch[name].Start ();
        }

        /// <summary>
        /// add specific time event
        /// </summary>
        /// <param name="name">uniqueue name for each delay event</param>
        /// <param name="time">specific time</param>
        /// <param name="loop">repeat ot not, if true, every 24 houra work!</param>
        private void AddWatcher (string name, DateTime time, bool loop, Action callback) {
            if (name2watch.ContainsKey (name)) {
                Debug.LogWarning ($"This name {name} of timer is exist!");
                return;
            }

            var w = new WatchObj (name, time, loop);
            name2watch[name] = w;
            name2action[name] = callback;

            w.Start ();
        }

        // /// <summary>
        // /// add specific time event with time string
        // /// </summary>
        // /// <param name="name">uniqueue name for each delay event</param>
        // /// <param name="time">specific time string include hour, minute, second, eg."18:00:00"</param>
        // /// <param name="loop">repeat or not</param>
        // /// <param name="callback"></param>
        // public void AddWatcher (string name, string time, bool loop, Action callback) {
        //     AddWatcher (name, DateTime.Parse (time), loop, callback);
        // }

        public void RemoveWatcher (string name) {
            if (!name2watch.ContainsKey (name)) {
                Debug.Log ($"RemoveWatcher has no event key:{name}");
                return;
            }

            name2watch[name].End ();
            name2watch.Remove (name);
            name2action.Remove (name);
            Debug.Log ($"RemoveWatcher event key: {name}, succ!");
        }

        public void RemoveWatcher_Frame (string name) {
            if (!name2watch_frame.ContainsKey (name)) {
                Debug.Log ($"RemoveWatcher has no event key:{name}");
                return;
            }

            name2watch_frame[name].End ();
            name2watch_frame_toremove.Add (name);
            Debug.Log ($"RemoveWatcher event key: {name}, succ!");
        }

        public void OnCallback_Frame (string name) {
            if (!name2watch_frame[name].loop) {
                RemoveWatcher_Frame (name);
            }
        }
        public void OnCallback (string name) {
            if (!name2action.ContainsKey (name))
                return;

            cbqueue.Enqueue (name2action[name]);
            if (!name2watch[name].loop || name2watch[name].loopLimitAndComplete) {
                RemoveWatcher (name);
            }
        }

        void Update () {
            if (cbqueue.Count > 0) {
                Action action = cbqueue.Dequeue ();
                action?.Invoke ();
            }

            if (name2watch_frame.Count > 0) {
                foreach (var key in name2watch_frame_toremove) {
                    name2watch_frame.Remove (key);
                    Debug.Log ($"Real key {key}, name2watch_frame.Count: {name2watch_frame.Count}");
                }
                name2watch_frame_toremove.Clear ();

                foreach (var item in name2watch_frame)
                    item.Value.UpdateFrame ();
            }
        }

        void OnApplicationQuit () {
            foreach (var kvp in name2watch) {
                kvp.Value.End ();
            }

            name2watch.Clear ();
            name2action.Clear ();
            cbqueue.Clear ();
        }

        // Check time event existed!
        public bool ContainKey (string key) {
            if (key == null) {
                Debug.LogWarning ("Invalid key:null");
                return false;
            }

            bool isContain = false;

            isContain = name2watch.ContainsKey (key);
            if (isContain)
                return true;

            isContain = name2watch_frame.ContainsKey (key);
            if (isContain)
                return true;

            return false;
        }
    }
}