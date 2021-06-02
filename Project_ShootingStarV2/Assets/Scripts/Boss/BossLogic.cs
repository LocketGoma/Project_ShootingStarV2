using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLogic : MonoBehaviour
{
    private BossAttackLogic attackLogic;
    private BossTracking trackingLogic;
    private BossTracking.eBossTrackState prevTrackState;
    public LayerMask layerMask;

    [Range(0, 10.0f)]
    [SerializeField] private float attackRange = 2.5f;

    [Header("Global Timer")]
    //패턴별 인터벌
    [Range(0, 25.0f)]
    [SerializeField] private float pattonLifeTime;
    [Range(0, 25.0f)]
    [SerializeField] private float pattonInterval;
    private float pattonCooltime;
    private bool pattonLock;

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
        attackLogic = gameObject.GetComponent<BossAttackLogic>();
        if (attackLogic == null)
        {
            Debug.LogError("Boss Attack Logic is not Ready!");
        }

        gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle", -1, 0.0f);

        pattonLock = false;
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
                    attackLogic.PattonD();
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
                
        pattonCooltime += Time.deltaTime;
        if (pattonLock == false && pattonCooltime > pattonLifeTime)
        {
            pattonLock = true;
            pattonCooltime = 0;

            //attackLogic.PattonStop();
        }
        else if (pattonLock == true && pattonCooltime > pattonInterval)
        {
            pattonLock = false;
            pattonCooltime = 0;
            
            attackLogic.PattonA();

        }


    }
}
