using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimations : MonoBehaviour {



	Animator m_Animator;
	AIBehaviours m_AIStates;
	NavMeshAgent m_NavMeshAgent;
	Vector3 velocity;
	Vector3 lastPosition;
	int i;
	public bool canAttack;


	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator> ();
		m_AIStates = GetComponent <AIBehaviours> ();
		m_NavMeshAgent = GetComponent<NavMeshAgent> ();
		canAttack = true;
	}

	void OnEnable(){
		canAttack = true;
	}

	void OnDisable(){
		canAttack = false;
	}

	// Update is called once per frame
	void Update () {

		//print ("canAttack" + canAttack);
		CalculateVelocity ();



		if (velocity.magnitude > 0.5f)
			Move ();
		else{
			m_Animator.SetFloat("Horizontal", Mathf.SmoothStep (m_Animator.GetFloat ("Horizontal"),0f,Time.deltaTime*10f));
			m_Animator.SetFloat("Vertical", Mathf.SmoothStep (m_Animator.GetFloat ("Vertical"),0f,Time.deltaTime*10f));

		}


		//print ("Velocity of AI: " + velocity);

		//print ("AI speed: " + navMeshAgent.velocity.magnitude);
	}

	void CalculateVelocity(){
		velocity = (transform.position - lastPosition) / Time.deltaTime;
		lastPosition = transform.position;

	}

	void Move(){
		//m_Animator.SetFloat ("Speed", m_NavMeshAgent.velocity.magnitude);
		m_Animator.SetFloat("Horizontal", velocity.x * Mathf.Sin ((transform.eulerAngles.y+90f)*Mathf.Deg2Rad) + velocity.z * Mathf.Cos ((transform.eulerAngles.y + 90f)*Mathf.Deg2Rad));
		m_Animator.SetFloat("Vertical", velocity.z * Mathf.Sin ((transform.eulerAngles.y+90f)*Mathf.Deg2Rad) - velocity.x * Mathf.Cos ((transform.eulerAngles.y + 90f)*Mathf.Deg2Rad));


	}

	public void DamageEffect(){
		if(!m_Animator.GetBool ("React"))
			m_Animator.SetBool ("React",true);
	}

	public void Attack(bool attack){
		m_Animator.SetBool ("Slash",attack);
	}

	public void Taunt(){
		m_Animator.SetBool ("Taunt",true);
	}

	public IEnumerator Reload(float reloadTime){
		print ("Reload Time: " + reloadTime);
		//print ("canAttack: " + canAttack);
		canAttack = false;
		yield return new WaitForSeconds (reloadTime);
		//print ("canAttack: " + canAttack);
		canAttack = true;
	}


}


