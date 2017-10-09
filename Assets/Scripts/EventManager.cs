using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
	public float eventTime;
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
			Debug.Log ("Agents notified");
			eventOverCalled = true;
		}

		if(eventOverCalled){
			//agentManager.destroyAgents ();
		}
	}
}
