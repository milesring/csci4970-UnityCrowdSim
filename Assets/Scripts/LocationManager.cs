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
	//private GameObject[] all;

	// Use this for initialization
	void Awake () {
		entrances = FindLocations("Entrance");
		exits = FindLocations("Exit");
		goals = FindLocations("Goal");
		/*all = new GameObject[entrances.Length + exits.Length];
		System.Array.Copy (entrances, 0, all, 0, entrances.Length);
		System.Array.Copy (exits, 0, all, entrances.Length, exits.Length);
		*/

	}

	GameObject[] FindLocations(string tag){
		//Finds all locations with the passed in tag
		return GameObject.FindGameObjectsWithTag (tag);
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

	//returns nearest unless at goal, this will return a random goal
	public GameObject NextLocation(Actions agentAction){
		GameObject[] locations = GetLocations (agentAction);
		GameObject nearest = null;
		if (locations != null) {
			//agent not at goal or waiting for goal
			if (!agentAction.Equals (Actions.AtGoal) && !agentAction.Equals(Actions.Idle)) {
				//calculate nearest
				foreach (GameObject location in locations) {
					if (nearest == null) {
						nearest = location;
					} else {
						float magnitudeToAltLocation
						= (location.transform.position - this.transform.position).magnitude;
						float magnitudeToCurrNearestLocation
						= (nearest.transform.position - this.transform.position).magnitude;

						if (magnitudeToAltLocation < magnitudeToCurrNearestLocation) {
							nearest = location;
						}
					}
				}
			} else {
				//random goal
				return locations [Random.Range (0, locations.Length)];
			}
		}
		return nearest;

	}

	public GameObject[] GetLocations(Actions agentAction){
		switch(agentAction){
		case Actions.FindingEntrance:
			return entrances;
		case Actions.FindingGoals:
		case Actions.Idle:
			return goals;
		case Actions.FindingExits:
			return exits;
		case Actions.Emergency:
			//return all;
		default:
			return null;
		}
	}
}
