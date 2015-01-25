using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : MonoBehaviour {

	public float remainingTime = 30.0f;
	public Canvas mainCanvas;
	public GameObject timerGUI;

	MenuController menuController;
	Text timerGUIText;

	// Use this for initialization
	void Start () {
		timerGUIText = timerGUI.GetComponent<Text> ();
		menuController = mainCanvas.GetComponent<MenuController> ();
	}
	
	// Update is called once per frame
	void Update () {
		remainingTime -= Time.deltaTime;
		timerGUIText.text = Mathf.Round (remainingTime).ToString ();

		//Time Over
		if (remainingTime <= 0.0f) {
			menuController.setState ("Victory");
				}

	}
}
