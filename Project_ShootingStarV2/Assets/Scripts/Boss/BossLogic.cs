using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    private BossTracking trackingLogic;
    private BossTracking.eBossTrackState prevTrackState;
    public LayerMask layerMask;

    [Range(0, 10.0f)]
    [SerializeField] private float attackRange = 2.5f;

    [Header("Patton Interval")]
    //각 공격별 인터벌
    [Range(0, 10.0f)]
    [SerializeField] private float attackAInterval;
    private float attackACooltime;
    private bool atackALock;

    [Range(0, 10.0f)]
    [SerializeField] private float attackBInterval;
    private float attackBCooltime;
    private bool atackBLock;

    [Range(0, 10.0f)]
    [SerializeField] private float attackCInterval;
    private float attackCCooltime;
    private bool atackCLock;


    [Header("Global Timer")]
    //패턴별 인터벌
    [Range(0, 25.0f)]
    [SerializeField] private float pattonLifeTime;
    [Range(0, 25.0f)]
    [SerializeField] private float pattonInterval;
    private float pattonCooltime;

    private RaycastHit hit;
    private bool boxCastHit;


    // Start is called before the first frame update
    void Start()
    {

        trackingLogic = gameObject.GetComponent<BossTracking>();
        if (trackingLogic == null)
        {
            Debug.LogError("Boss Tracking Logic is not Ready!");
        }


        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle", -1, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (prevTrackState != trackingLogic.TrackState)
        {
            switch (trackingLogic.TrackState)
            {
                case BossTracking.eBossTrackState.Finding:
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle", -1, 0.0f);
                    break;
                case BossTracking.eBossTrackState.Tracking:
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("BossWalk", -1, 0.0f);
                    break;
                case BossTracking.eBossTrackState.InAttackRange:
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle", -1, 0.0f);
                    break;
                case BossTracking.eBossTrackState.NoTracking:
                    break;
                case BossTracking.eBossTrackState.Kiting:
                    break;
                default:
                    break;
            }
            prevTrackState = trackingLogic.TrackState;



        }



    }
}
