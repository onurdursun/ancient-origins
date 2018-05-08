using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health{


	public bool m_IsAlive;
	// Use this for initialization
	void Awake(){

		healthPoints = m_StartingHP;
		//m_Rigidbody = GetComponent<Rigidbody> ();
	}

	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {

		m_CurrentHP = healthPoints;
		m_IsAlive = isAlive ();
		if (!isAlive ())
			Death ();
	}
}
