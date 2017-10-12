using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	[SerializeField]
	private Text numberOfActors;
	[SerializeField]
	private Button povButton;
//	[SerializeField]
//	private Button emergencyButton;

	private AgentManager agentManager;

	private string currentPOV = "default";

	private readonly string ACTOR_NUM_FORMAT= "Number of Actors: {0}";



	// Use this for initialization
	void Start () {
		Button btn = povButton.GetComponent<Button>();
		btn.onClick.AddListener(POVclick);

		agentManager = GameObject.Find ("AgentManager").GetComponent<AgentManager>();
	}

	void POVclick() {
		switch(currentPOV) {
		case "default":
			currentPOV = "topDown";
			Camera.main.transform.position = new Vector3 (10f, 0f, 0f); // topDown position
			print("Click");
			break;
		case "topDown":
			currentPOV = "default";
			Camera.main.transform.position = new Vector3 (-11.74333f, -7.529732f, -15.21297f); // default position
			break;
//		default: currentPOV = "default";
		}
	}

	// Update is called once per frame
	void Update () {
		//numberOfActors.text = string.Format(ACTOR_NUM_FORMAT, GameObject.FindGameObjectsWithTag("Agent").Length);
		numberOfActors.text = string.Format(ACTOR_NUM_FORMAT, agentManager.AgentCount());
	}
}
