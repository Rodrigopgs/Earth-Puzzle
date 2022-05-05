using UnityEngine;

/// <summary>
/// An abstract class that provides a simple framework for objects that can be interacted with in the world.
/// <br></br>
/// <br></br>
/// For the object to appear with a white outline, its material must be set to the '2d_lit_outline' or '2d_unlit_outline' material.
/// </summary>
public abstract class Interactable : MonoBehaviour
{
    /// <summary>
    /// The method that is called when this object is interacted with. This method must be overridden.
    /// </summary>
    public abstract void OnInteract();
    /// <summary>
    /// The method that is called to determine if this object can be interacted with. This method can be overridden.
    /// </summary>
    /// <returns>True if it can be interacted with, false if it cannot</returns>
    public virtual bool Conditions() => true;
}