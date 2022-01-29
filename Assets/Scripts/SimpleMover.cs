using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleMover : MonoBehaviour
{
	// public Rigidbody2D rb;
	public float PlayerSpeed = 10.0f;
	public NavMeshAgent bigBad;

    // Start is called before the first frame update
    void Start()
    {
		bigBad.updateUpAxis = false;
		//bigBad.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 newPosition = Vector3.zero;
		Vector3 dest;

        /* Wont work in new input system
		if (Input.GetMouseButtonDown (0)) {
			dest = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			dest.z = 0;
			bigBad.SetDestination (dest) ;
		}
        */


		/*
		if (Input.GetAxis ("Horizontal") > 0.0f) {
			newPosition += (Vector3)( Vector2.right * Time.deltaTime * PlayerSpeed);
		} else if (Input.GetAxis ("Horizontal") < 0.0f) {
			newPosition += (Vector3)(Vector2.left * Time.deltaTime * PlayerSpeed);
		}

		if (Input.GetAxis ("Vertical") > 0.0f) {
			newPosition +=  (Vector3)(Vector2.up * Time.deltaTime * PlayerSpeed);
		} else if (Input.GetAxis ("Vertical") < 0.0f) {
			newPosition +=  (Vector3)(Vector2.down * Time.deltaTime * PlayerSpeed);
		}


		rb.MovePosition (transform.position + newPosition);
		*/
    }
}
