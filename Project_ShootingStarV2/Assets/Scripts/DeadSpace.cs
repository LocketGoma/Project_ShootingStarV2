using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSpace : MonoBehaviour
{
    private int deadCount;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.ToString());

        if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerAvatar")
            Destroy(other.gameObject);
        else
        {
            if (other.gameObject.GetComponent<PlayerMovement>() != null)
                other.gameObject.GetComponent<PlayerMovement>().RevokePosition();
            
            deadCount++;

            if (deadCount > 5)
            {
                other.gameObject.transform.position = Vector3.zero;
                deadCount = 0;
            }
        }

        
    }

}
