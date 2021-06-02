using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomControlManager : MonoBehaviour
{
    public static RoomControlManager instance;

    public enum BossRoomMode
    {
        Auto, Fixed, None
    }
    [Header("Boss Room Mode")]
    public BossRoomMode roomMode;

    public int roomNumber;


}
