using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class ShakeCamera : MonoBehaviour {

	public float magnitude = 1f;
	public float roughness = 1f;
	public float fadeIn = .1f;
	public float fadeOut = 1f;

	public void ShakeCameraOnce(){
		
		CameraShaker.Instance.ShakeOnce(magnitude,roughness,fadeIn,fadeOut);
	}
}
