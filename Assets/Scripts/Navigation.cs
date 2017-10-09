using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {
	private LocationManager locationManager;

	[Header("Venue Navigation")]
	public bool useTarget;
    public Transform target;
	public float destinationPadding;
	private float tripTimer;
	private bool leaving;

	[Header("Venue Goals")]
	public float goalTimeMin;
	public float goalTimeMax;
	private float goalTimer;
	private float goalTime;
	private bool eventOver;

	[Header("Points of Interest")]
	public float distractionValue;
	public float distractionTime;
	private bool distracted;
	private float distractionTimer;

	//private Vector3 startPos;
	private Vector3 endPos;
	private Vector3 lastDestination;
	private float speed;
	private bool inVenue;

	// Use this for initialization
	void Start () {
		locationManager = GameObject.Find ("LocationManager").GetComponent<LocationManager> ();
		inVenue = false;
		distracted = false;
		leaving = false;
		eventOver = false;
		tripTimer = 0.0f;
		distractionTimer = 0.0f;
		//startPos = this.transform.position;
		goalTime = Random.Range (goalTimeMin, goalTimeMax);

		//assign fake "interest" value that must be exceeded by a POI to distract agent
		distractionValue = Random.Range(0.4f, 1.0f);

		var NavMeshAgent = this.GetComponent<NavMeshAgent>();

		if (useTarget) {
			if (target != null) {
				//Debug.Log(message: "Transform found: " + target.position);
				NavMeshAgent.destination = target.position;
			} else {
				// Use default location
				NavMeshAgent.destination = new Vector3 (100, 0, 8.4f);
			}
		} else {
			NavMeshAgent.destination = findNearestDestination ();
		}

		speed = NavMeshAgent.speed;

	}
	
	// Update is called once per frame
	void Update () {

		var NavMeshAgent = this.GetComponent<NavMeshAgent>();
		//increase total alive timer, used later for statistics
		tripTimer += Time.deltaTime;

		if (distracted) {
			distractionTimer += Time.deltaTime;
			//Debug.Log ("Distracted for: " + distractionTimer + "s");
			if (distractionTimer > distractionTime) {
				//restore last goal for agent
				NavMeshAgent.destination = lastDestination;
				distracted = false;

				//resume speed
				NavMeshAgent.speed = speed;

				//reset timer
				distractionTimer = 0.0f;
			}
		}

		if (eventOver && !leaving) {
			Debug.Log ("Event over, leaving");
			leaving = true;
			NavMeshAgent.destination = findNearestDestination ();
			NavMeshAgent.speed = speed;
		}
		//Stop moving if destination reached
		if ((NavMeshAgent.destination - this.transform.position).sqrMagnitude < destinationPadding*destinationPadding) {
			if (NavMeshAgent.destination.x == endPos.x && NavMeshAgent.destination.y == endPos.y) {
				Debug.Log ("destroy gameobject");
			}

			if (!inVenue && !eventOver) {
				Debug.Log ("Agent Entered Venue\nFinding new goal");
				inVenue = true;
				//find nearest destination in the building, at this point a goal should be sought out
				//aka dancefloor, bar, seating, etc.
				NavMeshAgent.destination = findNearestDestination ();
			} else if (eventOver && inVenue) {
				Debug.Log ("Agent reached exit, leaving from venue");
				inVenue = false;
				NavMeshAgent.destination = findNearestDestination ();
			} else if (inVenue) {
				//in venue, continue iterating through main goals in venue based on timer
				goalTimer += Time.deltaTime;
				NavMeshAgent.speed = 0.0f;
				if (goalTimer > goalTime) {
					//find new venue goal
					NavMeshAgent.destination = findNearestDestination ();

					//resume moving
					NavMeshAgent.speed = speed;

					//reset timer
					goalTimer = 0.0f;
				}
			} else {
				//head to the outside
				Debug.Log("Heading to destroy radius");
				NavMeshAgent.destination = findNearestDestination();
			}
		}
	}

	public void distract(){
		var NavMeshAgent = this.GetComponent<NavMeshAgent>();
		distractionTimer = 0.0f;
		distracted = true;

		//save last destination
		lastDestination = NavMeshAgent.destination;
	}

	Vector3 findNearestDestination(){
		GameObject[] locations;
		if (!inVenue && !eventOver) {
			locations = locationManager.getLocations ("Entrance");
			Debug.Log ("Finding closest entrance");
			//once goals are in place this will be the check for event over(or something like that) && inVenue

		} else if (eventOver && inVenue) {
			//event over, leave
			locations = locationManager.getLocations ("Exit");
			Debug.Log ("Finding closest exit");
		} else if(eventOver && !inVenue){
			endPos = locationManager.findNearestDestroyRadius (transform.position);
			Debug.Log ("Finding oustide location to be destroyed");
			Debug.Log (endPos);
			return endPos;
		} else if (inVenue) {
			//continue finding goals to do in venue
			locations = locationManager.getLocations ("Goal");
			int index = Random.Range (0, locations.Length);
			Debug.Log ("Found goal at :"+index);
			return locations [index].transform.position;
		} else {
			locations = locationManager.getLocations ("Exit");
		}


		//find nearest location
		GameObject nearest = null;
		for(int i=0;i<locations.Length;++i){
			if (nearest == null) {
				nearest = locations [i];
			}

			if ((locations [i].transform.position - this.transform.position).magnitude < (nearest.transform.position - this.transform.position).magnitude) {
					nearest = locations [i];
			}
		}

		//Debug.Log ("Nearest location found at: " + nearest.transform.position.x + ", " + nearest.transform.position.y + ", " + nearest.transform.position.z);
		return nearest.transform.position;
	}

	public void endEvent(){
		eventOver = true;
	}

	public bool isOutside(){
		return inVenue;
	}

}
