using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Health : MonoBehaviour {

	protected int healthPoints;
	public int m_StartingHP;
	public int m_CurrentHP;

	protected Rigidbody m_Rigidbody;
	protected Animator m_Animator;

	public float deathTime;
	bool deadOnce = false;
	public Material deathMaterial;
	Color currentColor;

	public float damageEffectFactor;


	// Use this for initialization
	
	// Update is called once per frame

	public void Damage(int amount, Vector3 direction){
		//print ("I receive damage" + gameObject.name);

		healthPoints -= amount;


		if (gameObject.CompareTag ("Player")) {
			//GetComponent<PlayerSetup>().DeactivatePlayer ();

		}else if(gameObject.CompareTag ("Enemy")){
			GetComponent<AIAnimations>().DamageEffect ();

			//m_Rigidbody.AddForce (direction*amount*damageEffectFactor);
		}
	}
	/*
	void DamageEffect(){
		
	}
*/
	protected bool isAlive(){
		if (healthPoints <= 0)
			return false;
		else
			return true;
	}

	protected void Death(){

		if (!deadOnce) {
			GetComponentInChildren<SkinnedMeshRenderer> ().material = deathMaterial;
			currentColor = GetComponentInChildren<SkinnedMeshRenderer> ().material.color;

			if (gameObject.CompareTag ("Player")) {
				GetComponent<PlayerSetup>().DeactivatePlayer ();
			}else if(gameObject.CompareTag ("Enemy")){
				GetComponent<AISetup> ().DeactivateAI ();
			}


			if (Extensions.TransformExtensions.FindWeaponWithTag (transform))
				Extensions.TransformExtensions.FindWeaponWithTag (transform).parent = null;
			
		}else{
			//print ("Color.a: " + currentColor.a);

			if (currentColor.a <= 0)
				return;
			currentColor.a -= Time.deltaTime / deathTime;
			GetComponentInChildren<SkinnedMeshRenderer> ().material.color = currentColor;
			//print ("Color Alpha: " + currentColor.a);
			return;
		}

		deadOnce = true;

		
	}

	void Update(){
		m_CurrentHP = healthPoints;
	}



}
