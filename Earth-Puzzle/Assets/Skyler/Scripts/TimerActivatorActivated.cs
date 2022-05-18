using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class TimerActivatorActivated : MonoBehaviour
{
    public float waitTime;

    public bool Count { get; set; }

    float time;

    public UnityEvent timeEvents;

    private void Update()
    {
        if (!Count)
            return;

        if (time >= waitTime)
        {
            timeEvents.Invoke();
            time = 0;
        }

        time += Time.deltaTime;
    }

}