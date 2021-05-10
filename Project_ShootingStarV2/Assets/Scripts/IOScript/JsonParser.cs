using System.IO;
using System.Collections.Generic;
using UnityEngine;

//https://202psj.tistory.com/1261
public class JsonParser : MonoBehaviour
{
    //중요한 파일 포인터니까 CC 대문자로
    private TextAsset LoadJson_Room = null;
    private TextAsset LoadJson_Bridge = null;
    
    private LoadData loadData = null;
    private BridgeLoadData bridgeLoadData = null;
    public LoadData LoadData { get { return loadData; } }
    public BridgeLoadData BridgeLoadData { get { return bridgeLoadData; } }
    private bool LoadDataState = true;

    
    public void Init()
    {
        LoadJson_Room = (TextAsset)Resources.Load("Room", typeof(TextAsset));
        LoadDataState = LoadJson_Room != null;
        if (LoadDataState == true) {
            loadData = JsonUtility.FromJson<LoadData>(LoadJson_Room.ToString());
        } else {
            Debug.LogError("Json - Room 파일 읽기 실패");
        }


        LoadJson_Bridge = (TextAsset)Resources.Load("Bridge", typeof(TextAsset));
        LoadDataState = LoadJson_Bridge != null;
        if (LoadDataState == true)      //여기까진 잘들어가는데
        {
            bridgeLoadData = JsonUtility.FromJson<BridgeLoadData>(LoadJson_Bridge.ToString());
        }
        else
        {
            Debug.LogError("Json - Bridge 파일 읽기 실패");
        }
        //Debug.Log(bridgeLoadData.Bridge[0]);

    }

    public void WriteRoom(HashSet<RoomData> roomListData)
    {
        string parseData = "{ \n  \"RoomCount\" : \" "+roomListData.Count+ "\", \n  \"Room\":[";

        int roomNumber = 0;
        foreach (RoomData rm in roomListData)
        {
            roomNumber++;
            parseData += JsonUtility.ToJson(rm);

            if (roomNumber < roomListData.Count)
                parseData += ",\n";
            else
                parseData += "\n";
        }
        parseData += "  \n]\n}";

        FileStream fileStream = new FileStream("./TestResult.json", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(parseData);
        writer.Close();
    }
    public void WriteBridge(HashSet<BridgeData> bridgeListData)
    {
        string parseData = "{ \n  \"BridgeCount\" : \" " + bridgeListData.Count + "\", \n  \"Bridge\":[";

        int BridgeNumber = 0;
        foreach (BridgeData rm in bridgeListData)
        {
            BridgeNumber++;
            parseData += JsonUtility.ToJson(rm);

            if (BridgeNumber < bridgeListData.Count)
                parseData += ",\n";
            else
                parseData += "\n";
        }
        parseData += "  \n]\n}";

        FileStream fileStream = new FileStream("./TestBridgeResult.json", FileMode.Create, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(parseData);
        writer.Close();
    }
}
