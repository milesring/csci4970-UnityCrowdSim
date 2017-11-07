using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

#region Private
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
    private Transform mainCameraTransform;

    private AgentManager agentManager;
    private EventManager eventManager;
	private string currentPOV = "topDown";
	private readonly string PATRON_NUM_FORMAT = "Number of Patrons: {0}";
    private readonly string EVENT_TIMER_FORMAT = "Event Time: {0:0.00}";
    private readonly string EMERGENCY_TIMER_FORMAT = "Evacuation Time: {0:0.00}";
    private GameObject selectedAgent;
    private Vector3 initialCamPos;
    private Quaternion initialCamRot;
    private float emergencyTimer = 0f;
#endregion

    // Use this for initialization
    void Start () {
		povButton.onClick.AddListener(POVClick);
        emergencyButton.onClick.AddListener(EmergencyClick);

        initialCamPos = mainCameraTransform.position;
        initialCamRot = mainCameraTransform.rotation;

        agentManager = GameObject.Find("AgentManager").GetComponent<AgentManager>();
        eventManager = GameObject.Find("EventManager").GetComponent<EventManager>();
        selectedAgent = GameObject.Find("CrowdAgent(Clone)");
    }

    /// <summary>
    /// How to toggle the camera when the Point of View button is clicked
    /// </summary>
	void POVClick() {
		switch(currentPOV) {
		case "agent":                 // if agent view, change to topDown Position
			currentPOV = "topDown";
                Camera.main.transform.position = initialCamPos;
                Camera.main.transform.rotation = initialCamRot;
                // Hard-coded default position, but above code will allow wherever the main camera starts in scene view
                //Camera.main.transform.position = new Vector3(19.3f, 28.1f, -27.7f);
                //Camera.main.transform.rotation = new Quaternion(0.4f, 0.0f, 0.0f, 0.8f);

                break;
		case "topDown":                 // if topDown view, change to agent Position
                selectedAgent = GameObject.Find("CrowdAgent(Clone)");
                if (selectedAgent)      // unless there is no agent in scene
                {
                    currentPOV = "agent";
                }
            break;
		}
	}

    /// <summary>
    /// What to do when the emergency button is clicked. It notifies all agents that the event is over.
    /// </summary>
    void EmergencyClick()
    {
        agentManager.notifyAgents(true);
        emergencyButton.interactable = false;
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
            Camera.main.transform.position = selectedAgent.transform.position - selectedAgent.transform.forward * 3 + selectedAgent.transform.up * 2;
            Camera.main.transform.rotation = selectedAgent.transform.rotation;
        }
    }
}
