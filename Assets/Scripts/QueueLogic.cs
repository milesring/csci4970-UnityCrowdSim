using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class encapsulates the logic of the queue itself. Its main function is to hold agents in
/// a FIFO list and slowly dequeue agents from the front of the queue.
/// TODO possibly alter interest level based on how many agents currently in queue
/// </summary>
public class QueueLogic : MonoBehaviour, IQueue {
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

	void Start () {
		queue = new List<GameObject> ();
        queueWorkTime = 0.0f;
	}

	/*
     * Update checks if the front of the queue isBeingServed. If the queue is not busy and the queue 
     * has another agent in it, begin serving the next agent.
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

        // Update each queued agent's look rotation to face the queue
        foreach (GameObject agent in queue) {
            agent.GetComponent<Navigation>().UpdateLookRotation();
        }
	}

    private void FinishWork() {
        Log("Finished with an agent.");
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
        } else {
            if (queue.Count == 0 && !busy) {
                currentAgent = agent;
                newAgent.SetDestination(this.transform.position, false);
                newAgent.BeingServed = true;
                busy = true;

                Log(newAgent.AgentName + " is being served at front of queue.");
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

                Log(newAgent.AgentName + " added at position " + (queue.Count - 1) + " of queue.");
            }
        }
    }

    // Remove the agent at the front of the queue
    private void Dequeue() {
        Navigation finishedAgent = currentAgent.GetComponent<Navigation>();
        finishedAgent.BeingServed = false;
        finishedAgent.ResumeAgentSpeed();
        currentAgent = null;

        if (queue.Count > 0) {
            currentAgent = queue[0];
            Navigation currentAgentNav = currentAgent.GetComponent<Navigation>();
            Log("Pulling agent " + currentAgentNav.AgentName + " to be served at front of queue.");

            currentAgentNav.SetDestination(this.transform.position, false);
            currentAgentNav.ResumeAgentSpeed();
            currentAgentNav.BeingServed = true;
            currentAgentNav.setInQueue(false);
            queue.RemoveAt(0);

            foreach (GameObject agent in queue) { 
                // Tell remaining queued agent to move forward in 
                Navigation agentNav = agent.GetComponent<Navigation>();
                agent.GetComponent<Navigation>().ResumeAgentSpeed();
            }
        }
    }

    /// <summary>
    /// Only call if emergency signaled or the event ends.
    /// </summary>
    public void DequeueAll() {
        Log("Dequeuing all " + queue.Count + " agents.");
        while (queue.Count > 0) {
            Dequeue();
        }
    }

    /// <returns>the name of this queue</returns>
    public string GetQueueName() {
        return queueName;
    }

    // Returns position of last agent in line
    private Vector3 getLast(){
		return queue[queue.Count-1].transform.position;
	}

    // Convenience method that names the queue before logging the message
    private void Log(string message) {
        Debug.Log(string.Format("{0}: {1}", queueName, message));
    }
}
