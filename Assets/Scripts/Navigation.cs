using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var NavMeshAgent = this.GetComponent<NavMeshAgent>();
        NavMeshAgent.destination = new Vector3(100, 0, 8.4f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
