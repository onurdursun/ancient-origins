using UnityEngine;
using Rewired;
using System;

public class PlayerMovement : MonoBehaviour
{
    
	public int playerId; // Used to identify which tank belongs to which player.  This is set by this tank's manager.


	public FloatVariable m_SpeedFactor;
	public FloatVariable currentSpeed;
	public FloatReference m_RunSpeed; 
	public FloatReference m_SprintSpeed;
	public FloatReference m_JumpHeight;// How fast the tank moves forward and back.
	public FloatReference m_TurnSpeed;              // How fast the tank turns in degrees per second.
    
	private float refSpeed;

	int i;
	int j;
    /*public AudioSource m_MovementAudio;           // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    public AudioClip m_EngineIdling;              // Audio to play when the tank isn't moving.
    public AudioClip m_EngineDriving;             // Audio to play when the tank is moving.
    public ParticleSystem m_LeftDustTrail;        // The particle system of dust that is kicked up from the left track.
    public ParticleSystem m_RightDustTrail; */      // The particle system of dust that is kicked up from the rightt track.
    			// Reference used to move the tank.
	//private LineRenderer m_LineRenderer;




	PlayerStates m_PlayerStates;
	PlayerAnimation m_PlayerAnimation;


	int layerMask;
	float timer = 0f;
	private Player player;
	CharacterController cController;


    private void Awake()
    {
		player = ReInput.players.GetPlayer(playerId);
		m_SpeedFactor.SetValue (m_RunSpeed.Value);
		refSpeed = m_SpeedFactor.Value;
		m_PlayerStates = GetComponent<PlayerStates> ();
		layerMask = LayerMask.GetMask ("Ground");
		cController = GetComponent <CharacterController> ();
		m_PlayerAnimation = GetComponent<PlayerAnimation> ();
    }

    private void FixedUpdate() {
		Move ();
		Turn ();
		Gravity ();
		Jump ();

	}

    private void Move()
	{

		print ("raw: " + player.GetAxis2DRaw ("Horizontal","Vertical"));


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
		currentSpeed.SetValue (velocityVector.magnitude);


		if (currentSpeed.Value < 2f) {
			currentSpeed.SetValue (0f);
			return;
		}


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

		if (m_PlayerStates.RunState == PlayerStates.ERunState.SPRINTING) {
			Quaternion rotation1 = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis ("Horizontal"), player.GetAxis ("Vertical"))) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 4 * Time.deltaTime);
			transform.rotation = rotation1;
		} else {
			

			if (player.GetAxis ("Horizontal") != 0f || player.GetAxis ("Vertical") != 0f)
				timer += Time.deltaTime;
			else
				timer -= Time.deltaTime;

			timer = Mathf.Clamp (timer, 0f, 0.4f);


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
				Quaternion rotation = Quaternion.Lerp (transform.rotation, Quaternion.Euler (0, (Mathf.Rad2Deg * Mathf.Atan2 (player.GetAxis ("HorizontalTurn"), player.GetAxis ("VerticalTurn"))) + Camera.main.transform.eulerAngles.y, 0), m_TurnSpeed.Value * 15 * Time.deltaTime);
				transform.rotation = rotation;
			}
		}
    }


	void Gravity(){
		if(!cController.isGrounded)
			cController.Move (Physics.gravity * Time.deltaTime);
	}

	void Jump(){
		if (player.GetButtonDown("Jump") && cController.isGrounded)
			cController.Move (new Vector3(0,Mathf.Sqrt(m_JumpHeight.Value * -2f * Physics.gravity.y),0) * Time.deltaTime);
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