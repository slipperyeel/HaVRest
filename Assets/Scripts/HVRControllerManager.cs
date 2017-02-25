using UnityEngine;
using System.Collections;
using System;
using VRTK;

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

    public int RightIndex { get { return (int)VRTK_DeviceFinder.GetControllerIndex(Right); } }
    public int LeftIndex { get { return (int)VRTK_DeviceFinder.GetControllerIndex(Left); } }

    public override void Awake()
    {
        Instance = this;
        base.Awake();
    }

    public override void OnEnable()
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
            m_leftEvents = GetLeftController().GetComponent<VRTK.VRTK_ControllerEvents>();
        }

        if (m_controllerRight != null)
        {
            m_rightEvents = GetRightController().GetComponent<VRTK.VRTK_ControllerEvents>();
        }
    }

    public GameObject GetRightController()
    {
        return VRTK_DeviceFinder.GetControllerRightHand(false);
    }

    public GameObject GetLeftController()
    {
        return VRTK_DeviceFinder.GetControllerLeftHand(false);
    }

    public GameObject GetControllerByGameObject(GameObject gameObject)
    {
        GameObject obj = null;
        if(gameObject != null)
        {
            int index = -1;
            index = (int)VRTK_DeviceFinder.GetControllerIndex(gameObject);

            if(index > -1)
            {
                obj = VRTK_DeviceFinder.GetControllerByIndex((uint)index, false);
            }
            else
            {
                Debug.LogError("Index is -1");
            }
        }
        else
        {
            Debug.LogError("Trying to get a controller from a go that has a null gameobject");
        }
        return obj;
    }

    public override void OnDeviceConnected(int index, bool connected)
    {
        // if a controller turns on, we need to refresh the references
        int connectedControllerIndex = index;

        if (connected)
        {
            SetControllers();
        }

        if (DeviceConnected != null)
        {
            ConnectedDeviceArgs deviceArgs = new ConnectedDeviceArgs();
            deviceArgs.controllerIndex = connectedControllerIndex;
            deviceArgs.IsConnected = connected;
            DeviceConnected(this, deviceArgs);
        }
        base.OnDeviceConnected(index, connected);
    }

    public GameObject GetControllerByIndex(int index)
    {
        if(index == RightIndex)
        {
            return Right;
        }
        else if (index == LeftIndex)
        {
            return Left;
        }
        return null;
    }

    public class ConnectedDeviceArgs : EventArgs
    {
        public int controllerIndex;
        public bool IsConnected;
    }
}
