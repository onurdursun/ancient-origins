using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AISetup : MonoBehaviour {

	public float deathTime;
	Collider m_Collider;
	NavMeshAgent m_NavMeshAgent;
	AIAnimations m_AIAnimations;
	AIMovement m_AIMovement;
	AIHealth m_AIHealth;
	Animator m_Animator;

	// Use this for initialization
	void Start () {
		m_Collider = GetComponent<Collider> ();
		m_NavMeshAgent = GetComponent<NavMeshAgent> ();
		m_AIAnimations = GetComponent<AIAnimations> ();
		m_AIMovement = GetComponent<AIMovement> ();
		m_AIHealth = GetComponent<AIHealth> ();
		m_Animator = GetComponent<Animator> ();
		m_AIHealth.deathTime = deathTime;
		ActivateAI ();
	}



	public void ActivateAI(){
		m_Collider.enabled = true;
		m_NavMeshAgent.enabled = true;
		m_AIAnimations.enabled = true;
		m_AIMovement.enabled = true;
		//m_Animator.enabled = true;
		//ToggleRagdoll (false);
	}

	public void DeactivateAI(){
		m_Collider.enabled = false;
		m_NavMeshAgent.enabled = false;
		m_AIAnimations.enabled = false;
		m_AIMovement.enabled = false;
		//m_Animator.enabled = false;
		ToggleRagdoll (true);
		StartCoroutine (DeactivateNumerator ());
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
