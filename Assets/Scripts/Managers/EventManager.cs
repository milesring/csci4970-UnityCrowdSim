using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds metadata on the environment
/// </summary>
public class EventManager : MonoBehaviour {
	public float eventTime;
	private float eventTimer;

    public bool emergency
    {
        get;
        private set;
    }

    /// <returns>true is the event is over, otherwise false</returns>
    public bool IsEventOver
    {
        get;
        internal set;
    }

    public AgentManager agentManager;

	// Use this for initialization
	void Start () {
        IsEventOver = false;
		eventTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		eventTimer += Time.deltaTime;
		if (!IsEventOver && eventTimer > eventTime) {
			agentManager.notifyAgents (false);
		}
	}

    public float getEventTimer() {
        return eventTimer;
    }

    public float getEventTime() {
        return eventTime;
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
