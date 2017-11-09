using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour {

	public LocationManager locationManager;
	private GameObject currentDestination;
	void Start(){
		//locationManager = GameObject.Find ("LocationManager").GetComponent<LocationManager> ();
		currentDestination = gameObject;	
	}

	public void Navigate(NavMeshAgent navAgent, Actions agentActions){
		GameObject newDest = locationManager.NextLocation (agentActions);
		if (newDest == null) {
			Debug.Log ("new dest = null");
		}
		currentDestination = newDest;
		navAgent.destination = currentDestination.transform.position;
		navAgent.isStopped = false;
	}

	public bool CheckReachedDestination(NavMeshAgent navAgent){
		return navAgent.remainingDistance <= navAgent.stoppingDistance && !navAgent.pathPending;
	}		

	public GameObject GetDestinationObject(){
		return currentDestination;
	}


}
