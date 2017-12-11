using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Spectator Camera, specifically smoothing and transferring between locations
/// </summary>
public class SpectatorController : MonoBehaviour
{
	// Input
    public KeyCode SwitchCameraKeycode = KeyCode.Space;
	public List<string> AlternateButtonInputNames;

	// Camera
    public Transform CameraTransform;
	public List<GameObject> AttachmentPoints = new List<GameObject>();

	// Smoothing
	public float PositionalSmoothing = 0.1f;
	public float RotationalSmoothing = 0.1f;

	// Bookkeeping
	Transform m_CurrentTransform;
    int m_CurrentCameraIndex = 0;
    bool m_CameraSwitchAlreadyTriggered = false;

    void Awake()
    {
        if(AttachmentPoints.Count > 0)
        {
			SwitchCamera (0);
        }
    }

    void Update()
    {
		bool triggered = Input.GetKey (SwitchCameraKeycode);
		for (int i = 0; i < AlternateButtonInputNames.Count && !triggered; i++) 
		{
			triggered = Input.GetButton (AlternateButtonInputNames [i]);
		}

		if(m_CameraSwitchAlreadyTriggered != triggered)
        {
            if(triggered)
            {
                SwitchCamera();
                m_CameraSwitchAlreadyTriggered = true;
            }
            else
            {
                m_CameraSwitchAlreadyTriggered = false;
            }
        }
    }

	// Smooth in LateUpdate in order to be sure that movements have already been done by all objects
	void LateUpdate()
	{
        if(m_CurrentTransform)
        {
            float rotationSpeed = Mathf.Clamp(1f - RotationalSmoothing, 0f, 1f);
			CameraTransform.rotation = Quaternion.Slerp(CameraTransform.rotation, m_CurrentTransform.rotation, rotationSpeed);

            float movementSpeed = Mathf.Clamp(1f - PositionalSmoothing, 0f, 1f);
			CameraTransform.position = Vector3.Lerp(CameraTransform.position, m_CurrentTransform.position, movementSpeed);
        }		
	}

	public void SwitchCamera()
	{
		SwitchCamera(m_CurrentCameraIndex + 1);
	}

	/// <summary>
	/// Increments the Selected Camera Index and snaps to that new camera
	/// </summary>
    public void SwitchCamera(int index)
    {
		// Disable Previous Camera
		GameObject previousAttachPointGO = AttachmentPoints [m_CurrentCameraIndex];
		CameraAttachPoint attachPointComponent = previousAttachPointGO.GetComponentInChildren<CameraAttachPoint> ();
		if (attachPointComponent != null) 
		{
			attachPointComponent.SetCameraAttached (false);
		}

		m_CurrentCameraIndex = index % AttachmentPoints.Count;

		// Enable new Camera
		GameObject attachmentPointGO = AttachmentPoints [m_CurrentCameraIndex];
		attachPointComponent = attachmentPointGO.GetComponentInChildren<CameraAttachPoint> ();
		if (attachPointComponent != null) 
		{
			attachPointComponent.SetCameraAttached (true);
			m_CurrentTransform = attachPointComponent.AttachPoint;
		} 
		else 
		{
			m_CurrentTransform = attachmentPointGO.transform;
		}

		Snap(m_CurrentTransform);
    }

	/// <summary>
	/// Instantly move to the selected transform, ignoring smoothing
	/// </summary>
	/// <param name="toTransform">Transform to snap to</param>
    void Snap(Transform toTransform)
    {
		CameraTransform.position = toTransform.position;
		CameraTransform.rotation = toTransform.rotation;
    }
}
