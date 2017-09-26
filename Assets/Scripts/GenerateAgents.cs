using UnityEngine;

public class GenerateAgents : MonoBehaviour {

	public bool randomLocation;
    public Transform AgentToGenerate;

    private int lastAgentSecond = 0;

    // Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        if (Time.time > lastAgentSecond + 1)
        {
			
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
        }
	}
}
