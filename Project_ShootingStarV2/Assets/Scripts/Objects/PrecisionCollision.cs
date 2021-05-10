using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrecisionCollision : MonoBehaviour
{
    private GameObject forceTarget = null;
    [Range(0.001f, 1.0f)]
    public float ReachGab;

    [Range(0,100)]
    public int iDamage;
    

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        float Reach = gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        RaycastHit hit;
        bool checkRayHit = Physics.Raycast(ray, out hit, Reach * ReachGab);
        if (checkRayHit)
        {
            if (CounterColliderCheck(hit) && (forceTarget == null || forceTarget != hit.collider.gameObject))
            {
                //Debug.Log("Collision Hit!");
                forceTarget = hit.collider.gameObject;
                forceTarget.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Rigidbody>().velocity,ForceMode.Impulse);


                if (hit.collider.tag == "Enemy")
                {
                    hit.collider.gameObject.GetComponent<EnemyStatus>().HurtHP(iDamage);
                }

                //이펙트 처리
                Instantiate(GetComponent<BasicAmmo>().exploseParticle, hit.collider.transform.position, transform.rotation);
                GetComponent<BasicAmmo>().exploseParticle.Play();

                Destroy(gameObject);
            }
        }
    }

    private bool CounterColliderCheck(RaycastHit hit)
    {
        if (hit.collider.tag != "Player" && hit.collider.tag != "Untagged" && hit.collider.tag != "CounterMove" && hit.collider.tag != "DeadSpace" && hit.collider.tag != "Item")
            return true;

        return false;
    }
}
