using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
	public Transform spineHolder;
	public Transform handHolder;

	public void Equip(){

		if (transform.parent == handHolder) {
			transform.SetParent (spineHolder);
			TransformReset (transform);
		} else {
			transform.SetParent (handHolder);
			TransformReset (transform);
		}
	}

	void TransformReset(Transform _transform){
		_transform.localPosition = Vector3.zero;
		_transform.localRotation = Quaternion.Euler (Vector3.zero);
	}
}
