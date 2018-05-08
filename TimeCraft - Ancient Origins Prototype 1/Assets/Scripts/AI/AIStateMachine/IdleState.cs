using UnityEngine;
using AIStateMachine;
using System.Security.Policy;

public class IdleState : State<AI>
{
	private static IdleState _instance;

	private IdleState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static IdleState Instance
	{
		get
		{
			if(_instance == null)
			{
				new IdleState();
			}

			return _instance;
		}
	}

	void Awake(){

	}

	public override void EnterState(AI _owner)
	{
//		Debug.Log("Entering Idle State");
		_owner.GetComponent<AIBehaviours> ().SetMoveState (AIBehaviours.EMoveState.IDLE);
	}

	public override void ExitState(AI _owner)
	{

	}

	public override void UpdateState(AI _owner)
	{
		//Debug.Log ("Updating Idle State");
	}
}
