using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// A debug class used to visualize the destination of an agent
/// </summary>
public class DestinationVisualization : MonoBehaviour {
	NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		agent = this.gameObject.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine (this.gameObject.transform.position, agent.destination);
	}
}
