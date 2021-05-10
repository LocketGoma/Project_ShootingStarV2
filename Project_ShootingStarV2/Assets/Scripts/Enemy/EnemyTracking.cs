using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTracking : MonoBehaviour
{
    public enum eTrackState
    {
        Finding, Tracking, InAttackRange
    }
    [Header("Tracking State")]
    [SerializeField] private eTrackState trackState;
    public eTrackState TrackState { get { return trackState; } set { trackState = value; } }

    [Header("Range")]
    [Range(0, 100)]
    public float trackingRange;
    [Range(0, 100)]
    public float attackRange;

    [Header("Speed")]
    [Range(0, 100)]
    public float moveSpeed;

    [SerializeField] private GameObject targetObject;

    // Start is called before the first frame update
    void Start()
    {
        trackState = eTrackState.Finding;
        targetObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if ((targetObject.transform.position - gameObject.transform.position).magnitude < attackRange)
        {
            trackState = eTrackState.InAttackRange;

            transform.rotation = GetRotFromVectors(gameObject.transform.position, targetObject.transform.position);

            //Attack
        }
        //트래킹 사거리보다 짧으면 트래킹
        else if ((targetObject.transform.position - gameObject.transform.position).magnitude < trackingRange)
        {
            trackState = eTrackState.Tracking;
            transform.rotation = GetRotFromVectors(gameObject.transform.position, targetObject.transform.position);

            transform.position += (transform.forward * moveSpeed * Time.deltaTime);
        }
        else
        {
            trackState = eTrackState.Finding;
        }
    }

    private Quaternion GetRotFromVectors(Vector3 posStart, Vector3 posEnd)
    {
        Vector3 lookDir = posEnd - posStart;
        transform.forward = lookDir.normalized;

        Vector3 rotate = Quaternion.LookRotation(lookDir, Vector3.up).eulerAngles;        
        rotate.x = 0.0f;

        return Quaternion.Euler(rotate);
    }

}
