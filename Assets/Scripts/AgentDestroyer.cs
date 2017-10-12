using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDestroyer : MonoBehaviour {
	EventManager eventManager;
	AgentManager agentManager;

	void Start(){
		eventManager = GameObject.Find ("EventManager").GetComponent<EventManager>();
		agentManager = GameObject.Find ("AgentManager").GetComponent<AgentManager> ();
	}
	void OnTriggerEnter(Collider other){
		//Debug.Log ("Collider entered. event over= "+eventManager.eventOver());
		//Debug.Log("Compare: "+other.gameObject.CompareTag("Agent"));
		if(eventManager.eventOver() && other.gameObject.CompareTag("Agent")){
			Destroy (other.gameObject);
			agentManager.agentDestroyed ();
		}
	}
}
