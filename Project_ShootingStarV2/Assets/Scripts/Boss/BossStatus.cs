using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : MonoBehaviour
{
    [Header("HP/MP Status")]
    [Range(1, 2500)]
    [SerializeField] private int fullHP = 2000;
    [Range(1, 2500)]
    [SerializeField] private int fullMP = 2000;
    [SerializeField] private float nowHP;
    [SerializeField] private float nowMP;

    public int FullHP { get { return fullHP; } }
    public int FullMP { get { return fullMP; } }
    public float NowHP { get { return nowHP; } }
    public float NowMP { get { return nowMP; } }

    private void Awake()
    {
        nowHP = fullHP;
        nowMP = FullMP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount == 0)
        {
            DeadMotion();
        }

    }

    public void HurtHP(int iDamage)
    {
        nowHP -= iDamage;

        if (nowHP <= 0)
        {
            //아무튼 죽음
            nowHP = 0;

            //GetComponent<EnemyDropItem>().ItemDrop();

            DeadMotion();
        }
    }

    //죽을때 퍼퍼펑
    private void DeadMotion()
    {
        Destroy(gameObject);
    }

}
