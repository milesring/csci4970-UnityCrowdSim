using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterConcroller : MonoBehaviour {

	// Use this for initialization
	void Start () {

        GetComponent<NavMeshAgent>().destination = new Vector3(0, 1, 10);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
