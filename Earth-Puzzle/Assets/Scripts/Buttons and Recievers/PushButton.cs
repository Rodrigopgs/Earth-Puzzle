using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class PushButton : Interactable
{
    public UnityEvent triggerEvents;
    public UnityEvent stopEvents;

    [Space]
    public float cooldown = 0.2f;

    public PushButtonType type;

    //Sprite
    [HideInInspector]
    public Sprite inactive;
    [HideInInspector]
    public Sprite active;

    SpriteRenderer spriteRenderer;

    //Animation
    [HideInInspector]
    public AnimationClip idle;
    [HideInInspector]
    public AnimationClip activated;

    Animator animator;

    float time;
    bool triggered;

    private void Start()
    {
        Visuals(false);

        switch (type)
        {
            case PushButtonType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                break;
            case PushButtonType.Animation:
                animator = GetComponent<Animator>();
                break;
        }
    }

    public override void OnInteract(int playerNumber)
    {
        triggerEvents.Invoke();
        triggered = true;
        Visuals(true);
    }

    public override bool Conditions() => !triggered;

    private void Update()
    {
        if (!triggered)
            return;

        if (time > cooldown)
        {
            triggered = false;
            time = 0;
            stopEvents.Invoke();
            Visuals(false);
        }

        time += Time.deltaTime;
    }

    private void Visuals(bool v)
    {
        switch (type)
        {
            case PushButtonType.Sprite:
                if (v)
                    spriteRenderer.sprite = active;
                else
                    spriteRenderer.sprite = inactive;
                break;
            case PushButtonType.Animation:
                if (v)
                    animator.Play(activated.name);
                else
                    animator.Play(idle.name);
                break;
        }
    }

    public enum PushButtonType
    {
        Static,
        Sprite,
        Animation
    }

}