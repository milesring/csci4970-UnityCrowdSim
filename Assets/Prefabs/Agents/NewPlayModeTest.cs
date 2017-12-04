using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class NewPlayModeTest {

	//tests for atleast 1 entrance
	[UnityTest]
	public IEnumerator EntranceTest() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		SetupScene();
		yield return new WaitForFixedUpdate();
		GameObject entrance = GameObject.FindGameObjectWithTag("Entrance");
		Assert.IsNotNull (entrance);
	}

	//tests for atleast 1 exit
	[UnityTest]
	public IEnumerator ExitTest() {
		SetupScene();
		yield return new WaitForFixedUpdate();
		GameObject exit = GameObject.FindGameObjectWithTag("Exit");
		Assert.IsNotNull (exit);
	}

	//tests for atleast 1 goal 
	[UnityTest]
	public IEnumerator GoalTest() {
		SetupScene();
		yield return new WaitForFixedUpdate();
		GameObject goal = GameObject.FindGameObjectWithTag("Goal");
		Assert.IsNotNull (goal);
	}

	[UnityTest, Timeout(1000000)]
	public IEnumerator AgentTest(){
		Time.timeScale = 50f;
		SetupScene ();

		yield return new WaitForFixedUpdate ();
		GameObject agentManager = GameObject.Find ("AgentManager");
		AgentManager amComp = agentManager.GetComponent<AgentManager> ();

		GameObject eventManager = GameObject.Find ("EventManager");
		EventManager emComp = eventManager.GetComponent<EventManager> ();
		for (int i = 0; i < emComp.eventTime; ++i) {
				yield return new WaitForSeconds (1);
				if (amComp.AgentCount () == amComp.agentAmount) {
					yield break;
				}
		}

		Assert.Fail ();
	}


	//tests to make sure all agents leave
	[UnityTest, Timeout(1000000)]
	public IEnumerator EmergencyTest(){
		Time.timeScale = 50;
		float emergencyTime = 50f;
		SetupScene ();

		yield return new WaitForFixedUpdate ();

		GameObject HUD = GameObject.Find ("HUD");
		HUD hudC = HUD.GetComponent<HUD> ();


		GameObject agentManager = GameObject.Find ("AgentManager");
		AgentManager amComp = agentManager.GetComponent<AgentManager> ();

		yield return new WaitForSeconds (emergencyTime);
		hudC.emergencyClick ();

		for (int i = 0; i < 50; ++i) {
			yield return new WaitForSeconds (1);
			if (amComp.AgentCount () == 0) {
				yield break;
			}
		}

		Assert.Fail ();
	}

	void SetupScene(){
		SceneManager.LoadScene ("POIScene");
	}
}
