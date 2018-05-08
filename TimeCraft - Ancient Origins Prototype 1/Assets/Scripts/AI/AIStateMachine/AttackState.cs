using UnityEngine;
using AIStateMachine;

public class AttackState : State<AI>
{
	private static AttackState _instance;

	private AttackState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static AttackState Instance
	{
		get
		{
			if(_instance == null)
			{
				new AttackState();
			}

			return _instance;
		}
	}

	public override void EnterState(AI _owner)
	{
		Debug.Log("Entering Attack State");
		_owner.GetComponent<AIBehaviours> ().SetMoveState (AIBehaviours.EMoveState.SPRINTING);
		//_owner.GetComponent<AIAnimations>().Attack (true);

	}

	public override void ExitState(AI _owner)
	{
		_owner.GetComponent<AIAnimations>().Attack (false);	
	}

	public override void UpdateState(AI _owner)
	{
		_owner.GetComponent<AIMovement>().Move (_owner.target.position);
		_owner.GetComponent<AIAnimations>().Attack (true);
	}
}
