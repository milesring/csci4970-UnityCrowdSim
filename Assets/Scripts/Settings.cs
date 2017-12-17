using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {

    // Use this for initialization
    AgentManager agentManager;
	public float numAgents;
	public float speed;
	GameObject agent;
	List<GameObject> agents;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Start(){
		agent = GameObject.FindGameObjectWithTag ("Agent");
		agents = new List<GameObject> ();
		agents.Add (agent);
	}

	public void setNumAgents(UnityEngine.UI.Slider slider)
    {
		numAgents = slider.value;
		UpdateList ();
    }

	public void setSpeed(UnityEngine.UI.Slider slider)
    {
		speed = slider.value;
    }

	void UpdateList(){
		while (true) {
			if (numAgents != agents.Count) {
				if (numAgents > agents.Count) {
					GameObject newAgent = Instantiate (agent, agent.transform.position, Quaternion.Euler(new Vector3(0f,180f,0f)));
					newAgent.transform.position = new Vector3 (Random.Range(-3.55f,0.47f), agent.transform.position.y, Random.Range(-93.56f,-88.24f));
					agents.Add (newAgent);
				} 

				if (numAgents < agents.Count) {
					GameObject removeAgent = agents [agents.Count - 1];
					agents.Remove (removeAgent);
					Destroy (removeAgent);
				}
			} else {
				break;
			}
		}
	}
}
