using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour 
{
	[Serializable]
	public struct InputAxis
	{
		public string AxisName;
		public float Threshold;
	}

	public List<InputAxis> InputAxes;
	public Vector3 Offset = Vector3.zero;
	public float PositionalSmoothing = 0.1f;
	public float RotationalSmoothing = 0.1f;
    public int GrabbableLayer = 11;
	public float Range = 0.25f;

	bool m_IsGrabbing = false;
	Transform m_GrabbedObject = null;
	Quaternion m_InitialRotation = Quaternion.identity;
	
	// Update is called once per frame
	void Update () 
	{
		UpdateIsGrabbing ();

		if (m_GrabbedObject != null) 
		{
			UpdateGrabbedObject (m_GrabbedObject);	
		}
	}

	bool ShouldGrab()
	{
		for (int i = 0; i < InputAxes.Count; i++) 
		{
			InputAxis axis = InputAxes [i];
			if (Input.GetAxis (axis.AxisName) > axis.Threshold) 
			{
				return true;
			}
		}
		return false;
	}

	void UpdateIsGrabbing()
	{
		bool shouldGrab = ShouldGrab ();
		if (shouldGrab != m_IsGrabbing) 
		{
			if (shouldGrab) 
			{
				Collider[] colliders = Physics.OverlapSphere (transform.position + Offset, Range,  1 << GrabbableLayer);
                if(colliders.Length > 0)
				{
					Collider currentCollider = colliders [0];
					m_GrabbedObject = currentCollider.transform;
					m_InitialRotation = Quaternion.Inverse(transform.rotation) * m_GrabbedObject.rotation;
				}
			} 
			else 
			{
				m_GrabbedObject = null;
			}
			m_IsGrabbing = shouldGrab;
		}
	}

	void UpdateGrabbedObject(Transform grabbedObject)
	{
		float clampedPositionalSmoothing = Mathf.Clamp (1f - PositionalSmoothing, 0f, 1f);
		grabbedObject.position = Vector3.Lerp(grabbedObject.position, transform.position + Offset, clampedPositionalSmoothing);

		float clampedRotationalSmoothing = Mathf.Clamp (1f - RotationalSmoothing, 0f, 1f);
		grabbedObject.rotation = Quaternion.Slerp (grabbedObject.rotation, transform.rotation * m_InitialRotation, clampedRotationalSmoothing);
	}
}
