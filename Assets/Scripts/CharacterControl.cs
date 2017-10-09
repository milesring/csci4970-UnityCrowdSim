using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// TODO I am unsure if this class is used anywhere
// Attempting to use this to control a single agent, not going to spend much time on this.

public class CharacterControl : MonoBehaviour {

	public Camera characterCamera;

	// Use this for initialization
	void Start () {
        
	}

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			Vector3 worldPos = new Vector3 ();
	
			worldPos = characterCamera.ScreenToWorldPoint (Input.mousePosition);
			Debug.Log ("screen pos: " + Input.mousePosition);
			Debug.Log ("World pos: " + worldPos);
			GetComponent<NavMeshAgent>().destination = worldPos;
		}
	 	
	}
}
