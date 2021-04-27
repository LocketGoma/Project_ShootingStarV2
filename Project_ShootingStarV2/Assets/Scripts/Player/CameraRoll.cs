using UnityEngine;

public class CameraRoll : MonoBehaviour
{
    private Vector2 mouseAbsolute;              //카메라 절대 좌표
    private Vector2 mouseSmooth;                //마우스 움직임 / '부드럽게'
    private Camera pCamera;

    private Vector2 targetDirection;            //카메라 좌표: 고정 필요.
    //private Vector2 targetCharactorDirection; //캐릭터 좌표
    [Header("Camera option & range")]
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 Smoothing = new Vector2(2, 2);
    public Vector2 clampInDegrees = new Vector2(360, 180);

    [SerializeField] private float gab = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        pCamera = gameObject.GetComponent<Camera>();
        targetDirection = transform.localRotation.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion targetOrientation = Quaternion.Euler(targetDirection);       //카메라 회전 오일러 값 -> 쿼터니언으로 변환
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //마우스 움직임 인식. (=vector2를 쓰는 이유)        
        //new Vector2(prevMousePos.x-mouseDelta.x,mouseDelta.y)
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * Smoothing.x, sensitivity.y * Smoothing.y));
        


        //보간 삽입
        mouseSmooth.x = Mathf.Lerp(mouseSmooth.x, mouseDelta.x, 1f / Smoothing.x);
        mouseSmooth.y = Mathf.Lerp(mouseSmooth.y, mouseDelta.y, 1f / Smoothing.y);
        mouseAbsolute += mouseSmooth;

        //clamp (범위 지정)
       // if (clampInDegrees.x <= 360)
       // {
            mouseAbsolute.x = Mathf.Clamp(mouseAbsolute.x, -clampInDegrees.x * gab, clampInDegrees.x * gab);
       // }
        if (clampInDegrees.y < 360)
        {
            mouseAbsolute.y = Mathf.Clamp(mouseAbsolute.y, -clampInDegrees.y * gab, clampInDegrees.y * gab);
        }       

        transform.localRotation = Quaternion.AngleAxis(-mouseAbsolute.y, targetOrientation * Vector3.right); //여기는 최종 절대값으로 (시작지점)

        transform.localRotation *= targetOrientation;       //쿼터니언 값 곱해주기.

        transform.localRotation *= Quaternion.AngleAxis(mouseSmooth.x, transform.InverseTransformDirection(Vector3.up)); //여기는 스무딩 끝난 값으로

    }
}
