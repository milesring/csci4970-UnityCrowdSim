using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {
	public bool useTarget;
    public Transform target;

    public GameObject[] targets;
    public float destinationPadding;
    public Vector3 destinationPaddingVector;
	private float tripTimer;

	private bool distracted;
	private float distractionTimer;
	public float distractionTime;

	public float stoppingValue;

	private Vector3 entrance;
	private Vector3 lastDestination;
	private float speed;

	// Use this for initialization
	void Start () {
		distracted = false;
		tripTimer = 0.0f;
		distractionTimer = 0.0f;

		//assign fake "interest" value that must be exceeded by a POI to distract agent
		stoppingValue = Random.Range(0.4f, 1.0f);

        // targets = GameObject.FindGameObjectsWithTag("Target 1");
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
        if ((NavMeshAgent.destination - transform.position).magnitude < destinationPadding) {
            Debug.Log("Stopping agent");
			NavMeshAgent.speed = 0.0f;
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
}
