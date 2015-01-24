using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {

	public static float speed = 15.0f;
	public static int currentZone;
	public float zoneCooldown;

	//Zones constants
	public const int RETIREMENTHOME = 0;
	public const int SCHOOL = 1;
	public const int POLICESTATION = 2;
	public const int DEFAULT = 3;
	public static int[] ZONES = {RETIREMENTHOME, SCHOOL, POLICESTATION, DEFAULT};
	public static float[] zonesFreq = {0.15f, 0.15f, 0.15f, 0.55f};
	//Pedestrians constants
	public const int OLDMAN = 0;
	public const int CHILD = 1;
	public const int OFFICER = 2;
	public const int INNOCENT = 3;
	public static int[] PEDESTRIANS = {OLDMAN, CHILD, OFFICER, INNOCENT};
	public static float[,] pedestriansFreq = {
		{0.67f, 0.11f, 0.11f, 0.11f},
		{0.11f, 0.67f, 0.11f, 0.11f},
		{0.0f, 0.0f, 1.0f, 0.0f},
		{0.25f, 0.25f, 0.25f, 0.25f},
	};
	public static float[] pedestriansScore = {10.0f, 30.0f, 20.0f, 20.0f};

	float currentZoneCooldown;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentZoneCooldown -= Time.deltaTime;
		if (currentZoneCooldown <= 0.0f)
		{
			if(currentZone != DEFAULT)
			{ 
				currentZone = DEFAULT;
			}
			else 
			{
				currentZone = ZONES[Random.Range (0, ZONES.Length)];
			}

			currentZoneCooldown = zoneCooldown;
		}
	}
}
