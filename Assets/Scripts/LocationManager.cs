using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour {
	private GameObject[] entrances;
	private GameObject[] exits;
	private GameObject[] goals;
	private GameObject center;

	// Use this for initialization
	void Start () {
		entrances = findLocations ("Entrance");
		exits = findLocations ("Exit");
		goals = findLocations ("Goal");
		center = GameObject.Find ("BuildingCenter");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	GameObject[] findLocations(string tag){
		//Finds all locations with the passed in tag
		GameObject[] tempArray = GameObject.FindGameObjectsWithTag (tag);
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


	//WIP
	public float activeRadius(){

		return 0.0f;
	}
}
