using UnityEngine;


namespace Extensions
{

	public static class TransformExtensions
	{
		/// <summary>
		/// Finds Melee Weapon on children
		/// </summary>
		/// <param name="origin">Transform origin.</param>
		/// <param name="target">Target direction</param>
		/// <param name="fieldOfView">Field of wiew</param>
		/// <param name="collisionMask">Check against layers</param>
		/// <param name="offset">transforms origin offset</param>
		///<returns>yes or no</returns>

		public static Transform FindWeaponWithTag(this Transform aParent)
		{
			for (int i = 0; i < aParent.childCount; i++) {
				bool result = aParent.GetChild (i).CompareTag ("Weapon");
				if (result)
					return aParent.GetChild (i);
			}


			foreach(Transform child in aParent)
			{
				var result = child.FindWeaponWithTag();
				if (result != null)
					return result;
			}
			return null;
		}


		/*
		public static MeleeWeapon FindMeleeWeapon(this Transform aParent, MeleeWeapon m_MeleeWeapon)
		{
			var result = aParent.GetComponentInChildren<MeleeWeapon>();
			if (result != null)
				return result;
			foreach(Transform child in aParent)
			{
				result = child.FindMeleeWeapon(m_MeleeWeapon);
				if (result != null)
					return result;
			}
			return null;
		}

		public static Shield FindShield(this Transform aParent, Shield m_Shield)
		{
			var result = aParent.GetComponentInChildren<Shield>();
			if (result != null)
				return result;
			foreach(Transform child in aParent)
			{
				result = child.FindShield(m_Shield);
				if (result != null)
					return result;
			}
			return null;
		}*/

	}

}