using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Room 상속을 받는게 나으려나
public class Bridge : MonoBehaviour
{
    [SerializeField] protected int BridgeNo;    //브릿지 번호
    [SerializeField] protected int NodeRoomANo; //연결된 방 번호 A
    [SerializeField] protected int NodeRoomBNo; //연결된 방 번호 B
    [SerializeField] protected int Axis_LX;
    [SerializeField] protected int Axis_LY;   
    [SerializeField] protected int Axis_RX;
    [SerializeField] protected int Axis_RY;


    // Start is called before the first frame update
    void Start()
    {
        //if (RoomManager.instance == null)
        //{
        //    Debug.LogError("RoomManager Setting Error!");
        //}
    }
    public void Initialized(BridgeData bridgeInput)
    {
        BridgeNo = bridgeInput.BridgeNo;
        Axis_LX = bridgeInput.Axis_LX;
        Axis_LY = bridgeInput.Axis_LY;
        Axis_RX = bridgeInput.Axis_RX;
        Axis_RY = bridgeInput.Axis_RY;


        //Debug.Log(RoomNo);

        InitBatch();
    }

    public void InitBatch()
    {
        //gameObject.transform.position = new Vector3(Axis_LX, 0, Axis_LY);
        gameObject.transform.position = new Vector3(Axis_LX + Mathf.Abs(Axis_LX - Axis_RX) / 2, 0.0f, Axis_LY + Mathf.Abs(Axis_LY - Axis_RY) / 2);
        gameObject.transform.localScale = new Vector3(Axis_RX - Axis_LX, 1, Axis_RY - Axis_LY);

    }
}
