using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds metadata on the environment
/// </summary>
public class EventManager : MonoBehaviour {
	private float eventTime;
	private float eventTimer;
	private bool eventOverCalled;

	public AgentManager agentManager;

	// Use this for initialization
	void Start () {
		eventOverCalled = false;
		eventTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		eventTimer += Time.deltaTime;
		if (!eventOverCalled && eventTimer > eventTime) {
			agentManager.notifyAgents ();
			//Debug.Log ("Agents notified");
			eventOverCalled = true;
		}
			
	}

    /// <returns>true is the event is over, otherwise false</returns>
	public bool eventOver(){
		return eventOverCalled;
	}
}
