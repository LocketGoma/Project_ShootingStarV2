using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager instance;

    [SerializeField] private LoadData loadData;
    [SerializeField] private BridgeLoadData bridgeLoadData;
    [SerializeField] private GameObject[] RoomList;     //Room List
    [SerializeField] private GameObject[] BridgeList;     //Bridge List
    [SerializeField] private GameObject RoomSample;     //Room Sample Prefab
    [SerializeField] private GameObject BridgeSample;     //Bridge Sample Prefab
    [SerializeField] private int roomCount;
    [SerializeField] private int bridgeCount;
    public JsonParser jsonParser;
    public int RoomCount { get { return roomCount; } set { roomCount = value; } }


    // Start is called before the first frame update
    void Start()
    {
        jsonParser.Init();

        loadData = jsonParser.LoadData;

        if (loadData != null && RoomSample != null) {
            roomCount = loadData.RoomCount;
            Debug.Log(loadData.RoomCount);
            RoomList = new GameObject[loadData.RoomCount];

            for (int i = 0; i < roomCount; i++) {
                Instantiate(RoomSample).transform.parent = gameObject.transform.GetChild(0);
                RoomList[i] = transform.GetChild(0).GetChild(i).gameObject;
                RoomList[i].GetComponent<Room>().Initialized(loadData.Room[i]);
            }
        } else {
            Debug.LogError("Error : Can not make Room");
        }


        bridgeLoadData = jsonParser.BridgeLoadData;
        if (bridgeLoadData != null && RoomSample != null)
        {
            bridgeCount = bridgeLoadData.BridgeCount;
            Debug.Log(bridgeLoadData.BridgeCount);
            BridgeList = new GameObject[bridgeLoadData.BridgeCount];

            for (int i = 0; i < bridgeCount; i++)
            {
                Instantiate(BridgeSample).transform.parent = gameObject.transform.GetChild(1);
                BridgeList[i] = transform.GetChild(1).GetChild(i).gameObject;                
                BridgeList[i].GetComponent<Bridge>().Initialized(bridgeLoadData.Bridge[i]);
            }
        }
        else
        {
            Debug.LogError("Error : Can not make Room");
        }

    }

}
