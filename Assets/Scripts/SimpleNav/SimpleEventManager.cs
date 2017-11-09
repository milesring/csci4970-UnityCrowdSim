using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEventManager : MonoBehaviour {
	public float eventTime;
	private float eventTimer;
	private bool eventOverCalled;

	public AgentManager agentManager;
	[HideInInspector]public EventStatus eventStatus;
	// Use this for initialization
	void Start () {
		eventOverCalled = false;
		eventTimer = 0.0f;
		eventStatus = EventStatus.EventOnGoing;
	}

	// Update is called once per frame
	void Update () {
		eventTimer += Time.deltaTime;
		if (!eventOverCalled && eventTimer > eventTime) {
			eventStatus = EventStatus.EventOver;
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


public enum EventStatus
{
	EventOnGoing,
	EventOver,
	EventEmergency
}
