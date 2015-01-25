using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGameScoreController : MonoBehaviour {

	public static float finalSaneScore;
	public static float finalInsaneScore;

	public Text victoryScoreText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return))
		{
			Application.LoadLevel (1);
		}

		if(EndGameScoreController.finalSaneScore > EndGameScoreController.finalInsaneScore)
		{
			victoryScoreText.text = "Congratulations, sane player (on the right). You've paired well enough with your troubled second self, and kept control of your trench coat (and body image) long enough to run away !";
		} else if (EndGameScoreController.finalSaneScore < EndGameScoreController.finalInsaneScore)
		{
			victoryScoreText.text = "Congratulations, insane player (on the left). You followed your pulsions and fought your good manners fiercely enough to proudely show the world the wonders of your undercoat !";
		} else 
		{
			victoryScoreText.text = "Looks like you found the secret Cooperation Mode ! It means you're playtesting our game, aren't you ? Or maybe you didn't read the tutorial. Wait, have we created the tutorial yet ?..";
		}
	}
}
