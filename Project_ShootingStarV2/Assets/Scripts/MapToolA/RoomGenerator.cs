using System.Collections.Generic;
using UnityEngine;

//= Room Generate Manager
//여기 코드는 사용자가 직접 접근하는것은 하나도 없도록 (전부 상위 클래스에서 관리)
public class RoomGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private HashSet<RoomData> RoomList = new HashSet<RoomData>();
    [Header("Room Size / Position setting")]
    [Range(0, 100)]
    [SerializeField] private int minRoomSize = 1;
    [Range(0, 100)]
    [SerializeField] private int maxRoomSize = 10;
    [Range(20, 1000)]
    [SerializeField] private int maxPosition_X = 50;
    [Range(20, 1000)]
    [SerializeField] private int maxPosition_Y = 50;


    [Header("Room Make Runs Setting")]
    [Range(0, 100)]
    [SerializeField] private int makeRuns;
    [Range(50, 250)]
    [SerializeField] private int maxRuns = 100;

    [Header("Seed / Try count")]
    [Range(10, 20)]   //2^10~2^20
    [SerializeField] public int maxTryExponential = 12;
    [SerializeField] private int maxTry = 1024;

    void Start()
    {

        if (makeRuns <= 0)
            makeRuns = maxRuns;

        maxTry = (int)Mathf.Pow(2, maxTryExponential);
        Debug.Log("Runs : " + makeRuns + "try : " + maxTry);
    }

    public bool GenerateRoom()
    {
        return GererateRoomData(makeRuns);
    }
    public bool GenerateRoom(int runs)
    {
        return GererateRoomData(runs);
    }
    public void SetParameters(int count, int width, int height, int size)
    {
        makeRuns = count;
        maxPosition_X = width;
        maxPosition_Y = height;
        minRoomSize = size;
    }


    public static void DataPrint(RoomData data)
    {
        Debug.Log("LeftX : " + data.Axis_LX + " ,LeftY : " + data.Axis_LY + "\nRightX :" + data.Axis_RX + " ,RightY : " + data.Axis_RY);
    }
    public static void DataPrint(int index, RoomData data)
    {
        Debug.Log("index : " + index + "\nLeftX : " + data.Axis_LX + " ,LeftY : " + data.Axis_LY + "\nRightX :" + data.Axis_RX + " ,RightY : " + data.Axis_RY);
    }
    private bool GererateRoomData(int runs)
    {
        int loopCount = 0;
        int roomCount = 1;
        while (loopCount++ < maxTry && RoomList.Count < runs)
        {
            Debug.Log(System.DateTime.Now.Ticks);
            Random.InitState((int)System.DateTime.Now.Ticks);      //랜덤 시드 초기화
            var LeftKeys = Random.Range(0, maxPosition_X);
            var RightKeys = Random.Range(0, maxPosition_Y);

            RoomData tempData = new RoomData(roomCount, LeftKeys, RightKeys, LeftKeys + Random.Range(minRoomSize, maxRoomSize), RightKeys + Random.Range(minRoomSize, maxRoomSize));
            DataPrint(loopCount, tempData);

            bool isIntersection = false;

            if (RoomList.Count > 0)
            {           //중복 체크 
                foreach (RoomData rm in RoomList)
                {
                    loopCount++;                //중복 체크 안에서도 루프 카운트 소비.

                    if (CheckIntersection(rm, tempData) == true)
                    {
                        Debug.Log("Check!");
                        isIntersection = true;
                        break;
                    }
                }
                if (isIntersection == false)
                {
                    Debug.Log("maked");
                    RoomList.Add(tempData);
                    roomCount++;
                }
            }
            else
            {
                RoomList.Add(tempData);
                roomCount++;
            }
        }

        Debug.Log("Try:" + loopCount + " RoomCount:" + RoomList.Count);

        return (RoomList.Count >= makeRuns);
    }

    //AABB
    private bool CheckIntersection(RoomData rmD1, RoomData rmD2)
    {
        //좌측 max -> 우측 min하고 비교시 max < min이면 충돌, 아니면 비충돌


        if (((Mathf.Max(rmD1.Axis_LX, rmD1.Axis_RX) <= Mathf.Min(rmD2.Axis_LX, rmD2.Axis_RX))|| (Mathf.Min(rmD1.Axis_LX, rmD1.Axis_RX)>= Mathf.Max(rmD2.Axis_LX, rmD2.Axis_RX))) &&
            ((Mathf.Max(rmD1.Axis_LY, rmD1.Axis_RY) <= Mathf.Min(rmD2.Axis_LY, rmD2.Axis_RY))|| (Mathf.Min(rmD1.Axis_LY, rmD1.Axis_RY) >= Mathf.Max(rmD2.Axis_LY, rmD2.Axis_RY))))
            return false;

        return true;
    }

    public HashSet<RoomData> GetRoomList()
    {
        return RoomList;
    }

}

