// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.Events;
using System;
using RoboRyanTron.Unite2017.Variables;

public class UnitHealth : MonoBehaviour {
	
	public StringVariable m_Tag;
	public FloatVariable HP;
	public bool ResetHP;
	public FloatReference StartingHP;
	public UnityEvent DamageEvent;
	public UnityEvent DeathEvent;

	public Transform particle;
       
	private void Start() {
		if (ResetHP)
			HP.SetValue(StartingHP);
        }

	/*
    private void OnTriggerEnter(Collider other) {

		print ("I'm " + gameObject.name +  ". " + other.name + " is attacking me.");

        DamageDealer damage = other.gameObject.GetComponent<DamageDealer>();
        
		if (damage) {

			/*print (gameObject.name +  ": " +"Damage dealer found.");
			print (gameObject.name +  ": " +"Damage CanAttack: " + damage.CanAttack.Value);
			print ("Damage isAttackedEnemyList.Contains: " + gameObject.name + " - " + damage.attackedEnemyList.Contains (this));
*//*
			if (!damage.CanAttack.Value || damage.MaskClass.Value == m_Tag.Value || damage.attackedEnemyList.Contains (this))
				return;
			//print (gameObject.name +  ": " +"Damage dealer is attacking.");
			HP.ApplyChange(-damage.DamageAmount);
			if(!damage.attackedEnemyList.Contains (this))
				damage.attackedEnemyList.Add (this);
            DamageEvent.Invoke();

			SetParticlePosition (other.ClosestPoint (other.transform.position),other.transform);
			//print (gameObject.name +  ": " +"DamageEvent Invoked");
        }

		if (HP.Value <= 0.0f) {
            DeathEvent.Invoke();
        }
    }

	public void SetParticlePosition(Vector3 _position,Transform other){
		particle.position = _position;
		particle.LookAt (other);
	}*/
}
