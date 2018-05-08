using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.AI;



public class PlayerAnimation : MonoBehaviour {
	Animator m_Animator;
	PlayerMovement m_PlayerMovement;
	PlayerStates m_PlayerStates;
	Class m_PlayerClass;
	public int playerId;

	TextMesh text;

	float m_HorizontalInput;
	float m_VerticalInput;

	Player player;
	CharacterController cController;
	public float m_Speed;

	float timer;
	public float comboTimer;
	int i = 0;
	float timeHolder = 0f;
	bool count = true;

	// Use this for initialization
	void Awake () {
		player = ReInput.players.GetPlayer (playerId);
		m_Animator = GetComponent<Animator> ();
		m_PlayerClass = GetComponent<Class> ();
		m_PlayerMovement = GetComponent<PlayerMovement> ();
		m_PlayerStates = GetComponent<PlayerStates> ();
		text = GetComponentInChildren<TextMesh> ();
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

		m_Animator.SetFloat ("Speed", m_Speed);

		Idle ();
		Driving ();
		Movement ();
		Combat ();



		//print (m_Animator.GetCurrentAnimatorStateInfo (0).shortNameHash);

		


	
			/*	else
			animator.SetBool ("Defence", false);*/

			//print(animator.GetFloat("Horizontal"));
			//print(animator.GetFloat("Vertical"));

	
	}




	void Idle(){
		if (m_Animator.GetCurrentAnimatorStateInfo (0).IsName ("Walking")) {
			timer += Time.deltaTime;
            
		}
        else
            timer = 0f;

		m_Animator.SetFloat("Time", timer);
	}
	void Driving(){
		if (m_PlayerStates.DrivingState != PlayerStates.EDrivingState.DRIVING) {	
			if (m_HorizontalInput != 0 || m_VerticalInput != 0) {

                m_Animator.SetFloat("Horizontal", m_HorizontalInput * Mathf.Sin ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y +90f)*Mathf.Deg2Rad) + m_VerticalInput * Mathf.Cos ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y+90f)*Mathf.Deg2Rad));
                m_Animator.SetFloat("Vertical", m_VerticalInput * Mathf.Sin ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y+90f)*Mathf.Deg2Rad) - m_HorizontalInput * Mathf.Cos ((transform.eulerAngles.y - Camera.main.transform.eulerAngles.y+90f)*Mathf.Deg2Rad));
                m_Animator.SetBool("Walking", true);


				//animator.SetFloat ("Horizontal", (Mathf.Abs ((m_MouseInput.x - transform.position.x) / Vector3.Distance (m_MouseInput, transform.position)) * ((Mathf.Abs (m_MouseInput.z - transform.position.z) / Vector3.Distance (m_MouseInput, transform.position)))));
				//animator.SetFloat ("Vertical", (Mathf.Abs ((m_MouseInput.z - transform.position.z) / Vector3.Distance (m_MouseInput, transform.position)) * ((Mathf.Abs (m_MouseInput.z - transform.position.z) / Vector3.Distance (m_MouseInput, transform.position)))));
			} else {
                m_Animator.SetBool("Walking", false);
				m_Animator.SetFloat("Horizontal", 0);
				m_Animator.SetFloat("Vertical", 0);
			}

		} else {
            m_Animator.SetBool("Walking", false);
			m_Animator.SetFloat("Horizontal", 0);
			m_Animator.SetFloat("Vertical", 0);
		}
	}
	void Movement(){
		if (m_PlayerStates.MoveState == PlayerStates.EMoveState.SPRINTING) {
			m_Animator.SetBool ("Sprinting", true);
			m_Animator.SetBool ("Walking", false);
			//m_MovementAudio.pitch = m_SprintSound;
		} else if (m_PlayerStates.MoveState == PlayerStates.EMoveState.WALKING) {
			m_Animator.SetBool ("Walking", true);		
			m_Animator.SetBool ("Sprinting", false);
		} else{
			m_Animator.SetBool ("Walking", false);		
			m_Animator.SetBool ("Sprinting", false);
		}



			
	}
	void Combat(){
		if (m_PlayerClass.EnumPlayerClass == Class.EClass.WARRIOR) {

			if (Input.GetMouseButton (0))
                m_Animator.SetBool("Slash", true);
			else
                m_Animator.SetBool("Slash", false);

			if (Input.GetMouseButtonDown (1))
				m_Animator.SetTrigger ("Defence");

			if (Input.GetButton ("Fire1"))
				m_Animator.SetBool("Slash", true);
			else
				m_Animator.SetBool("Slash", false);



		} else if (m_PlayerClass.EnumPlayerClass == Class.EClass.ARCHER) {

			if (m_PlayerStates.MoveState != PlayerStates.EMoveState.RUNNING) {

				if ((Input.GetMouseButton (0) || Input.GetMouseButton (1)) && !PlayerShooting.m_Fired) {
                    m_Animator.SetBool("Shot", true);

				} else {
                    m_Animator.SetBool("Shot", false);

				}

			}
			else
                m_Animator.SetBool("Shot", false);








		} else if (m_PlayerClass.EnumPlayerClass == Class.EClass.ENGINEER) {

			if (m_PlayerStates.DrivingState == PlayerStates.EDrivingState.DRIVING) {

				if (Input.GetAxis ("Vertical") > 0f) {
                    m_Animator.SetBool("PushBack", false);
                    m_Animator.SetBool("Push", true);
				} else if(Input.GetAxis("Vertical") < 0f){
                    m_Animator.SetBool("Push", false);
                    m_Animator.SetBool("PushBack", true);
				} else {
                    m_Animator.SetBool("Push", false);
                    m_Animator.SetBool("PushBack", false);
				}
			}





		} else if (m_PlayerClass.EnumPlayerClass == Class.EClass.AXE) {
//			print ("hi axe");

			//print ("i: " + i);


			if (m_Animator.GetAnimatorTransitionInfo (2).IsName ("Idle -> Attack")) {
				timeHolder = Time.time;

				if (count) {
					i++;
					count = false;
				}
				if (i > 2)
					i = 0;
			}






			if (player.GetButton ("Fire1") || player.GetAxis ("VerticalTurn") !=0f || player.GetAxis ("HorizontalTurn") != 0f) {
				m_Animator.SetFloat ("AttackType",i);

				m_Animator.SetBool ("Slash", true);
				//print ("TimeHolder: " + (Time.time-timeHolder));
			}
			else
				m_Animator.SetBool("Slash", false);
			//print ("Time - holder: " + (Time.time - timeHolder));
			//print ("TimeHolder: " + timeHolder);
			if(Time.time - timeHolder > comboTimer){


				i = 0;
			}

			if (m_Animator.GetAnimatorTransitionInfo (2).IsName ("Attack -> Idle")) {
				count = true;
			}

			text.text = (Time.time - timeHolder).ToString ("#.##");
		}


	}


}
	