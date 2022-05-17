using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class TimerActivator : MonoBehaviour
{
    public float waitTime;
    float time;

    public UnityEvent timeEvents;

    private void Update()
    {
        if (time >= waitTime)
        {
            timeEvents.Invoke();
            time = 0;
        }

        time += Time.deltaTime;
    }

}
