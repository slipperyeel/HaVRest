using UnityEngine;
using System.Collections;
using System;

public class HVRControllerManager : SteamVR_ControllerManager
{
    public static HVRControllerManager Instance;

    public event EventHandler DeviceConnected;

    private GameObject m_controllerRight;
    public GameObject Right { get { return m_controllerRight; } }

    private GameObject m_controllerLeft;
    public GameObject Left { get { return m_controllerLeft; } }

    private VRTK.VRTK_ControllerEvents m_rightEvents;
    public VRTK.VRTK_ControllerEvents RightEvents { get { return m_rightEvents; } }

    private VRTK.VRTK_ControllerEvents m_leftEvents;
    public VRTK.VRTK_ControllerEvents LeftEvents { get { return m_leftEvents; } }

    public int RightIndex { get { return m_rightEvents.ControllerIndex; } }
    public int LeftIndex { get { return m_leftEvents.ControllerIndex; } }

    protected override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetControllers();
    }

    private void SetControllers()
    {
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

    protected override void OnDeviceConnected(params object[] args)
    {
        base.OnDeviceConnected(args);

        // if a controller turns on, we need to refresh the references
        int connectedControllerIndex = (int)args[0];
        bool connected = (bool)args[1];
        if (connected)
        {
            SetControllers();
        }

        if (DeviceConnected != null)
        {
            ConnectedDeviceArgs deviceArgs = new ConnectedDeviceArgs();
            deviceArgs.controllerIndex = connectedControllerIndex;
            DeviceConnected(this, deviceArgs);
        }
    }

    public class ConnectedDeviceArgs : EventArgs
    {
        public int controllerIndex;
    }
}
