using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTauntAttack : MonoBehaviour
{
    [Range(0,1.5f)]
    [SerializeField] private float TauntLifeTime;
    [Range(0,20)]
    public int damage;

    // Start is called before the first frame update

    // Update is called once per frame
    private void Start()
    {
        Invoke("TimeOut", TauntLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.parent.GetComponent<PlayerStatus>().HurtHP(damage);
            Destroy(gameObject);
        }
    }


    private void TimeOut()
    {
        Destroy(gameObject);
    }
}
