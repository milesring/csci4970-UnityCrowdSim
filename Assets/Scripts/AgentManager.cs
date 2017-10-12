using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour {


	public int agentAmount;
	private int agentCount;
	EventManager eventManager;

	// Use this for initialization
	void Start () {
		agentCount = 0;
		eventManager = GameObject.Find ("EventManager").GetComponent<EventManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void agentSpawned(){
		agentCount++;
	}

	public void agentDestroyed(){
		agentCount--;
	}

	public bool spawnAllowed(){
		if (agentCount >= agentAmount || eventManager.eventOver()) {
			return false;
		}

		return true;
	}

	public void notifyAgents(){
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Agent");
		for (int i = 0; i < agents.Length; ++i) {
			agents [i].GetComponent<Navigation> ().endEvent ();
			//Debug.Log ("Agent " + i + " notified of event ending");
		}
	}

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
