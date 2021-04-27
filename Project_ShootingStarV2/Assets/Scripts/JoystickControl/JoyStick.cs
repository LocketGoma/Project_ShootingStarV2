using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour
{
    [SerializeField]
    private Transform Stick;

    [SerializeField]
    private GameObject Player;

    private Vector3 StickStartPos;
    private Vector3 JoyDir;
    private float Radius;


    // Start is called before the first frame update
    void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickStartPos = Stick.transform.position;

        float canv = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= canv;
    }

    private void Update()
    {
        Player.GetComponent<PlayerMovement>().DragControl(JoyDir);
    }

    public void StickDrag(BaseEventData eventData)
    {
        Debug.Log("drag");

        PointerEventData pData = eventData as PointerEventData;
        Vector3 pos = pData.position;

        JoyDir = (pos - StickStartPos).normalized;

        float dist = Vector3.Distance(pos, StickStartPos);

        if (dist < Radius)
        {
            Stick.position = StickStartPos + JoyDir * dist;
        }
        else
        {
            Stick.position = StickStartPos + JoyDir * Radius;
        }
    }

    public void DragEnd()
    {
        Stick.position = StickStartPos;
        JoyDir = Vector3.zero;
    }


}
