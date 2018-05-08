using UnityEngine;
using Rewired;
using System;

public class PlayerMovement : MonoBehaviour
{
    
	public int playerId = 0; // Used to identify which tank belongs to which player.  This is set by this tank's manager.

	public FloatVariable m_Speed;                  // How fast the tank moves forward and back.
	public FloatVariable m_TurnSpeed;              // How fast the tank turns in degrees per second.
    
	private float speed;
    /*public AudioSource m_MovementAudio;           // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;              // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;             // Audio to play when the tank is moving.
    public ParticleSystem m_LeftDustTrail;        // The particle system of dust that is kicked up from the left track.
    public ParticleSystem m_RightDustTrail; */      // The particle system of dust that is kicked up from the rightt track.
    			// Reference used to move the tank.
	//private LineRenderer m_LineRenderer;

	public FloatVariable m_SprintSpeed;


	PlayerStates m_PlayerStates;
	PlayerAnimation m_PlayerAnimation;


	int layerMask;
	float timer = 0f;
	private Player player;
	CharacterController cController;
	float jumpHeight;

    private void Awake()
    {
		player = ReInput.players.GetPlayer(playerId);
		speed = m_Speed.Value;

		m_PlayerStates = GetComponent<PlayerStates> ();
		layerMask = LayerMask.GetMask ("Ground");
		cController = GetComponent <CharacterController> ();
		m_PlayerAnimation = GetComponent<PlayerAnimation> ();
		m_PlayerAnimation.m_Speed = speed;
    }


    private void Start()
    {


        // The axes are based on player number.

	//	m_LineRenderer = GameObject.Find ("MouseDrawLine").GetComponent <LineRenderer>();

        // Store the original pitch of the audio source.
     //   m_OriginalPitch = m_MovementAudio.pitch;
    }

  


    private void EngineAudio()
    {
      /*  // If there is no input (the tank is stationary)...
		if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (m_MovementAudio.clip == m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and the idling clip is currently playing...
            if (m_MovementAudio.clip == m_EngineIdling)
            {
                // ... change the clip to driving and playing.
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                m_MovementAudio.Play();
            }
        }*/
    }


    private void FixedUpdate()
	{
		

		/*if (m_PlayerClass.EnumPlayerClass == PlayerClass.EPlayerClass.ENGINEER && m_PlayerStates.DrivingState == PlayerStates.EDrivingState.DRIVING) {
			DriveCatapult ();
			return;
		}*/

		//m_Rigidbody.useGravity = true;
		// Adjust the rigidbodies position and orientation in FixedUpdate.
		Move ();
		Turn ();
		Gravity ();
		Jump ();
		/*if (m_PlayerClass.EnumPlayerClass == PlayerClass.EPlayerClass.ARCHER) {
			if (!m_LineRenderer.gameObject.activeInHierarchy)
				m_LineRenderer.gameObject.SetActive (true);
			DrawLineToMouse ();
			return;
		}
		if (m_LineRenderer.gameObject.activeInHierarchy)
			m_LineRenderer.gameObject.SetActive (false);
    */
	}

