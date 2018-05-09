using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerShooting : NetworkBehaviour
{
    public uint m_PlayerNumber;            // Used to identify the different players.
    public Rigidbody m_Arrow;                 // Prefab of the shell.
    public Transform m_FireTransform; 
	// A child of the tank where the shells are spawned.
   // public Slider m_AimSlider;                // A child of the tank that displays the current launch force.
    public AudioSource m_ShootingAudio;       // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
    public AudioClip m_ChargingClip;          // Audio that plays when each shot is charging up.
    public AudioClip m_FireClip;              // Audio that plays when each shot is fired.
    public float m_MinLaunchForce ;      // The force given to the shell if the fire button is not held.
    public float m_MaxLaunchForce ;    

/// The force given to the shell if the fire button is held for the max charge time.
   // public float m_MaxChargeTime ;     // How long the shell can charge for before it is fired at max force.
	public Class m_PlayerClass;
	public float timeBetweenShoots;
	public GameObject m_RangeCanvas;

	float timer;

	public PlayerStates m_PlayerStates;
	PlayerMovement m_PlayerMovement;
	Vector3 unitVectorToMouse;
	Vector3 m_MouseInput;

	public Rigidbody m_Shell;



   // private string m_FireButton;            // The input axis that is used for launching shells.
    private Rigidbody m_Rigidbody;          // Reference to the rigidbody component.

	//[SyncVar]
    private float m_CurrentLaunchForce;     // The force that will be given to the shell when the fire button is released.
    public float m_ChargeSpeed; 
	public float m_ShotAngleConstant;  


	// How fast the launch force increases, based on the max charge time.
    public static bool m_Fired;                   // Whether or not the shell has been launched with this button press.

    private void Awake()
    {
        // Set up the references.
        m_Rigidbody = GetComponent<Rigidbody>();
		m_PlayerMovement = GetComponent<PlayerMovement> ();
    }


    private void Start()
    {
		//RangeCanvas.SetActive (false);
        // The fire axis is based on the player number.
       // m_FireButton = "Fire" + (m_localID + 1);

        // The rate that the launch force charges up is the range of possible forces by the max charge time.
      //  m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    [ClientCallback]
    private void Update()
    {


        if (!isLocalPlayer)
            return;

		//FireRanged ();



		if (m_PlayerClass.EnumPlayerClass == Class.EClass.ARCHER) {

			if (GetComponent<PlayerStates>().RunState == PlayerStates.ERunState.RUNNING)
				return;
		
			timer += Time.deltaTime;
			if (timer > timeBetweenShoots) {
				m_Fired = false;
				timer = 0f;
			}
		

			if (!m_Fired) {
				
				if (Input.GetMouseButton (0)) {

					if (m_CurrentLaunchForce >= m_MaxLaunchForce) {
						m_CurrentLaunchForce = m_MaxLaunchForce;
						FireRanged ();
						m_RangeCanvas.SetActive (false);

					}  else {
						m_RangeCanvas.SetActive (true);
						m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
						m_FireTransform.localEulerAngles = new Vector3 (-m_CurrentLaunchForce * m_ShotAngleConstant, 0, 0);
					//	m_RangeCanvas.GetComponent<RangeCanvas> ().angle = 360f - m_FireTransform.eulerAngles.x;
					//	m_RangeCanvas.GetComponent<RangeCanvas> ().velocity = m_Rigidbody.velocity.magnitude + m_CurrentLaunchForce;
						//m_RangeCanvas.GetComponent<RangeCanvas> ().angle = m_FireTransform.position.z;
					}  
				} else if (Input.GetMouseButtonUp (0)) {
					m_RangeCanvas.SetActive (false);
					FireRanged ();
				}

			}
		}else
			m_RangeCanvas.SetActive (false);
	}

    private void FireRanged()
    {
		timer = 0;
        // Set the fired flag so only Fire is only called once.
        m_Fired = true;

        // Change the clip to the firing clip and play it.
        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();



        CmdRangedFire(m_Rigidbody.velocity, m_CurrentLaunchForce, m_FireTransform.forward, m_FireTransform.position, m_FireTransform.rotation);

        // Reset the launch force.  This is a precaution in case of missing button events.
        m_CurrentLaunchForce = m_MinLaunchForce;
    }


    [Command]
	private void CmdRangedFire(Vector3 rigidbodyVelocity, float launchForce, Vector3 forward, Vector3 position, Quaternion rotation)
    {
       
		Rigidbody arrowInstance = Instantiate (m_Arrow, position, rotation) as Rigidbody;

			// Create a velocity that is the tank's velocity and the launch force in the fire position's forward direction.
		Vector3 velocity = rigidbodyVelocity + launchForce * forward;

			// Set the shell's velocity to this velocity.
		arrowInstance.velocity = velocity;


		NetworkServer.Spawn (arrowInstance.gameObject);



    }




    // This is used by the game manager to reset the tank.
    public void SetDefaults()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
//        m_AimSlider.value = m_MinLaunchForce;
    }

	public void FireCatapult(Vector3 velocity,Vector3 position, Quaternion rotation){

			CmdFireCatapult (velocity,position,rotation);
	}

	[Command]
	private void CmdFireCatapult (Vector3 velocity,Vector3 position, Quaternion rotation)
	{
		
		Rigidbody shellInstance = Instantiate (m_Shell, position, rotation) as Rigidbody;
		shellInstance.velocity = velocity;
		NetworkServer.Spawn (shellInstance.gameObject);



	}
	/*
	[ClientRpc]
	private void RpcFireCatapult(Vector3 velocity,Vector3 position, Quaternion rotation)
	{

		Rigidbody shellInstance = Instantiate (m_Shell, position, rotation) as Rigidbody;
		shellInstance.velocity = velocity;
		NetworkServer.Spawn (shellInstance.gameObject);

}*/

}