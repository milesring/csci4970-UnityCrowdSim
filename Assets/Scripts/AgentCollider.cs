using UnityEngine;

/// <summary>
/// Defines behavior of agent when it collides with other game objects
/// </summary>
public class AgentCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter(Collider other) {
        Navigation thisAgent = this.gameObject.GetComponent<Navigation>();
        if (other.gameObject.CompareTag("Agent")) {
            AgentCollision(thisAgent, other);
        } else if (other.gameObject.CompareTag("Goal")) {
            GoalCollision(thisAgent, other);
        }
    }

    private void AgentCollision(Navigation thisAgent, Collider other) {
        if (thisAgent.AtGoal) {
            // This agent is already at its goal and does not care about collisions
            Debug.Log(thisAgent.AgentName + " is already at goal!");
        } else if (thisAgent.BeingServed) {
            Debug.Log(thisAgent.AgentName + " is already being served!");
            // This agent is being served and does not care about collisions
        } else if (other.gameObject.CompareTag("Agent")) {
            // If the colliding object is an agent
            Navigation otherAgent = other.gameObject.GetComponent<Navigation>();

            if (thisAgent.isInQueue()) {
                /*
                 * If this agent is already in a queue and an agent collides with us, re-call stop
                 * to ensure momentum is halted. This agent needs to do nothing
                 */
                thisAgent.StopAgent();
            } else if ((otherAgent.AtGoal || otherAgent.BeingServed || otherAgent.isInQueue())
                   && otherAgent.GetDestination() == thisAgent.GetDestination()) {

                IQueue queue = thisAgent.GetDestination().GetComponent<IQueue>();
                if (queue != null) {
                    /*
                     * If this agent is not in a queue and we collide with an agent that is at its goal 
                     * (which is a queue) or in a queue for its goal AND this agent's destination is the 
                     * same, queue this agent
                     */
                    Debug.Log(thisAgent.AgentName + " and " + otherAgent.AgentName
                        + " have same goal. Adding agent " + thisAgent.AgentName + " in queue behind "
                        + otherAgent.AgentName);
                    queue.Enqueue(this.gameObject);
                }
            }
        }
    }

    private void GoalCollision(Navigation thisAgent, Collider other) {

    }
}
