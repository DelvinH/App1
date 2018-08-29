using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
	private Mob holder;
	private float timeSinceLastFireLeft;
	private float timeSinceLastFireRight;
	private bool leftIsReady;
	private bool rightIsReady;

	public Rigidbody projectileType;
	public Transform fireTransformLeft;
	public Transform fireTransformRight;
	public float fireSpeed;
	public float fireRate;

    private void OnEnable()
    {
		holder = gameObject.GetComponent<Mob>();
		if (!holder) {
			Debug.LogWarning ("Warning: Player firing assigned to non mob.", this);
			Destroy (this);
		}
        //possible ammo indicator
    }

    private void Start()
    {

	}


    private void Update()
    {
		handleFiring ();
        if (Input.GetButtonDown("Fire1") && !gameObject.GetComponent<Mob>().getChangingDepth())//randomizes the side that fires if both are ready
        {
			tryFire ();
        }
    }

	public void tryFire(){
		if (leftIsReady && rightIsReady) {
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

	}

	/*Firing*/
	private void handleFiring(){
		leftIsReady = Time.time - timeSinceLastFireLeft > 1 / fireRate;
		rightIsReady = Time.time - timeSinceLastFireRight > 1 / fireRate;
	}

	public void doFireLeft()
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
	}

}
