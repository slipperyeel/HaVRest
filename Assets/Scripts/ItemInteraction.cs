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
        GameObject gobject = other.gameObject;

        if(gobject)
        {
            BagDetector detector = gobject.GetComponent<BagDetector>();
            if(detector)
            {
                HVRControllerManager.Instance.GetControllerByGameObject(detector.ControllerObject).GetComponent<VRTK_ControllerActions>().TriggerHapticPulse(bagHapticVibration, 0.05f, 0.01f);
            }
            else
            {
                Debug.LogError("Detector is null or not set on the ui selector");
            }        
        }
        else
        {
            Debug.LogError("You tried to get a controller for haptic feedback in class ItemInteraction with a go that was null");
        }
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
                    mIsSlotBeingHovered = false;
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
                    mIsSlotBeingHovered = false;
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