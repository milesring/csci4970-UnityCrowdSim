using UnityEngine;
using System.Collections;


public class GenerateAgents : MonoBehaviour {

    public Transform AgentToGenerate;

    private int lastAgentSecond = 0;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(Time.time > lastAgentSecond + 1)
        {
            lastAgentSecond++;
            Instantiate(AgentToGenerate, 
                this.transform.position + new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1) * 10, 
                Quaternion.identity);
        }
            
	}
}
