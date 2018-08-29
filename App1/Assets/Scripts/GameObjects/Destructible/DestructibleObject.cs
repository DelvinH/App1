using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
	public float health;
	public float maxHealth;

    public bool startOnFullHealth;

	// Use this for initialization
	virtual public void Start ()
    {
        if (startOnFullHealth)
            health = maxHealth;
	}
	
	// Update is called once per frame
	virtual public void Update ()
    {
	}

	virtual public void FixedUpdate(){
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
