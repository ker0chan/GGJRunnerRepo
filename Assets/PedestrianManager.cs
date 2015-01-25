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

	//Remember the environment currentZone from last frame : You HAVE to try spawning asap if it did :)
	int previousFrameZone;

	EnvironmentManager environmentManager;

	float currentDistance;
	float currentTargetDistance;

	// Use this for initialization
	void Start () {
		environmentManager = GameObject.FindGameObjectsWithTag ("Environment")[0].GetComponent<EnvironmentManager>();
		resetTargetDistance ();
		previousFrameZone = EnvironmentManager.DEFAULT;
	}
	
	// Update is called once per frame
	void Update () {
	
		//If we changed zone : we want to spawn as soon as possible ! Let's fake the traveled distance !
		if (previousFrameZone != EnvironmentManager.currentZone)
		{
			currentDistance = Mathf.Max (currentDistance, Random.Range(0.9f, 1.0f) * currentTargetDistance);
		}
		//Storing it for next frame.
		previousFrameZone = EnvironmentManager.currentZone;

		currentDistance += Time.deltaTime * environmentManager.getRealSpeed();
		if (currentDistance >= currentTargetDistance)
		{
			resetTargetDistance ();
			Spawn ();
		}
	}
	
	void resetTargetDistance()
	{
		currentDistance = 0.0f;
		currentTargetDistance = Random.Range (minDistance, maxDistance);
		if (EnvironmentManager.currentZone != EnvironmentManager.DEFAULT) {
			currentTargetDistance *= 0.2f;
		}
	}

	void Spawn()
	{
		int type = getSpawnablePedestrian ();

		GameObject pedestrian = (GameObject) Instantiate (prefab, new Vector3 (0.0f, 0.0f, 0.0f), new Quaternion (0, 0, 0, 0));
		pedestrian.GetComponent<PedestrianController> ().playerController = playerController;
		pedestrian.GetComponent<AudioSource> ().clip = sounds [Random.Range (0, sounds.Length)];
		pedestrian.GetComponent<PedestrianController> ().type = type;
		GameObject pedestrianPrefab = (GameObject)Instantiate(prefabsList [type]);
		pedestrianPrefab.transform.parent = pedestrian.transform;
		//pedestrianPrefab.transform.localPosition = new Vector3 (0, 0, 0);
		pedestrianPrefab.transform.Translate (new Vector3 (30.0f, 0.0f, Random.Range(3.5f, 5.5f)));
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
