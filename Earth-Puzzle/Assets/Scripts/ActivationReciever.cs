using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ActivationReciever : MonoBehaviour
{

    public UnityEvent activateEvents;
    public UnityEvent deactivateEvents;

    public void Activate() => activateEvents.Invoke();
    public void Deactivate() => deactivateEvents.Invoke();
}
