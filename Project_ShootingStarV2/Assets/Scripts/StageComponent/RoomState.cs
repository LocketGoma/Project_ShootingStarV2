using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//클리어 여부 등 저장
//룸 별 마스터 클래스
public class RoomState : MonoBehaviour
{
    [Header("Play Status")]
    [SerializeField] private bool isClear = false;      //방을 클리어했는가
    [SerializeField] private bool isPlay = false;       //유저가 들어와서 전투중인가
    public enum eMapType
    {
        SafeZone, NormalBattle, MinigameA, MinigameB, Boss, Item
    }
    public eMapType nowMapType = eMapType.SafeZone;

    [Header("Wall")]
    [SerializeField] private GameObject [] battleWall ; //닫겼다가 열렸다가 하는 벽들

    [Header("Obstacle")]
    [SerializeField] private GameObject [] obstacleObject ; //장애물들

    public bool IsPlay { get { return isPlay; } }



    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.parent.GetComponent<Room>().RoomNumber != 0)
        {
            //nowMapType = (eMapType)Random.Range(0, 6);
            // nowMapType = (eMapType)Random.Range(0, 2);
            nowMapType = eMapType.NormalBattle;
        }
        else
        {
            nowMapType = eMapType.SafeZone;
        }
        
        if (RoomControlManager.instance.roomMode == RoomControlManager.BossRoomMode.Fixed)
        {
            if (gameObject.transform.parent.GetComponent<Room>().RoomNumber == RoomControlManager.instance.roomNumber)
            {
                nowMapType = eMapType.Boss;
            }
        }

        RoomActive(false);
        ObstacleActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isClear == true)
        {
            RoomActive(false);
            
        }
        if (Input.GetKeyDown(KeyCode.K) && isPlay == true)
        {
            isClear = true;
            isPlay = false;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("StageTag:"+other.gameObject.tag);

        if (nowMapType == eMapType.SafeZone || isClear == true)
        {
            return;
        }

        if (other.gameObject.tag == "Player")
        {
            
            RoomActive(true);
            ObstacleActive(true);
            //other.transform.position = gameObject.transform.position;


            if (nowMapType == eMapType.Boss)
            {
                ObstacleActive(false);
            }

            isPlay = true;
        }


    }
    private void RoomActive(bool state)
    {
        battleWall[0].SetActive(state);
        battleWall[1].SetActive(state);
        battleWall[2].SetActive(state);
        battleWall[3].SetActive(state);
    }
    private void RoomForceActive()
    {
        battleWall[0].SetActive(true);
        battleWall[1].SetActive(true);
        battleWall[2].SetActive(true);
        battleWall[3].SetActive(true);
    }
    private void ObstacleActive(bool state)
    {
        foreach (GameObject obj in obstacleObject)
        {
            obj.SetActive(state);
        }
    }

    public void RoomClear()
    {
        isClear = true;
        isPlay = false;
    }

}
