using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMeleeWeapon : Weapon {

	public int m_Damage;
	public float m_ReloadTime;

	AIBehaviours m_AIBehaviours;
	AIAnimations m_AIAnimations;
	// Use this for initialization
	void Start () {
		//damage = m_Damage;
		//reloadTime = m_ReloadTime;
		m_AIBehaviours = GetComponentInParent<AIBehaviours>();
		m_AIAnimations = GetComponentInParent<AIAnimations> ();
	}

	protected bool isAttacking{
		
		get { 
			if (m_AIBehaviours.WeaponState == AIBehaviours.EWeaponState.ATTACKING) {
				//StartCoroutine (m_AIAnimations.Reload (reloadTime));
				return true;
			} else
				return false;
		}
		set {
			return;
		}    
	}
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other){
		//print ("Other Gameobject: " + other.name);
		if(other.CompareTag ("Player") && other.GetComponent<Health>() && isAttacking) {
			print ("I'm Attacking: " + other.name);
			Vector3 direction = transform.position - other.transform.position;
			direction = direction.normalized;
			//DealDamage (other.gameObject,direction);

			// We then get the opposite (-Vector3) and normalize it
			direction = -direction.normalized;
		}
	}
}
