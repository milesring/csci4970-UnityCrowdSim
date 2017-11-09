using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleNavigation1 : MonoBehaviour {
	private NavMeshAgent navAgent;
	private Navigator navigator;
	private Actions agentAction;
	private Status agentStatus;

	// Use this for initialization
	void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
		navigator = GetComponent<Navigator> ();

		agentAction = Actions.FindingEntrance;
		agentStatus = Status.Outside;
		navigator.Navigate (navAgent, agentAction);
	}
	
	// Update is called once per frame
	void Update () {
		switch (agentAction) {
		case Actions.Emergency:
		case Actions.FindingExits:
			//NEED TO LEAVE NOW, not waiting to reach destination first
			DecideNextAction();
			break;
		default:
		//Agent has arrived at destinaton
			if (navigator.CheckReachedDestination(navAgent)) {
				navAgent.isStopped = true;
				agentAction = UpdateActions ();
				DecideNextAction ();
			}
			break;
		}
	}

	//Chooses what to do next 
	void DecideNextAction(){
		GameObject lastDest = navigator.GetDestinationObject ();
		switch (agentAction) {
		case Actions.FindingExits:
		case Actions.Emergency:
		case Actions.Idle:
		case Actions.FindingGoals:
			//navigate to where agent needs to go, don't go to same place twice in a row
			while (true) {
				navigator.Navigate (navAgent, agentAction);
				GameObject newDest = navigator.GetDestinationObject ();
				if(!lastDest.Equals(newDest)){
					break;
				}
			}
			break;
		case Actions.InQueue:
			//logic is handled by queue manager
			break;
		case Actions.AtGoal:
			Interact ();
			break;
		default:
			break;
		}
	}

	//agent is interacting with a POI or goal
	void Interact(){
		SimpleQueueLogic goal = navigator.GetDestinationObject().GetComponent<SimpleQueueLogic>();
		if (goal != null && agentAction != Actions.InQueue && agentAction != Actions.Idle) {
			goal.Enqueue (this.gameObject);
		}
	}

	Actions UpdateActions(){
		GameObject dest = navigator.GetDestinationObject ();
		if (dest != null){
			switch(dest.name){
			case "Entrance":
				return Actions.FindingGoals;
			case "Goal":
				return Actions.AtGoal;
			case "Exit":
				return Actions.FindingExits;
			default:
				break;
			}
		}
		return Actions.Idle;

	}

	public void ExternalUpdateEvent(Status status){
		agentStatus = status;
	}

	public void ExternalUpdateAction(Actions action){
		agentAction = action;
	}
}

public enum Actions
{
	FindingEntrance,
	FindingGoals,
	AtGoal,
	InQueue,
	FindingExits,
	Emergency,
	Idle
}
public enum Status
{
	Outside,
	EventOngoing,
	EventOver
}