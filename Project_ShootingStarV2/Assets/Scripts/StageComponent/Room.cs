using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] protected int RoomNo;   //방 번호
    [SerializeField] protected int Axis_LX;
    [SerializeField] protected int Axis_LY;   //2차원 기준, 3차원일때는 Y값 -> Z값으로 변경.
    [SerializeField] protected int Axis_RX;
    [SerializeField] protected int Axis_RY;

    public int RoomNumber { get { return RoomNo; } }

    // Start is called before the first frame update
    void Start()
    {
        //if (RoomManager.instance == null)
        //{
        //    Debug.LogError("RoomManager Setting Error!");
        //}
    }
    public void Initialized(RoomData roomInput)
    {
        RoomNo = roomInput.RoomNo;
        Axis_LX = roomInput.Axis_LX;
        Axis_LY = roomInput.Axis_LY;
        Axis_RX = roomInput.Axis_RX;
        Axis_RY = roomInput.Axis_RY;


        //Debug.Log(RoomNo);

        InitBatch();
    }

    public void InitBatch()
    {
        //gameObject.transform.position = new Vector3(Axis_LX, 0, Axis_LY);
        gameObject.transform.position = new Vector3(Axis_LX + Mathf.Abs(Axis_LX-Axis_RX)/2,0.0f, Axis_LY+ Mathf.Abs(Axis_LY - Axis_RY)/2);
        gameObject.transform.localScale = new Vector3(Axis_RX - Axis_LX, 1, Axis_RY - Axis_LY);

    }
}
