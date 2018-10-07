using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobTest : MonoBehaviour {
    private Mob Mob1;

	// Use this for initialization
	void Start () {
        Mob1 = gameObject.GetComponent<Mob>();
	}
	
	// Update is called once per frame
	void Update () {
        Mob1.relativePlayerSpeed();

        Mob1.setForwardPower(.5f);
	}
}
