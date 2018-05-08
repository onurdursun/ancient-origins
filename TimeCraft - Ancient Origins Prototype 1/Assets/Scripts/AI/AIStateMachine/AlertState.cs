using UnityEngine;
using AIStateMachine;
using System.Security.Policy;

public class AlertState : State<AI>
{
	private static AlertState _instance;


	private AlertState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static AlertState Instance
	{
		get
		{
			if(_instance == null)
			{
				new AlertState();
			}

			return _instance;
		}
	}

	void Awake(){

	}

	public override void EnterState(AI _owner)
	{
		Debug.Log("Entering Alert State");
	}

	public override void ExitState(AI _owner)
	{
		Debug.Log("Exiting Alert State");
	}

	public override void UpdateState(AI _owner)
	{

	}
}
