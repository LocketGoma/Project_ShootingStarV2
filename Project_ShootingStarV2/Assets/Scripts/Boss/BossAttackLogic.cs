using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossAttackLogic : MonoBehaviour
{
    [Header("Attack Obejcts")]
    [SerializeField] private GameObject chaserMissile;
    [SerializeField] private GameObject normalMissile;
    [SerializeField] private GameObject bigMissile;
    [SerializeField] private GameObject meteoStone;
    [SerializeField] private GameObject tauntObject;            //나중에 이펙트 넣으시오    

    [Header("Batch Obejcts")]
    [SerializeField] private GameObject missilePodL;
    [SerializeField] private GameObject missilePodC;
    [SerializeField] private GameObject missilePodR;

    public enum PattonInfo
    {
        PattonA, PattonB, PattonC, PattonD, PattonE, IDLE
    }
    [Header ("Patton Info")]
    public PattonInfo pattonInfo;

    [Header("Patton Interval / Cooltime")]
    [Range(0, 10.0f)]
    [SerializeField] private float globalAttackCooltime;
    private bool globalAttackLock;

    //인터벌 변수 : 각 공격별 인터벌
    //라이프타임 변수 : 공격 지속시간 (없는애도 있음)
    //쿨타임 변수 : 공격 종료후 쿨타임
    //패턴 선택 기준 : 패턴 선택 타이밍때 쿨타임이 끝난 패턴중 1개 랜덤

    [Tooltip("패턴 A 탄 발사 간격")]
    [Range(0, 10.0f)]
    [SerializeField] private float attackAInterval;
    [Range(0, 20.0f)]
    [SerializeField] private float attackALifeTime;
    [Range(0, 10.0f)]
    [SerializeField] private float attackACooltime;
    private bool atackALock;

    [Tooltip("패턴 B 발사전 유휴시간 (애니메이션용)")]
    [Range(0, 1.0f)]
    [SerializeField] private float attackBStartInterval;
    [Range(0, 10.0f)]
    [SerializeField] private float attackBInterval;
    [Range(0, 10.0f)]
    [SerializeField] private float attackBCooltime;
    private bool atackBLock;

    [Range(0, 10.0f)]
    [SerializeField] private float attackCInterval;
    [Range(0, 10.0f)]
    [SerializeField] private float attackCCooltime;
    private bool atackCLock;

    [Tooltip("패턴 D 대미지 판정 전 유휴시간 (애니메이션용)")]
    [Range(0, 10.0f)]
    [SerializeField] private float attackDStartInterval;
    [Range(0, 10.0f)]
    [SerializeField] private float attackDInterval;
    [Range(0, 10.0f)]
    [SerializeField] private float attackDCooltime;
    private bool atackDLock;

    [Range(0, 10.0f)]
    [SerializeField] private float attackEInterval;
    [Range(0, 10.0f)]
    [SerializeField] private float attackECooltime;
    private bool atackELock;

    [Header("Patton State")]
    //패턴 A용
    [Tooltip("패턴 A 미사일의 추적 간격")]
    [Range(0.0f, 2.0f)]
    [SerializeField] private float PattonATrackingInterval;
    [Tooltip("패턴 A 미사일의 라이프타임")]
    [Range(0.0f, 15.0f)]
    [SerializeField] private float PattonALifetime;

    //패턴 B용
    [Tooltip("패턴 B 미사일의 연속 발사 개수")]
    [Range(0, 15)]
    [SerializeField] private int PattonBShootCount;
    private int PattonBNowShootCount = 0;

    //패턴별 변수들
    [Header("Patton Variable")]
    private int iAswitch;            // 0 : 왼쪽, 1 : 오른쪽, -1 : 중단or양쪽

    private HashSet<int> pattons;

    private void Awake()
    {
        pattons = new HashSet<int>();

        pattons.Add(1);
        pattons.Add(2);
        pattons.Add(3);
        pattons.Add(4);
        pattons.Add(5);
    }

    // Start is called before the first frame update
    void Start()
    {
       // PattonB();
        pattonInfo = PattonInfo.PattonB;
    }

    private void Update()
    {        
        /*
        if (globalAttackLock == false)
        {
            globalAttackLock = true;

            switch (pattonInfo)
            {
                case PattonInfo.PattonA:
                    break;
                case PattonInfo.PattonB:
                    break;
                case PattonInfo.PattonC:
                    break;
                case PattonInfo.PattonD:
                    break;
                case PattonInfo.PattonE:
                    break;
                case PattonInfo.IDLE:
                    break;
                default:
                    break;
            }
        }
        StartCoroutine("PattonChanger");
        */
    }


    public void PattonStop()
    {
        StopAllCoroutines();
    }
    
    private int PattonSelect()
    {
        //https://unity-programmer.tistory.com/32
        var range = Enumerable.Range(0, 5).Where(i => pattons.Contains(i));
        var rand = new System.Random();
        int index = rand.Next(0, pattons.Count);

        return range.ElementAt(index);
    }


    IEnumerator PattonChanger()
    {
        yield return new WaitForSeconds(globalAttackCooltime);
        PattonStop();
        globalAttackLock = globalAttackLock == false;
    }

    //패턴 A : 좌우로 유도 미사일
    public void PattonA()
    {
        StartCoroutine("PattonACoroutine");
        Invoke("InvokePattonA", attackALifeTime);
    }

    private void InvokePattonA()
    {
        StopCoroutine(PattonACoroutine());
    }

    IEnumerator PattonACoroutine()
    {
        while (true)
        {
            GameObject missileObject = null;
            if (iAswitch == 1)
                missileObject = Instantiate(chaserMissile, missilePodL.transform.position, transform.rotation);
            else
                missileObject = Instantiate(chaserMissile, missilePodR.transform.position, transform.rotation);

            if (missileObject != null)
            {
                missileObject.GetComponent<MissileChaser>().trackingInterval = PattonATrackingInterval;
                missileObject.GetComponent<MissileChaser>().weaponLifetime = PattonALifetime;
            }
            
            yield return new WaitForSeconds(attackAInterval);
            iAswitch ^= 1;
        }
    }
    //패턴 B : 입 벌리고 3발 연사 - 비유도

    public void PattonB()
    {
        PattonBNowShootCount = 0;

        transform.GetChild(0).GetComponent<Animator>().Play("BigShot");

        Invoke("InvokePattonB", attackBStartInterval);
    }
    private void InvokePattonB()
    {
        StartCoroutine("PattonBCoroutine");
    }

    IEnumerator PattonBCoroutine()
    {
        while (true)
        {
            GameObject missileObject = null;
            missileObject = Instantiate(normalMissile, missilePodC.transform.position, transform.rotation);

            Debug.Log(PattonBNowShootCount);

            yield return new WaitForSeconds(attackBInterval);
            ++PattonBNowShootCount;

            if (PattonBNowShootCount >= PattonBShootCount)
            {
                StopCoroutine("PattonBCoroutine");
                break;
            }
        }
    }

    //패턴 C : 입 벌리고 큰거 발사 (큰 미사일은 중간에 터지면 파편이 튐 - 클러스터 밤!)

    //패턴 D : 내려찍기

    public void PattonD()
    {
        transform.GetChild(0).GetComponent<Animator>().Play("Taunt");
        Invoke("InvokePattonD", attackDStartInterval);
    }
    private void InvokePattonD()
    {
        StartCoroutine("PattonDCoroutine");
    }

    IEnumerator PattonDCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackDInterval);
            Vector3 vPos = transform.position;
            vPos.y = 0;
            Instantiate(tauntObject, vPos, transform.rotation);
            StopCoroutine("PattonDCoroutine");
        }
    }

    //패턴 E : 내려찍기 3회 -> 메테오

}
