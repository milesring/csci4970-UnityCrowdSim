using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Defines behavior of agent when it collides with other game objects
/// </summary>
public class AgentCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Agent")
            && !this.gameObject.GetComponent<Navigation>().IsInQueue()
            && other.gameObject.GetComponent<Navigation>().IsAtGoal()
            && other.gameObject.GetComponent<Navigation>().GetDestination() == this.gameObject.GetComponent<Navigation>().GetDestination()) {
            Debug.Log("Agents have same goal finding position in line");

            this.gameObject.GetComponent<Navigation>().GetDestination().GetComponent<QueueLogic>().Enqueue(this.gameObject);

        }
	}
}
