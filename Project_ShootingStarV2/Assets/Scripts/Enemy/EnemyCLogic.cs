using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCLogic : MonoBehaviour
{
    private EnemyTracking trackingLogic;
    private EnemyTracking.eTrackState prevTrackState;
    [SerializeField] GameObject targetObject;
    public LayerMask layerMask;

    [SerializeField] private MeshRenderer meshRenderer;


    [Range(0, 10.0f)]
    [SerializeField] private float AttackRange = 2.5f;

    [Range(0, 5.0f)]
    [SerializeField] private float SavotageTime;
                     private float nowSavotageTime;

    [Header("Explosion")]
    private bool attackLock;
    public int raySticks = 100;
    public float exploseSize = 5f;
    public ParticleSystem exploseParticle;
    [Range(0, 100)]
    public int exploseDamage;

    Vector3[] vt;
    Ray[] ry;

    // Start is called before the first frame update
    void Start()
    {
        vt = new Vector3[raySticks];
        ry = new Ray[raySticks];
        MakeRays();

        attackLock = false;

        trackingLogic = gameObject.GetComponent<EnemyTracking>();
        if (trackingLogic == null)
        {
            Debug.LogError("Tracking Logic is not ready!!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (attackLock == true)
        {
            trackingLogic.TrackState = EnemyTracking.eTrackState.InAttackRange;
        }

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
                    gameObject.transform.GetChild(0).GetComponent<Animator>().Play("Idle", -1, 0.0f);
                    trackingLogic.moveSpeed = 0.0f;
                    attackLock = true;
                    break;
                default:
                    break;
            }
            prevTrackState = trackingLogic.TrackState;
        }

        if (trackingLogic.TrackState == EnemyTracking.eTrackState.InAttackRange)
        {
            nowSavotageTime += Time.deltaTime;
            meshRenderer.material.color = new Color(nowSavotageTime / SavotageTime, 1 - nowSavotageTime / SavotageTime, 1 - nowSavotageTime / SavotageTime);

            if (SavotageTime < nowSavotageTime)
            {
                Explose();                
            }
        }
    }

    public void MakeRays()
    {
        for (int i = 0; i < raySticks; i++)
        {
            vt[i] = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            ry[i] = new Ray((vt[i] * MaxInVector3(gameObject.transform.localScale)) + gameObject.transform.position, vt[i]);
            //Debug.Log(vt[i]);
            //Debug.DrawRay(ry[i].Origin, ry[i].Direction * reach, new Color((vt[i].x+1)/2, (vt[i].y + 1) / 2, (vt[i].z + 1) / 2));
        }
    }
    private float MaxInVector3(Vector3 vt)
    {
        float result;
        result = vt.x > vt.y ? vt.x : vt.y;
        result = result > vt.z ? result : vt.z;

        return result;
    }
    private void Explose()
    {
        //파티클 자료 : https://docs.unity3d.com/kr/2018.4/Manual/PartSysExplosion.html

        //파티클 오브젝트가 생성이 안된 상태여서 "재생"이 되지 않았음.
        Instantiate(exploseParticle, transform.position, transform.rotation);

        if (exploseParticle != null)
            exploseParticle.Play();

        //if (exploseParticle.isEmitting) Debug.Log("Emit");
        //if (exploseParticle.isPlaying) Debug.Log("Play");
        //if (exploseParticle.isPaused) Debug.Log("Pause");
        //if (exploseParticle.isStopped) Debug.Log("Stop");        


        meshRenderer.material.color = Color.red;
        for (int i = 0; i < raySticks; i++)
        {
            //Debug.Log(ry[i].origin+">"+ry[i].direction);

            ry[i] = new Ray((vt[i] * MaxInVector3(gameObject.transform.localScale)) + gameObject.transform.position, vt[i]);
            Debug.DrawRay(ry[i].origin, ry[i].direction * exploseSize, new Color((vt[i].x + 1) / 2, (vt[i].y + 1) / 2, (vt[i].z + 1) / 2), 3f);
            RaycastHit hit;
            if (Physics.Raycast(ry[i], out hit, exploseSize) && hit.collider.GetComponent<Rigidbody>() != null)
            {
               // hit.collider.GetComponent<Rigidbody>().AddForce(ry[i].direction * explosePower);

                if (hit.collider.tag == "Player")
                {
                    hit.collider.gameObject.transform.parent.GetComponent<PlayerStatus>().HurtHP(exploseDamage);
                }
            }
        }
        Destroy(gameObject);
    }
}
