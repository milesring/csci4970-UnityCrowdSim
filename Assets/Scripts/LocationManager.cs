using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages a variety of important locations and provides ease of access to the arrays
/// </summary>
public class LocationManager : MonoBehaviour {
	private GameObject[] entrances;
	private GameObject[] exits;
	private GameObject[] goals;
	public float centerRadius;
	private GameObject center;

	// Use this for initialization
	void Start () {
		entrances = FindLocations("Entrance");
		exits = FindLocations("Exit");
		goals = FindLocations("Goal");
		center = GameObject.Find ("BuildingCenter");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameObject[] FindLocations(string tag){
		//Finds all locations with the passed in tag
		GameObject[] tempArray = GameObject.FindGameObjectsWithTag (tag);
		return tempArray;
	}

    /// <summary>
    /// Given a tag, return an array of the tag's location type
    /// </summary>
    /// <param name="tag">the type of location to return</param>
    /// <returns>an array of tag-locations</returns>
	public GameObject[] GetLocations(string tag){
		switch(tag){
		case "Entrance":
			return entrances;
		case "Goal":
			return goals;
		case "Exit":
			return exits;
		default:
			return null;
		}
	}

    /// <summary>
    /// Given an agent's position, find the nearest location that will destroy the agent's
    /// GameObject
    /// </summary>
    /// <param name="agentPos">an agent's position</param>
    /// <returns>the nearest location that will destroy the agent</returns>
	public Vector3 FindNearestDestroyRadius(Vector3 agentPos){
		Vector3 heading = agentPos - center.transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		//Debug.Log ("Direction: " + (direction*centerRadius));
		//Vector3 destroyPoint = -heading * centerRadius;
		return direction*centerRadius;
	}
}
