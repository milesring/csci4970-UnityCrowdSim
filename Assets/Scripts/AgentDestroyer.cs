using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Destroys agents when they come in contact with the parent object's collider
/// </summary>
public class AgentDestroyer : MonoBehaviour {
    public bool alwaysDestroy;

    EventManager eventManager;
	AgentManager agentManager;

	void Start(){
		eventManager = GameObject.Find ("EventManager").GetComponent<EventManager>();
		agentManager = GameObject.Find ("AgentManager").GetComponent<AgentManager> ();
	}
	void OnTriggerEnter(Collider other){
        //Debug.Log ("Collider entered. event over= "+eventManager.eventOver());
        //Debug.Log("Compare: "+other.gameObject.CompareTag("Agent"));
        if (other.gameObject.CompareTag("Agent")) {
            if (alwaysDestroy) {
                Destroy(other.gameObject);
                agentManager.agentDestroyed();
            } else if (eventManager.eventOver) {
                Destroy(other.gameObject);
                agentManager.agentDestroyed();
            }
        }
	}
}
