using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
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

	public float getHealth()
    {
		return health;
	}

	public void takeDamage(float amount)
    {
		if (amount <= 0)
			return;
		
		health = Mathf.Max(0, health - amount);
		if (health == 0)
			objectBreak(); 
	}

	public void objectBreak()
    {

	}

	public void healDamage(float amount)
    {
		if(amount <= 0)
			return;
		
		health = Mathf.Min(maxHealth, health + amount);
	}

}
