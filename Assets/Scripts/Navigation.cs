using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {
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
			NavMeshAgent.destination = new Vector3 (123.19f, 0, -13.56f);
		}

		speed = NavMeshAgent.speed;


		findEntrances ();
		findNearestDestination ();
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


				//temp code
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

	void findEntrances(){
		//Finds all entrances in the scene
		entrances = GameObject.FindGameObjectsWithTag ("Entrance");
		if (entrances != null) {
			for (int i = 0; i < entrances.Length; ++i) {
				GameObject temp = entrances [i];
				Debug.Log ("Entrance found at: " + temp.transform.position.x + ", " + temp.transform.position.y + ", " + temp.transform.position.z);
			}
		} else {
			Debug.Log ("No entrances found");
		}
	}

	void findNearestDestination(){
		var NavMeshAgent = this.GetComponent<NavMeshAgent>();


		//find nearest entrance
		if (!inVenue) {
			GameObject nearest = null;
			for(int i=0;i<entrances.Length;++i){
				if (nearest == null) {
					nearest = entrances [i];
				}

				if ((entrances [i].transform.position - this.transform.position).magnitude < (nearest.transform.position - this.transform.position).magnitude) {
					nearest = entrances [i];
				}
			}
			Debug.Log ("Nearest entrance found at: " + nearest.transform.position.x + ", " + nearest.transform.position.y + ", " + nearest.transform.position.z);
			NavMeshAgent.destination = nearest.transform.position;
		}


	}
}
