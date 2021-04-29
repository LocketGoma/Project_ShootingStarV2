using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//키에 따라 어깨 위치 조절하게 해줘야할듯
public class CameraPosition : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject targetPlayer;
    [SerializeField] private GameObject targetHead;
    [SerializeField] private GameObject targetLeftShoulder;
    [SerializeField] private GameObject targetRightShoulder;

    enum eShoulderState
    {
        Left,Right
    }
    [SerializeField] private eShoulderState shoulderState = eShoulderState.Right;

    private Camera cam;

    [Range(0.1f, 15.0f)]
    public float cameraRange = 1.0f;
    [Range(0.1f, 5.0f)]
    public float cameraUp = 1.0f;
    [Range(0.1f, 5.0f)]
    public float cameraShoulderGab = 1.0f;

    private bool lerpRange = false;

    private float originalCameraRange;
    private float prevCameraRange;
    private GameObject collisionObject;

    void Start()
    {
        originalCameraRange = cameraRange;
        prevCameraRange = cameraRange;
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            shoulderState = eShoulderState.Left;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            shoulderState = eShoulderState.Right;
        }

        Debug.Log(lerpRange);

        if (lerpRange == true)
            cameraRange = Mathf.Lerp(cameraRange, originalCameraRange, Time.deltaTime*3.0f);

        transform.position = targetHead.transform.position + (-transform.forward+(transform.up* cameraUp)+(transform.right*(cameraShoulderGab*((float)shoulderState-0.5f)*2))) * cameraRange;


        //if (collisionObject != null && cameraRange < (collisionObject.transform.position - targetHead.transform.position).magnitude - (collisionObject.transform.localScale.magnitude/2))
        //    lerpRange = true;

        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        ray.direction = -transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * cameraRange, new Color(1, 0, 0));
        RaycastHit hit;
        bool checkRayHit = Physics.Raycast(ray, out hit, cameraRange);
        if (!checkRayHit)
        {
            //if (hit.collider.tag != "Player" && hit.collider.tag != "PlayerAvatar")
            lerpRange = true;
        }


        prevCameraRange = cameraRange;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "PlayerAvatar")
        {
            lerpRange = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerAvatar")
        {
            cameraRange = Mathf.Lerp(cameraRange, 0, Time.deltaTime * 3.0f);
            lerpRange = false;            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "PlayerAvatar")
        {
            collisionObject = other.gameObject;            
        }
    }
}
