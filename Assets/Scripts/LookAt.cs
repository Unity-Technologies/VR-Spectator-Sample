using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    public Transform Target;
	public Vector3 PositionOffset;
    public Vector3 RotationOffset;

	// Update is called once per frame
	void Update ()
    {
		transform.LookAt(Target.position + PositionOffset);
		transform.rotation *= Quaternion.Euler(RotationOffset);
	}
}
