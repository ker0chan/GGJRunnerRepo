using UnityEngine;
using System.Collections;

public class PedestrianManager : MonoBehaviour {
	
	public float maxDistance;
	public float minDistance;
	//Pedestrian prefab to instantiate
	public GameObject prefab;
	//Pedestrian GEOMETRY prefabs to instantiate
	public GameObject[] prefabsList;
	public PlayerController playerController;
	public AudioClip[] sounds;

	EnvironmentManager environmentManager;

	float spawnTime;

	// Use this for initialization
	void Start () {
		environmentManager = GameObject.FindGameObjectsWithTag ("Environment")[0].GetComponent<EnvironmentManager>();
		resetSpawnTime ();
	}
	
	// Update is called once per frame
	void Update () {
		spawnTime -= Time.deltaTime;
		if (spawnTime <= 0.0f)
		{
			resetSpawnTime ();
			Spawn ();
		}
	}
	
	void resetSpawnTime()
	{
	
		spawnTime = Random.Range (minDistance, maxDistance) / environmentManager.getRealSpeed ();
		if (EnvironmentManager.currentZone != EnvironmentManager.DEFAULT) {
			spawnTime *= 0.2f;
		}
	}

	void Spawn()
	{
		int type = getSpawnablePedestrian ();

		GameObject pedestrian = (GameObject) Instantiate (prefab, new Vector3 (15, 0, Random.Range(3.5f, 5.5f)), new Quaternion (0, 0, 0, 0));
		pedestrian.GetComponent<PedestrianController> ().playerController = playerController;
		pedestrian.GetComponent<AudioSource> ().clip = sounds [Random.Range (0, sounds.Length)];
		pedestrian.GetComponent<PedestrianController> ().type = type;
		GameObject pedestrianPrefab = (GameObject)Instantiate(prefabsList [type]);
		pedestrianPrefab.transform.parent = pedestrian.transform;
		pedestrianPrefab.transform.localPosition = new Vector3 (0, 0, 0);
	}

	//Get a random Pedestrian identifier, according to the current zone
	int getSpawnablePedestrian()
	{
		float roll = Random.Range (0.0f, 1.0f);
		int chosenPedestrian = 0;
		while (roll - EnvironmentManager.pedestriansFreq[EnvironmentManager.currentZone, chosenPedestrian] > 0.0f)
		{
			roll -= EnvironmentManager.pedestriansFreq[EnvironmentManager.currentZone, chosenPedestrian];

			chosenPedestrian++;
		}

		return chosenPedestrian;
	}
}
