using System;
using System.Timers;
using UnityEngine;

namespace LowoUN.Util {
    public class WatchObj_Frame {
        private string name;
        public bool loop { private set; get; }
        private uint tarFrames;
        private uint curFrames;

        public WatchObj_Frame (string name, uint tarFrames, bool loop) {
            this.name = name;
            this.loop = loop;

            this.tarFrames = tarFrames;
            curFrames = 0;
        }

        public void UpdateFrame () {
            curFrames += 1;
            Debug.Log ($"curFrames {curFrames}!");
            if (curFrames >= tarFrames) {
                TimeWatcher.Instance.OnCallback_Frame (name);
                Debug.Log ("WatchData_Frame has reached target Frame, execute callback!");
                if (loop)
                    Reset ();
            }
        }

        private void Reset () {
            curFrames = 0;
        }

        public void End () {
            // loop or not loop
        }
    }

    public class WatchObj {
        private string name;
        private Timer timer;
        public bool loop { private set; get; }
        private bool isForDateTime;

        // limit times loop
        private bool loopLimit { get { return times > 1; } }
        public bool loopLimitAndComplete { get { return loopLimit && curTimes >= times; } }
        private uint times;
        private uint mseconds;
        private uint curTimes;

        public WatchObj (string name, uint mseconds, bool loop) {
            isForDateTime = false;

            this.name = name;
            this.loop = loop;

            timer = new Timer (mseconds);
            timer.Elapsed += OnElapsed;
            timer.AutoReset = loop;
        }

        public WatchObj (string name, uint mseconds, uint times) {
            isForDateTime = false;

            this.name = name;
            this.loop = true;
            this.times = times;
            this.mseconds = mseconds;

            timer = new Timer (mseconds);
            timer.Elapsed += OnElapsed;
            timer.AutoReset = loop;

            curTimes = 0;
        }

        public WatchObj (string name, DateTime time, bool loop) {
            isForDateTime = true;

            this.name = name;
            this.loop = loop;

            TimeSpan span = time - DateTime.Now;
            if (span.TotalMilliseconds > 0) {
                timer = new Timer (span.TotalMilliseconds);
            } else {
                span = time.AddDays (1) - DateTime.Now;
                timer = new Timer (span.TotalMilliseconds);
            }
            timer.Elapsed += OnElapsed;
            timer.AutoReset = false;
        }

        private void OnElapsed (object sender, ElapsedEventArgs e) {
            if (!isForDateTime) {
                if (loopLimit) {
                    curTimes += 1;
                    if (curTimes >= times) {
                        timer.Stop ();
                        TimeWatcher.Instance.OnCallback (name);
                    } else {
                        TimeWatcher.Instance.OnCallback (name);
                        timer.Interval = mseconds;
                        timer.Start ();
                    }
                } else {
                    if (!loop)
                        timer.Stop ();
                    TimeWatcher.Instance.OnCallback (name);
                }
            } else {
                timer.Stop ();
                TimeWatcher.Instance.OnCallback (name);

                if (loop) {
                    timer.Interval = new TimeSpan (24, 0, 0).Milliseconds;
                    timer.Start ();
                }
            }
        }

        public void Start () {
            timer.Start ();
        }

        public void Stop () {
            timer.Stop ();
        }

        public void End () {
            timer.Stop ();
            timer.Elapsed -= OnElapsed;
            timer.Dispose ();
        }
    }
}