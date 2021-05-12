using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//가지고 있는 오브젝트 이용해서 방을 채우는 역할.
public class RoomMaker : MonoBehaviour
{
    [Header("Base Room Data")]
    [SerializeField] private Room roomdata;
    [SerializeField] private RoomState roomState;

    [Header("Hierarchy")]
    [SerializeField] private GameObject forItemObject;
    [SerializeField] private GameObject forEnemyObject;
    [SerializeField] private GameObject forObstacleObject;

    [Header("Enemy")]
    [SerializeField] private GameObject [] enemy;
    [SerializeField] private int[] enemySpawnTable;
    private int maxPercent = 0;

    [Header("Items")]
    [SerializeField] private GameObject[] Items;

    private bool isSpawn = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < enemySpawnTable.Length; i++)
        {
            maxPercent += enemySpawnTable[i];
        }

        if (enemy.Length != enemySpawnTable.Length)
        {
            Debug.LogError("Error : Enemy Table size is not equal compare to Enemy Spawn Table!");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (roomState.nowMapType == RoomState.eMapType.NormalBattle && isSpawn == false && roomState.IsPlay == true)
        {
            forEnemyObject.SetActive(true);

            int respawnSize = Random.Range(1, 10);

            for (int i = 0; i < respawnSize; i++)
            {
                Vector3 pos = new Vector3(Random.Range(roomdata.RoomWidth*0.15f, roomdata.RoomWidth*0.85f), 2, Random.Range(roomdata.RoomHeight*0.15f, roomdata.RoomHeight*0.85f));
                
                Instantiate(enemy[RandomIndex()], new Vector3(roomdata.Axis_LX,0,roomdata.Axis_LY)+ pos, Quaternion.identity, forEnemyObject.transform);
                
            }

            isSpawn = true;
        }
        
        if (isSpawn == true && forEnemyObject.transform.childCount == 0)
        {
            roomState.RoomClear();
        }

     //if enemyobject.child == 0 -> clear   
    }

    private int RandomIndex()
    {
        int rand = Random.Range(0, maxPercent);
        int nowCount = 0;

        for (int i = 0; i < enemySpawnTable.Length; i++)
        {
            nowCount += enemySpawnTable[i];
            if (rand < nowCount)
            {
                return i;
            }
        }
        return 0;
    }


}
