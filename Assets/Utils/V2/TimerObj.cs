using System;
using UnityEngine;

namespace LowoUN.Util {
    public enum TimerObjState {
        NONE = 0, // 新建的
        Running,
        Pause,
        Done // 使用过的，会重复利用
    }
    public class TimerObj {
        public long id;
        float exeTime;
        Action done;
        Func<bool> bindCondition;
        // public bool isRealTimer;
        bool isFrameType;
        bool isIgnoreTimeScale;
        // bool isLoop => exeTimes==0;
        uint maxTimes = 1; // 0表示不限次数 1 表示1次 n表示多次
        uint curTimes;
        bool isInstanCall; // true 表示 立即调用一次event false(默认) 表示 经过delay的时间之后作为第一次调用; 适用于 Multi 和 Loop模式

        float curTime;
        TimerObjState curState;
        public TimerObjState CurState => curState;

        // public TimerObj (long id, float endTick, Action done, bool isRealTimer) {
        public TimerObj (long id, float exeTime, Action done, Func<bool> bindCondition, bool isFrameType, bool isIgnoreTimeScale, uint maxTimes,bool isInstanCall) {
            this.id = id;
            Init (exeTime, done, bindCondition, isFrameType, isIgnoreTimeScale, maxTimes,isInstanCall);
        }
        public void ReInit (long id, float exeTime, Action done, Func<bool> bindCondition, bool isFrameType, bool isIgnoreTimeScale, uint exeTimes,bool isInstanCall) {
            this.id = id;
            Init (exeTime, done, bindCondition, isFrameType, isIgnoreTimeScale, exeTimes,isInstanCall);
        }
        void Init (float exeTime, Action done, Func<bool> bindCondition, bool isFrameType, bool isIgnoreTimeScale, uint maxTimes, bool isInstanCall) {
            this.exeTime = exeTime;
            this.done = done;
            // this.isRealTimer = isRealTimer;
            this.bindCondition = bindCondition;
            this.isFrameType = isFrameType;
            this.isIgnoreTimeScale = isIgnoreTimeScale;
            this.maxTimes = maxTimes;
            this.isInstanCall = isInstanCall;

            // 对于 Loop/Multi模式 如果 isDelayOrInstan 第一次执行是延后，或者是立即
            if(maxTimes == 0 || maxTimes > 1) {
                if(isInstanCall)
                    CallEventInstan();
            }
        }

        public void Start () {
            curTime = 0;
            curTimes = 0;
            SetState_NotStop (TimerObjState.Running);
        }

        public void SetState_Stop_Manual () {
            this.curState = TimerObjState.Done;
            this.done = null; // 手动停止的timer不再执行done action
            SetStateDone ();
        }
        void SetState_Stop () {
            this.curState = TimerObjState.Done;
            SetStateDone ();
        }

        void SetState_NotStop (TimerObjState s) {
            this.curState = s;
        }

        public void SetState_Pause () { SetState_NotStop (TimerObjState.Pause); }
        public void SetState_Resume () { SetState_NotStop (TimerObjState.Running); }

        void CallEventOnce () {
            try {
                if (this == null) {
                    Debug.LogError ("TimerObj is null");
                    return;
                }
                if (done != null) {
                    if (bindCondition == null || (bindCondition != null && bindCondition.Invoke () == true)) {
                        done.Invoke ();
                    }
                }
            } catch (System.Exception e) {
                Debug.LogError ($"TimerObj -- CallLoopTypeEvent -- error, e:{e}");
            }
        }

        void SetStateDone () {
            try {
                if (this == null) {
                    Debug.LogError ("TimerObj is null");
                    return;
                }

                if (!isInstanCall) {
                    if (done != null) {
                        if (bindCondition == null || (bindCondition != null && bindCondition.Invoke () == true)) {
                            done.Invoke ();
                        }
                    }
                }
                Reset ();
                TimeMgr.Self.DoneToRecycle (this);
            } catch (System.Exception e) {
                Debug.LogError ($"TimerObj -- SetStateDone -- error, e:{e}");
            }
        }

        void Reset () {
            exeTime = 0;
            maxTimes = 1;
            done = null;
            curState = TimerObjState.Done;
        }

        public void SetUpdate () {
            if (curState != TimerObjState.Running)
                return;

            if (isFrameType) {
                if (Time.timeScale > 0)
                    curTime += 1;
            } else {
                if (isIgnoreTimeScale)
                    curTime += Time.unscaledDeltaTime;
                else
                    curTime += Time.deltaTime;
            }

            if (curTime >= exeTime) {
                curTime = 0;
                if (maxTimes == 0)
                    CallEventOnce ();
                else if (maxTimes == 1) {
                    curTimes = 0;
                    SetState_Stop ();
                }
                else if (maxTimes > 1) {
                    curTimes += 1;
                    if (curTimes >= maxTimes) {
                        curTimes = 0;
                        SetState_Stop ();
                    } else {
                        CallEventOnce ();
                    }
                }
            }
        }

        // 如果 isDelayOrInstan 第一次执行 延后/立即 是 立即
        void CallEventInstan () {
            // Multi模式
            if(maxTimes > 1)
                curTimes += 1;
            CallEventOnce ();
        }
    }
}