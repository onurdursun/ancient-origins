using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.AI;



public class PlayerAnimation : MonoBehaviour {

	Animator m_Animator;
	PlayerStates m_PlayerStates;

	public int playerId;



	float m_HorizontalInput;
	float m_VerticalInput;

	Player player;
	CharacterController cController;
	public FloatVariable m_Speed;

	public float comboTimer;
	int i = 0;


	// Use this for initialization
	void Awake () {
		player = ReInput.players.GetPlayer (playerId);
		m_Animator = GetComponent<Animator> ();
		m_PlayerStates = GetComponent<PlayerStates> ();
		cController = GetComponent<CharacterController> ();
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

		


	
			/*	else
			animator.SetBool ("Defence", false);*/

			//print(animator.GetFloat("Horizontal"));
			//print(animator.GetFloat("Vertical"));

	
	}





	void Combat(){

		if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING)
			return;


			if (player.GetButton ("Fire1") || player.GetAxis ("VerticalTurn") !=0f || player.GetAxis ("HorizontalTurn") != 0f) {
				m_Animator.SetBool ("Attack", true);
				//print ("TimeHolder: " + (Time.time-timeHolder));
			}
			else
				m_Animator.SetBool("Attack", false);
			//print ("Time - holder: " + (Time.time - timeHolder));
			

}
}
	