using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using static PushButton;

[RequireComponent(typeof(Collider2D))]
public class WeightButton : MonoBehaviour
{
    public UnityEvent triggerEvents;
    public UnityEvent stopEvents;

    int triggered;

    public WeightButtonType type;

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


    private void Start()
    {
        switch (type)
        {
            case WeightButtonType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                break;
            case WeightButtonType.Animation:
                animator = GetComponent<Animator>();
                break;
        }

        Visuals(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered <= 0)
        {
            triggerEvents.Invoke();
            Visuals(true);
        }

        triggered += 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        triggered = Mathf.Clamp(triggered - 1, 0, int.MaxValue);

        if (triggered <= 0)
        {
            stopEvents.Invoke();
            Visuals(false);
        }
    }

    private void Visuals(bool v)
    {
        switch (type)
        {
            case WeightButtonType.Sprite:
                if (v)
                    spriteRenderer.sprite = active;
                else
                    spriteRenderer.sprite = inactive;
                break;
            case WeightButtonType.Animation:
                if (v)
                    animator.Play(activated.name);
                else
                    animator.Play(idle.name);
                break;
        }
    }

    public enum WeightButtonType
    {
        Static,
        Sprite,
        Animation
    }
}
