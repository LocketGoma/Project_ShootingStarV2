using System.Collections.Generic;
using UnityEngine;

public class BSPBridgeGenerator : MonoBehaviour
{
    //수직, 수평, 십자
    enum eBridgeType
    {
        Vertical, Horizontral, Cross
    }
    private HashSet<RoomData> roomListData = null;

    private HashSet<BridgeData> BridgeList = new HashSet<BridgeData>();

    [Range(1, 5)]
    public int BridgeWidth = 2;         //다리 폭

    public HashSet<RoomData> RoomListData { set { roomListData = value; } }


    public bool GenerateBridge()
    {
        if (roomListData == null)
        {
            Debug.LogError("No Room Setted!");

            return false;
        }
        //1. 룸 전체를 순회하면서
        //2. 가장 가까운 룸을 찾아서
        //3. 브릿지 연결
        int i = 0;
        foreach (RoomData rm in roomListData)
        {
            int j = 0;
            RoomData targetRoom = rm;
            float fRange = float.MaxValue;            
            foreach(RoomData tm in roomListData)
            {
                if (!(i >= j))
                {
                    //Debug.Log("i : " + i + ", j :" + j);

                    Vector3 Pos = new Vector3(rm.Axis_LX + (rm.Axis_RX - rm.Axis_LX) / 2, 0.0f, rm.Axis_LY + (rm.Axis_RY - rm.Axis_LY) / 2)
                                - new Vector3(tm.Axis_LX + (tm.Axis_RX - tm.Axis_LX) / 2, 0.0f, tm.Axis_LY + (tm.Axis_RY - tm.Axis_LY) / 2);

                    //Debug.Log("pos : " + Pos.magnitude + ", now Range : " + fRange);

                    if (Pos.magnitude != 0 && Pos.magnitude < fRange)
                    {
                        //BSPRoomGenerator.DataPrint(tm);
                        targetRoom = tm;
                        fRange = Pos.magnitude;
                    }
                }
                j++;
            }


            Debug.Log("BridgeMake");
            BSPRoomGenerator.DataPrint(rm);
            BSPRoomGenerator.DataPrint(targetRoom);

            if (rm != targetRoom)
            {
                BridgeData data = new BridgeData();
                if (Mathf.Abs((rm.Axis_RX + rm.Axis_LX) / 2 - (targetRoom.Axis_RX + targetRoom.Axis_LX) / 2)
                    > Mathf.Abs((rm.Axis_RY + rm.Axis_LY) / 2 - (targetRoom.Axis_RY + targetRoom.Axis_LY) / 2))
                {
                    //Hori
                    data.BridgeNo = BridgeList.Count + 1;
                    data.NodeRoomANo = rm.RoomNo;
                    data.NodeRoomBNo = targetRoom.RoomNo;

                    

                    //우측 > 좌측
                    if (rm.Axis_LX > targetRoom.Axis_LX)
                    {
                        //반대라고?
                        data.Axis_RX = rm.Axis_LX;
                        data.Axis_LX = targetRoom.Axis_RX;
                    }
                    else
                    {
                        data.Axis_LX = rm.Axis_RX;
                        data.Axis_RX = targetRoom.Axis_LX;
                    }
                    data.Axis_LY = (((rm.Axis_RY + rm.Axis_LY) / 2)+((targetRoom.Axis_RY+targetRoom.Axis_LY)/2))/2;
                    data.Axis_RY = data.Axis_LY + BridgeWidth;
                }
                else
                {
                    //Vert
                    data.BridgeNo = BridgeList.Count + 1;
                    data.NodeRoomANo = rm.RoomNo;
                    data.NodeRoomBNo = targetRoom.RoomNo;
                    //위측 > 아래측
                    if (rm.Axis_LY > targetRoom.Axis_LY)
                    {
                        data.Axis_RY = rm.Axis_LY;                        
                        data.Axis_LY = targetRoom.Axis_RY;                        
                    }
                    else
                    {
                        data.Axis_LY = rm.Axis_RY;                        
                        data.Axis_RY = targetRoom.Axis_LY;
                    }

                    data.Axis_LX = (((rm.Axis_RX + rm.Axis_LX) / 2) + ((targetRoom.Axis_RX + targetRoom.Axis_LX) / 2)) / 2;
                    data.Axis_RX = data.Axis_LX + BridgeWidth;

                }
                BridgeList.Add(data);
            }
            else
            {
                Debug.LogError("rm tm is equal!");
            }
            i++;
        }
        
        return true;
    }



    public HashSet<BridgeData> GetBridgeList()
    {
        return BridgeList;
    }

    public static void DataPrint(BridgeData data)
    {
        Debug.Log("LeftX : " + data.Axis_LX + " ,LeftY : " + data.Axis_LY + "\nRightX :" + data.Axis_RX + " ,RightY : " + data.Axis_RY);
    }
    public static void DataPrint(int index, BridgeData data)
    {
        Debug.Log("index : " + index + "\nLeftX : " + data.Axis_LX + " ,LeftY : " + data.Axis_LY + "\nRightX :" + data.Axis_RX + " ,RightY : " + data.Axis_RY);
    }

}
