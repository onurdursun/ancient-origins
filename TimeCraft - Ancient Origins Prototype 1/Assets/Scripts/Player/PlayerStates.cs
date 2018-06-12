using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Rewired;
using System.Xml.Schema;



public class PlayerStates : MonoBehaviour {



	public enum ERunState
	{
		RUNNING,
		SPRINTING,

	}

	public enum EWeaponState
	{
		IDLE,
		ATTACKING,
		DEFENCING,
	}


	public enum ELifeState{
		ALIVE,
		DEAD,
	}

	public ERunState RunState;
	public EWeaponState WeaponState;
	public ELifeState LifeState;


	public FloatVariable playerCurrentSpeed;


	public BoolVariable isAlive;
	public bool isMoving { get{ return !Mathf.Approximately (playerCurrentSpeed.Value, 0);
		} private set{}
	}
	public bool isSprinting{ get{ return !Mathf.Approximately (player.GetAxis ("Sprint"), 0);
		} private set{}
	}

	[HideInInspector]
	public int playerId;
	private Player player;

	void Awake() {
	
		player = ReInput.players.GetPlayer(playerId);
	}



	void Update()
	{	
		SetMoveState ();
		SetLifeState ();

	}
	public void SetWeaponState(int value) {
		if (value == 0)
			WeaponState = EWeaponState.IDLE;
		else if (value == 1)
			WeaponState = EWeaponState.ATTACKING;
		else
			WeaponState = EWeaponState.DEFENCING;
	}
	void SetMoveState() {
		RunState = isSprinting && isMoving ? ERunState.SPRINTING : ERunState.RUNNING;
}

	void SetLifeState() {
		LifeState = isAlive.Value ? ELifeState.ALIVE : ELifeState.DEAD;

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
