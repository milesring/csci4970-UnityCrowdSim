using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// This class encapsulates the logic of the queue itself. Its main function is to hold agents in
/// a FIFO list and slowly dequeue agents from the front of the queue.
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
                // TODO Agent is complete with work. Dequeue agent (issue command to continue
                // to goal) and signal the queue is ready for another agent (either send signal
                // to agent in front of queue, or make `busy` public and have agents check the bool.
                // The latter is the preferred approach so that all agents in queue know to move?)
                FinishWork();
            }
        }

        // We have possibly finished with an agent above- check again if the queue is busy
        if (!busy && currentAgent != null) {
            busy = true;
		}
	}

    private void FinishWork() {
        Debug.Log("Queue " + queueName + " finished with an agent.");
        busy = false;
        Dequeue();
        queueWorkTime = 0.0f;
    }

    /// <summary>
    /// TODO An agent at the front of the queue IS in the queue still. `InQueue(false)` should
    ///     only be called from Dequeue
    /// Add agent to end of the FIFO list
    /// </summary>
    public void Enqueue(GameObject agent) {
        Navigation newAgent = agent.GetComponent<Navigation>();

        // Check if the passed agent is already accounted for
        if (currentAgent != null && newAgent.AgentName == currentAgent.GetComponent<Navigation>().AgentName) {
            //Do nothing, agent already acounted for
        } else {
            if (queue.Count == 0 && !busy) {
                currentAgent = agent;
                currentAgent.GetComponent<NavMeshAgent>().destination = this.transform.position;
                busy = true;

                Debug.Log(newAgent.AgentName + " is being served at front of queue " + queueName);
            } else {
                newAgent.InQueue = true;
                //agent.GetComponent<NavMeshAgent>().destination = getLast();
                newAgent.StopAgent();
                queue.Add(agent);
                Debug.Log(newAgent.AgentName + " added at position " + (queue.Count - 1) + " of list.");
            }
        }
    }

    /// <summary>
    /// TODO Does this need to be public? Other elements should not be able to dequeue agents
    /// Remove the agent at the front of the queue
    /// </summary>
	public void Dequeue() {
        Navigation finishedAgent = currentAgent.GetComponent<Navigation>();
        // TODO Testing purposes only- remove for other scenes. Don't like modifying agent goal from here
        finishedAgent.AgentOfDestruction();
        finishedAgent.AtGoal = false;
        currentAgent = null;

        if (queue.Count > 0) {
            currentAgent = queue[0];
            currentAgent.GetComponent<NavMeshAgent>().destination = this.transform.position;
            currentAgent.GetComponent<Navigation>().ResumeAgentSpeed();
            currentAgent.GetComponent<Navigation>().InQueue = false;
            queue.RemoveAt(0);

            foreach (GameObject agent in queue) {
                // Tell remaining queued agent to move forward in line
                agent.GetComponent<Navigation>().ResumeAgentSpeed();
            }
        }
	}

	/// <summary>
    /// Returns number of agents in queue
	/// TODO possibly alter interest level based on how many agents currently in queue
    /// </summary>
	public int Length(){
		return queue.Count;
	}

	//Returns position of last agent in line
	Vector3 getLast(){
		return queue [queue.Count-1].transform.position;
	}
}
