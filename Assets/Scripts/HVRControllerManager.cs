using UnityEngine;
using System.Collections;

public class HVRControllerManager : SteamVR_ControllerManager
{
    public static HVRControllerManager Instance;

    private GameObject m_controllerRight;
    public GameObject Right { get { return m_controllerRight; } }

    private GameObject m_controllerLeft;
    public GameObject Left { get { return m_controllerLeft; } }

    private VRTK.VRTK_ControllerEvents m_rightEvents;
    public VRTK.VRTK_ControllerEvents RightEvents { get { return m_rightEvents; } }

    private VRTK.VRTK_ControllerEvents m_leftEvents;
    public VRTK.VRTK_ControllerEvents LeftEvents { get { return m_leftEvents; } }

    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        m_controllerLeft = left;
        m_controllerRight = right;

        if (m_controllerLeft != null)
        {
            m_leftEvents = m_controllerLeft.GetComponent<VRTK.VRTK_ControllerEvents>();
        }

        if (m_controllerRight != null)
        {
            m_rightEvents = m_controllerRight.GetComponent<VRTK.VRTK_ControllerEvents>();
        }
    }
}
