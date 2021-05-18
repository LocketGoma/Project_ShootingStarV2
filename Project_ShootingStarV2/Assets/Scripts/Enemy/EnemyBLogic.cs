using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBLogic : MonoBehaviour
{
    private EnemyTracking trackingLogic;
    private EnemyTracking.eTrackState prevTrackState;
    [SerializeField] private GameObject shootAmmo;
    public LayerMask layerMask;

    [Range(0, 10.0f)]
    [SerializeField] private float AttackRange = 2.5f;

    [Range(0, 10.0f)]
    [SerializeField] private float AttackCooltime;
    private float nowAttackCooltime;
    private bool nowAttackLock;



    // Start is called before the first frame update
    void Start()
    {
        trackingLogic = gameObject.GetComponent<EnemyTracking>();
        if (trackingLogic == null)
        {
            Debug.LogError("Tracking Logic is not ready!!");
        }

    }

    // Update is called once per frame
    void Update()
    {
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
                
                Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

                  Instantiate(shootAmmo, pos, transform.rotation);
                

                nowAttackLock = true;
            }

            nowAttackCooltime += Time.deltaTime;

            if (nowAttackCooltime > AttackCooltime)
            {
                gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Attack", -1, 0.0f);

                nowAttackLock = false;
                nowAttackCooltime = 0;
            }
        }
    }
}
