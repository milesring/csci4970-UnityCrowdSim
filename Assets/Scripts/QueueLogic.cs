using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// This class encapsulates the logic of the queue itself. Its main function is to hold agents in
/// a FIFO list and slowly dequeue agents from the front of the queue.
/// TODO possibly alter interest level based on how many agents currently in queue
/// </summary>
public class QueueLogic : MonoBehaviour {
    /// <summary>
    /// The name of the queue- used for debugging purposes
    /// </summary>
    public string queueName;

    List<GameObject> queue;
    GameObject currentAgent;
	bool busy;

    /// How long it takes for the agent at front of queue to complete its task.
    public float maxQueueWorkTime;
    float queueWorkTime;

	// Initializes the class
	void Start () {
		queue = new List<GameObject> ();
        queueWorkTime = 0.0f;
	}

	/*
     * Update checks if the front of the queue is currently being "worked". If the queue is not
     * busy and the queue has another agent in it, begin working with the next agent.
     */
	void Update () {
        if (busy) {
            queueWorkTime += Time.deltaTime;
            if (queueWorkTime >= maxQueueWorkTime) {
                FinishWork();
            }
        }

        // We have possibly finished with an agent above- check again if the queue is busy
        if (!busy && currentAgent != null) {
            busy = true;
		}

        foreach (GameObject agent in queue) {
            agent.GetComponent<Navigation>().UpdateLookRotation();
        }
	}

    private void FinishWork() {
        Debug.Log("Queue " + queueName + " finished with an agent.");
        busy = false;
        Dequeue();
        queueWorkTime = 0.0f;
    }

    /// <summary>
    /// Add agent to end of the FIFO list
    /// </summary>
    public void Enqueue(GameObject agent) {
        Navigation newAgent = agent.GetComponent<Navigation>();

        if (currentAgent == agent) {
            //Do nothing, agent already accounted for
            Debug.Log(string.Format("Invalid enqueue command- agent {0} already in queue.", newAgent.AgentName));
        } else {
            if (queue.Count == 0 && !busy) {
                currentAgent = agent;
                newAgent.SetDestination(this.transform.position, false);
                newAgent.BeingServed = true;
                busy = true;

                Debug.Log(newAgent.AgentName + " is being served at front of queue " + queueName);
            } else {
                newAgent.setInQueue(true);
                newAgent.StopAgent();
                queue.Add(agent);
                
                if (queue.Count == 1) {
                    // Position new agent behind the agent being served;
                    newAgent.SetDestination(currentAgent.transform.position, true);
                } else {
                    // Position new agent behind the last agent in queue
                    newAgent.SetDestination(getLast(), true);
                }
                
                Debug.Log(newAgent.AgentName + " added at position " + (queue.Count - 1) + " of list.");
            }
        }
    }

    // Remove the agent at the front of the queue
    private void Dequeue()
    {
        Navigation finishedAgent = currentAgent.GetComponent<Navigation>();
        // TODO Testing purposes only- remove for other scenes. Don't like modifying agent goal from here
        finishedAgent.AgentOfDestruction();
        finishedAgent.AtGoal = false;
        finishedAgent.BeingServed = false;
        currentAgent = null;

        if (queue.Count > 0) {
            currentAgent = queue[0];
            Navigation currentAgentNav = currentAgent.GetComponent<Navigation>();
            Debug.Log("Pulling agent " + currentAgentNav.AgentName + " to be served at front of queue.");

            currentAgentNav.SetDestination(this.transform.position, false);
            currentAgentNav.ResumeAgentSpeed();
            currentAgentNav.BeingServed = true;
            currentAgentNav.setInQueue(false);
            queue.RemoveAt(0);

            foreach (GameObject agent in queue) { 
                // Tell remaining queued agent to move forward in 
                Navigation agentNav = agent.GetComponent<Navigation>();
                Debug.Log("Queue ordering " + agentNav.AgentName + " to move and face goal");
                agent.GetComponent<Navigation>().ResumeAgentSpeed();
            }
        }
    }

	//Returns position of last agent in line
	Vector3 getLast(){
		return queue[queue.Count-1].transform.position;
	}
}
