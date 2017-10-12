using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class QueueLogic : MonoBehaviour {
	List<GameObject> queue;
	bool busy;
	// Use this for initialization
	void Start () {
		queue = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!busy && queue.Count != 0) {
			WorkWithSomeone ();
		}
	}

	//Add agent to end of list
	public void Enqueue(GameObject agent){
		
		if (queue.Count == 0) {
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

	//Agent at front of queue leaves, next agent takes place
	public void WorkWithSomeone(){
		busy = true;
		// wait for random amount of time then no longer busy

		//Dequeue();

	}

	public void Dequeue(){
		queue [0].GetComponent<Navigation> ().AtGoal (false);
		queue.RemoveAt (0);
		if (queue.Count != 0) {
			queue [0].GetComponent<Navigation> ().AtGoal (true);
			queue [0].GetComponent<Navigation> ().InQueue (false);

		}

		// end
		busy = false;
	}
		
	//Returns number of agents in queue
	//possibly of altering interest level with how many currently in queue
	public int Length(){
		return queue.Count;
	}

	//Returns position of last agent in line
	Vector3 getLast(){
		return queue [queue.Count-1].transform.position;
	}
	
	bool isEmpty(){
		if (queue.Count != 0) {
			return false;
		}
		return true;
	}

}
