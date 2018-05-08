using UnityEngine;
using AIStateMachine;
using System.Security.Policy;

public class ShootState : State<AI>
{
	private static ShootState _instance;


	private ShootState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static ShootState Instance
	{
		get
		{
			if(_instance == null)
			{
				new ShootState();
			}

			return _instance;
		}
	}

	void Start(){
		//Waypoints = new GameObject[];

	}

	public override void EnterState(AI _owner)
	{

	}

	public override void ExitState(AI _owner)
	{
		_owner.GetComponent<Animator>().SetBool ("Shoot",false);
	}

	public override void UpdateState(AI _owner)
	{
		
		_owner.GetComponent<AIMovement>().Turn (_owner.target.position,0);
		if (_owner.GetComponent<AIShooting> ().canShoot) {
			_owner.GetComponent<Animator> ().SetBool ("Shoot", true);
		}
	}
}
