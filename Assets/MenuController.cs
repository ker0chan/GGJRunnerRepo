using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuController : MonoBehaviour {

	string state = "Ingame";
	public Text victoryScoreText;
	public GameObject player;
	float countdown = 3.0f;
	bool countdownStarted = false;

	PlayerController playerController;

	// Use this for initialization
	void Start () {
		playerController = player.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (countdown <= 0.0f)
		{
			switch (state)
			{
				case "Victory":
				Application.LoadLevel (2);
				break;
				case "GameOver":
				Application.LoadLevel (3);
				break;
			}
		}

		if (countdownStarted)
		{
			countdown -= Time.deltaTime;
			/*bgm.volume = countdown / 3.0f;
			bgm2.volume = bgm.volume;*/
		}
	}

	public void setState(string state)
	{
		this.state = state;

		switch (state)
		{
			case "Victory":
			EndGameScoreController.finalSaneScore = playerController.saneScore;
			EndGameScoreController.finalInsaneScore = playerController.insaneScore;
			break;
		}
		countdownStarted = true;

		GetComponent<Animator> ().SetTrigger ("screenFade");
	}

}
