using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

	float countdown = 3.0f;
	bool started = false;
	public AudioSource startSound;
	public AudioSource bgm;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return) && !started)
		{
			started = true;
			startSound.Play();
			bgm.Stop ();
		}
		if(started && countdown <= 0.0f)
		{
			Application.LoadLevel (4);
		}
		countdown -= Time.deltaTime;
	}
}
