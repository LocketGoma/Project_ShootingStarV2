using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyALogic : MonoBehaviour
{
    private EnemyTracking trackingLogic;
    private EnemyTracking.eTrackState prevTrackState;
    [SerializeField] GameObject targetObject;
    public LayerMask layerMask;

    [Range(0,10.0f)]
    [SerializeField] private float AttackRange = 2.5f;

    [Range(0, 10.0f)]
    [SerializeField] private float AttackCooltime;
    private float nowAttackCooltime;
    private bool nowAttackLock;

    private RaycastHit hit;
    private bool boxCastHit;

    // Start is called before the first frame update
    void Start()
    {
        trackingLogic = gameObject.GetComponent<EnemyTracking>();
        if (trackingLogic == null)
        {
            Debug.LogError("Tracking Logic is not ready!!");
        }
        //gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle",-1,0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(targetObject.transform.localScale);
        //targetObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.45f);
        //Debug.Log(targetObject.transform.localScale);

        if (prevTrackState != trackingLogic.TrackState)
        {
            switch (trackingLogic.TrackState)
            {
                case EnemyTracking.eTrackState.Finding:
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle", -1, 0.0f);
                    break;
                case EnemyTracking.eTrackState.Tracking:
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Walk", -1, 0.0f);
                    break;
                case EnemyTracking.eTrackState.InAttackRange:
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Attack", -1, 0.0f);
                    break;
                default:
                    break;
            }
            prevTrackState = trackingLogic.TrackState;
        }


        if (trackingLogic.TrackState == EnemyTracking.eTrackState.InAttackRange)
        {
            if (nowAttackLock == false)
            {
                boxCastHit = Physics.BoxCast(transform.position, transform.lossyScale*2, transform.forward, out hit, transform.rotation, AttackRange, layerMask);

                if (boxCastHit)
                {
                   // Debug.Log(hit.collider.tag);
                    if (hit.collider.tag == "Player")
                    {
                        hit.collider.gameObject.transform.parent.GetComponent<PlayerStatus>().HurtHP(GetComponent<EnemyStatus>().AttackDamage);
                        nowAttackLock = true;
                    }

                }
            }
            nowAttackCooltime += Time.deltaTime;

            if (nowAttackCooltime > AttackCooltime)
            {
                gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Attack", -1, 0.0f);

                nowAttackLock = false;
                nowAttackCooltime = 0;
            }
        }
        else
        {
            nowAttackLock = false;
            nowAttackCooltime = 0;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (boxCastHit)
        {
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);
            Gizmos.DrawWireCube(transform.position + transform.forward * hit.distance, transform.lossyScale*2);

        }
        else
        {
            Gizmos.DrawRay(transform.position, transform.forward * AttackRange);
            Gizmos.DrawWireCube(transform.position + transform.forward * AttackRange, transform.lossyScale*2);
        }
    }

}
