using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {
	private LocationManager locationManager;
	public bool useTarget;
    public Transform target;

	public float destinationPadding;

	private float tripTimer;
	private bool distracted;
	private float distractionTimer;
	public float distractionTime;

	public float stoppingValue;

	private Vector3 startPos;
	private Vector3 lastDestination;
	private float speed;
	private GameObject[] entrances;
	private GameObject[] exits;
	private bool inVenue;

	// Use this for initialization
	void Start () {
		locationManager = GameObject.Find ("LocationManager").GetComponent<LocationManager> ();
		inVenue = false;
		distracted = false;
		tripTimer = 0.0f;
		distractionTimer = 0.0f;
		startPos = this.transform.position;

		//assign fake "interest" value that must be exceeded by a POI to distract agent
		stoppingValue = Random.Range(0.4f, 1.0f);

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


		//findEntrances ();

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
				//Debug.Log ("Agent no longer distracted");
			}
		}
		//Stop moving if destination reached
		if ((NavMeshAgent.destination - this.transform.position).magnitude < destinationPadding) {
			if (!inVenue) {
				inVenue = true;
				//find nearest destination in the building, at this point a goal should be sought out
				//aka dancefloor, bar, seating, etc.
				Debug.Log ("Agent inside venue");


				//find next destination (for now, exit)
				NavMeshAgent.destination = findNearestDestination();

			
			} else if(inVenue){
				//this will be the spot for finding goals or traversing through goals in venue

				//for now, return to start
				NavMeshAgent.destination = startPos;

			//the Y value in destination becomes distorted, so it is ignored for now
			} else if (NavMeshAgent.destination.x == startPos.x && NavMeshAgent.destination.z == startPos.z) {
				Debug.Log ("Agent returned to start pos, despawning");
				Destroy (this.gameObject);
			} else {
				//inside at goal destination
				NavMeshAgent.speed = 0.0f;
				//Debug.Log ("Agent destination: " + NavMeshAgent.destination.x + ", " + NavMeshAgent.destination.y + ", " + NavMeshAgent.destination.z);
				//Debug.Log ("Agent start pos: " + startPos.x + ", " + startPos.y + ", " + startPos.z);
			}
		}
	}

	public void distract(){
		var NavMeshAgent = this.GetComponent<NavMeshAgent>();
		distractionTimer = 0.0f;
		distracted = true;

		//save last destination
		lastDestination = NavMeshAgent.destination;
		//Debug.Log ("Agent distracted");
	}

	Vector3 findNearestDestination(){
		GameObject[] locations;
		if (!inVenue) {
			locations = locationManager.getLocations ("Entrance");
			//once goals are in place this will be the check for event over(or something like that) && inVenue
		} else {
			//temp code for finding exit
			locations = locationManager.getLocations("Exit");
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

		Debug.Log ("Nearest location found at: " + nearest.transform.position.x + ", " + nearest.transform.position.y + ", " + nearest.transform.position.z);
		return nearest.transform.position;
	}
}
