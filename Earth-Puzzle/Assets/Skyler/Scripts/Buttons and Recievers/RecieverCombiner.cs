using UnityEngine;
using UnityEngine.Events;

public class RecieverCombiner : MonoBehaviour
{
    public UnityEvent triggerEvents;
    public UnityEvent stopEvents;

    public bool[] activationsRequired;

    public RecieverCombinerType type;

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
            case RecieverCombinerType.Sprite:
                spriteRenderer = GetComponent<SpriteRenderer>();
                break;
            case RecieverCombinerType.Animation:
                animator = GetComponent<Animator>();
                break;
        }

        Visuals(false);
    }

    public void Activate(int boolPos)
    {
        activationsRequired[boolPos] = true;

        if (AllTrue())
        {
            triggerEvents.Invoke();
            Visuals(true);
        }
    }

    public void Deactivate(int boolPos)
    {
        activationsRequired[boolPos] = false;

        if (!AllTrue())
        {
            stopEvents.Invoke();
            Visuals(false);
        }
    }

    private bool AllTrue()
    {
        for (int i = 0; i < activationsRequired.Length; i++)
        {
            if (!activationsRequired[i])
                return false;
        }

        return true;
    }

    private void Visuals(bool v)
    {
        switch (type)
        {
            case RecieverCombinerType.Sprite:
                if (v)
                    spriteRenderer.sprite = active;
                else
                    spriteRenderer.sprite = inactive;
                break;
            case RecieverCombinerType.Animation:
                if (v)
                    animator.Play(activated.name);
                else
                    animator.Play(idle.name);
                break;
        }
    }

    public enum RecieverCombinerType
    {
        Static,
        Sprite,
        Animation
    }
}
