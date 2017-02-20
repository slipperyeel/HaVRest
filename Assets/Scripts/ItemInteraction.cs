using UnityEngine;
using System.Collections;
using System;
using VRTK;

public class ItemInteraction : MonoBehaviour
{
    [SerializeField]
    private int mArrow;
    [SerializeField]
    private int mSlotNumber;
    [SerializeField]
    private BagInteraction mBagInteraction;

    public delegate void TriggerEnter(Collider other);
    public static event TriggerEnter OnEntered;
    public delegate void TriggerExit();
    public static event TriggerExit OnExited;

    private bool mIsSlotBeingHovered = false;
    private ushort bagHapticVibration = 1000;

    void OnEnable()
    {
        HVRControllerManager.Instance.RightEvents.TriggerReleased += DoTriggerReleasedRight;
        HVRControllerManager.Instance.LeftEvents.TriggerReleased += DoTriggerReleasedLeft;
    }

    void OnDisable()
    {
        HVRControllerManager.Instance.RightEvents.TriggerReleased -= DoTriggerReleasedRight;
        HVRControllerManager.Instance.LeftEvents.TriggerReleased -= DoTriggerReleasedLeft;
    }

    void OnDestroy()
    {
        OnDisable();
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
                if (mArrow > 0)
                {
                    GameManager.Instance.Player.BackPack.ChangePage(mArrow);
                }
                else
                {
                    GameManager.Instance.Player.BackPack.FocusOnSlot(mSlotNumber, true);
                }
            }
        }
    }

    void DoTriggerReleasedRight(object sender, ControllerInteractionEventArgs e)
    {
        if (mBagInteraction.mIsBagOnLeft)
        {
            if (mIsSlotBeingHovered)
            {
                if (mArrow > 0)
                {
                    GameManager.Instance.Player.BackPack.ChangePage(mArrow);
                }
                else
                {
                    GameManager.Instance.Player.BackPack.FocusOnSlot(mSlotNumber, true);
                }
            }
        }
    }
}