using UnityEngine;
using UnityEngine.UI;

//https://docs.unity3d.com/kr/530/Manual/InverseKinematics.html
public class PlayerShooterController : MonoBehaviour
{
    //모션 종류. 기본 / 권총 / 라이플 / 던지기
    public enum eMotionState
    {
        Idle,Handgun,Rifle,Throwable
    };

    //스킬 종류 : 기본 / 권총 / 손가락 총 (스킬1) / 라이플 / 정밀사격 / 투척 / 스킬 2
    public enum eSkillState
    {
        Idle,Handgun, HandShootSkill, Rifle, Precision, Throw, Skill,END
    }
    [SerializeField] private eMotionState motionState;
    [SerializeField] private eSkillState skillState;
    [SerializeField] private bool ReadyState;
    [SerializeField] private bool shootState;
    [SerializeField] private Animator anim;
    [SerializeField] private Text text;

    [Header("Weapon")]
    [SerializeField] private GameObject handPosition;
    [SerializeField] private GameObject weaponHandGun;
    [SerializeField] private GameObject weaponRifle;
    [SerializeField] private GameObject weaponAmmo;
    [Range(0,250.0f)]
    [SerializeField] private float weaponMaxRange;
    [Range(0, 250.0f)]
    [SerializeField] private float weaponAmmoSpeed;
    [SerializeField] private float toTargetRange;
    [Tooltip("60/N : 1min / AmmoCrate")]
    [SerializeField] private float [] weaponROF;
    private float weaponCooltime = float.MaxValue;
    private Vector3 ammoDir;

    [Header("Auto Targeting")]
    [Range(0, 15.0f)]
    [SerializeField] private float AutoTargetRange;
    [SerializeField] private bool autoTargetState;
    [Tooltip("오토타게팅 된 적")]
    [SerializeField] private GameObject autoLookTarget;
    [Tooltip("오토타게팅 마크")]
    [SerializeField] private GameObject autoTargetMarker;
    [SerializeField] private LayerMask layerMask;


    [Header("TargetLook")]
    [SerializeField] private Camera targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        weaponHandGun.SetActive(false);
        weaponRifle.SetActive(false);
        autoTargetMarker.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        WheelState();
        motionState = GetMotion(skillState);

        if (motionState != eMotionState.Throwable)
        {
            GetComponent<PlayerStatus>().SetAmmoUseCount((int)motionState);
        }

        //이거 정밀사격 옵션에다 넣어둡시다. 발동시 초당 1mp 소비.
        AutoTargetSystem();
        
