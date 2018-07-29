using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour {
    public Rigidbody ProjectileType;
	public float FiringDelay = 5f;
	public float lastFire = 0f;
    public Transform firingtransform1;
    public Transform firingtransform2;
    public float firingForce;
	public float maxTTL = 10;

	void Awake() {
		//firingtransform1 = Instantiate(gameObject.transform);
		//firingtransform2 = Instantiate(gameObject.transform);
		//firingtransform1.Translate (Vector3.right * 3);
		//firingtransform2.Translate (Vector3.left * 3);
	}

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
		if (Input.GetAxis ("Fire1") > 0) {
			if ((Time.realtimeSinceStartup - lastFire) > FiringDelay) {
				FireProjectile ();
			}
		}
    }
    
    bool FireProjectile() {
        Rigidbody Projectile1;
		Rigidbody Projectile2;
		Projectile1 = Instantiate (ProjectileType, firingtransform1.position, transform.rotation) as Rigidbody;
		Projectile2 = Instantiate (ProjectileType, firingtransform2.position, transform.rotation) as Rigidbody;
		Projectile1.velocity = (firingtransform1.forward * firingForce);
		Projectile2.velocity = (firingtransform2.forward * firingForce);
		lastFire = Time.realtimeSinceStartup;
		Destroy (Projectile1.gameObject, maxTTL);
		Destroy (Projectile2.gameObject, maxTTL);
		return true;

    }
}
