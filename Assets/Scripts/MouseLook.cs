using UnityEngine;

public enum RotationAxes{
	MouseXAndY = 0,
	MouseX = 1, 
	MouseY = 2
}

/// <summary>
/// Attaching this script to a camera object will allow the user to look around with the mouse.
/// </summary>
public class MouseLook : MonoBehaviour {

	public RotationAxes axes = RotationAxes.MouseXAndY;
    /// <summary>
    /// The camera's horizontal sensitivity
    /// </summary>
	public float sensitivityHor = 5.0f;
    /// <summary>
    /// The camera's vertical sensitivity
    /// </summary>
    public float sensitivityVert = 5.0f;

    private float minimumVert = -90.0f;
    private float maximumVert = 90.0f;

    /// <summary>
    /// How quickly the camera moves
    /// </summary>
	public float speed;
	private float tempSpeed;

	private float rotationX = 0;

	private bool rotationEnabled;

	private Camera mouseCamera;

	// Use this for initialization
	void Start () {
		rotationEnabled = false;

		mouseCamera = this.GetComponent<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (rotationEnabled) {
            RotateCamera();
        }

		if (Input.GetMouseButtonDown (1)) {
			rotationEnabled = true;
		}
		if (Input.GetMouseButtonUp (1)) {
			rotationEnabled = false;
		}

		if (Input.GetKey (KeyCode.LeftShift)) {
			tempSpeed = speed * 5;
		} else {
			tempSpeed = speed;
		}

		float deltaX = Input.GetAxis ("Horizontal");
		float deltaZ = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude (movement, tempSpeed);
		movement *= tempSpeed;
		movement *= Time.deltaTime;

        mouseCamera.transform.Translate(movement);
	}

    private void RotateCamera() {
        if (axes == RotationAxes.MouseX) {
            transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityHor, 0);
        } else if (axes == RotationAxes.MouseY) {
            //vertical rotation 
            rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

            float rotationY = transform.localEulerAngles.y;

            transform.localEulerAngles = new Vector3(rotationX, rotationY);
        } else {
            //both horiz and vert rotation
            rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
            rotationX = Mathf.Clamp(rotationX, minimumVert, maximumVert);

            float delta = Input.GetAxis("Mouse X") * sensitivityHor;
            float rotationY = transform.localEulerAngles.y + delta;

            transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
        }
    }
}
