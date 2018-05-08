using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIRadar : MonoBehaviour {

	public AI AIParentBehaviour;
	public LayerMask layerMask;
	//public GameObject lastSeenObj;
	Vector3 lastSeenPosition;
	public Transform eyes;

	void Start(){
		AIParentBehaviour = GetComponentInParent<AI> ();

	//	layerMask = LayerMask.GetMask ("Player");
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag ("Player")){
			AIParentBehaviour.target = other.transform;
		}
	}


	void OnTriggerExit(Collider other){

		if(other.transform == AIParentBehaviour.potentialTarget){
			AIParentBehaviour.target = null;
		}
	}

	void Update(){
		//Raycasting ();
	}

	void Raycasting ()
	{

		/*

		if (!AIParentBehaviour.potentialTarget)
			return;
		AIParentBehaviour.target = AIParentBehaviour.potentialTarget.position;
		RaycastHit hit;
	
	// Does the ray intersect any objects excluding the player layer
		if (Physics.Raycast(eyes.position,AIParentBehaviour.potentialTarget.position - eyes.parent.position, out hit, Mathf.Infinity,layerMask,QueryTriggerInteraction.Ignore))
		{
			Debug.DrawRay(eyes.position, AIParentBehaviour.potentialTarget.position - eyes.parent.position, Color.blue);
			Debug.Log("Did Hit");
			if (hit.collider.CompareTag ("Player")) {
				AIParentBehaviour.target = AIParentBehaviour.potentialTarget.position;
				lastSeenPosition = AIParentBehaviour.target;
			}
			else{
				AIParentBehaviour.target = lastSeenPosition;
			}
		}
		else
		{
			AIParentBehaviour.target = lastSeenPosition;
			Debug.DrawRay(eyes.position, AIParentBehaviour.potentialTarget.position - eyes.parent.position , Color.red);
			Debug.Log("Did not Hit");
		}
	}*/
	}
}
