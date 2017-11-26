using UnityEngine;

/// <summary>
/// Destroys agents when they come in contact with the parent object's collider
/// </summary>
public class AgentDestroyer : MonoBehaviour {
    private EventManager eventManager;
	private AgentManager agentManager;

	void Start(){
		eventManager = GameObject.Find ("EventManager").GetComponent<EventManager>();
		agentManager = GameObject.Find ("AgentManager").GetComponent<AgentManager> ();
	}

	void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Agent")) {
            Navigation agent = other.gameObject.GetComponent<Navigation>();
            if (eventManager.IsEventOver || agent.IsLeaving()) {
                Destroy(other.gameObject);
                agentManager.agentDestroyed();
            }
        }
	}
}
