using UnityEngine;
using AIStateMachine;

public class ChaseState : State<AI>
{
	private static ChaseState _instance;
	GameObject[] Waypoints;
	int waypoint = 0;


	private ChaseState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static ChaseState Instance
	{
		get
		{
			if(_instance == null)
			{
				new ChaseState();
			}

			return _instance;
		}
	}

	public override void EnterState(AI _owner)
	{
		Debug.Log("Entering Chase State");
		_owner.GetComponent<AIBehaviours> ().SetMoveState (AIBehaviours.EMoveState.SPRINTING);

	}

	public override void ExitState(AI _owner)
	{
		Debug.Log("Exiting Chase State");
	}

	public override void UpdateState(AI _owner)
	{
		_owner.GetComponent<AIMovement>().Move (_owner.target.position);
	}
}
