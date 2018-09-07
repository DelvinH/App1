using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTorpedoController : MonoBehaviour
{
    public Rigidbody rigidbody;
    public LayerMask torpedoMask;
    public float ignoreMaskNumber;
	public float maxDamage;
	public float minDamage;
	public float maxLifetime;
	public float explosionRadius;
	public float torpedoSpeed;
    public float minSpeedMultiplier;
    public bool homing;
    public bool doesDOT;
    public float timeForDOT;

    private float torpedoDamage;

    
    void Start()
    {
        torpedoDamage = Random.Range(minDamage, maxDamage);
        torpedoSpeed *= minSpeedMultiplier;
        Destroy(gameObject, maxLifetime);
    }

    
    void Update()
    {
        if (homing)
        {
            TrackTarget();
        }
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + transform.forward * torpedoSpeed * Time.deltaTime);
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
        if (doesDOT)
        {
            targetObject.TakeDamageOverTime(torpedoDamage, timeForDOT);
        }
        else
        {
            targetObject.TakeDamage(torpedoDamage);
        }
		
	}

	private void TorpedoEffects()
    {

	}

    private void TrackTarget()
    {

    }
}
