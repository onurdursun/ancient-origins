using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMeleeWeapon : Weapon {

	public int m_Damage;
	PlayerStates m_PlayerStates;
	// Use this for initialization
	void Start () {
		//damage = m_Damage;
		m_PlayerStates = GetComponentInParent<PlayerStates>();
	}
	protected bool isAttacking(){
		if (m_PlayerStates.WeaponState == PlayerStates.EWeaponState.ATTACKING)
			return true;
		else
			return false;
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//print ("Other Gameobject: " + other.name);
		if((other.CompareTag ("Enemy")) && other.GetComponent<Health>() && isAttacking ()){
			//print ("I'm Attacking: " + other.name);
			Vector3 direction = transform.position - other.transform.position;
			direction = direction.normalized;
			//DealDamage (other.gameObject,direction);

			// We then get the opposite (-Vector3) and normalize it
			direction = -direction.normalized;
		}
	}
}
