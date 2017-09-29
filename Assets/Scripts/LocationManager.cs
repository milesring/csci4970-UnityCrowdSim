using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour {
	private GameObject[] entrances;
	private GameObject[] exits;
	private GameObject[] goals;

	// Use this for initialization
	void Start () {
		entrances = findLocations ("Entrance");
		exits = findLocations ("Exit");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameObject[] findLocations(string tag){
		//Finds all locations with the passed in tag
		GameObject[] tempArray = GameObject.FindGameObjectsWithTag (tag);
		if (tempArray != null) {
			for (int i = 0; i < tempArray.Length; ++i) {
				GameObject tempObject = tempArray [i];
				//Debug.Log (tag +" found at: " + tempObject.transform.position.x + ", " + tempObject.transform.position.y + ", " + tempObject.transform.position.z);
			}
		} else {
			Debug.Log ("No "+tag+"s found");
		}
		return tempArray;
	}

	public GameObject[] getLocations(string tag){
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
}
