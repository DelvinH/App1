using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorpedoController : MonoBehaviour
{
	public LayerMask torpedoMask;
	public float maxDamage;
	public float minDamage;
	private float torpedoDamage;
	public float maxLifetime;
	public float explosionRadius;
	private bool activated = false;

	public void Activate ()
    {
		Destroy(gameObject, maxLifetime);
		torpedoDamage = Random.Range(minDamage, maxDamage);
		activated = true;
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
		if (!activated)
			return;
		
		Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius, torpedoMask);
		for (int i = 0; i < targets.Length; i++)
        {
			Rigidbody target = targets [i].GetComponent<Rigidbody>();
			if (!target)
				continue;
			torpedoImpact(target);
			torpedoEffects();
		}
		Destroy(gameObject);
	}

	private void torpedoImpact(Rigidbody target)
    {
		DestructibleObject targetObject = target.GetComponent<DestructibleObject>();
		if (!targetObject) 
			return;
		targetObject.takeDamage(torpedoDamage);
	}

	private void torpedoEffects()
    {

	}

}
