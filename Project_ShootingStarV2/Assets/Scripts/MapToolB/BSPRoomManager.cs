using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BSPRoomManager : MonoBehaviour
{
    [Header("Sample, Make List")]
    [SerializeField] private GameObject[] RoomList;     //Room List
    [SerializeField] private GameObject[] BridgeList;     //Bridge List
    [SerializeField] private GameObject RoomSample;     //Room Sample Prefab
    [SerializeField] private GameObject BridgeSample;     //Bridge Sample Prefab

    [Header("Count")]
    [SerializeField] private int roomCount;
    [SerializeField] private int bridgeCount;

    [Header("Generator Machine")]
    [SerializeField] private JsonParser jsonParser;
    [SerializeField] private LoadData loadData;
    [SerializeField] private BSPRoomGenerator roomGenerator;
    [SerializeField] private BSPBridgeGenerator bridgeGenerator;


    [SerializeField] private int makeCount;                  //필드 - 총 생성 개수
    [SerializeField] private int makeHeigth;                 //필드 - 전체 구역의 높이
    [SerializeField] private int makeWidth;                  //필드 - 전체 구역의 폭    
    [SerializeField] private int makeSize;                   //필드 - 각 블럭당 폭

    private HashSet<RoomData> roomListData;
    private HashSet<BridgeData> bridgeListData;
    public int RoomCount { get { return roomCount; } set { roomCount = value; } }

    public void InputCount(Text text)
    {
        makeCount = int.Parse(text.text);

        if (makeCount > 128)
        {
            makeCount = 128;
        }
        else if (makeCount < 0)
        {
            makeCount = 1;
        }
    }
    public void InputHeigth(Text text)
    {
        makeHeigth = int.Parse(text.text);
    }
    public void InputWidth(Text text)
    {
        makeWidth = int.Parse(text.text);
    }
    public void InputSize(Text text)
    {
        makeSize = int.Parse(text.text);
    }
    void Start()
    {
       // if (jsonParser != null)
       //     MakeRoomFromJson();

        // MakeRoomFromGenerator();

    }
    public void MakeRoomFromJson()
    {

        jsonParser.Init();
        loadData = jsonParser.LoadData;
        roomCount = loadData.RoomCount;
        Debug.Log(loadData.RoomCount);
        RoomList = new GameObject[loadData.RoomCount];


        for (int i = 0; i < roomCount; i++)
        {
            Instantiate(RoomSample).transform.parent = gameObject.transform.GetChild(0);
            RoomList[i] = transform.GetChild(0).GetChild(i).gameObject;
            RoomList[i].GetComponent<Room>().Initialized(loadData.Room[i]);
        }
    }
    public void MakeRoomFromSetting()
    {
        roomGenerator.SetParameters(makeCount, makeWidth, makeHeigth, makeSize);

        MakeRoomFromGenerator();
    }


    public void MakeRoomFromGenerator()
    {
        roomGenerator.GenerateRoom();
        roomListData = roomGenerator.GetRoomList();
        roomCount = roomListData.Count;
        RoomList = new GameObject[roomCount];

        int i = 0;
        foreach (RoomData rm in roomListData)
        {
            Instantiate(RoomSample).transform.parent = gameObject.transform.GetChild(0);
            RoomList[i] = transform.GetChild(0).GetChild(i).gameObject;
            RoomList[i].GetComponent<Room>().Initialized(rm);

            i++;
        }
    }

    public void MakeBridge()
    {
        bridgeGenerator.RoomListData = roomListData;        

        bridgeGenerator.GenerateBridge();

        bridgeListData = bridgeGenerator.GetBridgeList();
        bridgeCount = bridgeListData.Count;
        BridgeList = new GameObject[bridgeCount];

        int i = 0;
        foreach (BridgeData bm in bridgeListData)
        {
            Instantiate(BridgeSample).transform.parent = gameObject.transform.GetChild(1);
            BridgeList[i] = transform.GetChild(1).GetChild(i).gameObject;
            BridgeList[i].GetComponent<Bridge>().Initialized(bm);            
            i++;
        }

    }

    public void RoomReset()
    {
        roomListData.Clear();
        foreach (Transform room in transform)
        {
            Destroy(room.gameObject);
        }
    }

    public void SaveRoom()
    {
        Debug.Log(roomListData.Count);
        jsonParser.WriteRoom(roomListData);
    }
    public void SaveBridge()
    {
        Debug.Log(bridgeListData.Count);
        jsonParser.WriteBridge(bridgeListData);
    }
}


