using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {

	public float deathTime;
	public BoolVariable isPlayerAlive;

	CharacterController m_CharacterController;
	PlayerController m_PlayerController;
	PlayerStates m_PlayerStates;
	Animator m_Animator;
	Collider m_Collider;
	public int playerId;


	// Use this for initialization
	void Start () {
		m_CharacterController = GetComponent<CharacterController> ();
		m_PlayerController = GetComponent<PlayerController> ();
		m_Animator = GetComponent<Animator>();
		m_Collider = GetComponent<Collider> ();
		m_PlayerStates = GetComponent<PlayerStates> ();
		ActivatePlayer ();

	}



	public void ActivatePlayer(){
		m_CharacterController.enabled = true;
		m_PlayerController.enabled = true;
		m_Animator.enabled = true;
		m_Collider.enabled = true;
		ToggleRagdoll (false);
		isPlayerAlive.SetValue (true);

	}

	public void DeactivatePlayer(){
		m_CharacterController.enabled = false;
		m_PlayerController.enabled = false;
		m_Animator.enabled = false;
		m_Collider.enabled = false;
		ToggleRagdoll (true);
		DeactivateNumerator ();
		isPlayerAlive.SetValue (false);
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
