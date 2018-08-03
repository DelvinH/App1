using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorpedoController : MonoBehaviour
{
	public LayerMask torpedoMask;
    public float ignoreMaskNumber;
	public float maxDamage;
	public float minDamage;
	public float maxLifetime;
	public float explosionRadius;
    public bool homing;

    private float torpedoDamage;

    
    void Start()
    {
        Destroy(gameObject, maxLifetime);
        torpedoDamage = Random.Range(minDamage, maxDamage);
    }

    
    void Update()
    {
        if (homing)
        {
            TrackTarget();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
		if (!gameObject.activeSelf || other.gameObject.layer == ignoreMaskNumber)
			return;
		
		Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius, torpedoMask);
		for (int i = 0; i < targets.Length; i++)
        {
			Rigidbody target = targets [i].GetComponent<Rigidbody>();
			if (!target)
				continue;
			TorpedoImpact(target);
			TorpedoEffects();
		}
		Destroy(gameObject);
	}

	private void TorpedoImpact(Rigidbody target)
    {
		DestructibleObject targetObject = target.GetComponent<DestructibleObject>();
		if (!targetObject) 
			return;
		targetObject.TakeDamage(torpedoDamage);
	}

	private void TorpedoEffects()
    {

	}

    private void TrackTarget()
    {

    }
}
