using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Rewired.ComponentControls.Effects;

public class AIMovement : MonoBehaviour
{


	       // Reference to the player's position.
	//PlayerHealth playerHealth;      // Reference to the player's health.
	//EnemyHealth enemyHealth;        // Reference to this enemy's health.
	NavMeshAgent nav;               // Reference to the nav mesh agent.
	public float m_TurnSpeed;
	Quaternion rot;

	void Awake ()
	{
		// Set up the references.

		//playerHealth = player.GetComponent <PlayerHealth> ();
		//enemyHealth = GetComponent <EnemyHealth> ();
		nav = GetComponent <NavMeshAgent> ();
	}
		 

	public void Move(Vector3 target){
		nav.SetDestination (target);
		Turn (target,0);
	}

	public void Turn(Vector3 target,float amount){
		Vector3 targetDir = target - transform.position;
		targetDir.y = 0.0f;
		transform.rotation = rot;
		transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(targetDir), Time.deltaTime * m_TurnSpeed);
		rot = transform.rotation;
		transform.eulerAngles += new Vector3 (0, amount, 0);
	}



	public void AdjustSpeed(float speed){
		nav.speed = speed;
	}
}