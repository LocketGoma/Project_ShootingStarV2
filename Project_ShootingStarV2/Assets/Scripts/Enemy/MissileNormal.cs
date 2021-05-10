using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileNormal : MonoBehaviour
{
    // Start is called before the first frame update

    [Range(0, 10)]
    public float speed;
    
    [Range(0, 10)]
    public int damage;

    void Start()
    {    
        GetComponent<Rigidbody>().AddForce((GameObject.FindGameObjectWithTag("Player").transform.position-transform.position).normalized*speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    private void Update()
    {
        //GetComponent<Rigidbody>().AddForce(transform.forward*speed, ForceMode.Impulse);
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.GetComponent<PlayerStatus>().HurtHP(damage);
        }

        //파티클 팡 터트려주고
        if (other.collider.tag != "Enemy")
            Destroy(gameObject);

    }

}
