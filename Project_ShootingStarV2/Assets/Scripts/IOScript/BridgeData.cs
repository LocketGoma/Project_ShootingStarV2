using System;
using UnityEngine;

[Serializable]
public class BridgeData
{
    public int BridgeNo;    //브릿지 번호
    public int NodeRoomANo; //연결된 방 번호 A
    public int NodeRoomBNo; //연결된 방 번호 B
    public int Axis_LX;
    public int Axis_LY;
    public int Axis_RX;
    public int Axis_RY;

    public int BridgeNumber { get { return BridgeNo; } set { BridgeNo = value; } }

    public BridgeData() { BridgeNo = -1; Axis_LX = 0; Axis_LY = 0; Axis_RX = 1; Axis_RY = 1; }
    public BridgeData(int bridgeNo, int lx, int ly, int rx, int ry) { BridgeNo = bridgeNo; Axis_LX = lx; Axis_LY = ly; Axis_RX = rx; Axis_RY = ry; }
    public BridgeData(int bridgeNo, int NodeANo, int NodeBNo, int lx, int ly, int rx, int ry) { BridgeNo = bridgeNo; NodeRoomANo = NodeANo; NodeRoomBNo = NodeBNo; Axis_LX = lx; Axis_LY = ly; Axis_RX = rx; Axis_RY = ry; }
    public BridgeData(int lx, int ly, int rx, int ry) { Axis_LX = lx; Axis_LY = ly; Axis_RX = rx; Axis_RY = ry; }


    public static bool operator >(BridgeData rm1, BridgeData rm2)
    {
        return (rm1.Axis_RX - rm1.Axis_LX > rm2.Axis_RX - rm2.Axis_LX) || (rm1.Axis_RY - rm1.Axis_LY > rm2.Axis_RY - rm2.Axis_LY);

    }
    public static bool operator <(BridgeData rm1, BridgeData rm2)
    {
        return !(rm1 > rm2);
    }
    public override bool Equals(object o)
    {
        BridgeData data = (BridgeData)o;

        return this == data;
    }
    public static bool operator ==(BridgeData rm1, BridgeData rm2)
    {
        return (rm1.Axis_LX == rm2.Axis_LX && rm1.Axis_LY == rm2.Axis_LY && rm1.Axis_RX == rm2.Axis_RX && rm1.Axis_RY == rm2.Axis_RY);

    }
    public static bool operator !=(BridgeData rm1, BridgeData rm2)
    {
        return !(rm1 == rm2);
    }
}
