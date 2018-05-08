using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviours : MonoBehaviour {



	public enum EMoveState {
		IDLE,
		WALKING,
		RUNNING,
		SPRINTING,

	}

	public enum EWeaponState {
		IDLE,
		ATTACKING,
		DEFENCING,
		IMPACT,
		COMBO,
	}

	public enum ELifeState {
		ALIVE,
		DEAD,
	}

	public EMoveState MoveState;
	public EWeaponState WeaponState;
	public ELifeState LifeState;



	public float walkSpeed;
	public float runSpeed;
	public float sprintSpeed;


	Animator m_Animator;
	AIHealth m_AIHealth;
	AIMovement m_AIMovement;

	void Awake(){
		// Class

		m_AIHealth = GetComponent <AIHealth> ();
		m_Animator = GetComponent<Animator> ();
		m_AIMovement = GetComponent<AIMovement> ();

	}

	void Start(){
		AdjustSpeed ();
	}
		


	void Update() {	
		
		SetLifeState ();

		//print ("Timer : " + timer);

	}
	void SetWeaponState(EWeaponState weaponState) {
		WeaponState = weaponState;
	}
	public void SetMoveState(EMoveState moveState) {
		MoveState = moveState;
		AdjustSpeed ();
	
	}

	void SetLifeState() {
		
		if (m_AIHealth.m_IsAlive)
			LifeState = ELifeState.ALIVE;
		else
			LifeState = ELifeState.DEAD;

	}

	void AdjustSpeed(){

		m_Animator.SetBool ("Walking",false);
		m_Animator.SetBool ("Sprinting",false);

		if (MoveState == EMoveState.IDLE) {
			m_AIMovement.AdjustSpeed (0);
		} else if (MoveState == EMoveState.WALKING) {
			m_Animator.SetBool ("Walking", true);
			m_AIMovement.AdjustSpeed (walkSpeed);
		} else if (MoveState == EMoveState.RUNNING)
			m_AIMovement.AdjustSpeed (runSpeed);
		else {
			m_AIMovement.AdjustSpeed (sprintSpeed);
			m_Animator.SetBool ("Sprinting",true);
		}
	}
}
