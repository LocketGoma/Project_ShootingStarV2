using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTracking : MonoBehaviour
{
    //어택레인지하고 카이팅 둘다 공격임
    public enum eBossTrackState
    {
        Finding, Tracking, InAttackRange, NoTracking, Kiting    //kite
    }
    [Header("Tracking State")]
    [SerializeField] private eBossTrackState trackState;
    [SerializeField] private bool bossAwake;
    public eBossTrackState TrackState { get { return trackState; } set { trackState = value; } }

    [Header("Range")]
    [Range(0, 250)]
    public float awakeRange;
    [Range(0, 100)]
    public float trackingRange;
    [Range(0, 100)]
    public float attackRange;
    private float rangeGab;
    [Range(0, 5.0f)]
    [SerializeField] private float rangeGabPercent;

    [Header("Speed")]
    [Range(0, 100)]
    public float moveSpeed;

    [SerializeField] private GameObject targetObject;


    private void Awake()
    {
        bossAwake = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        rangeGab = 0.0f;
        trackState = eBossTrackState.NoTracking;
        targetObject = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossAwake)
        {
            if ((targetObject.transform.position - gameObject.transform.position).magnitude < awakeRange)
                bossAwake = true;


            return;
        }

        if (trackState == eBossTrackState.Kiting)
        {
            KitingMove();
        }
        else
        {
            if ((targetObject.transform.position - gameObject.transform.position).magnitude < attackRange + rangeGab)
            {
                trackState = eBossTrackState.InAttackRange;
                rangeGab = attackRange * rangeGabPercent;

                

                transform.rotation = GetRotFromVectors(gameObject.transform.position, targetObject.transform.position);

                //Attack
            }
            //트래킹 사거리보다 짧으면 트래킹
            else if ((targetObject.transform.position - gameObject.transform.position).magnitude < trackingRange)
            {
                rangeGab = 0.0f;

                trackState = eBossTrackState.Tracking;
                transform.rotation = GetRotFromVectors(gameObject.transform.position, targetObject.transform.position);

                ForwardMove();
            }
            else
            {
                trackState = eBossTrackState.Finding;
            }
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
    private void KitingMove()
    {
        transform.position += (transform.right * moveSpeed * 0.5f * Time.deltaTime);
    }
    private void ForwardMove()
    {
        transform.position += (transform.forward * moveSpeed * Time.deltaTime);
    }
}
