using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firing : MonoBehaviour {
    public Rigidbody ProjectileType;
	public float FiringDelay;
	public float lastFire = 0;
    public Transform firingtransform1;
    public Transform firingtransform2;
    public float firingForce;

	void Awake() {
		firingtransform1 = Instantiate(gameObject.transform);
		firingtransform2 = Instantiate(gameObject.transform);
		firingtransform1.Translate (Vector3.right * 3);
		firingtransform2.Translate (Vector3.left * 3);
	}

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
		if (Input.GetKey ("Fire1")) {
			if (lastFire < Time.realtimeSinceStartup + FiringDelay) {
				FireProjectile ();
			}
		}
    }
    
    bool FireProjectile() {
        Rigidbody Projectile1;
		Rigidbody Projectile2;
		Projectile1 = Instantiate (ProjectileType, firingtransform1) as Rigidbody;
		Projectile2 = Instantiate (ProjectileType, firingtransform2) as Rigidbody;
		Projectile1.AddRelativeForce (Vector3.forward * firingForce);
		Projectile2.AddRelativeForce (Vector3.forward * firingForce);
		lastFire = Time.realtimeSinceStartup;
		return true;

    }
}
