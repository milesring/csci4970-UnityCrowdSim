using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class BlendTreeControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        var NavMeshAgent = this.GetComponent<NavMeshAgent>();


        NavMeshAgent.updatePosition = false;
        //NavMeshAgent.updateRotation = true;

        /* angle = Vector3.Angle(NavMeshAgent.velocity.normalized, this.transform.forward);
        if (NavMeshAgent.velocity.normalized.x < this.transform.forward.x)
        {
            angle *= -1;
        }
        angle = (angle + 180.0f) % 360.0f;
        angle = Mathf.Deg2Rad * angle;*/

        var Animator = this.GetComponent<Animator>();
        Animator.SetFloat("Speed", NavMeshAgent.desiredVelocity.magnitude);
        Animator.SetFloat("Heading", 0);


        Vector3 worldDeltaPosition = NavMeshAgent.nextPosition - transform.position;


        if (worldDeltaPosition.magnitude > 0)
            NavMeshAgent.nextPosition = transform.position + 0.9f * worldDeltaPosition;


    }

    void OnAnimatorMove()
    {
        var NavMeshAgent = this.GetComponent<NavMeshAgent>();
        var anim = this.GetComponent<Animator>();

        Vector3 position = anim.rootPosition;
        position.y = NavMeshAgent.nextPosition.y;
        transform.position = position;
    }
}
