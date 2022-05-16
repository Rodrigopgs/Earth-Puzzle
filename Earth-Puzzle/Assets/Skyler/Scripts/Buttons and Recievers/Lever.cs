using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    public UnityEvent triggerEvents;
    public UnityEvent stopEvents;

    public LeverType type;

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

    bool triggered;

    private void Start()
    {
        switch (type)
        {
            case LeverType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                break;
            case LeverType.Animation:
                animator = GetComponent<Animator>();
                break;
        }

        Visuals(false);
    }

    public override void OnInteract(int playerNumber)
    {
        if (!triggered)
        {
            triggerEvents.Invoke();
            triggered = true;
            Visuals(true);
        }
        else
        {
            stopEvents.Invoke();
            triggered = false;
            Visuals(false);
        }
    }

    private void Visuals(bool v)
    {
        switch (type)
        {
            case LeverType.Sprite:
                if (v)
                    spriteRenderer.sprite = active;
                else
                    spriteRenderer.sprite = inactive;
                break;
            case LeverType.Animation:
                if (v)
                    animator.Play(activated.name);
                else
                    animator.Play(idle.name);
                break;
        }
    }

    public enum LeverType
    {
        Static,
        Sprite,
        Animation
    }
}
