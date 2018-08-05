﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{
    public Rigidbody projectileType;
    public Transform fireTransformLeft;
    public Transform fireTransformRight;
    public float fireSpeed;
    public float fireRate;
    public float minSpeedMultiplier;//varies torpedo speed

    private float timeSinceLastFireLeft;
    private float timeSinceLastFireRight;
    private bool leftIsReady;
    private bool rightIsReady;


    private void OnEnable()
    {
        //possible ammo indicator
    }

    private void Start()
    {
        leftIsReady = false;
        rightIsReady = false;
    }

    
    private void Update()
    {
        leftIsReady = Time.time - timeSinceLastFireLeft > 1 / fireRate;
        rightIsReady = Time.time - timeSinceLastFireRight > 1 / fireRate;

        if (Input.GetButtonDown("Fire1") && leftIsReady && !gameObject.GetComponent<PlayerMovement>().getChangingDepth())
        { 
			FireLeft();
            timeSinceLastFireLeft = Time.time;
		} else if (Input.GetButtonDown("Fire1") && rightIsReady && !gameObject.GetComponent<PlayerMovement>().getChangingDepth())
        {
            FireRight();
            timeSinceLastFireRight = Time.time;

        }
        else
        {
            leftIsReady = false;
            rightIsReady = false;
        }
    }
    
    private void FireLeft()
    {
		Rigidbody projectileLeft = Instantiate(projectileType, fireTransformLeft.position, transform.rotation) as Rigidbody;
		projectileLeft.velocity = fireTransformLeft.forward * fireSpeed * Random.Range(minSpeedMultiplier, 1);
        leftIsReady = false;
        //Audio
    }

    private void FireRight()
    {
        Rigidbody projectileRight = Instantiate(projectileType, fireTransformRight.position, transform.rotation) as Rigidbody;
        projectileRight.velocity = fireTransformRight.forward * fireSpeed * Random.Range(minSpeedMultiplier, 1);
        rightIsReady = false;
        //Audio
    }
}
