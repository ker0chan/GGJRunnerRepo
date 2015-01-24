using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {

	public float switchingTime = 0.6f;
	public float invincibilityDuration = 3.0f;
	public float coatStep = 0.01f;
	public float idleSoundPeriod = 1.5f;
	public float footstepsSoundPeriod = 2.5f;
	public Animator gameOverAnimator;

	//GUI bindings
	public Image coatLevelCursor;
	public Text multiplierText;
	public Text insaneScoreGUI;
	public Text saneScoreGUI;

	//Sound banks
	public AudioClip[] jumpSounds;
	public AudioClip[] idleSounds;
	public AudioClip[] footstepsSounds;

	//Sound bindings
	public AudioSource jumpSound;
	public AudioSource idleSound;
	public AudioSource crouchSound;
	public AudioSource footstepsSound;
	public AudioSource openStepSound;
	public AudioSource closeStepSound;
	public AudioSource copsPursuitSound;
	public AudioSource impactSound;

	BoxCollider collider;
	Animator animator;
	EnvironmentManager environmentManager;
	
	string state = "running";
	float switchingTimeRemaining = 0;
	int lane = 0;
	float coatLevel = 0.5f;
	float idleSoundTime;
	float footstepsSoundTime;
	float insaneScore = 0.0f;
	float saneScore = 0.0f;
	bool copsPursuit = false;
	float invincibilityTime = 0.0f;
	
	// Use this for initialization
	void Start () {
		collider = GetComponent<BoxCollider> ();
		animator = GetComponent<Animator> ();
		increaseScore (0);
		environmentManager = GameObject.FindGameObjectsWithTag ("Environment")[0].GetComponent<EnvironmentManager>();
	}

	void OnTriggerEnter(Collider obstacle)
	{
		//print (obstacle.tag);
		if(obstacle.tag == "Obstacle" && invincibilityTime == 0.0f)
		{
			hit();
			//Destroy(obstacle.transform.gameObject);
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
		//print ((Mathf.Round(saneScore)).ToString () + " " + (Mathf.Round(insaneScore)).ToString ());
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
			footstepsSoundTime = footstepsSoundPeriod / environmentManager.speed;
		}

		//Invincibility
		invincibilityTime -= Time.deltaTime;
		if (invincibilityTime < 0.0f) {
			animator.SetBool ("blinking", false);
			invincibilityTime = 0.0f;
		}

		//DebugText.content = getState ();
		//DebugText.content = coatLevel.ToString ();
		//print (coatLevelCursor.rectTransform.anchoredPosition);
		//print ((coatLevel - 0.5f) * 2.0f * 300.0f);
		Vector2 coatLevelCursorPos = coatLevelCursor.rectTransform.anchoredPosition;
		coatLevelCursorPos.x = (coatLevel - 0.5f) * 2.0f * 300.0f;
		coatLevelCursor.rectTransform.anchoredPosition = coatLevelCursorPos;
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
		footstepsSoundTime = footstepsSoundPeriod / environmentManager.speed;

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

	void hit()
	{
		impactSound.Play ();
		if (!copsPursuit) {
			copsPursuit = true;
			copsPursuitSound.Play ();
			environmentManager.impactSpeed();
			animator.SetBool ("blinking", true);
			invincibilityTime = invincibilityDuration;
		} else {
			gameOverAnimator.SetTrigger ("GameOver");
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

		multiplierText.text = "x"+Mathf.Round(multiplier).ToString ();

		if(coatLevel >= 0.5f)
		{
			saneScore += amount*multiplier;
		}
		if(coatLevel <= 0.5f)
		{
			insaneScore += amount*multiplier;
		}

		insaneScoreGUI.text = Mathf.Round (insaneScore).ToString ();
		saneScoreGUI.text = Mathf.Round (saneScore).ToString ();
	}
}
