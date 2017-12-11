using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAttachPoint : MonoBehaviour 
{
	public Transform AttachPoint;

	public Material OffMaterial;
	public Material OnMaterial;

	public MeshRenderer CameraVisual;
    public GameObject PreviewVisual;

	public void SetCameraAttached(bool attached)
	{
		if(CameraVisual != null)
		{
			CameraVisual.material = attached ? OnMaterial : OffMaterial;
		}

        if(PreviewVisual != null)
        {
            PreviewVisual.SetActive(attached);
        }
	}
}
