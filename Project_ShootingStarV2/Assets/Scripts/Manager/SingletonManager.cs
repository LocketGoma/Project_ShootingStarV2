using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonManager : MonoBehaviour
{
    public static SingletonManager instance;

    [SerializeField] RoomManager roomManager = null;


    private void Awake()
    {
        instance = this;

        RoomManager.instance = roomManager;
    }
}

