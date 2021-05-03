using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [Header("HP/MP Status")]
    [Range (1,250)]
    [SerializeField] private int fullHP = 200;
    [Range (1,250)]
    [SerializeField] private int fullMP = 200;
    [SerializeField] private float nowHP;
    [SerializeField] private float nowMP;


    public int FullHP { get { return fullHP; } }
    public int FullMP { get { return fullMP; } }
    public float NowHP { get { return nowHP; } }
    public float NowMP { get { return nowMP; } }

   // public void HurtHP (int iDamage) { nowHP -= iDamage; if (iDamage < 0) nowHP = 0; }
    
    public void RestoreHP (int iRestore) { nowHP += iRestore; if (nowHP > FullHP) nowHP = fullHP; }
    public void RestoreMP (int iRestore) { nowMP += iRestore; if (nowMP > FullMP) nowMP = fullMP; }

    [Header("Restore Cooltime")]
    //HP 회복 진입 필요시간 / 전체 회복에 걸리는 시간
    [SerializeField] private float hpRechargeCooltime = 5.0f;
    [SerializeField] private float hpRestoreTime = 200.0f;

    //MP 회복 진입 필요시간 / 전체 회복에 걸리는 시간
    [SerializeField] private float mpRechargeCooltime = 0.0f;
    [SerializeField] private float mpRestoreTime = 100.0f;

    [Header("Shield, use MP")]
    [SerializeField] private bool useMagicShield;

    [Header("Visual")]
    [SerializeField] private GameObject HPBar;
    [SerializeField] private GameObject MPBar;
    [SerializeField] private Text HPText;
    [SerializeField] private Text MPText;
    private bool HPWarning;
    private bool MPWarning;
    private float HPWarningTime;
    private float MPWarningTime;

    private void Awake()
    {
        //fullHP = 200;
        //fullMP = 200;
        //hpRechargeCooltime = 5.0f;
        //hpRestoreTime = 200.0f;
        //mpRechargeCooltime = 0.0f;
        //mpRestoreTime = 100.0f;


        nowHP = fullHP;
        nowMP = fullMP;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nowMP < fullMP)
        {
            nowMP += Time.deltaTime * fullMP * (1 / mpRestoreTime);
        }

        HPBar.transform.localScale = new Vector3((nowHP / fullHP),1,1);
        MPBar.transform.localScale = new Vector3((nowMP / fullMP),1,1);
        HPText.text = (int)nowHP + "/" + fullHP;
        MPText.text = (int)nowMP + "/" + fullMP;

        if (MPWarning)
        {
            StartCoroutine("WarningMPSign");
            MPWarning = false;
        }
    }

    public void HurtHP(int iDamage)
    {
        if (useMagicShield)
        {
            if (nowMP * 2 > iDamage)
            {
                nowMP -= (iDamage * 2);
                return;
            }
            else if (nowMP > 0)
            {
                iDamage -= (int)(nowMP / 2);
                nowMP = 0;
            }
        }
        nowHP -= iDamage;
    }

    public bool UseMP(int iCost) 
    { 
        if (nowMP < iCost)
        {
            MPWarning = true;
        }
        else
        {
            nowMP -= iCost; 
            
            MPWarning = false;
        }
        return !MPWarning;
    }


    IEnumerator WarningMPSign()
    {
        while (true)
        {
            MPText.color = new Color(1, 1 - ((Mathf.Sin(MPWarningTime) + 1) / 2), 1 - ((Mathf.Sin(MPWarningTime) + 1) / 2));
            MPWarningTime += Time.deltaTime*5.0f;

            Debug.Log(MPWarningTime + "vs" + Mathf.Deg2Rad * 90.0f + "=" + (Mathf.Sin(MPWarningTime) + 1) / 2);

            if (MPWarningTime > Mathf.Deg2Rad * 720.0f)
            {
                StopCoroutine("WarningMPSign");

                MPText.color = new Color(1, 1, 1);
                MPWarningTime = 0.0f;

                yield return null;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
