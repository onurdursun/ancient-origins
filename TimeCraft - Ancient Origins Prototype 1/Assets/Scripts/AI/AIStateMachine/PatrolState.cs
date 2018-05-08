using UnityEngine;
using AIStateMachine;
using System.Security.Policy;

public class PatrolState : State<AI>
{
	private static PatrolState _instance;
	GameObject[] Waypoints;
	int waypoint = 0;

	private PatrolState()
	{
		if(_instance != null)
		{
			return;
		}

		_instance = this;
	}

	public static PatrolState Instance
	{
		get
		{
			if(_instance == null)
			{
				new PatrolState();
			}

			return _instance;
		}
	}

	void Start(){
		//Waypoints = new GameObject[];

	}

	public override void EnterState(AI _owner)
	{
		//Debug.Log("Entering Patrol State");
		Waypoints = GameObject.FindGameObjectsWithTag ("Waypoint");
		_owner.GetComponent<AIBehaviours> ().SetMoveState (AIBehaviours.EMoveState.WALKING);
		//Debug.Log (Waypoints[waypoint]);
		_owner.target = Waypoints [waypoint].transform;


	}

	public override void ExitState(AI _owner)
	{
		
	}

	public override void UpdateState(AI _owner)
	{

		//Debug.Log("Updating Patrol State: " + Vector3.Distance (_owner.transform.position, Waypoints [waypoint].transform.position));
		if (Vector3.Distance (_owner.transform.position, Waypoints [waypoint].transform.position) < _owner.waypointDistance) {
			waypoint++;
			if (waypoint >= Waypoints.Length)
				waypoint = 0;
			_owner.target = Waypoints [waypoint].transform;
		}

		_owner.GetComponent<AIMovement>().Move (_owner.target.position);
	}
}
