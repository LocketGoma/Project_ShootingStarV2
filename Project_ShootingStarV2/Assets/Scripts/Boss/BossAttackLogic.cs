using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackLogic : MonoBehaviour
{
    [Header("Obejcts")]
    [SerializeField] private GameObject chaserMissile;
    [SerializeField] private GameObject normalMissile;
    [SerializeField] private GameObject bigMissile;
    [SerializeField] private GameObject meteoStone;
    [SerializeField] private GameObject missilePodL;
    [SerializeField] private GameObject missilePodR;

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

    [Range(0, 10.0f)]
    [SerializeField] private float attackDInterval;
    private float attackDCooltime;
    private bool atackDLock;

    [Range(0, 10.0f)]
    [SerializeField] private float attackEInterval;
    private float attackECooltime;
    private bool atackELock;


    //패턴별 변수들
    [Header("Patton Variable")]
    private int iAswitch;            // 0 : 왼쪽, 1 : 오른쪽, -1 : 중단or양쪽


    // Start is called before the first frame update
    void Start()
    {
        PattonA();
    }

    public void PattonStop()
    {
        StopAllCoroutines();
    }


    //패턴 A : 좌우로 유도 미사일
    public void PattonA()
    {
        StartCoroutine("PattonACoroutine");
    }

    IEnumerator PattonACoroutine()
    {
        while (true)
        {
            if (iAswitch == 1)
                Instantiate(chaserMissile, missilePodL.transform.position, transform.rotation);
            else
                Instantiate(chaserMissile, missilePodR.transform.position, transform.rotation);



            yield return new WaitForSeconds(attackAInterval);
            iAswitch ^= 1;
        }
    }

    //패턴 B : 입 벌리고 3발 연사

    //패턴 C : 입 벌리고 큰거 발사 (큰 미사일은 중간에 터지면 파편이 튐)

    //패턴 D : 내려찍기

    //패턴 E : 내려찍기 3회 -> 메테오

}
