using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ActivationReciever : MonoBehaviour
{

    public UnityEvent activateEvents;
    public UnityEvent deactivateEvents;

    public virtual void Activate() => activateEvents.Invoke();
    public virtual void Deactivate() => deactivateEvents.Invoke();
}
