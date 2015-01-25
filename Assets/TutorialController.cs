using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialController : MonoBehaviour {

	public Image first;
	public Image second;
	public Image third;

	int current = 1;
	// Use this for initialization
	void Start () {
		Color c = second.color;
		c.a = 0.0f;
		second.color = c;

		c = third.color;
		c.a = 0.0f;
		third.color = c;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return))
		{
			if (current == 1) {
				Color c = first.color;
				c.a = 0.0f;
				first.color = c;

				c = second.color;
				c.a = 1.0f;
				second.color = c;
				current++;
			} else if(current == 2)
			{
				Color c = second.color;
				c.a = 0.0f;
				second.color = c;
				
				c = third.color;
				c.a = 1.0f;
				third.color = c;
				current++;
			} else if (current == 3)
			{
				Application.LoadLevel (1);
			}
		}

	}
}
