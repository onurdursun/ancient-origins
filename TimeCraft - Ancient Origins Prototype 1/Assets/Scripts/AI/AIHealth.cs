using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : Health {

	public bool m_IsAlive;
	// Use this for initialization
	void Start () {
		m_Rigidbody = GetComponent<Rigidbody> ();
		//m_Rigidbody.isKinematic = true;
		healthPoints = m_StartingHP;
	}
	
	// Update is called once per frame
	void Update () {
			


		m_IsAlive = isAlive ();
		if (!isAlive ())
			Death ();
	}
}
