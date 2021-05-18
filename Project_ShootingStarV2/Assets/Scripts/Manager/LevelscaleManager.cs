using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelscaleManager : MonoBehaviour
{
    public static LevelscaleManager instance;

    [SerializeField] private int scaleLevel;
    [SerializeField] private int nowLevel;

    public int ScaleLevel { get { return scaleLevel; } set { scaleLevel = value; } }
    public int NowLevel { get { return nowLevel; } set { nowLevel = value; } }

    //FileIO 필수
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
