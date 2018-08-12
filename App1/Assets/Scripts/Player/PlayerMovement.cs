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

    private float turnVelocity;
    private float turnValue;

    private bool atSurface;
    private bool changingDepth;
    private bool beginCoroutine;
    
    public float moveAcceleration;
    public float turnAcceleration;
    public float moveSpeed;
    public float turnSpeed;
    public float minTurnSpeed;
    
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

        turnValue = 0f;

        atSurface = true;
        changingDepth = false;
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

    private void Move()
    {
        Vector3 movement = transform.forward * movementAxisValue * moveAcceleration;
        rigidbody.AddForce(movement);

        if (rigidbody.velocity.magnitude >= moveSpeed)//limits speed
            rigidbody.velocity = rigidbody.velocity.normalized * moveSpeed;

        rigidbody.velocity = transform.forward * rigidbody.velocity.magnitude;//prevents drifting
        
        if (Mathf.Abs(movementAxisValue) < 0.01f && rigidbody.velocity.magnitude < 0.01f)//ensures stopping speed is 0
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
        turnValue = 
            Mathf.SmoothDamp(turnValue, 
            turnAxisValue * Mathf.Clamp(turnSpeed * (rigidbody.velocity.magnitude / moveSpeed), minTurnSpeed, turnSpeed * (rigidbody.velocity.magnitude / moveSpeed)),
            ref turnVelocity, 1 / turnAcceleration);
        Quaternion turnRotation = Quaternion.Euler(0f, turnValue, 0f);
        rigidbody.MoveRotation(rigidbody.rotation * turnRotation);

        //Debug.Log(turn + "turn");
        //Debug.Log(turnAxisValue * turnSpeed * (rigidbody.velocity.magnitude / moveSpeed));
        //Debug.Log(turnRotation.eulerAngles);
        //Debug.Log(rigidbody.rotation.eulerAngles;  
    }
    private void ToggleDepth()
    {
        if (Input.GetButton("Fire2") && !changingDepth)
        {
            changingDepth = true;
            beginCoroutine = true;
            //Debug.Log("Fire2");
        }
        
        if (changingDepth && beginCoroutine)
        {
            IEnumerator coroutine = ChangeDepth(atSurface);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator ChangeDepth(bool atSurface)
    {
        Vector3 movement;
        float timeElapsed = Time.time;
        beginCoroutine = false;//prevents more coroutines from starting

        if (atSurface)
        {
            this.atSurface = false;//submarine leaves surface plane at beginning of dive
            //Audio
        }
       
        while (Time.time - timeElapsed < changeDepthTime)
        {
            if (atSurface)//if loop must be in while loop for proper Time.deltaTime values
                movement = transform.up * changeDepthSpeed * Time.deltaTime * -1f;//negative makes submarine go down
            else
                movement = transform.up * changeDepthSpeed * Time.deltaTime;//positive makes submarine go up
            rigidbody.MovePosition(rigidbody.position + movement);
            yield return null;
        }

        if (!atSurface)
        {
            this.atSurface = true;//submarine enters surface plane at end of surface
            //Audio
        }

        changingDepth = false;
    }



    public bool getChangingDepth()
    {
        return changingDepth;
    }
}

