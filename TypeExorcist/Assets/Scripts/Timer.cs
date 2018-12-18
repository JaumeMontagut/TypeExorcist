using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer {

    private float start_time;

    public Timer()
    {
        this.start_time = 0.0F;
    }

    public void StarTimer()
    {
        start_time = Time.time;
    }

    public float GetCurrentTime()
    {
        return (Time.time - start_time);
    }
}
