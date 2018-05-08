using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIShooting : MonoBehaviour {

	public int damage;
	public float shootingDistance;
	public bool canShoot;
	public Transform arrowContainer;
	public Transform m_ShootPoint;
	//public float launchForce;
	public float reloadTime;
	Arrow[] Arrows;
	int arrowNumber;

	AI ai;
	// Use this for initialization
	void Start () {
		canShoot = true;
		arrowNumber = 0;
		Arrows = arrowContainer.GetComponentsInChildren <Arrow> ();
		ai = GetComponent<AI> ();
	}
	
	// Update is called once per frame


	public void Shoot(){
		
		canShoot = false;
		Arrows [arrowNumber].gameObject.SetActive (true);
		Arrows [arrowNumber].transform.SetParent (null);
		Arrows [arrowNumber].transform.position = m_ShootPoint.position;
		Arrows [arrowNumber].transform.rotation = m_ShootPoint.rotation;

		float launchForce = Mathf.Sqrt((Vector3.Distance (transform.position, ai.target.position) * -Physics.gravity.y) / (Mathf.Sin (15 * Mathf.Deg2Rad) * Mathf.Cos (15 * Mathf.Deg2Rad) * 2));
		//print ("LaunchForce: " + launchForce);

		// Create a velocity that is the tank's velocity and the launch force in the fire position's forward direction.
		Vector3 velocity = GetComponent<NavMeshAgent>().velocity + launchForce * -m_ShootPoint.right;

		// Set the shell's velocity to this velocity.
		Arrows [arrowNumber].GetComponent<Rigidbody>().velocity = velocity;
		arrowNumber++;
		if (arrowNumber == Arrows.Length)
			arrowNumber = 0;
		StartCoroutine (Reload ());
	}

	IEnumerator Reload(){
		yield return new WaitForSeconds (reloadTime);
		canShoot = true;
	}


}
