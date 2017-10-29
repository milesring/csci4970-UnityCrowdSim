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
        Navigation thisAgent = this.gameObject.GetComponent<Navigation>();
        Navigation otherAgent = other.gameObject.GetComponent<Navigation>();
        /*
         * If the colliding object is an agent
         * AND this object is not in a queue
         * AND the colliding object is at its goal
         * AND the colliding object's destination is the same as this object's destination
         * THEN enqueue this object in the destination's queue
         */
        if (other.gameObject.CompareTag("Agent")
            && !thisAgent.InQueue
            && (otherAgent.AtGoal || otherAgent.InQueue)
            && otherAgent.GetDestination()
                == thisAgent.GetDestination()) {
            Debug.Log(thisAgent.AgentName + " and " + otherAgent.AgentName
                + " have same goal. Adding agent " + thisAgent.AgentName + " in queue behind " 
                + otherAgent.AgentName);

            this.gameObject.GetComponent<Navigation>().GetDestination()
                .GetComponent<QueueLogic>().Enqueue(this.gameObject);

        }
	}
}
