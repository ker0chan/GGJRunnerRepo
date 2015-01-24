using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public float switchingTime = 0.6f;
	public float coatStep = 0.05f;
	public float idleSoundPeriod = 1.5f;
	public float footstepsSoundPeriod = 0.3f;

	public AudioClip[] jumpSounds;
	public AudioClip[] idleSounds;
	public AudioClip[] footstepsSounds;

	public AudioSource jumpSound;
	public AudioSource idleSound;
	public AudioSource crouchSound;
	public AudioSource footstepsSound;
	public AudioSource openStepSound;
	public AudioSource closeStepSound;

	BoxCollider collider;
	Animator animator;

	string state = "running";
	float switchingTimeRemaining = 0;
	int lane = 0;
	float coatLevel = 0.5f;
	float idleSoundTime;
	float footstepsSoundTime;
	float insaneScore = 0.0f;
	float saneScore = 0.0f;
	
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider> ();
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider obstacle)
	{
		if (Time.realtimeSinceStartup > 2.0f) {
			//print ("LOSE");
		}
	}

	void OnCollisionEnter(Collision collision) 
	{
		if (collision.collider.tag == "Ground") {
			setState("running");
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		print ((Mathf.Round(saneScore)).ToString () + " " + (Mathf.Round(insaneScore)).ToString ());
		//INPUT
		//Jumping (both key pressed and running)
		if (Input.GetKey (KeyCode.UpArrow) && Input.GetKey (KeyCode.Z) && getState () == "running") {
			setState("jumping");

			jumpSound.clip = jumpSounds[UnityEngine.Random.Range (0,jumpSounds.Length)];
			jumpSound.Play();

			Vector3 v = rigidbody.velocity;
			v.y = 5;
			rigidbody.velocity = v;
		}

		//Crouching (both key pressed and running)
		if (Input.GetKey (KeyCode.DownArrow) && Input.GetKey (KeyCode.S) && getState () == "running") {
			setState("crouching");

			crouchSound.Play();
		}
		//Uncrouching (not both keys pressed + already crouching)
		if ((!Input.GetKey (KeyCode.DownArrow) || !Input.GetKey (KeyCode.S)) && getState () == "crouching") {
			setState("running");
		}

		//Switching left
		if (Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.Q) && switchingTimeRemaining <= 0.0f) {
			lane = Math.Min (lane+1, 1);
			switchingTimeRemaining = switchingTime;
		}
		//Switching right
		if (Input.GetKey (KeyCode.RightArrow) && Input.GetKey (KeyCode.D) && switchingTimeRemaining <= 0.0f) {
			lane = Math.Max (lane-1, -1);
			switchingTimeRemaining = switchingTime;
		}

		//"Close Clothes"
		if (Input.GetKeyDown (KeyCode.Space) && getCoatState() != "closed")
		{
			coatLevel -= coatStep;
			closeStepSound.Play();
			if(coatLevel <= 0.0f)
			{
				coatLevel = 0.0f;
				//closeStepSound.Play();
			}
		}
		//"Open Clothes" !
		if (Input.GetKeyDown (KeyCode.RightControl) && getCoatState() != "open")
		{
			coatLevel += coatStep;
			openStepSound.Play();
			if(coatLevel >= 1.0f)
			{
				coatLevel = 1.0f;
				//openStepSound.Play();
			}
		}

		// UPDATE
		//Resize collider on crouching
		if (getState() == "crouching") {
			collider.center = new Vector3(0, -0.25f, 0);
			collider.size = new Vector3(1, 0.5f, 1);
		} else {
			collider.center = new Vector3(0, 0, 0);
			collider.size = new Vector3(1, 1, 1);
		}

		//Switching interpolation
		if (switchingTimeRemaining > 0.0f)
		{
			transform.position = Vector3.Lerp (transform.position, new Vector3(transform.position.x, transform.position.y, lane * 2), (switchingTime - switchingTimeRemaining) / switchingTime);
			switchingTimeRemaining -= Time.deltaTime;
		}

		//Random creepy sounds
		idleSoundTime -= Time.deltaTime;
		if (idleSoundTime < 0.0f)
		{
			idleSound.clip = idleSounds[UnityEngine.Random.Range (0,idleSounds.Length)];
			idleSound.Play();
			idleSoundTime = idleSoundPeriod;
		}

		//Footsteps
		footstepsSoundTime -= Time.deltaTime;
		if (footstepsSoundTime < 0.0f && getState () == "running")
		{
			footstepsSound.clip = footstepsSounds[UnityEngine.Random.Range (0,footstepsSounds.Length)];
			footstepsSound.Play();
			footstepsSoundTime = footstepsSoundPeriod;
		}

		//DebugText.content = getState ();
		//DebugText.content = coatLevel.ToString ();
	}

	string getState()
	{
		return state;
	}
	void setState(string state)
	{
		//Stop the idle sound and reset its time
		idleSound.Stop ();
		idleSoundTime = idleSoundPeriod;

		//Stop the footsteps sound and reset their time
		footstepsSound.Stop ();
		footstepsSoundTime = footstepsSoundPeriod;

		this.state = state;
		animator.SetTrigger (state);
	}
	string getCoatState()
	{
		if (coatLevel <= 0.0f) {
				return "closed";
		} else if (coatLevel < 1.0f) {
				return "default";
		} else {
				return "open";
		}
	}

	public void increaseScore(float amount)
	{
		//How much of the multiplier bar is filled (either side)
		float ratio = Mathf.Abs (coatLevel - 0.5f) * 2;
		//ratio cap
		if (ratio > 0.9f)
			ratio = 0.9f;

		float multiplier = (ratio / 0.9f)*3 + 1;
		if(coatLevel >= 0.5f)
		{
			saneScore += amount*multiplier;
		}
		if(coatLevel <= 0.5f)
		{
			insaneScore += amount*multiplier;
		}
	}
}
