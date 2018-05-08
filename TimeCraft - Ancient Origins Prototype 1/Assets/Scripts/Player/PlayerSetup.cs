using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

	public float deathTime;
	CharacterController m_CharacterController;
	PlayerAnimation m_PlayerAnimation;
	PlayerMovement m_PlayerMovement;
	PlayerHealth m_PlayerHealth;
	Animator m_Animator;
	Collider m_Collider;

	// Use this for initialization
	void Start () {
		m_CharacterController = GetComponent<CharacterController> ();
		m_PlayerAnimation = GetComponent<PlayerAnimation> ();
		m_PlayerMovement = GetComponent<PlayerMovement> ();
		m_PlayerHealth = GetComponent<PlayerHealth> ();
		m_Animator = GetComponent<Animator>();
		m_Collider = GetComponent<Collider> ();
		m_PlayerHealth.deathTime = deathTime;
		ActivatePlayer ();

	}



	public void ActivatePlayer(){
		m_CharacterController.enabled = true;
		m_PlayerAnimation.enabled = true;
		m_PlayerMovement.enabled = true;
		m_Animator.enabled = true;
		m_Collider.enabled = true;
		ToggleRagdoll (false);

	}

	public void DeactivatePlayer(){
		m_CharacterController.enabled = false;
		m_PlayerAnimation.enabled = false;
		m_PlayerMovement.enabled = false;
		m_Animator.enabled = false;
		m_Collider.enabled = false;
		ToggleRagdoll (true);
		DeactivateNumerator ();
	}

	IEnumerator DeactivateNumerator(){
		
		yield return new WaitForSeconds (deathTime);
		gameObject.SetActive (false);
	
	}


	void ToggleRagdoll(bool toggle){

		foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>()) {

			if (rb.transform.parent == null) {
				rb.isKinematic = toggle;
				rb.useGravity = !toggle;
				rb.GetComponent<Collider> ().isTrigger = toggle;
			} else {
				rb.isKinematic = !toggle;
				rb.useGravity = toggle;
				rb.GetComponent<Collider> ().isTrigger = !toggle;
			}
		}
	}

}
