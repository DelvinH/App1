﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : OurGameObject
{
	public float health;
	public float maxHealth;

    public bool startOnFullHealth;

	// Use this for initialization
	override public void Start ()
    {
		base.Start ();

        if (startOnFullHealth)
            health = maxHealth;
	}
	
	// Update is called once per frame
	override public void Update ()
    {
		base.Update ();
	}

	public float GetHealth()
    {
		return health;
	}

	public void TakeDamage(float amount)
    {
		if (amount <= 0)
			return;
		
		health = Mathf.Max(0, health - amount);
		if (health <= 0)
			ObjectBreak(); 
	}

	public void ObjectBreak()
    {
        Destroy(gameObject);
	}

	public void HealDamage(float amount)
    {
		if(amount <= 0)
			return;
		
		health = Mathf.Min(maxHealth, health + amount);
	}

}
