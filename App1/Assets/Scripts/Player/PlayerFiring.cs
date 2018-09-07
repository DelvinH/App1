using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
	private Mob playerMob;
    /*private float timeSinceLastFireLeft;
	private float timeSinceLastFireRight;
	private bool leftIsReady;
	private bool rightIsReady;*/
    private bool rightFire = true; //for alternating fire
    private float timeForNextFire = 0.0f;//used for reloading delay

	public Rigidbody projectileType;
	public Transform fireTransformLeft;
	public Transform fireTransformRight;
    public float fireRate;
    public float accuracyVariation;

    private void OnEnable()
    {
		playerMob = gameObject.GetComponent<Mob>();
		if (!playerMob) {
			Debug.LogWarning ("Warning: Player firing assigned to non mob.", this);
			Destroy (this);
		}
    }

    private void Start()
    {

	}


    private void Update()
    {
        if (Input.GetButton/*Down*/("Fire1") && !gameObject.GetComponent<Mob>().getChangingDepth())
        {
            handleFiring();
            //tryFire ();
        }
    }

	/*public void tryFire(){
		if (leftIsReady && rightIsReady) {//randomizes the side that fires if both are ready
			float rand = Random.value - 0.5f;
			if (rand <= 0) {
				doFireLeft ();
			} else if (rand > 0) {
				doFireRight ();
			}
			//Debug.Log(rand + "rand");
		} else if (leftIsReady) {
			doFireLeft ();
		} else if (rightIsReady) {
			doFireRight ();
		}

	}*/

	/*Firing*/
	private void handleFiring()
    {
        playerMob.handleFiring();
        /*if (Time.time > timeForNextFire)
        {
            timeForNextFire = Time.time + fireRate;
            if (rightFire)
            {
                Vector3 rotation = fireTransformRight.TransformDirection(fireTransformRight.forward);
                rotation = fireTransformRight.rotation.eulerAngles;
                rotation = new Vector3(rotation.x, rotation.y + Random.Range(accuracyVariation, -accuracyVariation), rotation.z);
                Instantiate(projectileType, fireTransformRight.position, Quaternion.Euler(rotation));
            } else
            {
                Vector3 rotation = fireTransformLeft.TransformDirection(fireTransformLeft.forward);
                rotation = fireTransformLeft.rotation.eulerAngles;
                rotation = new Vector3(rotation.x, rotation.y + Random.Range(accuracyVariation, -accuracyVariation), rotation.z);
                Instantiate(projectileType, fireTransformLeft.position, Quaternion.Euler(rotation));
            }
            rightFire = !rightFire;
        }*/

		//leftIsReady = Time.time - timeSinceLastFireLeft > 1 / fireRate;
		//rightIsReady = Time.time - timeSinceLastFireRight > 1 / fireRate;
	}

	/*public void doFireLeft()
	{
		//Debug.Log("firedleft");
		Rigidbody projectileLeft = Instantiate(projectileType, fireTransformLeft.position, transform.rotation) as Rigidbody;
		projectileLeft.velocity = fireTransformLeft.forward * fireSpeed * projectileLeft.GetComponent<PlayerTorpedoController>().torpedoSpeed;
		projectileLeft.GetComponent<PlayerTorpedoController>().Activate ();
		leftIsReady = false;
		timeSinceLastFireLeft = Time.time;
		//Audio
	}

	public void doFireRight()
	{
		//Debug.Log("firedright");
		Rigidbody projectileRight = Instantiate(projectileType, fireTransformRight.position, transform.rotation) as Rigidbody;
		projectileRight.velocity = fireTransformRight.forward * fireSpeed * projectileRight.GetComponent<PlayerTorpedoController>().torpedoSpeed;
		projectileRight.GetComponent<PlayerTorpedoController>().Activate ();
		rightIsReady = false;
		timeSinceLastFireRight = Time.time;
		//Audio
	}*/
}
