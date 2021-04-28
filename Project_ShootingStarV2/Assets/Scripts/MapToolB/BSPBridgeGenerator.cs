using System.Collections.Generic;
using UnityEngine;

public class BSPBridgeGenerator : MonoBehaviour
{
    //수직, 수평, 십자
    enum eBridgeType
    {
        Vertical,Horizontral,Cross
    }


    private HashSet<BridgeData> BridgeList = new HashSet<BridgeData>();

    [SerializeField] private BinaryTree roomTree = null;
    [SerializeField] private BinaryTreeNode roomTreeNodeA = null;
    [SerializeField] private BinaryTreeNode roomTreeNodeB = null;
    [Range(1, 5)]
    public int BridgeWidth = 2;         //다리 폭

    public BinaryTree RoomBinaryTree { set { roomTree = value; } }


    public bool GenerateBridge()
    {
        if (roomTree == null)
            return false;

        //후위연산 해야하는데..


        return true;
    }


    BinaryTreeNode PostOrderTraversal(BinaryTreeNode Node)
    {
        if (Node == null)
        {
            return null;
        }
        roomTreeNodeA=PostOrderTraversal(Node.Left);
        if (roomTreeNodeA == null)
        {
            roomTreeNodeA = Node;
        }

        roomTreeNodeB=PostOrderTraversal(Node.Right);
        if (roomTreeNodeB == null)
        {
            roomTreeNodeB = Node;
        }

        if (roomTreeNodeA != roomTreeNodeB)
        {
            BridgeData bridgeData = new BridgeData();
            bridgeData.BridgeNo = BridgeList.Count + 1;
            bridgeData.NodeRoomANo = roomTreeNodeA.Data.RoomNo;
            bridgeData.NodeRoomBNo = roomTreeNodeB.Data.RoomNo;

            //1. 가로 연결인지 세로 연결인지 파악
            eBridgeType eType = eBridgeType.Cross;
            if (Mathf.Abs(Mathf.Abs(roomTreeNodeA.Data.Axis_LX- roomTreeNodeA.Data.Axis_RX)- Mathf.Abs(roomTreeNodeB.Data.Axis_LX - roomTreeNodeB.Data.Axis_RX)) > Mathf.Abs(Mathf.Abs(roomTreeNodeA.Data.Axis_LY - roomTreeNodeA.Data.Axis_RY) - Mathf.Abs(roomTreeNodeB.Data.Axis_LY - roomTreeNodeB.Data.Axis_RY)))
            {
                eType = eBridgeType.Horizontral;
            }
            else
            {
                eType = eBridgeType.Vertical;
            }


            //2. 각 방의 끝 라인이 어딘지 검사
            if (eType == eBridgeType.Horizontral)
            {






            }
            else if (eType == eBridgeType.Vertical)
            {










            }





            //3. 각 방을 이어줌


        }


        return Node;
    }

}
