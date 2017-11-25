using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class controls the navigation behavior of an agent. This includes updating the agent's destination
/// and tracking various status values such as the agent's distraction
/// </summary>
public class Navigation : MonoBehaviour {
    private LocationManager locationManager;
    private static int agentNumber = 0;
    public string Name
    {
        get; set;
    }

    public string AgentName {
        get; set;
    }

    [Header("Venue Navigation")]
    /// <summary>
    /// Distance to stop prior to destination
    /// </summary>
   	public float destinationPadding;
    private float tripTimer;
    private bool leaving;

    [Header("Venue Goals")]
    public float goalTimeMin;
    public float goalTimeMax;
    private float goalTimer;
    private float goalTime;
    // TODO "eventOver" isn't an agent's property, it is an environment property. This should be moved.
    private bool eventOver;

    [Header("Points of Interest")]
    /// <summary>
    ///  How long an agent will be distracted for
    /// </summary>
	public float distractionTime;
    private float distractionValue;
    private bool distracted;
    private float distractionTimer;

    private Vector3 endPos;
    // This variable stores what the agent's end-goal is. This is required as the NavMeshAgent's
    // destination sometimes needs to change while its end-goal is the same.
    internal Vector3 NavigationDestination {
        get; set;
    }

    private float speed;
    private bool inVenue;
    // The game object representation of the agent's current Destination.
    private GameObject goalDestination;
    private List<GameObject> visitedGoals = new List<GameObject>();

    // true if the agent is at its goal
    internal bool AtGoal {
        get; set;
    }

    // true is agent is in a queue
    private bool InQueue;

    public void setInQueue(bool isInQueue) {
        agent.updateRotation = isInQueue ? false : true;
        InQueue = isInQueue;

    }
    public bool isInQueue() {
        return InQueue;
    }

    // true if agent is being served at front of queue
    internal bool BeingServed {
        get; set;
    }

    private NavMeshAgent agent;

    // Use this for initialization
    void Start() {
        AgentName = string.Format("Agent {0}", agentNumber.ToString("D3"));
        agentNumber++;

        locationManager = GameObject.Find("LocationManager").GetComponent<LocationManager>();

        inVenue = false;
        distracted = false;
        leaving = false;
        eventOver = false;
        tripTimer = 0.0f;
        distractionTimer = 0.0f;
        AtGoal = false;
        InQueue = false;

        // FIXME this is unused
        goalTime = Random.Range(goalTimeMin, goalTimeMax);
        distractionValue = Random.Range(0.4f, 1.0f);

        agent = this.GetComponent<NavMeshAgent>();
        goalDestination = findNearestDestination();
        agent.destination = goalDestination.transform.position;

        speed = agent.speed;
    }

    // Update is called once per frame
    void Update() {
        //increase total alive timer, used later for statistics
        tripTimer += Time.deltaTime;

        if (eventOver && !leaving) {
            AtGoal = false;
            leaving = true;
            goalDestination = findNearestDestination();
            agent.destination = goalDestination.transform.position;
        }

        if (distracted) {
            distractionUpdate();
        }

        destinationUpdate();
    }

    // Updates counters associated with being distracted.
    private void distractionUpdate() {
        distractionTimer += Time.deltaTime;
        //Debug.Log ("Distracted for: " + distractionTimer + "s");

        if (distractionTimer > distractionTime) {
            //restore last goal for agent
            agent.destination = NavigationDestination;
            distracted = false;

            //resume speed
            ResumeAgentSpeed();

            //reset timer
            distractionTimer = 0.0f;
        }
    }

    // Checks agent's status booleans and updates its goals / destinations as required
    private void destinationUpdate() {
        if (AtGoal) {
            if (BeingServed) {
                // Still under control of a queue
            } else {
                UpdateVisitedGoal();
                AtGoal = false;
                goalDestination = findNearestDestination();
                agent.destination = goalDestination.transform.position;
            }
            // Agent waiting at goal for new orders
        } else if (InQueue) {
            // Agent waiting in queue, and the queue 
        } else if ((agent.destination - this.transform.position).sqrMagnitude < Mathf.Pow(destinationPadding, 2)) {
            // If the squared distance between the agent's destination and the agent is less that the squared
            // destination padding, then...
            if (!inVenue && !eventOver) {
                Debug.Log(AgentName + " entered venue. Finding new goal.");
                inVenue = true;
                //find nearest destination in the building, at this point a goal should be sought out
                //aka dancefloor, bar, seating, etc.
                goalDestination = findNearestDestination();
                agent.destination = goalDestination.transform.position;
            } else if (inVenue && (eventOver || leaving)) {
                // If the event is over and we are in the venue, our only destinations are exits.
                inVenue = false;
                Debug.Log(AgentName + " reached exit. Leaving the venue. inVenue = " + inVenue + " --- leaving = " + leaving + " --- eventOver = " + eventOver);
                goalDestination = findNearestDestination();
                agent.destination = goalDestination.transform.position;
            } else if (inVenue) {
                Debug.Log(AgentName + " reached goal.");
                if (!InQueue && goalDestination.GetComponent<IQueue>() != null) {
                    goalDestination.GetComponent<QueueLogic>().Enqueue(this.gameObject);
                }

                AtGoal = true;
            } else {
                Debug.Log(AgentName + " error in destinationUpdate(). Stopping.");
                agent.speed = 0.0f;
            }
        }
    }

