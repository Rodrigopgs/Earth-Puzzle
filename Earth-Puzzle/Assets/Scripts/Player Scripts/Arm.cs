using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Arm : MonoBehaviour
{

    public float dragSpeedModifier = 0.75f;
    public float direction;

    public bool attatch = true;
    public bool pickup;
    public bool throwPlayer;

    public ArmAnimations animtaions;
    public ArmStates states;

    //push/pull
    public PushPullReciever attatched;

    //pickup/place
    public PickupReciever holding;
    public GameObject holdingPreview;

    //player throw
    public PlayerThrower playerThrower;

    private void Start()
    {
        if (ArmTransfer.Instance == null)
            new ArmTransfer(this);
        else
            LoadValues();
    }

    private void LoadValues()
    {
        attatch = ArmTransfer.Instance.attatch;
        pickup = ArmTransfer.Instance.pickup;
        throwPlayer = ArmTransfer.Instance.throwPlayer;
    }

    public void UnlockPickup() => pickup = true;

    public void UnlockThrowPlayer() => throwPlayer = true;

    [System.Serializable]
    public class ArmAnimations
    {
        public AnimationClip pull;
        public AnimationClip push;

        public AnimationClip walkHolding;
        public AnimationClip jumpHolding;
        public AnimationClip idleHolding;

        public bool HasNull() => pull == null || push == null || walkHolding == null || jumpHolding == null || idleHolding == null;

    }

    [System.Serializable]
    public class ArmStates
    {
        public sbyte pushDirection;

        public bool attatched;
        public bool holding;


        public bool HasState()
        {
            return attatched || holding;
        }

        public void Cancel()
        {
            attatched = false;
            holding = false;
        }
    }

    private void OnDestroy()
    {
        new ArmTransfer(this);
    }
}