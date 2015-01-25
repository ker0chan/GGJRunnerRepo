using UnityEngine;
using System.Collections;

public class BuildingController : MonoBehaviour {
	
	EnvironmentManager environmentManager;
	
	// Use this for initialization
	void Start () {
		environmentManager = GameObject.FindGameObjectsWithTag ("Environment")[0].GetComponent<EnvironmentManager>();
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(-environmentManager.getRealSpeed() * Time.deltaTime, 0, 0));
		
		if(transform.position.x < -100.0f)
		{
			Destroy(gameObject);
		}
	}
}
