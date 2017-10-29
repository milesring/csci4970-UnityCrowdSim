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

	void OnTriggerEnter(Collider other) {
        Navigation thisAgent = this.gameObject.GetComponent<Navigation>();
        if (thisAgent.AtGoal) {
            // This agent is already at its goal and does not care about collisions
        } else if (other.gameObject.CompareTag("Agent")) {
            Navigation otherAgent = other.gameObject.GetComponent<Navigation>();

            /*
             * If the colliding object is an agent
             * AND this object is not already in a queue
             * AND the colliding object is at its goal OR is in a queue
             * AND the colliding object's destination is the same as this object's destination
             * THEN enqueue this object in the destination's queue
             */
            if (!thisAgent.InQueue
                   && (otherAgent.AtGoal || otherAgent.InQueue)
                   && otherAgent.GetDestination() == thisAgent.GetDestination()) {
                Debug.Log(thisAgent.AgentName + " and " + otherAgent.AgentName
                    + " have same goal. Adding agent " + thisAgent.AgentName + " in queue behind "
                    + otherAgent.AgentName);

                thisAgent.GetDestination().GetComponent<QueueLogic>().Enqueue(this.gameObject);
            }
        }
	}
}
