using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A class that provides a basic interaction with objects that triggers an event that can be assigned in the inspector.
/// <br></br>
/// <br></br>
/// For the object to appear with a white outline, its material must be set to the '2d_lit_outline' or '2d_unlit_outline' material.
/// </summary>
public class BasicInteract : Interactable
{

    [Tooltip("The events that will be invoked when the object is interacted with")]
    public UnityEvent events;

    public override void OnInteract() => events.Invoke();

}
