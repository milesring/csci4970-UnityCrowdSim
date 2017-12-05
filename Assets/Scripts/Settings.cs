using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    // Use this for initialization
    AgentManager agentManager;
	public float numAgents;
	public float speed;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	public void setNumAgents(UnityEngine.UI.Slider slider)
    {
		numAgents = slider.value;
    }

	public void setSpeed(UnityEngine.UI.Slider slider)
    {
		speed = slider.value;
    }
}
