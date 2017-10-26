using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class controls the navigation behavior of an agent. This includes updating the agent's destination
/// and tracking various status values such as the agent's distraction
/// </summary>
public class Navigation : MonoBehaviour {
	private LocationManager locationManager;

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
	private Vector3 lastDestination;
	private float speed;
	private bool inVenue;
	private GameObject destination;

	private bool atGoal;
	private bool inQueue;

    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		locationManager = GameObject.Find ("LocationManager").GetComponent<LocationManager> ();
		inVenue = false;
		distracted = false;
		leaving = false;
		eventOver = false;
		tripTimer = 0.0f;
		distractionTimer = 0.0f;
		atGoal = false;
		inQueue = false;

        // FIXME this is unused
        goalTime = Random.Range(goalTimeMin, goalTimeMax);
        distractionValue = Random.Range(0.4f, 1.0f);

        agent = this.GetComponent<NavMeshAgent>();
        agent.destination = findNearestDestination().transform.position;

		speed = agent.speed;
	}

	// Update is called once per frame
	void Update () {
		//increase total alive timer, used later for statistics
		tripTimer += Time.deltaTime;

		if (eventOver && !leaving) {
			//Debug.Log ("Event over, leaving");
			inQueue = false;
			atGoal = false;
			leaving = true;
            agent.destination = findNearestDestination ().transform.position;
            agent.speed = speed;
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
            agent.destination = lastDestination;
            distracted = false;

            //resume speed
            agent.speed = speed;

            //reset timer
            distractionTimer = 0.0f;
        }
    }

    // Checks agent's status booleans and updates its goals / destinations as required
    private void destinationUpdate() {
        if (IsAtGoal()) {
            // TODO any logic here?
        } else if (IsInQueue()) {
            // TODO Check position in queue. Move if necessary. If at front of queue and queue
            // ready for next agent, move up
        } else if ((agent.destination - this.transform.position).sqrMagnitude < Mathf.Pow(destinationPadding, 2)) {
            if (agent.destination.x == endPos.x && agent.destination.y == endPos.y) {
                //Stop moving if destination reached
            }

            // FIXME is this supposed to be an "else if"?
            if (!inVenue && !eventOver) {
                Debug.Log("Agent entered venue. Finding new goal");
                inVenue = true;
                //find nearest destination in the building, at this point a goal should be sought out
                //aka dancefloor, bar, seating, etc.
                destination = findNearestDestination();
                agent.destination = destination.transform.position;
            } else if (eventOver && inVenue) {
                Debug.Log("Agent reached exit. Leaving the venue");
                inVenue = false;
                destination = findNearestDestination();
                agent.destination = destination.transform.position;
            } else if (inVenue) {
                Debug.Log("Agent reached goal");
                destination.GetComponent<QueueLogic>().Enqueue(this.gameObject);
                atGoal = true;
                agent.speed = 0.0f;
            } else {
                Debug.Log("Agent error in destinationUpdate(). Stopping.");
                agent.speed = 0.0f;
            }
        }
    }

    /// <summary>
    /// Calling this method distracts the agent, causing the agent to stop for a predetermined amount of time.
    /// Once the distraction time has expired, the agent will continue to its previous destination.
    /// </summary>
	public void distract(){
		var NavMeshAgent = this.GetComponent<NavMeshAgent>();
		distractionTimer = 0.0f;
		distracted = true;

		//save last destination
		lastDestination = NavMeshAgent.destination;
	}

    // Based on this agent's status booleans, find an appropriate destination
    private GameObject findNearestDestination(){
		GameObject[] locations;
		if (!inVenue && !eventOver) {
			locations = locationManager.GetLocations ("Entrance");
			//Debug.Log ("Finding closest entrance");
			// TODO once goals are in place this will be the check for event over(or something like that) && inVenue

		} else if (eventOver && inVenue) {
			//event over, leave
			locations = locationManager.GetLocations("Exit");
			//Debug.Log ("Finding closest exit");
		} else if(eventOver && !inVenue){
			endPos = locationManager.FindNearestDestroyRadius(transform.position);
			//Debug.Log ("Finding oustide location to be destroyed");
			//Debug.Log (endPos);
			GameObject temp = new GameObject();
			temp.transform.position = (endPos);
			return temp;
		} else if (inVenue) {
			//continue finding goals to do in venue
			locations = locationManager.GetLocations("Goal");
			int index = Random.Range (0, locations.Length);
			//Debug.Log ("Found goal at :"+index);
			return locations [index];
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

    /// <summary>
    /// Ends the event, causing agents to begin exiting
    /// </summary>
	public void endEvent(){
		eventOver = true;
	}

    /// <returns>true if the agent is outside the venue, otherwise false</returns>
	public bool isOutside(){
		return inVenue;
	}

    /// <summary>
    /// Sets whether or not the agent has reached its goal
    /// </summary>
    /// <param name="value">true if agent has reached its goal, otherwise false</param>
	public void AtGoal(bool value){
		atGoal = value;
	}

    /// <returns>true if the agent is at its goal, otherwise false</returns>
	public bool IsAtGoal(){
		return atGoal;
	}

    /// <summary>
    /// Sets whether or not the agent is in a queue
    /// </summary>
    /// <param name="value">true if agent is in a queue, otherwise false</param>
	public void InQueue(bool value){
		inQueue = value;
	}

    /// <returns>true if the agent is in a queue, otherwise false</returns>
	public bool IsInQueue(){
		return inQueue;
	}

    /// <returns>the destination of the agent</returns>
	public GameObject GetDestination(){
		return destination;
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
