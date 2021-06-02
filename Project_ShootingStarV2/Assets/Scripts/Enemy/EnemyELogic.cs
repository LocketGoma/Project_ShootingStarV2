using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyELogic : MonoBehaviour
{

    [Header("Basic")]
    [Range (0,180)]
    [SerializeField] private float rotateSpeed;
    private float fRotate;
    private EnemyTracking trackingLogic;
    private EnemyTracking.eTrackState prevTrackState;
    private LineRenderer lineRenderer;

    [Header("Attack Logic")]
    [Range(0, 2.5f)]
    [SerializeField] private float shootInterval;   //발사 간격
    private float shootCooltime;                    //사격 쿨타임
    [SerializeField] private Vector3 targetPosition;

    [Range(0,5.0f)]
    [SerializeField] private float globalShootInterval;   //발사 간격
    private float globalShootCooltime;                    //사격 쿨타임

    [Range(0, 1000)]
    [SerializeField] private int ReachRange;
    private int attackDamage;
    bool checkRayHit;


    [SerializeField] private GameObject targetObject;

    //발사 방식
    //1. 쿨타임 > 인터벌일때 플레이어 좌표 저장
    //2. 쿨타임 = 0
    //3. 다시 쿨타임 > 인터벌이 될때 이전 좌표로 사격 후 다시 좌표 변경 - 까지 구현
    //4. 글로벌 쿨타임이 차면 사격 1턴 쉼.
    // Start is called before the first frame update
    void Start()
    {
        trackingLogic = GetComponent<EnemyTracking>();
        if (trackingLogic == null)
        {
            Debug.LogError("Tracking Logic is not ready!!");
        }

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("lineRenderer is not ready!!");
        }

        fRotate = 0;
        targetObject = GameObject.FindGameObjectWithTag("Player");
        attackDamage = GetComponent<EnemyStatus>().AttackDamage;

        targetPosition = transform.position;
    }

    //나중에 빗나가면 바닥에 튀게 만들기
    // Update is called once per frame
    void Update()
    {
        fRotate += Time.deltaTime * 180.0f * rotateSpeed;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0.0f, fRotate, 0.0f);

        shootCooltime += Time.deltaTime;

        Debug.DrawRay(transform.position, targetPosition - transform.position, new Color(0,1,0));

        if (shootCooltime > shootInterval)
        {
            if (trackingLogic.TrackState == EnemyTracking.eTrackState.InAttackRange)
            {
                //사격 판정
                Ray ray = new Ray(transform.position, targetPosition - transform.position);

                RaycastHit hit;

                checkRayHit = Physics.Raycast(ray, out hit, ReachRange);
                if (checkRayHit)
                {
                    //Debug.Log(hit.collider.tag);
                    if (hit.collider.tag == "Player")
                    {
                        //Debug.Log("attackDamage" + attackDamage);
                        hit.collider.gameObject.transform.parent.GetComponent<PlayerStatus>().HurtHP(attackDamage);
                    }
                    targetPosition = hit.point;
                }
                StartCoroutine("ShootEffect");

            }
            targetPosition = targetObject.transform.position;
            shootCooltime = 0;
        }

    }

    IEnumerator ShootEffect()
    {
        lineRenderer.SetPosition(0, transform.position);

        //Debug.Log("Hit state :" + checkRayHit);

        if (checkRayHit == true)
            lineRenderer.SetPosition(1, targetPosition);
        else
            lineRenderer.SetPosition(1, targetPosition* ReachRange);

        lineRenderer.enabled = true;



        yield return new WaitForSeconds(0.1f);

        lineRenderer.enabled = false;
        StopCoroutine("ShootEffect");
    }

}
