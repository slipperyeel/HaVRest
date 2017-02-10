using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class ItemSlotInteraction : MonoBehaviour
{
    [SerializeField]
    private int mSlotNumber;
    [SerializeField]
    private BagInteraction mBagInteraction;
    private Backpack mBackpack;

    public delegate void TriggerEnter(Collider other);
    public static event TriggerEnter OnEntered;
    public delegate void TriggerExit();
    public static event TriggerExit OnExited;

    private bool mIsSlotBeingHovered = false;
    private ushort bagHapticVibration = 1000;

    void Awake()
    {
        HVRControllerManager.Instance.RightEvents.TriggerReleased += DoTriggerReleasedRight;
        HVRControllerManager.Instance.LeftEvents.TriggerReleased += DoTriggerReleasedLeft;

        mBackpack = GameObject.Find("Backpack").GetComponent<Backpack>();
    }

    void OnDestroy()
    {
        HVRControllerManager.Instance.RightEvents.TriggerReleased -= DoTriggerReleasedRight;
        HVRControllerManager.Instance.LeftEvents.TriggerReleased -= DoTriggerReleasedLeft;
    }

    void OnTriggerEnter(Collider other)
    {
        mIsSlotBeingHovered = true;
        other.transform.parent.gameObject.GetComponent<VRTK_ControllerActions>().TriggerHapticPulse(bagHapticVibration, 0.05f, 0.01f);
    }

    void OnTriggerExit(Collider other)
    {
        mIsSlotBeingHovered = false;
    }

    void DoTriggerReleasedLeft(object sender, ControllerInteractionEventArgs e)
    {
        if (!mBagInteraction.mIsBagOnLeft)
        {
            if (mIsSlotBeingHovered)
            {
                mBackpack.FocusOnSlot(mSlotNumber, true);
            }
        }
    }

    void DoTriggerReleasedRight(object sender, ControllerInteractionEventArgs e)
    {
        if (mBagInteraction.mIsBagOnLeft)
        {
            if (mIsSlotBeingHovered)
            {
                mBackpack.FocusOnSlot(mSlotNumber, true);
            }
        }
    }
}