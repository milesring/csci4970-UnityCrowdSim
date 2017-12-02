using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    // Use this for initialization
    AgentManager agentManager;
	float numAgents;
	float speed;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnLevelWasLoaded(int level)
    {
		if (level == 1) {
			agentManager = GameObject.Find ("AgentManager").GetComponent<AgentManager> ();
			agentManager.agentAmount = (int)numAgents;
		}
            
    }

    public void setNumAgents(float value)
    {
		numAgents = value;
    }

    public void setSpeed(float value)
    {
		speed = value;
    }
}
