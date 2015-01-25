using UnityEngine;
using System.Collections;

public class DecorManager : MonoBehaviour {

	public float targetDistance;
	//Obstacle geometry prefabs
	public GameObject[] prefabsList;
	//Obstacle prefab
	public GameObject prefab;

	EnvironmentManager environmentManager;
	float currentDistance;

	// Use this for initialization
	void Start () {
		environmentManager = GetComponentInParent<EnvironmentManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		currentDistance += Time.deltaTime * environmentManager.getRealSpeed();
		if (currentDistance >= targetDistance)
		{
			resetSpawnTime ();
			Spawn ();
		}
	}
	
	void resetSpawnTime()
	{
		currentDistance = 0.0f;
	}

	void Spawn()
	{
		int type = Random.Range (0, prefabsList.Length);
		int lane = Random.Range (0, 3) - 1;

		GameObject obstacle = (GameObject) Instantiate (prefab, new Vector3 (0.0f, 0.0f, 0.0f), new Quaternion (0, 0, 0, 0));
		GameObject obstaclePrefab = (GameObject)Instantiate(prefabsList [type]);
		obstaclePrefab.transform.parent = obstacle.transform;
		obstaclePrefab.transform.Translate (new Vector3 (30.0f, 0.0f, lane*2));
		//obstaclePrefab.transform.localPosition = new Vector3 (0, 0, 0);

	}
}
