using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    // Use this for initialization
    AgentManager agentManager;

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
        if (level == 1)
        {
            agentManager = GameObject.Find("AgentManager").GetComponent<AgentManager>();
            agentManager.agentAmount = 50;
        }
            
    }

    void setNumAgents(int numAgents)
    {
        agentManager.agentAmount = numAgents;
    }

    void setSpeed(float speed)
    {
        GameObject[] agents = GameObject.FindGameObjectsWithTag("Agent");
        
        for(int i = 0; i < agents.Length; ++i)
        {
            agents[i].GetComponent<UnityEngine.AI.NavMeshAgent>().speed = speed;
        }
    }
}
