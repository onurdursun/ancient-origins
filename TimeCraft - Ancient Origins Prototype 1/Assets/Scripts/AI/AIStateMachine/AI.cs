using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStateMachine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public bool switchState = false;
    public float gameTimer;
    public int seconds = 0;
	public Transform target;
	public Transform potentialTarget;


	AIMovement m_AIMovement;
	NavMeshAgent m_NavMeshAgent;
	AIAnimations m_AIAnimations;
	AIHealth m_AIHealth;
	Animator m_Animator;
	AIBehaviours m_AIBehaviours;
	Class m_Class;
	AIShooting m_AIShooting;

	public float turnAmount;
	public float waypointDistance;


	public StateMachine<AI> stateMachine { get; set; }



	private void Start()
	{
		stateMachine = new StateMachine<AI>(this);
		stateMachine.ChangeState(IdleState.Instance);
	}

	void Awake(){
		m_AIMovement = GetComponent<AIMovement> ();
		m_NavMeshAgent = GetComponent<NavMeshAgent> ();
		m_AIAnimations = GetComponent<AIAnimations> ();
		m_AIHealth = GetComponent<AIHealth> ();
		m_Animator = GetComponent<Animator> ();
		m_AIBehaviours = GetComponent<AIBehaviours> ();
		m_Class = GetComponent<Class> ();
		m_AIShooting = GetComponent<AIShooting> ();
	}

	// Update is called once per frame
	void FixedUpdate () {

		//print ("Current State: " + stateMachine.currentState);

		if (!m_AIHealth.m_IsAlive) {
			stateMachine.ChangeState (DeathState.Instance);
			return;
		}

		if (!target) {
			if (stateMachine.currentState != PatrolState.Instance) {
				stateMachine.ChangeState (PatrolState.Instance);
			}
		}

		else if(!target.GetComponent<PlayerHealth>()) {
			if (stateMachine.currentState != PatrolState.Instance)
				stateMachine.ChangeState (PatrolState.Instance);

		} else {

			if (target.GetComponent<PlayerHealth> ().m_IsAlive) {

				if (m_Class.EnumAIClass == Class.EClass.ARCHER) {
					if (Vector3.Distance (target.position, transform.position) < m_AIShooting.shootingDistance) {
						if (stateMachine.currentState != ShootState.Instance)
							stateMachine.ChangeState (ShootState.Instance);

					} else {
						if (stateMachine.currentState != ChaseState.Instance)
							stateMachine.ChangeState (ChaseState.Instance);
						
					}

				} else {

					if (Vector3.Distance (target.position, transform.position) < m_NavMeshAgent.stoppingDistance) {
						if (m_AIAnimations.canAttack) {
							if (stateMachine.currentState != AttackState.Instance)
								stateMachine.ChangeState (AttackState.Instance);
						} else if (stateMachine.currentState != ChaseState.Instance) {
							stateMachine.ChangeState (ChaseState.Instance);
						}

					} else {
						if (stateMachine.currentState != ChaseState.Instance)
							stateMachine.ChangeState (ChaseState.Instance);
						}
				}

			} else {
				m_AIAnimations.Taunt ();
				if (stateMachine.currentState != IdleState.Instance)
					stateMachine.ChangeState (IdleState.Instance);
					}
				} 

		stateMachine.Update ();
			
	}
}
	

