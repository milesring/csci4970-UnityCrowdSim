using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class POI : MonoBehaviour {
	public float interestLevel;
	public float turnSpeed = 2.0f;
	// Use this for initialization
	void Start () {
		interestLevel = Random.Range(0.0f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//Test to make sure collision is by agent with interest value
		if (other.gameObject.tag == "Agent" && other.gameObject.GetComponent<Navigation> ().distractionValue < interestLevel) {
			other.gameObject.GetComponent<Navigation> ().distract ();
			var NavMeshAgent = other.gameObject.GetComponent<NavMeshAgent> ();
		
			Vector3 targetDir = this.transform.position - other.gameObject.transform.position;
			float step = turnSpeed * Time.deltaTime;
			Vector3 newDir = Vector3.RotateTowards(other.gameObject.transform.forward, targetDir, step, 0.0F);
			//Debug.DrawRay(other.gameObject.transform.position, newDir, Color.red);
			transform.rotation = Quaternion.LookRotation(newDir);


			//assign new "destination"
			NavMeshAgent.destination = this.transform.position;
		} else if (other.gameObject.tag == "Agent") {
			//Debug.Log ("Agent is aware, but not interested " + other.gameObject.GetComponent<Navigation> ().stoppingValue + " vs " + interestLevel);
		}
	}
}
