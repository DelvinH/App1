using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    protected Rigidbody rigidbody;

	public float maxHealth;
    
    public float health;
    private bool currentlyTakingDamage = false;

	virtual public void Start ()
    {
        health = maxHealth;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        if (!rigidbody)
        {
            Debug.LogWarning("Game Object without rigidbody", this);
        }
    }
	
	virtual public void Update ()
    {
		
	}

    virtual public void FixedUpdate()
    {
        if (health < 0)
        {
            health = 0;
            ObjectBreak();
        }
        if (!currentlyTakingDamage)
        {
            health = Mathf.RoundToInt(health);
        }
    }

	public float GetHealth()
    {
		return health;
	}

    public void SetHealth(float newHealth)
    {
        health = newHealth;
    }

	public void TakeDamage(float amount)
    {
		if (amount <= 0)
			return;

        health -= amount;
	}

    public void TakeDamageOverTime(float amount, float time)
    {
        float damageRate = amount / time;
        StartCoroutine(TakeDOTCoroutine(damageRate, time));
    }

    private IEnumerator TakeDOTCoroutine(float damageRate, float time)
    {
        float timeAtStart = Time.time;
        currentlyTakingDamage = true;

        while (Time.time - timeAtStart < time)
        {
            health -= damageRate * Time.deltaTime;
            yield return null;
        }
        currentlyTakingDamage = false;
    }

	public void HealDamage(float amount)
    {
		if(amount <= 0)
			return;
		
		health = Mathf.Min(maxHealth, health + amount);
	}

    public void HealDamageOverTime(float amount, float time)
    {
        float healRate = amount / time;
        StartCoroutine(HealDOTCoroutine(healRate, time));
    }

    private IEnumerator HealDOTCoroutine(float healRate, float time)
    {
        float timeAtStart = Time.time;
        currentlyTakingDamage = true;

        while (Time.time - timeAtStart < time)
        {
            health += healRate * Time.deltaTime;
            yield return null;
        }
        currentlyTakingDamage = false;
    }

    public void ObjectBreak()
    {
        Destroy(gameObject);
    }
}
