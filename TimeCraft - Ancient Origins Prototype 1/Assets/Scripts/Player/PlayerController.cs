using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Rewired;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;



public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public int playerId;

	public UnityEvent WeaponAttackEvent, WeaponStopEvent;

	MeleeWeapon meleeWeapon;
	Animator m_Animator;
	CharacterController cController;
	PlayerStates m_PlayerStates;
	Player player;

	public Ability[] abilityArray;
	int abilityNumber = 0;

	float layerWeight = 1f;
	public BoolVariable isAttacking;
	public BoolVariable IsInputRestricted;

	public Transform hips;

	[Space]
	//Animation
	[Header("Animation")]
	public UnityEvent EquipEvent;

	float m_HorizontalInput;
	float m_VerticalInput;

	bool m_Attack;
	readonly int m_HashStateTime = Animator.StringToHash("StateTime");
	readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");

	WaitForSeconds m_AttackInputWait;
	Coroutine m_AttackWaitCoroutine;

	Coroutine updateLayerWeight;

	[Space]
	//Movement
	[Header("Movement")]

	public FloatVariable m_SpeedFactor;
	public FloatVariable m_CurrentSpeed;
	public FloatReference m_RunSpeed; 
	public FloatReference m_SprintSpeed;
	public FloatReference m_JumpHeight;// How fast the tank moves forward and back.
	public FloatReference m_TurnSpeed;              // How fast the tank turns in degrees per second.

	private float refSpeed;
	float timer = 0f;

	void Awake(){
		meleeWeapon = GetComponentInChildren<MeleeWeapon> ();
		m_Animator = GetComponent<Animator> ();
		cController = GetComponent<CharacterController> ();
		m_PlayerStates = GetComponent<PlayerStates> ();

		player = ReInput.players.GetPlayer (playerId);

		m_AttackInputWait = new WaitForSeconds(0.03f);
		meleeWeapon.SetOwner (gameObject);

		IsInputRestricted.SetValue (false);
		isAttacking.SetValue (false);
		player = ReInput.players.GetPlayer(playerId);
		m_SpeedFactor.SetValue (m_RunSpeed.Value);
		refSpeed = m_SpeedFactor.Value;
	}
	// Use this for initialization
	void Start () {
		for (int i = 0; i < abilityArray.Length; i++) {
			abilityArray [i].Initialize (meleeWeapon.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {


		// ---- Animation ---- //

		m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(4).normalizedTime, 1f));
		m_Animator.ResetTrigger(m_HashMeleeAttack);

		if (m_Attack)
			m_Animator.SetTrigger(m_HashMeleeAttack);

		//if (m_Animator.GetCurrentAnimatorStateInfo (1).IsName ("Idle"))
		//	MeleeWeapon.isHitShield = false;

		//if (!isLocalPlayer)
		//	return;

		m_Animator.SetFloat ("Speed", m_CurrentSpeed.Value);


		Combat ();

		if (!IsInputRestricted.Value) {
			m_HorizontalInput = player.GetAxis ("Horizontal");
			m_VerticalInput = player.GetAxis ("Vertical");



			m_Animator.SetFloat ("Horizontal", m_HorizontalInput * Mathf.Sin ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y + 90f) * Mathf.Deg2Rad) + m_VerticalInput * Mathf.Cos ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y + 90f) * Mathf.Deg2Rad));
			m_Animator.SetFloat ("Vertical", m_VerticalInput * Mathf.Sin ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y + 90f) * Mathf.Deg2Rad) - m_HorizontalInput * Mathf.Cos ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y + 90f) * Mathf.Deg2Rad));
		}



		//print (m_Animator.GetCurrentAnimatorStateInfo (0).shortNameHash);


		if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING)
			m_Animator.SetLayerWeight (3, 1f);
		else
			m_Animator.SetLayerWeight (3, 0f);


		/*	else
			animator.SetBool ("Defence", false);*/

		//print(animator.GetFloat("Horizontal"));
		//print(animator.GetFloat("Vertical"));

	}

	private void FixedUpdate() {

		// ---- Movement ---- //

		Gravity ();
		Jump ();


		if (isAttacking.Value) {
			m_CurrentSpeed.SetValue (0f);

			//transform.position = hips.position;
			return;
		}

		Turn ();
		Move ();


	}



	// ---- Controller ----//



	// This is called by an animation event.
	public void MeleeAttackStart(int throwing = 0) {
		meleeWeapon.BeginAttack(throwing != 0);
		WeaponAttackEvent.Invoke ();
	}

	// This is called by an animation event.
	public void MeleeAttackEnd() {
		WeaponStopEvent.Invoke ();
		meleeWeapon.EndAttack();
	}


	public void AbilityStart(){
		//abilityArray[abilityNumber].TriggerAbility ();
		ActivateAttackingState (6);
		StartCoroutine (UpdatePosition ());
	}

	public void AbilityEvent (){
		//abilityArray[abilityNumber].Event ();
	}

	public void AbilityEnd(){
		DeactivateAttackingState (6);
		m_Animator.ResetTrigger ("AbilityStart");
		//abilityArray[abilityNumber].EndAbility ();
	}

	void ActivateAttackingState(int layerMask) {
		print ("Attacking State Activated");
		if(updateLayerWeight != null)
			StopCoroutine (updateLayerWeight);
		layerWeight = 1f;
		m_Animator.SetLayerWeight (layerMask,layerWeight);
		isAttacking.SetValue (true);
	}

	void DeactivateAttackingState(int layerMask) {
		print ("Attacking State Deactivated");
		isAttacking.SetValue (false);
		updateLayerWeight = StartCoroutine (UpdateLayerWeight (layerMask));
	}

	IEnumerator UpdateLayerWeight(int layerMask) {
		while (layerWeight >= 0.2f) {
			Mathf.SmoothDamp (1, 0f, ref layerWeight, 1f);
			m_Animator.SetLayerWeight (layerMask, layerWeight);
			yield return null;
		}
		print ("CoroutineStopped");
		if(updateLayerWeight != null)
			StopCoroutine (updateLayerWeight);
	}

	IEnumerator UpdatePosition() {
		Vector3 hipsPos = hips.transform.localPosition;
		while(isAttacking.Value)
		{
			print ("UpdatingPosition");
			//cController.Move(targetPosition*speed*Time.deltaTime);
			cController.Move ((transform.forward * m_Animator.GetFloat ("zPos") + transform.right * m_Animator.GetFloat ("xPos") +  transform.up * m_Animator.GetFloat ("yPos"))*Time.deltaTime);
			hips.transform.localPosition = hipsPos;
			yield return null;
		}
		cController.Move (Vector3.one*Time.deltaTime);
		IsInputRestricted.SetValue (false);
		refSpeed = m_RunSpeed.Value;
		m_SpeedFactor.SetValue (refSpeed);
	}


	// ---- Animation ---- //

		
	void Combat(){
		if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING)
			return;

		if(player.GetButtonUp ("AbilityTrigger")&&!isAttacking.Value){
			m_Animator.ResetTrigger ("AbilityStart");
			m_Animator.SetTrigger ("AbilityStart");

		
		} else if (player.GetButton ("AbilityTrigger")) {
			if (m_CurrentSpeed.Value <= 0) {
				refSpeed = 0;
				m_SpeedFactor.SetValue (refSpeed);
				return;
			}
			Mathf.SmoothDamp (m_CurrentSpeed.Value, 0, ref refSpeed, 2f);
			m_SpeedFactor.SetValue (refSpeed);

			if (IsInputRestricted.Value)
				return;
			IsInputRestricted.SetValue (true);
		}



		if (player.GetButton ("Attack1") || player.GetAxis ("VerticalTurn") != 0f || player.GetAxis ("HorizontalTurn") != 0f) {
			if (IsInputRestricted.Value)
				return;
			if (m_AttackWaitCoroutine != null)
				StopCoroutine(m_AttackWaitCoroutine);

			m_AttackWaitCoroutine = StartCoroutine(AttackWait());

				//print ("TimeHolder: " + (Time.time-timeHolder));
		}
			//print ("Time - holder: " + (Time.time - timeHolder));

	}

	IEnumerator AttackWait() {
		m_Attack = true;

		yield return m_AttackInputWait;

		m_Attack = false;
	}

	public void EquipToggle() {
		EquipEvent.Invoke ();
	}


	// ---- Movement ---- // 

	private void Move()
	{

		//print ("raw: " + player.GetAxis2DRaw ("Horizontal", "Vertical"));
		if (!cController.isGrounded)
			return;

		if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING) {
			Mathf.SmoothDamp (m_RunSpeed.Value, 20, ref refSpeed, 0.5f, m_SprintSpeed.Value);
			m_SpeedFactor.SetValue (refSpeed);


		} else {
			if (m_SpeedFactor.Value > m_RunSpeed.Value) {
				Mathf.SmoothDamp (m_SprintSpeed.Value, m_RunSpeed.Value, ref refSpeed, 2f);
				m_SpeedFactor.SetValue (refSpeed);
			}

		}


		Vector3 verticalMovement = Camera.main.transform.forward * player.GetAxis2DRaw ("Horizontal", "Vertical").y;
		Vector3 horizontalMovement = Camera.main.transform.right * player.GetAxis2DRaw ("Horizontal", "Vertical").x;
		Vector3 movementVector = verticalMovement + horizontalMovement;
		if (movementVector.magnitude > 1f || m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING)
			movementVector = movementVector.normalized;
		Vector3 velocityVector = movementVector * m_SpeedFactor.Value;
		m_CurrentSpeed.SetValue (velocityVector.magnitude);

		if (m_CurrentSpeed.Value < 2f) {
			m_CurrentSpeed.SetValue (0f);
			return;
		}



		cController.Move (velocityVector*Time.deltaTime);


		/*if (Input.GetAxis ("Vertical") != 0 || Input.GetAxis ("Horizontal") != 0) {
			if(!m_MovementAudio.isPlaying)
				m_MovementAudio.Play ();
		} else
			m_MovementAudio.Stop ();
		*/

	}

	private void Turn()
	{
		//print (timer);

		if (IsInputRestricted.Value) {
			if (player.GetAxis2DRaw ("Horizontal", "Vertical").magnitude == 0f)
				return;
			Quaternion rotation1 = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis2DRaw ("Horizontal", "Vertical").x, player.GetAxis2DRaw ("Horizontal", "Vertical").y)) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 4 * Time.deltaTime);
			transform.rotation = rotation1;
		} else {

			if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING) {
				Quaternion rotation1 = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis2DRaw ("Horizontal", "Vertical").x, player.GetAxis2DRaw ("Horizontal", "Vertical").y)) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 4 * Time.deltaTime);
				transform.rotation = rotation1;
			} else {


				if (player.GetAxis ("Horizontal") != 0f || player.GetAxis ("Vertical") != 0f)
					timer += Time.deltaTime;
				else
					timer -= Time.deltaTime;

				timer = Mathf.Clamp (timer, 0f, 0.4f);


				if (player.GetAxis ("VerticalTurn") == 0 && player.GetAxis ("HorizontalTurn") == 0) {


					if (timer > 0.2f && (player.GetAxis ("Horizontal") != 0f || player.GetAxis ("Vertical") != 0f)) {
						Quaternion rotation1 = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis2DRaw ("Horizontal", "Vertical").x, player.GetAxis2DRaw ("Horizontal", "Vertical").y)) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 4 * Time.deltaTime);
						transform.rotation = rotation1;
						//print ("rotatewithleftstick");
						return;
					} else if (player.GetAxis ("Horizontal") == 0f && player.GetAxis ("Vertical") == 0f) {
						timer = 0f;
						return;
					} else
						return;
				} else {
					timer = 0f;
					Quaternion rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis2DRaw ("HorizontalTurn", "VerticalTurn").x, player.GetAxis2DRaw ("HorizontalTurn", "VerticalTurn").y)) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 15 * Time.deltaTime);
					transform.rotation = rotation;
				}
			}
		}
	}

	void Gravity(){
		if(!cController.isGrounded)
			cController.Move (Physics.gravity*Time.deltaTime);
	}

	void Jump(){
		if (player.GetButtonDown("Jump") && cController.isGrounded)
			cController.Move (new Vector3(0,Mathf.Sqrt(m_JumpHeight.Value * -2f * Physics.gravity.y),0) * Time.deltaTime);
	}

}


