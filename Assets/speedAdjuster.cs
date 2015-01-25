using UnityEngine;
using System.Collections;

public class speedAdjuster : MonoBehaviour {

	Animator animator;
	public EnvironmentManager env;
	public float mult = 5.0f;
	//public float angle;
	// Use this for initialization
	void Start () {
		animator = GetComponent <Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		animator.speed = env.getRealSpeed ()/mult;
/*
		GameObject[] targets = GameObject.FindGameObjectsWithTag ("openCoatTargetJoint");
		foreach (GameObject t in targets) {
			Vector3 r = t.transform.localEulerAngles;

			t.transform.localEulerAngles = new Vector3(r.x, angle, r.z);
				}

		GameObject[] reverseTargets = GameObject.FindGameObjectsWithTag ("openCoatReverseTargetJoint");
		foreach (GameObject rt in reverseTargets) {
			Vector3 r = rt.transform.localEulerAngles;

			rt.transform.localEulerAngles = new Vector3(r.x, -angle, r.z);
		}*/
	}
}
