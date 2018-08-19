using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFiring : MonoBehaviour
{

    private void OnEnable()
    {
        //possible ammo indicator
    }

    private void Start()
    {

	}


    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && !gameObject.GetComponent<Mob>().getChangingDepth())//randomizes the side that fires if both are ready
        {
			gameObject.GetComponent<Mob> ().tryFire ();
        }
    }
    
}
