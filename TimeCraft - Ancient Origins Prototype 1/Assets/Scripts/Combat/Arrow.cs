using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

	int damage;
	// Use this for initialization
	void Start () {
		damage = GetComponentInParent<AIShooting> ().damage;

	}

	void OnEnable(){
		StartCoroutine (SetActivate ());
	}
	// Update is called once per frame
	void FixedUpdate(){
		transform.RotateAround (transform.position,transform.forward,.5f);
	}

	void OnTriggerEnter(Collider other){
		print ("Other Name: " +other.name);
		if(other.CompareTag ("Player"))	{
			other.GetComponent<Health>().Damage (damage,-transform.forward);
		}
		gameObject.SetActive (false);
	}

	IEnumerator SetActivate(){
		yield return new WaitForSeconds (6f);
		gameObject.SetActive (false);
	}


}
