using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {

	public float speed = 15.0f;
	public float speedImpactValue = 6.0f;
	public static int currentZone;
	public float zoneCooldown;
	public float defaultCooldown;

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
	float speedModifier = 0.0f;

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
				currentZoneCooldown = defaultCooldown / getRealSpeed();
			}
			else 
			{
				currentZone = ZONES[Random.Range (0, ZONES.Length)];
				currentZoneCooldown = zoneCooldown / getRealSpeed();
			}
		}
		//Impact deceleration
		speedModifier -= Time.deltaTime;
		if (speedModifier < 0.0f) {
			speedModifier = 0.0f;
		}
	}

	public void impactSpeed()
	{
		speedModifier = speed * 0.9f;
	}

	public float getRealSpeed()
	{
		//print (speed.ToString () + "   " + (speed - speedModifier).ToString ());
		return speed - speedModifier;
	}
}
