using UnityEngine;
using System.Collections;

public class VRSetup : MonoBehaviour {
    [SerializeField] private bool enableVR;
    private float startingRotation;
    private float initializerTotal = 0.0f;
    private int initializerCount = 1;
    private bool initialized = false;

    void Start () {
        if (!enableVR)
        {
            GetComponent<Camera>().stereoTargetEye = StereoTargetEyeMask.None;
        }
        startingRotation = transform.rotation.eulerAngles.y;
	}

	void Update () {
        if (enableVR)
        {
            // This will check to see that the headset is initialized and taking over the camera rotation
            if ((!initialized) && (startingRotation != transform.rotation.eulerAngles.y))
            {
                initialized = true;
            }

            if (initialized)
            {
                // We are checking the first one hundred frames of camera rotaion
                if (initializerCount <= 100)
                {
                    // Only the latter 50 are accurate
                    if (initializerCount > 50)
                    {
                        initializerTotal += transform.localEulerAngles.y;
                    }
                    // We average the 50 frames to see the starting rotation of the headset
                    if (initializerCount == 100)
                    {
                        transform.parent.transform.localEulerAngles = new Vector3(transform.parent.transform.localEulerAngles.x
                                                                                 , -initializerTotal / 50
                                                                                 , transform.parent.transform.localEulerAngles.z);
                    }
                    initializerCount++;
                }
            }
        }
	}
}