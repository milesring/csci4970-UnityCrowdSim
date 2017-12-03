using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Provides utility methods for working with all agents, as well as keeping metadata on the 
/// agents in the environment
/// </summary>
public class AgentManager : MonoBehaviour {
    /// <summary>
    /// The maximum number of agents allowed in the environment
    /// </summary>
	private int agentAmount;
	public float agentSpeed;
	private int agentCount;
	private EventManager eventManager;
	Settings settings;

	void Start () {
		agentCount = 0;
		agentSpeed = 0f;
		eventManager = GameObject.Find ("EventManager").GetComponent<EventManager> ();
		settings = GameObject.Find ("Settings").GetComponent<Settings> ();
		if (settings) {
			agentAmount = (int)settings.numAgents;
		} else {
			agentAmount = 30;
		}
		Debug.Log (agentAmount);
		Debug.Log ("Speed: " + settings.speed);
	}

    /// <summary>
    /// Increments the number of agents in the environment
    /// </summary>
	public void agentSpawned(){
		agentCount++;
	}

    /// <summary>
    /// Decrements the number of agents in the environment
    /// </summary>
	public void agentDestroyed(){
		agentCount--;
	}
    
    /// <returns>true if agent generation is allowed, otherwise false</returns>
	public bool spawnAllowed(){
        if (agentCount >= agentAmount || eventManager.IsEventOver) {
            return false;
        } else {
            return true;
        }
    }

    /// <summary>
    /// Notifies all agents that the event has ended
    /// </summary>
	public void notifyAgents(bool emergency){
		GameObject[] agents = GameObject.FindGameObjectsWithTag ("Agent");
		for (int i = 0; i < agents.Length; ++i)
        {
            agents [i].GetComponent<Navigation> ().endEvent ();
            // speeds up by randomly by 1.5x minimum, 3x maximum
            if (emergency) {
                agents[i].GetComponent<NavMeshAgent>().speed *= (Random.value + 1) * 1.5f;
				agents [i].GetComponent<Navigation> ().emergency = true;
            }
            eventManager.IsEventOver = true;
		}

	}

    /// <summary>
    /// Destroys all existing agents
    /// </summary>
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
