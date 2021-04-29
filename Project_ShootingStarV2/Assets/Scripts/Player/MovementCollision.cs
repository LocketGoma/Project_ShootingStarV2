using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCollision : MonoBehaviour
{
    public GameObject playerParent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeadSpace")
        {
            playerParent.GetComponent<PlayerMovement>().RevokePosition();
            transform.localPosition = playerParent.transform.position;
            transform.localPosition = new Vector3(0.02f, 0.0f, -0.1f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Untagged" || other.gameObject.tag == "Room")
        {
            playerParent.GetComponent<PlayerMovement>().SavePositionLately();            
        }

        if (other.gameObject.tag == "CounterMove")
        {
            playerParent.GetComponent<PlayerMovement>().RevokeCall();
        }
    }
}
