using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class Timer {
    float left_time;
    Action callback;

    public Timer(float wait_time, Action callback) {
        this.left_time = wait_time;
        this.callback = callback;
    }

    public bool run_forward(float delta_time) {
        this.left_time -= delta_time;
        if (this.left_time < 0) {
            callback();
            return true;
        }
        return false;
    }
}


public class TimerController : Singleton<TimerController>
{

    List<Timer> timers;

    public void Start()
    {
        timers = new List<Timer>();
    }

    public void Update()
    {
        for (int i = 0; i < timers.Count; i++) {
            if (timers[i].run_forward(Time.deltaTime)) {
                timers.RemoveAt(i);
            }
        }
    }
    
    public int do_after_seconds(float seconds, Action callback) {
        timers.Add(new Timer(seconds, callback));
        return 0;
    }

}
