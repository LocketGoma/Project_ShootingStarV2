using System.Collections;
using UnityEngine;

public class MissileChaser : MonoBehaviour
{
    [Range(0, 5)]
    public float speed;

    [Range(0, 20)]
    public int damage;

    [Range(0.0f, 2.0f)]
    public float trackingInterval;
    private float trackingCoolTime;

    [Range(0.0f, 10.0f)]
    public float weaponLifetime;

    [SerializeField] private GameObject targetObject;
    [SerializeField] private Vector3 targetDist;
    [SerializeField] private Quaternion trackRotate;

    // Start is called before the first frame update
    void Start()
    {
        targetObject = null;
        targetDist = transform.forward;
        trackRotate = transform.rotation;

        Invoke("TrackLately", trackingInterval * 3);
        Invoke("TimeBreak", weaponLifetime);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.forward);

        transform.position += (transform.forward * speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, trackRotate, Time.deltaTime);
    }

    private void TrackLately()
    {
         targetObject = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine("updateTracking");
    }

    private void TimeBreak()
    {
        Destroy(gameObject);
    }

    IEnumerator updateTracking()
    {
        while (true)
        {
            //targetDist = targetObject.transform.position-transform.position;

        //    transform.LookAt(targetObject.transform.position);
            yield return new WaitForSeconds(trackingInterval);


            trackRotate = GetRotFromVectors(gameObject.transform.position, targetObject.transform.position);

        }
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


    private Quaternion GetRotFromVectors(Vector3 posStart, Vector3 posEnd)
    {
        Vector3 lookDir = posEnd - posStart;
        //transform.forward = lookDir.normalized;

        Vector3 rotate = Quaternion.LookRotation(lookDir, Vector3.up).eulerAngles;        

        return Quaternion.Euler(rotate);
    }

}
