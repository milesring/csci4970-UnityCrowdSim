using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// This class encapsulates the logic of the queue itself. Its main function is to hold agents in
/// a FIFO list and slowly dequeue agents from the front of the queue.
/// </summary>
public class QueueLogic : MonoBehaviour {
	List<GameObject> queue;
	bool busy;

	// Initializes the class
	void Start () {
		queue = new List<GameObject> ();
	}

	/*
     * Update checks if the front of the queue is currently being "worked". If the queue is not
     * busy and the queue has another agent in it, begin working with the next agent.
     */
	void Update () {
		if (!busy && queue.Any()) {
			WorkWithSomeone ();
		}
	}

	/// <summary>
    /// Add agent to end of list
    /// </summary>
	public void Enqueue(GameObject agent){

		if (!queue.Any()) {
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

	// TODO Does this need to be public?
    // FIXME This needs functionality
    /// <summary>
    /// Dequeues the agent at the front of the queue, allowing all agents in queue to move up
    /// on position
    /// </summary>
	public void WorkWithSomeone(){
		busy = true;
		// wait for random amount of time then no longer busy

		//Dequeue();
	}

    /// <summary>
    /// Remove the agent at the front of the queue
    /// </summary>
	public void Dequeue(){
		queue [0].GetComponent<Navigation> ().AtGoal (false);
		queue.RemoveAt (0);
		if (queue.Any()) {
			queue [0].GetComponent<Navigation> ().AtGoal (true);
			queue [0].GetComponent<Navigation> ().InQueue (false);
		}

		busy = false;
	}

	/// <summary>
    /// Returns number of agents in queue
	/// TODO possibly of altering interest level with how many currently in queue
    /// </summary>
	public int Length(){
		return queue.Count;
	}

	//Returns position of last agent in line
	Vector3 getLast(){
		return queue [queue.Count-1].transform.position;
	}
}
