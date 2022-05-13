using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class AnimationActivationReciever : ActivationReciever
{

    public AnimationClip activated;
    public AnimationClip deactivated;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public override void Activate() => InternalActivate();
    public override void Deactivate() => InternalDeactivate();

    private void InternalActivate()
    {
        anim.Play(activated.name);
        activateEvents.Invoke();
    }
    private void InternalDeactivate()
    {
        anim.Play(deactivated.name);
        deactivateEvents.Invoke();
    }

}
