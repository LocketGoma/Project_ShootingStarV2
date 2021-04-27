using UnityEngine;
using UnityEngine.UI;

public class PlayerShooterController : MonoBehaviour
{
    //모션 종류. 기본 / 권총 / 라이플 / 던지기
    public enum eMotionState
    {
        Idle,Handgun,Rifle,Throwable,END
    };

    //스킬 종류 : 기본 / 권총 / 손가락 총 (스킬1) / 라이플 / 정밀사격 / 투척 / 스킬 2
    public enum eSkillState
    {
        Idle,Handgun, HandShootSkill, Rifle, Precision, Throw, Skill,END
    }
    [SerializeField] private eMotionState motionState;
    [SerializeField] private eSkillState skillState;
    [SerializeField] private bool shootState;
    [SerializeField] private Animator anim;
    [SerializeField] private Text text;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WheelState();
        motionState = GetMotion(skillState);

        anim.SetInteger("WeaponType", (int)motionState);

        if (Input.GetKey(KeyCode.Mouse1))
        {            
            shootState = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            shootState = false;            
        }
        anim.SetBool("Shoot", shootState);
    }
    public eMotionState GetMotion(eSkillState eSkill)
    {
        switch (eSkill)
        {
        case eSkillState.Idle:
            {
                text.text = "";
                return eMotionState.Idle;
            }
        case eSkillState.Handgun:
            {
                text.text = "HandGun";
                return eMotionState.Handgun;
            }
        case eSkillState.HandShootSkill:
            {
                text.text = "HandSkill";
                return eMotionState.Handgun;
            }
        case eSkillState.Rifle:
            {
                text.text = "Rifle";
                return eMotionState.Rifle;
            }
        case eSkillState.Precision:
            {
                text.text = "Precision";
                return eMotionState.Rifle;
            }
        case eSkillState.Throw:
            {
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

}
