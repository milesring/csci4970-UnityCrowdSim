using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class POI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//Test to make sure collision is by agent
		if (other.gameObject.tag == "Agent") {
			other.gameObject.GetComponent<Navigation> ().distract();
			var NavMeshAgent = other.gameObject.GetComponent<NavMeshAgent>();

			//assign new "destination"
			NavMeshAgent.destination = this.transform.position;
			//Debug.Log ("Agent redirected and held");
		}
	}
}
