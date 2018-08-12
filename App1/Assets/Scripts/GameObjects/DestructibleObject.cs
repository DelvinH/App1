using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : Rigidbody
{
	public float health;
	public float maxHealth;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
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
