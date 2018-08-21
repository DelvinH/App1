using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransformController : MonoBehaviour
{
    public GameObject camera;

    private float cameraDistance;
    private float cameraAngle;
    private float cameraHeight;

    public Rigidbody playerRigidbody;
	
	private void Start ()
    {
        cameraDistance = camera.GetComponent<CameraControl>().getCameraDistance();
        cameraAngle = camera.GetComponent<CameraControl>().getCameraAngle();
        cameraHeight = camera.GetComponent<CameraControl>().getCameraHeight();

        transform.position = playerRigidbody.position + new Vector3(0f, cameraHeight, -cameraDistance);

        transform.rotation = Quaternion.Euler(cameraAngle, 0f, 0f);

        //Debug.Log(cameraDistance);
        //Debug.Log(cameraAngle);
        //Debug.Log(cameraHeight);
    }
	
	
	private void Update ()
    {
        cameraDistance = camera.GetComponent<CameraControl>().getCameraDistance() + camera.GetComponent<CameraControl>().getCameraZoomDistance();
        Debug.Log(cameraDistance);
        transform.position = playerRigidbody.position + new Vector3(0, cameraHeight, -cameraDistance);//keeps camera on same y plane
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
