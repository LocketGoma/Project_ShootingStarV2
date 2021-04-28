using System.IO;
using System.Collections.Generic;
using UnityEngine;

//https://202psj.tistory.com/1261
public class JsonParser : MonoBehaviour
{
    //중요한 파일 포인터니까 CC 대문자로
    private TextAsset LoadJson = null;
    
    private LoadData loadData = null;
    public LoadData LoadData { get { return loadData; } }
    private bool LoadDataState = true;

    
    public void Init()
    {
        LoadJson = (TextAsset)Resources.Load("result", typeof(TextAsset));
        LoadDataState = LoadJson != null;
        if (LoadDataState == true) {
            loadData = JsonUtility.FromJson<LoadData>(LoadJson.ToString());

        } else {
            Debug.LogError("Json 파일 읽기 실패");
        }
    }

    public void Writer(HashSet<RoomData> roomListData)
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

        FileStream fileStream = new FileStream("./TestResult.json", FileMode.OpenOrCreate, FileAccess.Write);
        StreamWriter writer = new StreamWriter(fileStream, System.Text.Encoding.Unicode);

        writer.WriteLine(parseData);
        writer.Close();
    }

}
