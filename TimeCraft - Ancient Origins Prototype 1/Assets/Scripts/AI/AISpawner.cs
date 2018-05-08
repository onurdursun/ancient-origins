using UnityEngine;
using System.Collections;

public class AISpawner : MonoBehaviour
{
	public GameObject AIPrefab;
	public Vector3 spawnValues;
	public int count;
	public float spawnWait;
	public float startWait;
	public float waveWait;
	public TextMesh textMesh;
	float textFloat;
	public int AIcount;

	void Start ()
	{
		StartCoroutine (SpawnWaves ());
	}

	IEnumerator SpawnWaves ()
	{
		
		yield return new WaitForSeconds (startWait);
		while (true)
		{
			
			for (int i = 0; i < count; i++)
			{

				textFloat = 0f;
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (AIPrefab, spawnPosition, spawnRotation);
				AIcount++;
				yield return new WaitForSeconds (spawnWait);
			}
			yield return new WaitForSeconds (waveWait);
		}
	}

	void Update(){
		textFloat += Time.deltaTime;
		textMesh.text = textFloat.ToString ("##");
	}
}