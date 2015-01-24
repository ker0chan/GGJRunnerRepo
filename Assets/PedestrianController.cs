using UnityEngine;
using System.Collections;

public class PedestrianController : MonoBehaviour {

	public PlayerController playerController;
	public AudioSource sound;
	public int type;

	bool counted = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.x < playerController.transform.position.x && !counted)
		{
			sound.Play ();
			playerController.increaseScore(EnvironmentManager.pedestriansScore[type]);
			counted = true;
		}
		transform.Translate(new Vector3(-EnvironmentManager.speed * Time.deltaTime, 0, 0));
		
		if(transform.position.x < -40.0f)
		{
			Destroy(gameObject);
		}
	}
}
