using UnityEngine;

/// <summary>
/// This class causes a GameObject to generate agents.
/// </summary>
public class GenerateAgents : MonoBehaviour {

    /// <summary>
    /// Whether or not to spawn agents at a fixed location at the generator or in random location
    /// near the generator
    /// </summary>
	public bool randomLocation;
    /// <summary>
    /// The type of agent to generate
    /// </summary>
    public Transform AgentToGenerate;

	public AgentManager agentManager;

    private int lastAgentSecond = 0;

    // Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (agentManager.spawnAllowed ()) {
			if (Time.time > lastAgentSecond + 1) {
			
				lastAgentSecond++;
				if (randomLocation) {
					// Spawns AgentToGenerate in random position near the AgentGenerator
					Instantiate (AgentToGenerate,
						this.transform.position + new Vector3 (Random.value * 2 - 1, 0, Random.value * 2 - 1) * 10, Quaternion.identity);	
				} else {
					// Spawns AgentToGenerate in fixed position near the AgentGenerator
					Instantiate (AgentToGenerate,
						this.transform.position, Quaternion.identity);
				}
				agentManager.agentSpawned ();
			}
		}
	}
}
