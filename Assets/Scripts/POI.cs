using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// POI (Point of Interest) encapsulates the logic for areas designated as points of interest.
/// POIs are created with a random interest value and have a chance to distract nearby agents.
/// </summary>
public class POI : MonoBehaviour {
    /// A random value between .4 and .8. If equal to agent's distractionValue, agent becomes
    /// distracted
	public float interestLevel;
	public float turnSpeed = 2.0f;

	// Use this for initialization
	void Start () {
		interestLevel = Random.Range(0.4f, 0.8f);
	}

	// Update is called once per frame
	void Update () {

	}

    /*
     * If the colliding object is an Agent, and the agent's distractionValue is lower than the POI's
     * interestLevel, the agent becomes distracted and rotates towards the POI.
     */
	void OnTriggerEnter(Collider other){
		//Test to make sure collision is by agent with interest value
		if (other.gameObject.tag == "Agent") {
            if (other.gameObject.GetComponent<Navigation> ().getDistractionValue() < interestLevel) {
    			other.gameObject.GetComponent<Navigation> ().distract ();
    			var NavMeshAgent = other.gameObject.GetComponent<NavMeshAgent> ();

    			Vector3 targetDir = this.transform.position - other.gameObject.transform.position;
    			float step = turnSpeed * Time.deltaTime;
    			Vector3 newDir = Vector3.RotateTowards(other.gameObject.transform.forward, targetDir, step, 0.0F);
    			//Debug.DrawRay(other.gameObject.transform.position, newDir, Color.red);
    			transform.rotation = Quaternion.LookRotation(newDir);

    			//assign new "destination"
    			NavMeshAgent.destination = this.transform.position;
    		} else {
    			//Debug.Log ("Agent is aware, but not interested " + other.gameObject.GetComponent<Navigation> ().stoppingValue + " vs " + interestLevel);
    		}
        }
	}
}