    /// <summary>
    /// Calling this method distracts the agent, causing the agent to stop for a predetermined amount of time.
    /// Once the distraction time has expired, the agent will continue to its previous destination.
    /// </summary>
	public void distract() {
        var NavMeshAgent = this.GetComponent<NavMeshAgent>();
        distractionTimer = 0.0f;
        distracted = true;

        //save last destination
        NavigationDestination = NavMeshAgent.destination;
    }

    // Based on this agent's status booleans, find an appropriate destination
    internal GameObject findNearestDestination() {
        List<GameObject> locationsList = new List<GameObject>();

        if (!inVenue) {
            if (eventOver || leaving) {
                // Agent has left and must be DESTROYED
                endPos = locationManager.FindNearestDestroyRadius(transform.position);
                GameObject temp = new GameObject();
                temp.transform.position = (endPos);
                return temp;
            } else if (!eventOver) {
                locationsList = locationManager.GetLocations(LocationTypes.ENTRANCE);
            }
        } else {
            // Agent in venue
            if (eventOver || leaving) {
                // Agent needs to leave
                Debug.Log(AgentName + " is finding an exit");
                locationsList = locationManager.GetLocations(LocationTypes.EXIT);
            } else {
                //continue finding goals to do in venue
                locationsList = locationManager.GetLocations(LocationTypes.GOAL);
                locationsList.RemoveAll(location => visitedGoals.Contains(location));
                if (locationsList.Count == 0) {
                    Debug.Log(AgentName + " has visited all goals and is now leaving.");
                    leaving = true;
                    locationsList = locationManager.GetLocations(LocationTypes.EXIT);
                } else {
                    Debug.Log(AgentName + " has finished at a goal and is choosing a new random goal.");
                    int index = Random.Range(0, locationsList.Count);
                    return locationsList[index];
                }
            }
        }

        //find nearest location
        return calculateNearest(locationsList);
    }

    // Given an array of locations, determine which location is the nearest to the agent
    private GameObject calculateNearest(List<GameObject> locations) {
        GameObject nearest = null;

        foreach (GameObject location in locations) {
            if (nearest == null) {
                nearest = location;
            } else {
                float magnitudeToAltLocation
                    = (location.transform.position - this.transform.position).magnitude;
                float magnitudeToCurrNearestLocation
                    = (nearest.transform.position - this.transform.position).magnitude;

                if (magnitudeToAltLocation < magnitudeToCurrNearestLocation) {
                    nearest = location;
                }
            }
        }
        
        return nearest;
    }

    /// <summary>
    /// Ends the event, causing agents to begin exiting
    /// </summary>
	public void endEvent() {
        eventOver = true;
    }

    /// <returns>true if the agent is outside the venue, otherwise false</returns>
	public bool isOutside() {
        return inVenue;
    }

    /// <returns>the destination of the agent</returns>
	public GameObject GetDestination() {
        return goalDestination;
    }

    /// <returns>true is the agent is leaving the venue, otherwise false</returns>
    public bool IsLeaving() {
        return leaving;
    }

    internal void UpdateVisitedGoal() {
        visitedGoals.Add(goalDestination);
    }

    internal void SetDestination(Vector3 destination, bool saveCurrentDestination) {
        if (saveCurrentDestination) {
            NavigationDestination = agent.destination;
        }
        
        agent.destination = destination;
    }

    internal void UpdateLookRotation() {
        Vector3 targetDir = (NavigationDestination - this.transform.position).normalized;
        float step = speed * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(this.transform.forward, targetDir, step, 10.0f);
        Debug.DrawRay(this.transform.position, newDir, Color.red);
        this.transform.rotation = Quaternion.LookRotation(newDir);
    }

    // Resumes the agent's previous speed
    internal void ResumeAgentSpeed() {
        agent.isStopped = false;
        // Only reset the agent's speed if the agen'ts speed is 0. This allows emergencies to speed the agent up
        if (agent.speed == 0.0f) {
            agent.speed = speed;
        }
    }

    // Saves the agent's current speed, then sets the agent's speed to 0.
    internal void StopAgent() {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        agent.speed = 0.0f;
    }

    /// <summary>
    /// On start, a random value between .4 and 1.0 that is generated and assigned as this agent's
    /// distractionValue. This is used when comparing to a point of interest's interestLevel.
    /// </summary>
    /// <returns>this agent's distraction value</returns>
    internal float getDistractionValue() {
        return distractionValue;
    }
}
