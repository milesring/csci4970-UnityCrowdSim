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

    internal string AgentName {
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
    private GameObject GoalDestination;

    // true if the agent is at its goal
    internal bool AtGoal {
        get; set;
    }

    // true is agent is in a queue
    private bool InQueue;

    public void setInQueue(bool isInQueue)
    {
        agent.updateRotation = isInQueue ? false : true;
        InQueue = isInQueue;

    }
    public bool isInQueue()
    {
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
        agent.destination = findNearestDestination().transform.position;

        speed = agent.speed;
    }

    // Update is called once per frame
    void Update() {
        //increase total alive timer, used later for statistics
        tripTimer += Time.deltaTime;

        if (eventOver && !leaving) {
            //Debug.Log ("Event over, leaving");
            InQueue = false;
            AtGoal = false;
            leaving = true;
            agent.destination = findNearestDestination().transform.position;
            // agent.speed = speed; Interferes with emergency button agent speed (in HUD)
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
            // TODO any logic here
            // Agent being worked with
        } else if (InQueue) {
            // Agent waiting in queue

            // TODO Check position in queue. Move if necessary. If at front of queue and queue
            // ready for next agent, move up
        } else if ((agent.destination - this.transform.position).sqrMagnitude < Mathf.Pow(destinationPadding, 2)) {
            // If the squared distance between the agent's destination and the agent is less that the squared
            // destination padding, then...
            if (agent.destination.x == endPos.x && agent.destination.y == endPos.y) {
                // TODO This is unlikely to ever happen, check may be removed in future revisions
                Debug.Log("Agent " + AgentName + " has touched its destination.");
            }

            if (!inVenue && !eventOver) {
                Debug.Log(AgentName + " entered venue. Finding new goal.");
                inVenue = true;
                //find nearest destination in the building, at this point a goal should be sought out
                //aka dancefloor, bar, seating, etc.
                GoalDestination = findNearestDestination();
                agent.destination = GoalDestination.transform.position;
            } else if (eventOver && inVenue) {
                // If the event is over and we are in the venue, our only destinations are exits.
                Debug.Log(AgentName + " reached exit. Leaving the venue.");
                inVenue = false;
                GoalDestination = findNearestDestination();
                agent.destination = GoalDestination.transform.position;
            } else if (inVenue) {
                Debug.Log(AgentName + " reached goal.");
                if (!InQueue) {
                    GoalDestination.GetComponent<QueueLogic>().Enqueue(this.gameObject);
                }

                AtGoal = true;
                agent.speed = 0.0f;
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
    private GameObject findNearestDestination() {
        GameObject[] locations;
        if (!inVenue && !eventOver) {
            locations = locationManager.GetLocations("Entrance");
            //Debug.Log ("Finding closest entrance");
            // TODO once goals are in place this will be the check for event over(or something like that) && inVenue

        } else if (eventOver && inVenue) {
            //event over, leave
            locations = locationManager.GetLocations("Exit");
            //Debug.Log ("Finding closest exit");
        } else if (eventOver && !inVenue) {
            endPos = locationManager.FindNearestDestroyRadius(transform.position);
            //Debug.Log ("Finding oustide location to be destroyed");
            //Debug.Log (endPos);
            GameObject temp = new GameObject();
            temp.transform.position = (endPos);
            return temp;
        } else if (inVenue) {
            //continue finding goals to do in venue
            locations = locationManager.GetLocations("Goal");
            int index = Random.Range(0, locations.Length);
            //Debug.Log ("Found goal at :"+index);
            return locations[index];
        } else {
            locations = locationManager.GetLocations("Exit");
        }

        //find nearest location
        return calculateNearest(locations);
    }

    // Given an array of locations, determine which location is the nearest to the agent
    private GameObject calculateNearest(GameObject[] locations) {
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

        //Debug.Log ("Nearest location found at: " + nearest.transform.position.x + ", " + nearest.transform.position.y + ", " + nearest.transform.position.z);
        return nearest;
    }

    // TODO Rename this...
    public void AgentOfDestruction() {
        agent.destination = calculateNearest(locationManager.GetLocations("Exit")).transform.position;
        ResumeAgentSpeed();
    }

    /// <summary>
    /// Ends the event, causing agents to begin exiting
    /// </summary>
	public void endEvent() {
        eventOver = true;
    }

    /// <returns>true if the agent is outside the venue, otherwise false</returns>
	public bool isOutside()
    {
        return inVenue;
    }

    /// <returns>the destination of the agent</returns>
	public GameObject GetDestination() {
        return GoalDestination;
    }

    internal void SetDestination(Vector3 destination, bool saveCurrentDestination) {
        NavMeshAgent navMeshAgent = this.GetComponent<NavMeshAgent>();
        if (saveCurrentDestination) {
            NavigationDestination = navMeshAgent.destination;
        }
        
        navMeshAgent.destination = destination;
    }

    internal Vector3 GetNavMeshAgentDestination() {
        return this.GetComponent<NavMeshAgent>().destination;
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
        agent.speed = speed;
    }

    // Saves the agent's current speed, then sets the agent's speed to 0.
    internal void StopAgent() {
        agent.velocity = Vector3.zero;
        agent.isStopped = true;
        //agent.speed = 0.0f;
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
