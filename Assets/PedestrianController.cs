using UnityEngine;
using System.Collections;

public class PedestrianController : MonoBehaviour {

	public PlayerController playerController;
	public AudioSource sound;
	public int type;

	EnvironmentManager environmentManager;

	bool counted = false;

	// Use this for initialization
	void Start () {
		environmentManager = GameObject.FindGameObjectsWithTag ("Environment")[0].GetComponent<EnvironmentManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < playerController.transform.position.x && !counted)
		{
			if(Random.Range (0.0f,1.0f) < 0.3f)
				sound.Play ();
			playerController.increaseScore(EnvironmentManager.pedestriansScore[type]);
			counted = true;
		}
		transform.Translate(new Vector3(-environmentManager.getRealSpeed() * Time.deltaTime, 0, 0));
		
		if(transform.position.x < -40.0f)
		{
			Destroy(gameObject);
		}
	}
}
