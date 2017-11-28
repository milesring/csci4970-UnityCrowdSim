using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages a variety of locations and provides ease of access to the lists
/// </summary>
public class LocationManager : MonoBehaviour {
    private List<GameObject> entrances;
	private List<GameObject> exits;
	private List<GameObject> goals;
	public float centerRadius;
	private GameObject center;

	// Use this for initialization
	void Start () {
		entrances = FindLocations(LocationTypes.ENTRANCE);
		exits = FindLocations(LocationTypes.EXIT);
		goals = FindLocations(LocationTypes.GOAL);
		center = GameObject.Find (LocationTypes.BUILDING_CENTER.ToString());
	}

    //Finds all locations with the passed in tag, then converts the returned array to a list
    private List<GameObject> FindLocations(LocationTypes type) {
        return new List<GameObject>(GameObject.FindGameObjectsWithTag(type.ToString()));
	}

    /// <summary>
    /// Given a type, return a list of the location type
    /// </summary>
    /// <param name="type">the type of location to return</param>
    /// <returns>a list of tag-locations</returns>
	public List<GameObject> GetLocations(LocationTypes type) {
        if (type == LocationTypes.ENTRANCE) {
            return new List<GameObject>(entrances);
        } else if (type == LocationTypes.GOAL) {
            return new List<GameObject>(goals);
        } else if (type == LocationTypes.EXIT) {
            return new List<GameObject>(exits);
        } else {
            Debug.Log("Error choosing a location list to return.");
            return null;
        }
	}

    /// <summary>
    /// Given an agent's position, find the nearest location that will destroy the agent's GameObject
    /// </summary>
    /// <param name="agentPos">an agent's position</param>
    /// <returns>the nearest location that will destroy the agent</returns>
	public Vector3 FindNearestDestroyRadius(Vector3 agentPos){
		Vector3 heading = agentPos - center.transform.position;
		float distance = heading.magnitude;
		Vector3 direction = heading / distance;

		return direction*centerRadius;
	}
}
