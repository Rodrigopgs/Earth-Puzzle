using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class WeightButton : MonoBehaviour
{
    public UnityEvent triggerEvents;
    public UnityEvent stopEvents;

    int triggered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered <= 0)
            triggerEvents.Invoke();

        triggered += 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = Mathf.Clamp(triggered - 1, 0, int.MaxValue);

        if (triggered <= 0)
            stopEvents.Invoke();
    }

}
