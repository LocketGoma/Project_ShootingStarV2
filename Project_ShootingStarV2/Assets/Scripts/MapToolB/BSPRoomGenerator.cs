using System.Collections.Generic;
using UnityEngine;

public class BSPRoomGenerator : MonoBehaviour
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

    [Header("Seed / Try count")]
    //= 몇번 쪼갤것인가
    [Range(0, 10)]   //2^2~2^10
    [SerializeField] public int maxTryExponential = 12;
    [SerializeField] private int maxTry = 1024;

    private BinaryTree roomTree;
    private bool phaseLock = false;                     //생성타이밍에 true 걸고 리턴시키는 역할


    void Start()
    {
        if (maxTry <= 0)
            maxTry = 1;

        roomTree = new BinaryTree();

        maxTry = (int)Mathf.Pow(2, maxTryExponential);
        Debug.Log("Runs : " + maxTryExponential + " try : " + maxTry);
    }
    public bool GenerateRoom()
    {
        return GererateRoomData(maxTryExponential);
    }
    public bool GenerateRoom(int runs)
    {
        return GererateRoomData(runs);
    }
    public void SetParameters(int count, int width, int height, int size)
    {
        maxTryExponential = count;
        maxPosition_X = width;
        maxPosition_Y = height;
        minRoomSize = size;

        maxTry = (int)Mathf.Pow(2, maxTryExponential);
    }

    private bool GererateRoomData(int runs)
    {
        RoomData rootRoom = new RoomData(0,0, maxPosition_X, maxPosition_Y);
        roomTree.Root = new BinaryTreeNode(rootRoom);

        GenerationRoomNode(0, roomTree.Root);

        return RoomList.Count >= maxTry;
    }

    //재귀임
    private bool GenerationRoomNode(int iDepth, BinaryTreeNode nowNode)
    {
        if (phaseLock)
        {
            return phaseLock;
        }

        if (iDepth >= maxTryExponential)
        {
            return true;
        }
        if (Mathf.Abs(nowNode.Data.Axis_LX- nowNode.Data.Axis_RX)<minRoomSize || Mathf.Abs(nowNode.Data.Axis_LY - nowNode.Data.Axis_RY) < minRoomSize)
        {
            return false;
        }
        RoomData rm1 = new RoomData(0,0,0,0);
        RoomData rm2 = new RoomData(0,0,0,0);

        if (Mathf.Abs(nowNode.Data.Axis_LX - nowNode.Data.Axis_RX)> Mathf.Abs(nowNode.Data.Axis_LY - nowNode.Data.Axis_RY))   //가로 자르기
        {
            rm1.Axis_LX = nowNode.Data.Axis_LX;
            rm1.Axis_LY = nowNode.Data.Axis_LY;
            rm1.Axis_RY = nowNode.Data.Axis_RY;

            rm2.Axis_RX = nowNode.Data.Axis_RX;
            rm2.Axis_LY = nowNode.Data.Axis_LY;
            rm2.Axis_RY = nowNode.Data.Axis_RY;


            int iSize = Random.Range(minRoomSize+nowNode.Data.Axis_LX, nowNode.Data.Axis_RX- minRoomSize);

            rm1.Axis_RX = iSize;
            rm2.Axis_LX = iSize;
        }
        else                //세로 자르기
        {
            rm1.Axis_LX = nowNode.Data.Axis_LX;
            rm1.Axis_LY = nowNode.Data.Axis_LY;
            rm1.Axis_RX = nowNode.Data.Axis_RX;

            rm2.Axis_LX = nowNode.Data.Axis_LX;
            rm2.Axis_RX = nowNode.Data.Axis_RX;
            rm2.Axis_RY = nowNode.Data.Axis_RY;


            int iSize = Random.Range(minRoomSize + nowNode.Data.Axis_LY, nowNode.Data.Axis_RY- minRoomSize);

            rm1.Axis_RY = iSize;
            rm2.Axis_LY = iSize;
        }
        //DataPrint(rm1);
        //DataPrint(rm2);

        nowNode.Left = new BinaryTreeNode(rm1);
        nowNode.Right = new BinaryTreeNode(rm2);

        //최 하위 노드일때만 넣기
        if (!GenerationRoomNode(iDepth+1, nowNode.Left) && phaseLock == false)
        {
            RoomData data = nowNode.Data;
            data.RoomNo = RoomList.Count + 1;
            data.Axis_LX++;
            data.Axis_LY++;
            data.Axis_RX--;
            data.Axis_RY--;

            RoomList.Add(data);

            phaseLock = true;

            return true;
        }
         if(!GenerationRoomNode(iDepth + 1, nowNode.Right) && phaseLock == false)
        {
            RoomData data = nowNode.Data;
            data.RoomNo = RoomList.Count+1;
            data.Axis_LX++;
            data.Axis_LY++;
            data.Axis_RX--;
            data.Axis_RY--;

            RoomList.Add(data);

            phaseLock = true;
            return true;
        }

        phaseLock = false;
        return true;
    }

    //높은확률로 쓸일 없음
    private bool CheckIntersection(RoomData rmD1, RoomData rmD2)
    {
        //좌측 max -> 우측 min하고 비교시 max < min이면 충돌, 아니면 비충돌


        if (((Mathf.Max(rmD1.Axis_LX, rmD1.Axis_RX) <= Mathf.Min(rmD2.Axis_LX, rmD2.Axis_RX)) || (Mathf.Min(rmD1.Axis_LX, rmD1.Axis_RX) >= Mathf.Max(rmD2.Axis_LX, rmD2.Axis_RX))) &&
            ((Mathf.Max(rmD1.Axis_LY, rmD1.Axis_RY) <= Mathf.Min(rmD2.Axis_LY, rmD2.Axis_RY)) || (Mathf.Min(rmD1.Axis_LY, rmD1.Axis_RY) >= Mathf.Max(rmD2.Axis_LY, rmD2.Axis_RY))))
            return false;

        return true;
    }


    public HashSet<RoomData> GetRoomList()
    {
        return RoomList;
    }

    public static void DataPrint(RoomData data)
    {
        Debug.Log("LeftX : " + data.Axis_LX + " ,LeftY : " + data.Axis_LY + "\nRightX :" + data.Axis_RX + " ,RightY : " + data.Axis_RY);
    }
    public static void DataPrint(int index, RoomData data)
    {
        Debug.Log("index : " + index + "\nLeftX : " + data.Axis_LX + " ,LeftY : " + data.Axis_LY + "\nRightX :" + data.Axis_RX + " ,RightY : " + data.Axis_RY);
    }
}


//룸 속성 그대로 가져감
public class BinaryTreeNode
{
    public RoomData Data { get; set; }
    public BinaryTreeNode Left { get; set; }
    public BinaryTreeNode Right { get; set; }

    public BinaryTreeNode(RoomData data)
    {
        this.Data = data;
    }
}

public class BinaryTree
{
    public BinaryTreeNode Root { get; set; }
    public void PreOrderTraversal(BinaryTreeNode Node)      //전위 순회
    {
        if (Node == null)
            return;

        BSPRoomGenerator.DataPrint(Node.Data);
        PreOrderTraversal(Node.Left);
        PreOrderTraversal(Node.Right);
    }
}