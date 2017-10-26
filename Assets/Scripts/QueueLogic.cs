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
            }
        }

        // We have possibly finished with an agent above- check again if the queue is busy
        if (!busy && queue.Count > 0) {
            // TODO pull agent to front of queue
			WorkWithSomeone ();
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
	public void Enqueue(GameObject agent){
		if (queue.Count == 0) {
            //TODO this should be accessed via an accessor, NOT a direct call
			agent.GetComponent<NavMeshAgent> ().destination = this.transform.position;
			agent.GetComponent<Navigation> ().AtGoal (true);
			agent.GetComponent<Navigation> ().InQueue (false);

			Debug.Log ("Agent added to front of list");
		} else {
			agent.GetComponent<NavMeshAgent> ().destination = getLast ();
			agent.GetComponent<Navigation> ().InQueue (true);
			agent.GetComponent<Navigation> ().AtGoal (false);
			agent.GetComponent<NavMeshAgent> ().speed = 0.0f;
			Debug.Log ("Agent added to last position of list");
		}

		queue.Add (agent);
	}

    /// <summary>
    /// Dequeues the agent at the front of the queue, allowing all agents in queue to move up
    /// on position.
    ///
    /// FIXME This needs functionality
    /// </summary>
	private void WorkWithSomeone(){
		busy = true;
		// wait for random amount of time then no longer busy

		//Dequeue();
	}

    /// <summary>
    /// TODO Does this need to be public? Other elements should not be able to dequeue agents
    /// Remove the agent at the front of the queue
    /// </summary>
	public void Dequeue(){
		queue [0].GetComponent<Navigation> ().AtGoal (false);
		queue.RemoveAt (0);
		if (queue.Count > 0) {
			queue [0].GetComponent<Navigation> ().AtGoal (true);
			queue [0].GetComponent<Navigation> ().InQueue (false);
		}

		busy = false;
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
