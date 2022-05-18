using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The class that allows players to interact with objects tagged as "Interact" in the world.
/// <br></br>
/// <br></br>
/// For the object to appear with a white outline, its material must be set to the '2d_lit_outline' or '2d_unlit_outline' material.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Player2Interactions : MonoBehaviour
{
    public static Player2Interactions Instance { get; private set; }

    TwoPlayerActions inputs;
    TwoPlayerActions.Player2Actions defaultActions;

    InputAction interact;
    List<Interactable> interactables = new List<Interactable>();
    Interactable outlined;

    // arms
    public Arm arms;

    private void Awake()
    {
        Instance = this;

        inputs = new TwoPlayerActions();
        defaultActions = inputs.Player2;
        interact = defaultActions.Interact;
    }

    private void OnEnable()
    {
        inputs.Enable();
        defaultActions.Enable();
        interact.Enable();

        interact.performed += OnInteract;
        interact.canceled += CancelInteract;
    }

    private void Start()
    {
        arms = GetComponent<Arm>();
    }

    private void OnInteract(InputAction.CallbackContext cb)
    {
        if (Time.timeScale == 0)
            return;

        if (arms.states.holding == true)
        {
            if (arms.holding != null)
            {
                if(arms.holding.Place())
                StartCoroutine(NullValues(0));
            }
            else if (arms.playerThrower != null)
            {
                arms.playerThrower.Throw();
                StartCoroutine(NullValues(1));
            }
            return;
        }

        if (arms.states.HasState())
            return;

        if (outlined != null)
            outlined.OnInteract(2);
    }

    private IEnumerator NullValues(int n)
    {
        yield return null;

        if (n == 0)
            arms.holding = null;
        else
            arms.playerThrower = null;
    }

    private void CancelInteract(InputAction.CallbackContext cb)
    {
        if (arms.states.HasState())
        {
            if (arms.states.attatched)
            {
                if (cb.canceled)
                {
                    arms.attatched.stuck = false;
                    arms.states.Cancel();
                    arms.attatched.rb2d.isKinematic = true;
                    arms.attatched.rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
                }
            }
        }
    }

    private void Update()
    {
        if (interactables.Count > 0)
        {
            if (outlined != null)
                outlined.GetComponent<SpriteRenderer>().material.SetInt("_Glow", 0);

            float smallest = float.MaxValue;
            Interactable inter = null;

            for (int i = 0; i < interactables.Count; i++)
            {
                float temp = Vector3.Distance(transform.position, interactables[i].transform.position);
                if (temp < smallest)
                {
                    smallest = temp;
                    inter = interactables[i];
                }
            }
            outlined = inter;
        }
        if (outlined != null)
            outlined.GetComponent<SpriteRenderer>().material.SetInt("_Glow", 1);
    }

    private void OnDestroy()
    {
        interact.performed -= OnInteract;
        interact.canceled -= CancelInteract;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interact"))
        {
            Interactable temp = collision.GetComponent<Interactable>();
            if (!interactables.Contains(temp) && temp.Conditions(gameObject))
                interactables.Add(temp);
            else
                temp.GetComponent<SpriteRenderer>().material.SetInt("_Glow", 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Interact"))
        {
            Interactable temp = collision.GetComponent<Interactable>();
            interactables.Remove(temp);
            collision.GetComponent<SpriteRenderer>().material.SetInt("_Glow", 0);
            if (outlined == temp)
                outlined = null;
        }
    }

    /// <summary>
    /// Call when the interactables list changes or when interactable conditions might change
    /// </summary>
    public void UpdateInteractables()
    {
        List<Interactable> temps = new List<Interactable>(interactables);
        foreach (Interactable interactable in temps)
        {
            if (!interactable.Conditions(gameObject))
            {
                interactables.Remove(interactable);
                if (outlined == interactable)
                {
                    outlined.GetComponent<SpriteRenderer>().material.SetInt("_Glow", 0);
                    outlined = null;
                }
            }
        }
    }

}
