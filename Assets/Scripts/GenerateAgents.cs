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
	private float sceneTime;

	void Start(){
		sceneTime = 0f;
	}

	// Update is called once per frame
	void Update () {
		sceneTime += Time.deltaTime;
		if (agentManager.spawnAllowed ()) {
			if (sceneTime > lastAgentSecond + 1) {
				lastAgentSecond++;
				Transform agent;
				
				if (randomLocation) {
					// Spawns AgentToGenerate in random position near the AgentGenerator
					agent = Instantiate (AgentToGenerate,
						this.transform.position + new Vector3 (Random.value * 2 - 1, 0, Random.value * 2 - 1) * 10, Quaternion.identity);	
				} else {
					// Spawns AgentToGenerate in fixed position near the AgentGenerator
					agent = Instantiate (AgentToGenerate,
						this.transform.position, Quaternion.identity);
				}
					
				agentManager.agentSpawned ();
			}
		}
	}
}
