using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides utility methods for working with all agents, as well as keeping metadata on the 
/// agents in the environment
/// </summary>
public class AgentManager : MonoBehaviour {
    /// <summary>
    /// The maximum number of agents allowed in the environment
    /// </summary>
	public int agentAmount;
	private int agentCount;
	SimpleEventManager eventManager;

	// Use this for initialization
	void Start () {
		agentCount = 0;
		eventManager = GameObject.Find ("SimpleEventManager").GetComponent<SimpleEventManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Increments the number of agents in the environment
    /// </summary>
	public void agentSpawned(){
		agentCount++;
	}

    /// <summary>
    /// Decrements the number of agents in the environment
    /// </summary>
	public void agentDestroyed(){
		agentCount--;
	}
    
    /// <returns>true if agent generation is allowed, otherwise false</returns>
	public bool spawnAllowed(){
        if (agentCount >= agentAmount || eventManager.eventOver()) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Notifies all agents that the event has ended
    /// </summary>
	public void notifyAgents(){
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Agent");
		for (int i = 0; i < agents.Length; ++i) {
			//agents [i].GetComponent<Navigation> ().endEvent ();
			agents[i].GetComponent<SimpleNavigation1>().ExternalUpdateEvent(Status.EventOver);
			agents [i].GetComponent<SimpleNavigation1> ().ExternalUpdateAction (Actions.FindingExits);
			//Debug.Log ("Agent " + i + " notified of event ending");
		}
	}

    /// <summary>
    /// Destroys all existing agents
    /// </summary>
	public void destroyAgents(){
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Agent");
		for (int i = 0; i < agents.Length; ++i) {
			if (agents [i].GetComponent<Navigation> ().isOutside()) {
				Destroy (agents [i].gameObject);
			}
			//Debug.Log ("Agent " + i + " notified of event ending");
		}
	}

	public int AgentCount(){
		return agentCount;
	}
}