        if (Input.GetKey(KeyCode.X))
        {
            if (autoTargetState == true)
            {
                // autoTargetMarker.SetActive(true);
                targetCamera.transform.LookAt(autoLookTarget.transform);
                autoTargetMarker.transform.position = targetCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0.5f));
            }
        }
       // else
       // {
            //autoTargetMarker.SetActive(false);
        //}

        //여기까지

        anim.SetInteger("WeaponType", (int)motionState);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            ReadyState = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            ReadyState = false;            
        }
        anim.SetBool("ShootReady", ReadyState);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (motionState == eMotionState.Handgun || motionState == eMotionState.Rifle)
            {
                if (weaponCooltime > weaponROF[(int)motionState-1])
                {
                    if (AmmoDirectionCaculator())
                    {
                        if (motionState == eMotionState.Handgun)
                            weaponHandGun.GetComponent<WeaponEffect>().EffectPlay();

                        if (motionState == eMotionState.Rifle)
                            weaponRifle.GetComponent<WeaponEffect>().EffectPlay();

                    }
                    weaponCooltime = 0.0f;
                }
            }
            shootState = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            weaponCooltime = float.MaxValue;
            shootState = false;
        }
        weaponCooltime += Time.deltaTime;

        anim.SetBool("Shoot", shootState);
    }
    public eMotionState GetMotion(eSkillState eSkill)
    {
        switch (eSkill)
        {
        case eSkillState.Idle:
            {
                weaponHandGun.SetActive(false);
                text.text = "";
                return eMotionState.Idle;
            }
        case eSkillState.Handgun:
            {
                weaponHandGun.SetActive(true);
                text.text = "HandGun";
                return eMotionState.Handgun;
            }
        case eSkillState.HandShootSkill:
            {
                weaponHandGun.SetActive(true);
                weaponRifle.SetActive(false);

                text.text = "HandSkill";
                return eMotionState.Handgun;
            }
        case eSkillState.Rifle:
            {
                weaponHandGun.SetActive(false);
                weaponRifle.SetActive(true);
                text.text = "Rifle";
                return eMotionState.Rifle;
            }
        case eSkillState.Precision:
            {
                weaponRifle.SetActive(true);
                text.text = "Precision";
                return eMotionState.Rifle;
            }
        case eSkillState.Throw:
            {
                weaponRifle.SetActive(false);
                text.text = "Throw";
                return eMotionState.Throwable;
            }
        case eSkillState.Skill:
            {
                text.text = "Skill";
                return eMotionState.Idle;
            }
        default:
            {
                text.text = "";
                return eMotionState.Idle;
            }
        }
    }

    private void WheelState()
    {
        float fScroll = Input.GetAxisRaw("Mouse ScrollWheel");
        if (fScroll < 0)
        {
            skillState++;
            if (skillState == eSkillState.END)
            {
                skillState = eSkillState.Idle;
            }
        }
        else if (fScroll > 0)
        {
            if (skillState == 0)
            {
                skillState = eSkillState.END;
            }
            skillState--;
        }
    }

    private bool AmmoDirectionCaculator()
    {
        if (!GetComponent<PlayerStatus>().ShootAmmo((int)motionState))
            return false;

        Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        bool checkRayHit = Physics.Raycast(ray, out hit, weaponMaxRange);
        if (checkRayHit && CounterColliderCheck(hit))
        {

            //toTargetRange = (hit.point - gameObject.transform.position).magnitude;
            ammoDir = hit.point - handPosition.transform.position;

            Instantiate(weaponAmmo, handPosition.transform.position, transform.rotation).GetComponent<Rigidbody>().AddForce(ammoDir.normalized * weaponAmmoSpeed, ForceMode.Impulse);
        }
        else
        {
            toTargetRange = weaponMaxRange;
            ammoDir = ray.direction * toTargetRange;// - handPosition.transform.position;
            Instantiate(weaponAmmo, handPosition.transform.position, transform.rotation).GetComponent<Rigidbody>().AddForce(ammoDir.normalized * weaponAmmoSpeed, ForceMode.Impulse);
        }
        return true;
    }
        
    private void AutoTargetSystem()
    {        
        RaycastHit[] checkRayHit = Physics.SphereCastAll(targetCamera.ScreenToWorldPoint(new Vector3(0.5f, 0.5f, 0f)), AutoTargetRange, targetCamera.transform.forward,weaponMaxRange, layerMask);
        autoLookTarget = null;

        float minDistance = float.MaxValue;        

        for (int i = 0; i < checkRayHit.Length; i++)
        {
            Vector3 targetPos = targetCamera.WorldToScreenPoint(checkRayHit[i].collider.transform.position);
            float screenDistance = (new Vector2(targetPos.x,targetPos.y) - (new Vector2(targetCamera.pixelWidth/2, targetCamera.pixelHeight/2))).magnitude;
            
            if ((checkRayHit[i].collider.CompareTag("Enemy") || checkRayHit[i].collider.CompareTag("Boss"))&& screenDistance < minDistance)
            {
                minDistance = screenDistance;
                autoLookTarget = checkRayHit[i].collider.gameObject;
            }
        }

        if (autoLookTarget != null)
        {
            autoTargetState = true;
            autoTargetMarker.SetActive(true);
            //targetCamera.transform.LookAt(autoLookTarget.transform);          //이건 자동조준포함시
            autoTargetMarker.transform.position = targetCamera.WorldToScreenPoint(autoLookTarget.transform.position);
        }
        else
        {
            autoTargetState = false;
            autoTargetMarker.SetActive(false);
        }
    }


    //https://sorting.tistory.com/12
    private bool CounterColliderCheck(RaycastHit hit)
    {
        if (!hit.collider.CompareTag("Player") && !(hit.collider.CompareTag("PlayerAvatar") && !(hit.collider.CompareTag("DeadSpace"))))
            return true;

        return false;
    }
}
