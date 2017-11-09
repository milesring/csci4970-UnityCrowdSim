using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

/// <summary>
/// This class encapsulates the logic of the queue itself. Its main function is to hold agents in
/// a FIFO list and slowly dequeue agents from the front of the queue.
/// TODO possibly alter interest level based on how many agents currently in queue
/// </summary>
public class SimpleQueueLogic : MonoBehaviour {
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
		

		if (currentAgent == agent) {
			//Do nothing, agent already accounted for
		} else {
			//Queue is empty
			queue.Add(agent);
			agent.GetComponent<SimpleNavigation1> ().ExternalUpdateAction (Actions.InQueue);
			//position agent at getLast pos;
			if (queue.Count == 1 && !busy) {
				currentAgent = agent;
				busy = true;
				Debug.Log("Agent is being served at front of queue " + queueName);
			} 
			//Queue is not empty
			else {
				Debug.Log("Agent added at position " + (queue.Count - 1) + " of list.");
			}
		}
	}

	// Remove the agent at the front of the queue
	private void Dequeue()
	{
		queue [0].GetComponent<SimpleNavigation1> ().ExternalUpdateAction (Actions.FindingGoals);
		queue.RemoveAt(0);
		currentAgent = null;
	}

	//Returns position of last agent in line
	Vector3 getLast(){
		if (queue.Count != 0) {
			return queue [queue.Count - 1].transform.position;
		} 
		return transform.position;
	}
}
