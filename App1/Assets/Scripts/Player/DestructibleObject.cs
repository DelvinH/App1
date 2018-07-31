using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {
	public float health = 500;
	public float maxHealth = 500;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float getHealth(){
		return health;
	}

	public void takeDamage(amount){
		if(amount <= 0){
			return;
		}
		health = Math.Max(0, health - amount);
		if(health == 0){
			objectBreak();
		}
	}

	public void objectBreak(){

	}

	public void healDamage(amount){
		if(amount <= 0){
			return;
		}
		health = Math.Min(maxHealth, health + amount);
	}

}
