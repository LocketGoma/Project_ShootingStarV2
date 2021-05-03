using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementCollision : MonoBehaviour
{
    public GameObject playerParent;

    private void Update()
    {
        transform.localPosition = new Vector3(0.02f, transform.localPosition.y, -0.1f);
    }

    //대미지 왜 두번 들어가냐
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeadSpace")
        {
            playerParent.GetComponent<PlayerMovement>().RevokePosition();
            transform.localPosition = playerParent.transform.position;
            playerParent.GetComponent<PlayerStatus>().HurtHP(20);
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
