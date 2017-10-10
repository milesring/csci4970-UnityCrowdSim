using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	[SerializeField]
	private Text _numberOfActors;

	private readonly string ACTOR_NUM_FORMAT= "Number of Actors: {0}";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		_numberOfActors.text = string.Format(ACTOR_NUM_FORMAT, GameObject.FindGameObjectsWithTag("Agent").Length);
	}
}
