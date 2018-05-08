using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerStates : MonoBehaviour {


	public enum EMoveState
	{
		WALKING,
		RUNNING,
		SPRINTING,



	}
	public enum EWeaponState
	{
		IDLE,
		ATTACKING,
		DEFENCING,
		IMPACT,
		COMBO,


	}
	public enum EDrivingState{
		DRIVING,
		IDLE,
	}

	public enum ELifeState{
		ALIVE,
		DEAD,
	}

	public EMoveState MoveState;
	public EWeaponState WeaponState;
	public EDrivingState DrivingState;
	public ELifeState LifeState;

	public static bool onShieldHit;
	public float Vertical;
	public float Horizontal;
	public Vector2 MouseInput;
	public bool Fire;
	public bool Defence;
	public bool Reload;
	public bool isWalking;
	public bool isRunning;
	public bool isSprinting;
	public bool isDriver;

	public int playerId;
	private Player player;


	public Transform driveTransform;

	Animator m_Animator;
	Class m_PlayerClass;
	PlayerHealth m_PlayerHealth;

	float timer;
	public float timeBetweenSwitch = 1f;

	void Awake(){
		m_PlayerClass = GetComponent<Class> ();
		m_Animator = GetComponent <Animator> ();
		m_PlayerHealth = GetComponent<PlayerHealth> ();
		player = ReInput.players.GetPlayer(playerId);
	}

	void InputControl()
	{

		Fire = m_Animator.GetCurrentAnimatorStateInfo (2).IsName ("Attack");
		Defence = m_Animator.GetCurrentAnimatorStateInfo (2).IsName ("Defence");

		Vertical = Input.GetAxis ("Vertical");
		Horizontal = Input.GetAxis ("Horizontal");

			

		//	Fire = Input.GetButtonDown ("Fire1");
		//	Defence = Input.GetButton ("Fire2");

		isWalking = player.GetAxis ("Walk") != 0f;
		isSprinting = player.GetAxis ("Sprint") != 0f;

		//	MouseWheelUp=Input.GetAxis("Mouse ScrollWheel")>0;
		//	MouseWheelDown = Input.GetAxis ("Mouse ScrollWheel")<0;

	


	
	}


	void Update()
	{	

	//	print ("trigger  : " + trigger);

	
		InputControl ();
	//	SetWeaponState ();
		SetMoveState ();
		SetLifeState ();


//		print (Fire);
		timer += Time.deltaTime;
		//print ("Timer : " + timer);

	}
	void SetWeaponState(EWeaponState weaponState)
	{
		WeaponState = weaponState;
	}
	void SetMoveState()
	{
		MoveState = EMoveState.RUNNING;


		if (isWalking)
			MoveState = EMoveState.WALKING;
		else if (isSprinting)
			MoveState = EMoveState.SPRINTING;

			
			
			

	
}

	void SetLifeState(){
		if (m_PlayerHealth.m_IsAlive)
			LifeState = ELifeState.ALIVE;
		else
			LifeState = ELifeState.DEAD;

	}
	/*void SetDrivingState(EDrivingState _DrivingState){
		if (m_PlayerClass.EnumPlayerClass != PlayerClass.EPlayerClass.ENGINEER)
			return;
		DrivingState = _DrivingState;
	}


	void OnTriggerStay(Collider other){

		if (!isLocalPlayer)
			return;


		if (m_PlayerClass.EnumPlayerClass != PlayerClass.EPlayerClass.ENGINEER)
			return;

		if (other.tag != "Catapult")
			return;
	
		this.GetComponent<Rigidbody> ().useGravity = trigger;
		this.GetComponent<CapsuleCollider> ().isTrigger = !trigger;
		other.GetComponentInParent<CatapultStates> ().SetColliderTrigger (trigger);

		if (!isLocalPlayer)
			return;
		
		//other.GetComponentInParent<NetworkIdentity> ().AssignClientAuthority (this.GetComponent<NetworkIdentity> ().connectionToClient);

		

			if (DrivingState == EDrivingState.IDLE && other.GetComponentInParent<CatapultStates>().CatapultState == CatapultStates.ECatapultState.IDLE && other.GetComponentInParent<CatapultStates> ().driverId == 50000) {
					if (Input.GetKeyDown (KeyCode.E)) {
						if (timer > timeBetweenSwitch) {
							timer = 0f;
							SetDrivingState (EDrivingState.DRIVING);
							driveTransform = other.transform.GetChild (0);
							other.GetComponentInParent<CatapultStates>().SetCatapultStates (this.GetComponent<NetworkIdentity>().playerControllerId,CatapultStates.ECatapultState.USING);
							SetTrigger (false);
						}
					}

			} else if(other.GetComponentInParent<CatapultStates> ().driverId == this.GetComponent<NetworkIdentity> ().playerControllerId) {
					if (Input.GetKeyDown (KeyCode.E)) {
						if (timer > timeBetweenSwitch) {
							timer = 0f;
							SetDrivingState (EDrivingState.IDLE);
							other.GetComponentInParent<CatapultStates>().SetCatapultStates (50000,CatapultStates.ECatapultState.IDLE);
							SetTrigger (true);
					}
				}
			}
		}


	void SetTrigger(bool _trigger){
		trigger = _trigger;
	}

	void OnTriggerExit(Collider other){
		if (other.tag != "Catapult")
			return;
		//other.GetComponentInParent<NetworkIdentity> ().RemoveClientAuthority (this.GetComponent<NetworkIdentity> ().connectionToClient);
	}




*/

}
