using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.5f;
    [Range(0.0f,100.0f)]
    public float jumpPower = 10f;
    public Animator anim;
    private Rigidbody rbody;
    [SerializeField]
    private float inputH;
    [SerializeField]
    private float inputV;
    [SerializeField]
    private bool runState = false;
    private int runWeight = 0;
    [SerializeField]
    private bool jumpState = false;

    private float moveX;
    private float moveZ;

    private Vector3 prevPosA;           //1차 (계속 갱신됨)
    private Vector3 prevPosB;           //2차 (한 프레임 늦게 갱신됨)

    private bool collisionWall = false;


    private bool isInvokeCooltime = false;
    private void Start()
    {
        //rbody = playerCharactor.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ControlManager.instance.controlMode == ControlManager.InputMode.PC)
        {
            inputH = Input.GetAxis("Horizontal");
            inputV = Input.GetAxis("Vertical");
        }

        inputH *= (1 + runWeight);
        inputV *= (1 + runWeight);

        anim.SetFloat("inputH", inputH);
        anim.SetFloat("inputV", inputV);
        anim.SetBool("Jump", jumpState);

        moveX = ReduceMovement(inputH) * speed * Time.deltaTime;
        moveZ = ReduceBackMovement(inputV) * speed * Time.deltaTime;

        if (collisionWall != true)        
            transform.Translate(moveX, 0f, moveZ);

        transform.rotation = Quaternion.Euler(0, transform.GetChild(1).transform.rotation.eulerAngles.y, 0);

        if (Input.GetKey(KeyCode.Space))
        {
            anim.PlayInFixedTime("Jump01", 0, 0.35f);       
        }

        if (Input.GetKeyUp(KeyCode.Space) && jumpState == false)
        {
            anim.speed = 1.0f;
            SavePosition();
            JumpAction();
            //Invoke("JumpAction", 0.5f);
            Invoke("JumpRelease", 0.9f);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            runState = true;
            runWeight = 1;
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            runState = false;
            runWeight = 0;
        }

        //Ray ray = new Ray(transform.GetChild(0).transform.position, transform.GetChild(0).transform.forward);
        //RaycastHit hit;
        //Physics.Raycast(ray, out hit, 0.75f);
        //if (hit.collider.tag == "Untagged")
        //{
        //    collisionWall = true;
        //}
        //else
        //{
        //    collisionWall = false;
        //}
        //Debug.DrawRay(transform.GetChild(0).transform.position, transform.GetChild(0).transform.forward,Color.green);

    }
    

    private float ReduceBackMovement(float input)
    {
        return input > 0 ? input : input / 3;
    }
    private float ReduceMovement(float input)
    {
        return input / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);

       // jumpState = false;

        anim.Play("MoveBlend", 0,0.0f);
    }
    private void OnTriggerStay(Collider other)
    {        
        if (other.gameObject.tag == "Untagged")
        {
            RevokeCall();
           //transform.GetChild(0).localPosition = new Vector3(0.02f, 0.0f, -0.1f);
        }
        
    }

    public void DragControl(Vector3 dir)
    {
        if (ControlManager.instance.controlMode == ControlManager.InputMode.Mobile)
        {
            inputH = dir.x*2;
            inputV = dir.y*2;
        }
    }

    public void RevokeCall()
    {
        transform.Translate(-moveX, 0f, -moveZ);
    }

    private void SavePosition()
    {
        prevPosB = prevPosA;
        prevPosA = transform.position;
        isInvokeCooltime = false;
    }
    public void SavePositionLately()
    {
        if (isInvokeCooltime == false)
        {
            Invoke("SavePosition", 0.15f);
            isInvokeCooltime = true;
        }
    }

    public void RevokePosition()
    {
        //Debug.Log(prevPosA - prevPosB);
        transform.position = (prevPosA - (prevPosA-prevPosB)*2.5f);
    }

    private void JumpAction()
    {
        gameObject.GetComponentInChildren<Rigidbody>().AddForce(Vector3.up * jumpPower * gameObject.GetComponentInChildren<Rigidbody>().mass, ForceMode.Impulse);
        
        jumpState = true;
    }
    private void JumpRelease()
    {
        jumpState = false;
        anim.Play("MoveBlend", 0, 0.0f);
    }
}
