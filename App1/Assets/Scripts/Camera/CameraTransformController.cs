using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformController : MonoBehaviour
{
    public GameObject camera;

    private float cameraDistance;
    private float cameraAngle;
    private float cameraHeight;

    public float cameraTilt;
	
	private void Start ()
    {
        cameraDistance = camera.GetComponent<CameraControl>().getCameraDistance();
        cameraAngle = camera.GetComponent<CameraControl>().getCameraAngle();
        cameraHeight = camera.GetComponent<CameraControl>().getCameraHeight();

        transform.position = new Vector3(0f, Mathf.Sin(Mathf.Deg2Rad * cameraAngle) * cameraDistance + cameraHeight, -Mathf.Cos(Mathf.Deg2Rad * cameraAngle) * cameraDistance);

        transform.rotation = Quaternion.Euler(cameraAngle, 0f, 0f);

        //Debug.Log(cameraDistance);
        //Debug.Log(cameraAngle);
        //Debug.Log(cameraHeight);
    }
	
	
	private void Update ()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Mathf.Deg2Rad * cameraAngle) * cameraDistance + cameraHeight, transform.position.z);//keeps camera on same y plane
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
