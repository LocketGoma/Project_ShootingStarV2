using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        None, Ammo, HP, MP
    }
    public ItemType itemType = ItemType.Ammo;
    [Range(0, 200)]
    [SerializeField] private int itemAmount;
    private float fRotate;
    [SerializeField] private float rotateSpeed;


    public ParticleSystem exploseParticle;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fRotate += Time.deltaTime*180.0f* rotateSpeed;
        transform.rotation = Quaternion.Euler(0.0f, fRotate, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Player")
        {
            if (itemType == ItemType.HP)
            {
                if (other.gameObject.transform.parent.GetComponent<PlayerStatus>().RestoreHP(itemAmount))
                {
                    Instantiate(exploseParticle, transform.position, transform.rotation);

                    if (exploseParticle != null)
                        exploseParticle.Play();

                    Destroy(gameObject);
                }
            }


            if (itemType == ItemType.MP)
            {
                if (other.gameObject.transform.parent.GetComponent<PlayerStatus>().RestoreMP(itemAmount))
                {
                    Instantiate(exploseParticle, transform.position, transform.rotation);

                    if (exploseParticle != null)
                        exploseParticle.Play();

                    Destroy(gameObject);
                }
            }

            if (itemType == ItemType.Ammo)
            {
                if (other.gameObject.transform.parent.GetComponent<PlayerStatus>().RestoreAmmo(itemAmount))
                {
                    Instantiate(exploseParticle, transform.position, transform.rotation);

                    if (exploseParticle != null)
                        exploseParticle.Play();

                    Destroy(gameObject);
                }
            }
        }

    }

}
