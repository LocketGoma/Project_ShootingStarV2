using UnityEngine;
using UnityEngine.UI;
//레이캐스트 잔뜩
public class CrossHair : MonoBehaviour
{
    [Header("Base Setting")]
    [SerializeField] private Camera pCamera;
    [Range(0.1f, 25.0f)]
    public float Reach = 25.0F;          //인식 사거리

    [Header("CrossHair")]
    [SerializeField] private GameObject normalCrosshairPrefab;
    //[SerializeField] private GameObject selectedCrosshairPrefab;
    [HideInInspector] private GameObject normalCrosshairPrefabInstance;         //기본 크로스헤어 복사본
    //[HideInInspector] private GameObject selectedCrosshairPrefabInstance;       //선택 크로스헤어 복사본
        
    [Header("Target")]
    [SerializeField] private Vector3 targetVector;

    [Header("Debug Setting")]
    [SerializeField] private bool isOn = true;
    [SerializeField] private Color debugRayColor;   //디버그 레이 시각화 색상
    [Range(0.0f, 1.0f)]
    [SerializeField] private float opacity = 1.0f;   //투명도

    // Start is called before the first frame update
    void Start()
    {
        if (normalCrosshairPrefab != null)
        {
            normalCrosshairPrefabInstance = normalCrosshairPrefab;
        }
        else
        {
            Debug.LogError("System : NormalCrosshair Image was not found!!");
        }
        //if (selectedCrosshairPrefab != null)
        //{
        //    selectedCrosshairPrefabInstance = selectedCrosshairPrefab;
        //}
        //else
        //{
        //    Debug.LogError("System : SelectedCrosshair Image was not found!!");
        //}


        debugRayColor.a = opacity;

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));       //메인 카메라

        RaycastHit hit;
        var checkRayHit = Physics.Raycast(ray, out hit, Reach);
        if (checkRayHit)
        {
            //Debug.Log("Ray hit : "+hit.collider.tag);            
            if (CounterColliderCheck(hit))
            {
                normalCrosshairPrefab.GetComponent<Image>().color = new Color(1, 0, 0);
            }

        }
        if (checkRayHit == false || !CounterColliderCheck(hit))
        {
            normalCrosshairPrefab.GetComponent<Image>().color = new Color(1, 1, 1);
        }

        if (isOn == true)
        {
            Debug.DrawRay(ray.origin, ray.direction * Reach, debugRayColor);
        }

        targetVector = pCamera.transform.forward;
    }
    private bool CounterColliderCheck(RaycastHit hit)
    {
        if (hit.collider.tag != "Player" && hit.collider.tag != "Untagged" && hit.collider.tag != "CounterMove" && hit.collider.tag != "DeadSpace")
            return true;

        return false;
    }

    public Vector3 getTargetVector()
    {
        return targetVector;
    }

}