    private void Move()
	{
		// Create a movement vector based on the input, speed and the time between frames, in the direction the tank is facing.
		/*	if (m_PlayerStates.WeaponState == PlayerStates.EWeaponState.FIRING)
			return;*/
		




		if (m_PlayerStates.MoveState == PlayerStates.EMoveState.SPRINTING) {
			Mathf.SmoothDamp (m_Speed.Value, 20, ref speed, 0.5f, m_SprintSpeed.Value);
			//m_MovementAudio.pitch = m_SprintSound;
		} else {
			if (speed > m_Speed.Value)
				Mathf.SmoothDamp (m_SprintSpeed.Value, m_Speed.Value, ref speed, 2f);
		}

		/*
		if (Input.GetAxis ("Vertical") != 0 && Input.GetAxis ("Horizontal") != 0) {
			float refSpeed = speed;
			refSpeed = speed / Mathf.Sqrt (Mathf.Abs (player.GetAxis ("Horizontal")) + Mathf.Abs (player.GetAxis ("Vertical")));
			speed = refSpeed;
		}*/


		// Apply this movement to the rigidbody's position.

		Vector3 verticalMovement = Camera.main.transform.forward * player.GetAxis ("Vertical");
		Vector3 horizontalMovement = Camera.main.transform.right * player.GetAxis ("Horizontal");
		Vector3 movementVector = verticalMovement + horizontalMovement;
		//print ("movementVector.mag: " + movementVector.magnitude);
		if (movementVector.magnitude > 1f)
			movementVector = movementVector.normalized;
		Vector3 velocityVector = movementVector * speed;


		if (velocityVector.magnitude<2f) {

			//Mathf.SmoothDamp (2f, 0f, ref m_PlayerAnimation.m_Speed, 1f);
			m_PlayerAnimation.m_Speed = 0f;
			return;
		}

		m_PlayerAnimation.m_Speed = velocityVector.magnitude;

		cController.Move (velocityVector * Time.deltaTime);

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

		if (player.GetAxis ("Horizontal") != 0f || player.GetAxis ("Vertical") != 0f)
			timer += Time.deltaTime;
		else
			timer -= Time.deltaTime;

		timer = Mathf.Clamp (timer,0f,0.4f);


		if (player.GetAxis ("VerticalTurn") == 0 && player.GetAxis ("HorizontalTurn") == 0) {
			

			if (timer > 0.2f && (player.GetAxis ("Horizontal") != 0f || player.GetAxis ("Vertical") != 0f)) {
				Quaternion rotation1 = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis ("Horizontal"), player.GetAxis ("Vertical"))) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 4 * Time.deltaTime);
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
			Quaternion rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0,( Mathf.Rad2Deg * Mathf.Atan2 ( player.GetAxis ("HorizontalTurn"),player.GetAxis ("VerticalTurn"))) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 15 * Time.deltaTime);
			transform.rotation = rotation;
		}
		//print ("Mouse Position"+netId+" : " + Input.mousePosition);
		/*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 100, layerMask)) {
			m_MouseInput = hit.point;
			m_TurnInput = Quaternion.Euler(0,90-Mathf.Atan2((hit.point.z - transform.position.z),(hit.point.x - transform.position.x))*Mathf.Rad2Deg, 0);
		}
		Vector3.Normalize(m_TurnInput.eulerAngles);
		//if (m_TurnInput.eulerAngles.magnitude != Vector3.one.magnitude)
		//	return;
		m_Rigidbody.MoveRotation (m_TurnInput);
		
			*/
	//	transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	
       /* // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInput * m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion inputRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * inputRotation);*/
    }


	void Gravity(){
		if(!cController.isGrounded)
			cController.Move (Physics.gravity * Time.deltaTime);
	}

	void Jump(){
		if (player.GetButtonDown("Jump") && cController.isGrounded)
			cController.Move (new Vector3(0,Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y),0) * Time.deltaTime);
	}


    // This function is called at the start of each round to make sure each tank is set up correctly.
   
	/*public void SetDefaults()
    {
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;

        m_LeftDustTrail.Clear();
        m_LeftDustTrail.Stop();

        m_RightDustTrail.Clear();
        m_RightDustTrail.Stop();
    }

    public void ReEnableParticles()
    {
        m_LeftDustTrail.Play();
        m_RightDustTrail.Play();
    }



	private void DrawLineToMouse(){

		m_LineRenderer.SetPosition (0, transform.position - 2*(transform.position - m_MouseInput) / Vector3.Magnitude(transform.position - m_MouseInput));
		m_LineRenderer.SetPosition (1, m_MouseInput);
	}

	public void DriveCatapult(){
		motor =  Input.GetAxis("Vertical");
		steering = Input.GetAxis("Horizontal");
		transform.position = m_PlayerStates.driveTransform.position;
		transform.rotation = m_PlayerStates.driveTransform.rotation;
	}*/
}