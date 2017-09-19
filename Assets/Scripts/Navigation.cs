using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {
    public Transform target;

	// Use this for initialization
	void Start () {
        var NavMeshAgent = this.GetComponent<NavMeshAgent>();
        if (target != null) {
            //Debug.Log(message: "Transform found: " + target.position);
            NavMeshAgent.destination = target.position;
        } else {
            // Use default location
            NavMeshAgent.destination = new Vector3(100, 0, 8.4f);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
