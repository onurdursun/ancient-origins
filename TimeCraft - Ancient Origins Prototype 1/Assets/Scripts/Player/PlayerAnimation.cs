using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.Events;



public class PlayerAnimation : MonoBehaviour {

	Animator m_Animator;
	PlayerStates m_PlayerStates;
	PlayerController m_PlayerController;

	public int playerId;

	public UnityEvent EquipEvent;

	float m_HorizontalInput;
	float m_VerticalInput;

	Player player;
	CharacterController cController;
	public FloatVariable m_Speed;



	bool m_Attack;
	readonly int m_HashStateTime = Animator.StringToHash("StateTime");
	readonly int m_HashMeleeAttack = Animator.StringToHash("MeleeAttack");



	WaitForSeconds m_AttackInputWait;
	Coroutine m_AttackWaitCoroutine;
	// Use this for initialization
	void Awake () {

		m_AttackInputWait = new WaitForSeconds(0.03f);
		player = ReInput.players.GetPlayer (playerId);
		m_Animator = GetComponent<Animator> ();
		m_PlayerStates = GetComponent<PlayerStates> ();
		cController = GetComponent<CharacterController> ();
		m_PlayerController = GetComponent <PlayerController> ();
	}


	// Update is called once per frame
	void Update () {



		//if (m_Animator.GetCurrentAnimatorStateInfo (1).IsName ("Idle"))
		//	MeleeWeapon.isHitShield = false;
		
		//if (!isLocalPlayer)
		//	return;
		m_HorizontalInput = player.GetAxis ("Horizontal");
		m_VerticalInput = player.GetAxis ("Vertical");

		m_Animator.SetFloat ("Speed", m_Speed.Value);


		Combat ();


		m_Animator.SetFloat("Horizontal", m_HorizontalInput * Mathf.Sin ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y +90f)*Mathf.Deg2Rad) + m_VerticalInput * Mathf.Cos ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y+90f)*Mathf.Deg2Rad));
		m_Animator.SetFloat("Vertical", m_VerticalInput * Mathf.Sin ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y+90f)*Mathf.Deg2Rad) - m_HorizontalInput * Mathf.Cos ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y+90f)*Mathf.Deg2Rad));
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

	void FixedUpdate(){
		m_Animator.SetFloat(m_HashStateTime, Mathf.Repeat(m_Animator.GetCurrentAnimatorStateInfo(4).normalizedTime, 1f));
		m_Animator.ResetTrigger(m_HashMeleeAttack);

		if (m_Attack)
			m_Animator.SetTrigger(m_HashMeleeAttack);
	}



	void Combat(){



		if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING)
			return;

		if(player.GetButton ("AbilityTrigger")&&!m_PlayerController.isAttacking.Value){
			m_Animator.ResetTrigger ("AbilityStart");
			m_Animator.SetTrigger ("AbilityStart");
		}


		if (player.GetButton ("Attack1") || player.GetAxis ("VerticalTurn") != 0f || player.GetAxis ("HorizontalTurn") != 0f) {
			if (m_AttackWaitCoroutine != null)
				StopCoroutine(m_AttackWaitCoroutine);

			m_AttackWaitCoroutine = StartCoroutine(AttackWait());
			//print ("TimeHolder: " + (Time.time-timeHolder));
		}
			//print ("Time - holder: " + (Time.time - timeHolder));
			

}

	IEnumerator AttackWait()
	{
		m_Attack = true;

		yield return m_AttackInputWait;

		m_Attack = false;
	}

	public void EquipToggle(){
		EquipEvent.Invoke ();
	}

}
	