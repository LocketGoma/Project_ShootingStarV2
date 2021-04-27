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

    void Start()
    {
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

        transform.position = targetHead.transform.position + (-transform.forward+(transform.up* cameraUp)+(transform.right*(cameraShoulderGab*((float)shoulderState-0.5f)*2))) * cameraRange;        
    }
}
