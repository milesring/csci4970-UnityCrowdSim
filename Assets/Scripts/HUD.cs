using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour {

#region Private
    // link to children of the HUD component
    [SerializeField]
	private Text patronCountText;
    [SerializeField]
    private Text eventTimerText;
    [SerializeField]
    private Text emergencyTimerText;
    [SerializeField]
	private Button povButton;
    [SerializeField]
    private Button emergencyButton;
    [SerializeField]
    private Button resetButton;
    [SerializeField]
    private Vector2 patronCameraPosition;

    // access to manager functions
    AgentManager agentManager;
    EventManager eventManager;

    // text formatters
	readonly string PATRON_NUM_FORMAT = "Number of Patrons: {0}";
    readonly string EVENT_TIMER_FORMAT = "Event Time: {0:0.00}";
    readonly string EMERGENCY_TIMER_FORMAT = "Evacuation Time: {0:0.00}";
    float emergencyTimer = 0f;      // timer to record evacuation time

    // camera position variables
    string currentPOV = "topDown";  // current camera mode
    GameObject selectedAgent;       // agent selected for camera to follow 
    Vector3 initialCamPos;          // holds main camera's initial position in scene view
    Quaternion initialCamRot;       // holds main camera's initial rotation in scene view
#endregion

    // Use this for initialization
    void Start () {
        // button listeners
		povButton.onClick.AddListener(povClick);
        emergencyButton.onClick.AddListener(emergencyClick);
        resetButton.onClick.AddListener(resetClick);

        // hold main camera's position in scene view
        initialCamPos = Camera.main.transform.position;
        initialCamRot = Camera.main.transform.rotation;

        // Find other external GameObjects
        agentManager = GameObject.Find("AgentManager").GetComponent<AgentManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        selectedAgent = GameObject.Find("CrowdAgent(Clone)");
    }

    /// <summary>
    /// This method determines how to toggle the camera when the 'Point of View' button is clicked
    /// </summary>
	void povClick() {
		switch(currentPOV) {
		case "agent":                                       // if agent view, change to topDown position
			currentPOV = "topDown";
                Camera.main.transform.position = initialCamPos;
                Camera.main.transform.rotation = initialCamRot;

                // Hard-coded default position, but above code will allow wherever the main camera starts in scene view
                //Camera.main.transform.position = new Vector3(19.3f, 28.1f, -27.7f);
                //Camera.main.transform.rotation = new Quaternion(0.4f, 0.0f, 0.0f, 0.8f);
                break;

		case "topDown":                                     // if topDown view, change to agent position
                selectedAgent = GameObject.Find("CrowdAgent(Clone)");
                if (selectedAgent) currentPOV = "agent";    // unless there is no agent in scene
            break;
		}
	}

    /// <summary>
    /// What to do when the emergency button is clicked. It notifies all agents that the event is over.
    /// </summary>
    void emergencyClick()
    {
        agentManager.notifyAgents(true);
        eventManager.SignalEmergency();
        emergencyButton.interactable = false;
    }

    /// <summary>
    /// Currently broken. Can be used to reload the scene or load next/different scene if fixed.
    /// </summary>
    void resetClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

	// Update is called once per frame
	void Update () {
        // display patron count
		patronCountText.text = string.Format(PATRON_NUM_FORMAT, agentManager.AgentCount());

        // display event timer, red if event is over
        double eventTimer = System.Math.Round(eventManager.getEventTime() - eventManager.getEventTimer());
        if (eventTimer <= 0) eventTimerText.color = Color.red;
        eventTimerText.text = string.Format(EVENT_TIMER_FORMAT, System.Math.Round(eventManager.getEventTime() - eventManager.getEventTimer(), 2));
        
        // Display evacuation timer if emergency button was pressed
        if(!emergencyButton.interactable)
        {
            if(agentManager.AgentCount() > 0)
                emergencyTimerText.text = string.Format(EMERGENCY_TIMER_FORMAT, System.Math.Round(emergencyTimer += Time.deltaTime, 2));
            else emergencyTimerText.text = string.Format(EMERGENCY_TIMER_FORMAT, System.Math.Round(emergencyTimer, 2));
        }

        // have main camera follow an agent if currentPOV is set to agent view
        if (selectedAgent != null && currentPOV.Equals("agent"))
        {
            Camera.main.transform.position = selectedAgent.transform.position - selectedAgent.transform.forward * patronCameraPosition.x + selectedAgent.transform.up * patronCameraPosition.y;
            Camera.main.transform.rotation = selectedAgent.transform.rotation;
        }
    }
}
