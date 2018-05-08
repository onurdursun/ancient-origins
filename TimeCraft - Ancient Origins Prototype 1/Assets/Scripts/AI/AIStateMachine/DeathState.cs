using UnityEngine;
using AIStateMachine;

public class DeathState : State<AI>
{
	private static DeathState _instance;

	private DeathState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static DeathState Instance
	{
		get
		{
			if(_instance == null)
			{
				new DeathState();
			}

			return _instance;
		}
	}

	public override void EnterState(AI _owner)
	{
		Debug.Log ("DIE!");
		_owner.GetComponent<Animator>().SetBool ("Death",true);
	}

	public override void ExitState(AI _owner)
	{

	}

	public override void UpdateState(AI _owner)
	{

	}
}
