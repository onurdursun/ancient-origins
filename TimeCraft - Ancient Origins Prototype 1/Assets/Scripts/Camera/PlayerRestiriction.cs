using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRestiriction : MonoBehaviour {


	public Vector3 desiredPosition;


	SphereCollider coll;
	// Use this for initialization
	void Start () {
		coll = GetComponent<SphereCollider> ();	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = desiredPosition;
	
	}
}
