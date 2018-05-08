using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class CameraControl : MonoBehaviour
{
	public float m_DampTime;                 // Approximate time for the camera to refocus.
	public float m_ScreenEdgeBuffer;           // Space between the top/bottom most target and the screen edge.
	public float m_MinHeight;
	public float m_MaxHeight;
	public float m_CameraHeight; 
	public float m_HeightConstant;
	public float m_RestirctConstant;		
	GameObject[] m_Targets; // All the targets the camera needs to encompass.


	private Camera m_Camera;                        // Used for referencing the camera.
	private float m_ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.
	private Vector3 m_MoveVelocity;                 // Reference velocity for the smooth damping of the position.
	private Vector3 m_DesiredPosition;              // The position the camera is moving towards.

	List<float> xPositions;
	List<float> zPositions;

	public PlayerRestiriction playerRest;

	private void Awake ()
	{
		m_Camera = GetComponentInChildren<Camera> ();
		m_Targets = GameObject.FindGameObjectsWithTag ("Player");
		xPositions = new List<float>();
		zPositions = new List<float>();
	}



	private void FixedUpdate ()
	{
		// Move the camera towards a desired position.
		Move ();

		// Change the size of the camera based.
		Zoom ();
	}


	private void Move ()
	{
		// Find the average position of the targets.
		FindAveragePosition ();

		// Smoothly transition to that position.
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition - transform.forward * m_CameraHeight, ref m_MoveVelocity, m_DampTime);

	}


	private void FindAveragePosition ()
	{
		Vector3 averagePos = new Vector3 ();
		int numTargets = 0;

		// Go through all the targets and add their positions together.
		for (int i = 0; i < m_Targets.Length; i++)
		{
			// If the target isn't active, go on to the next one.
			if (!m_Targets[i].gameObject.activeSelf)
				continue;

			// Add to the average and increment the number of targets in the average.
			averagePos += m_Targets[i].transform.position;
			numTargets++;
		}

		// If there are targets divide the sum of the positions by the number of them to find the average.
		if (numTargets > 0)
			averagePos /= numTargets;

		// Keep the same y value.
		//averagePos.y = transform.position.y;

		// The desired position is the average position;
		m_DesiredPosition = averagePos;
		//playerRest.desiredPosition = averagePos;
	}


	private void Zoom ()
	{
		// Find the required size based on the desired position and smoothly transition to that size.
		m_CameraHeight = FindRequiredZoom();

		//m_DesiredPosition = Vector3.SmoothDamp (m_DesiredPosition, requiredZoom , ref m_MoveVelocity, m_DampTime);
	}


	private float FindRequiredZoom ()
	{
		// Find the position the camera rig is moving towards in its local space.

		// Start the camera's size calculation at zero.
		float zoom = 0f;
		xPositions.Clear ();
		zPositions.Clear ();

		// Go through all the targets...
		foreach (GameObject target in m_Targets){
			xPositions.Add(target.transform.position.x);
			zPositions.Add(target.transform.position.z);
		}

		float maxX = Mathf.Max(xPositions.ToArray ());
		float maxZ = Mathf.Max(zPositions.ToArray ());
		float minX = Mathf.Min(xPositions.ToArray ());
		float minZ = Mathf.Min(zPositions.ToArray ());

		Vector2 minPosition = new Vector2(minX, minZ);
		Vector2 maxPosition = new Vector2(maxX, maxZ);

		zoom = Vector2.Distance (maxPosition, minPosition);


		// Add the edge buffer to the size.
		//zoom += m_ScreenEdgeBuffer;

		// Make sure the camera's size isn't below the minimum.
		zoom = Mathf.Clamp (zoom, m_MinHeight ,m_MaxHeight);

		return zoom + m_HeightConstant;
	}


	public void SetStartPositionAndSize ()
	{
		// Find the desired position.
		FindAveragePosition ();

		// Set the camera's position to the desired position without damping.
		transform.position = m_DesiredPosition;



	}
}