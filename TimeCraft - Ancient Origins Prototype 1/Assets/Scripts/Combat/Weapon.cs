using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	protected int damage;
	protected float reloadTime;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected void DealDamage(GameObject go, Vector3 direction){
		go.GetComponent<Health>().Damage (damage,direction);
		//print ("I'm dealing damage to: " + go.name);
	}
}
