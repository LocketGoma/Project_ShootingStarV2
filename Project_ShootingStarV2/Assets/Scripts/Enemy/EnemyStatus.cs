using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    [Header("HP/MP Status")]
    [Range(1, 250)]
    [SerializeField] private int fullHP = 200;
    [Range(1, 250)]
    [SerializeField] private int fullMP = 200;
    [SerializeField] private float nowHP;
    [SerializeField] private float nowMP;
    public int FullHP { get { return fullHP; } }
    public int FullMP { get { return fullMP; } }
    public float NowHP { get { return nowHP; } }
    public float NowMP { get { return nowMP; } }


    //베이직 : 그냥 걸어다님
    //핀홀딩 : 공격이후 고정되어서 쏨 (터렛스타일)
    //플라이어 : 날아다님
    //스피더 : 베이직인데 빠름
    public enum eEnemyType
    {
        Basic,PinHolder,Flyer,Speeder
    }
    //보디체크 : 근접공격
    //슈팅 : 원거리공격
    //밤 투척 : 지연식 원거리 공격
    //멀티 : 다수 공격 수행 (사보타지 제외 나머지)
    //사보타지 : 자폭
    public enum eEnemyAttackType
    {
        BodyCheck,Shoot,ThrowBomb,Multy,Sabotage
    }
    [Header("EnemyType")]
    public eEnemyType enemyType;
    public eEnemyAttackType enemyAttackType;
    [Tooltip("Damage")]
    [SerializeField] private int attackDamage;
    public int AttackDamage { get { return attackDamage; } }


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
            Destroy(gameObject);
        }


    }

    public void HurtHP(int iDamage)
    {        
        nowHP -= iDamage;

        if (nowHP <= 0)
        {
            //아무튼 죽음
            nowHP = 0;

            GetComponent<EnemyDropItem>().ItemDrop();

            Destroy(gameObject);
        }
    }

}
