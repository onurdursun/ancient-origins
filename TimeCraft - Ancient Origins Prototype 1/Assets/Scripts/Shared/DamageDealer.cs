using UnityEngine;
using System.Collections.Generic;


public class DamageDealer : MonoBehaviour {
	public List<UnitHealth> attackedEnemyList;
   	public FloatReference DamageAmount;
	public StringVariable MaskClass;
	public BoolVariable CanAttack;

	public void ToggleAttack(bool value){
		CanAttack.SetValue (value);
		if (!value)
			attackedEnemyList.Clear ();
	}

	void Awake(){
		attackedEnemyList = new List<UnitHealth> ();
	}
}


