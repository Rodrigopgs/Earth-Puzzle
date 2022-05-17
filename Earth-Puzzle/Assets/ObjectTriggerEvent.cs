using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ObjectTriggerEvent : MonoBehaviour
{
    public UnityEvent triggerEvent;

    public string matchTag;
    public bool allowTriggers;
    public int required = 1;

    int number;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((!allowTriggers && collision.isTrigger) || !collision.CompareTag(matchTag))
            return;

        number++;
        if (number >= required)
            triggerEvent.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((!allowTriggers && collision.isTrigger) || !collision.CompareTag(matchTag))
            return;
        number--;
    }
}
