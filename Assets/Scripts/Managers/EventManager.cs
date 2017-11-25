using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds metadata on the environment
/// </summary>
public class EventManager : MonoBehaviour {
	public float eventTime;
	private float eventTimer;
	private bool eventOverCalled;
    public bool emergency
    {
        get;
        private set;
    }

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
			agentManager.notifyAgents (false);
        
			//Debug.Log ("Agents notified");
		}
	}

    public float getEventTimer() {
        return eventTimer;
    }

    public float getEventTime() {
        return eventTime;
    }

    /// <returns>true is the event is over, otherwise false</returns>
	public bool eventOver {
		get { return eventOverCalled; }
        set { eventOverCalled = value; }
	}

    internal void SignalEmergency() {
        emergency = true;
        LocationManager locationManager = GameObject.Find("LocationManager").GetComponent<LocationManager>();
        List<GameObject> goals = locationManager.GetLocations(LocationTypes.GOAL);
        foreach (GameObject goal in goals) {
            IQueue queue = goal.GetComponent<IQueue>();
            if (queue != null) {
                queue.DequeueAll();
            }
        }
    }
}
