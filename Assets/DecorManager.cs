using UnityEngine;
using System.Collections;

public class DecorManager : MonoBehaviour {
	public GameObject[] parts;

	//Decor parts geometry prefabs
	public GameObject[] prefabsList;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//DEBUG SPEED MODIFIER
		if (Input.GetKeyDown (KeyCode.A)) {
			EnvironmentManager.speed += 1;
				}
		if (Input.GetKeyDown (KeyCode.B)) {
			EnvironmentManager.speed -= 1;
		}

		foreach (GameObject o in parts) {
			o.transform.Translate(new Vector3(-EnvironmentManager.speed * Time.deltaTime, 0, 0));

			if(o.transform.position.x < -15.0f)
			{
				o.transform.Translate (new Vector3(30.0f, 0, 0));
			}
		}
	}
}
