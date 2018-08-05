using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidbody;
    private string movementAxisName;
    private string turnAxisName;
    private float movementAxisValue;
    private float turnAxisValue;

    private bool atSurface;
    private bool diving;
    private bool surfacing;
    private float timeSinceStart;

    public float moveAcceleration;
    public float turnSpeed;
    public float maxMoveSpeed;

    public float changeDepthSpeed;
    public float changeDepthTime;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movementAxisName = "Vertical";
        turnAxisName = "Horizontal";

        atSurface = true;
        diving = false;
        surfacing = false;
    }

    /*private void OnDisable()
    {
        rigidbody.isKinematic = false;

        movementAxisValue = 0f;
        turnAxisValue = 0f;
    }

    private void OnEnable()
    {
        rigidbody.isKinematic = true;
    }*/

    private void Update()
    {
        movementAxisValue = Input.GetAxis(movementAxisName);
        turnAxisValue = Input.GetAxis(turnAxisName);
    }

    private void FixedUpdate()
    {
        Move();
        Turn();
        ToggleDepth();
    }

    private void Move()//edit for joystick
    {
        Vector3 movement = transform.forward * movementAxisValue * moveAcceleration;

        rigidbody.AddForce(movement);


        if (rigidbody.velocity.magnitude >= maxMoveSpeed)
            rigidbody.velocity = rigidbody.velocity.normalized * maxMoveSpeed;

        rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;//prevents drifting

        if (Mathf.Abs(movementAxisValue) < 0.1f && rigidbody.velocity.magnitude < 0.5f)
            rigidbody.velocity = Vector3.zero;

        if (atSurface)
        {
            //SurfaceAudio
        }
        else if (!atSurface)
        {
            //SubmergedAudio
        }

        //Debug.Log(rigidbody.velocity.magnitude);
    }

    private void Turn()
    {


        /*Vector3 turn = transform.up * turnAxisValue * turnAcceleration * (rigidbody.velocity.magnitude / maxMoveSpeed);

        rigidbody.AddTorque(turn);

        if (rigidbody.angularVelocity.magnitude >= maxTurnSpeed * (rigidbody.velocity.magnitude / maxMoveSpeed))
            rigidbody.angularVelocity = rigidbody.angularVelocity.normalized * maxTurnSpeed * (rigidbody.velocity.magnitude / maxMoveSpeed);


        if (Mathf.Abs(turnAxisValue) < 0.1f)
        rigidbody.angularVelocity = Vector3.zero;

        if (Mathf.Abs(turnAxisValue) < 0.1f)
           rigidbody.angularVelocity = rigidbody.angularVelocity.normalized * rigidbody.angularVelocity.magnitude / angularDrag;*/

        float turn = turnAxisValue * turnSpeed * (rigidbody.velocity.magnitude / maxMoveSpeed) * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);

    }

    private void ToggleDepth()
    {
        if (Input.GetButton("Fire2") && atSurface && (!diving && !surfacing))
        {
            diving = true;
            timeSinceStart = Time.time;
            Debug.Log("Fire2");
        }
        else if (Input.GetButton("Fire2") && !atSurface && (!diving && !surfacing))
        {
            surfacing = true;
            timeSinceStart = Time.time;
            Debug.Log("Fire2");
        }
       

        if (diving)
        {
            Dive();
        }
        else if (surfacing)
        {
            Surface();
        }
    }

    private void Dive()
    {
        Vector3 diveMovement = transform.up * changeDepthSpeed * Time.deltaTime * -1f;//Negative makes submarine go down

        atSurface = false;

        //Audio

        if (Time.time - timeSinceStart < changeDepthTime)
        {
            rigidbody.MovePosition(rigidbody.position + diveMovement);
        }
        else if (Time.time - timeSinceStart > changeDepthTime)
        {
            diving = false;
            surfacing = false;
        }
    }

    private void Surface()
    {
        float timeElapsed = Time.time;

        Vector3 surfaceMovement = transform.up * changeDepthSpeed * Time.deltaTime;

        if (Time.time - timeSinceStart < changeDepthTime)
        {
            rigidbody.MovePosition(rigidbody.position + surfaceMovement);
        }
        else if (Time.time - timeSinceStart > changeDepthTime)
        {
            diving = false;
            surfacing = false;

            atSurface = true;

            //Audio
        }
    }



    public bool getChangingDepth()
    {
        return diving || surfacing;
    }
}

