using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys agents when they come in contact with the parent object's collider
/// </summary>
public class AgentDestroyer : MonoBehaviour {
    EventManager eventManager;
	AgentManager agentManager;

	void Start(){
		eventManager = GameObject.Find ("EventManager").GetComponent<EventManager>();
		agentManager = GameObject.Find ("AgentManager").GetComponent<AgentManager> ();
	}

	void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Agent")) {
            Navigation agent = other.gameObject.GetComponent<Navigation>();
            if (eventManager.eventOver || agent.IsLeaving()) {
                Destroy(other.gameObject);
                agentManager.agentDestroyed();
            }
        }
	}
}
