using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDropItem : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private int [] itemDropTable;

    [Range(0,25)]
    [SerializeField] private int DropItems;

    [Range(0, 25)]
    [SerializeField] private float ItemDropPower;

    private int maxPercent = 0;
    void Start()
    {
        for (int i = 0; i < itemDropTable.Length; i++)
        {
            maxPercent += itemDropTable[i];
        }
    }


    public void ItemDrop()
    {
        int DropCount = Random.Range(0, DropItems);

        for (int i = 0; i < DropCount; i++)
        {
            int itemIndex=0;
            int rand = Random.Range(0, maxPercent);
            int randPos = Random.Range(0, 360);
            //아 영 이상한데
            if (rand < itemDropTable[0])
            {
                itemIndex = 0;
            }
            else if (rand < itemDropTable[0]+itemDropTable[1])
            {
                itemIndex = 1;
            }
            else if (rand < itemDropTable[0] + itemDropTable[1] + itemDropTable[2])
            {
                itemIndex = 2;
            }

            Quaternion v3Rotation = Quaternion.Euler(0f, randPos, 0f);  // 회전각
            Vector3 v3Direction = Vector3.forward; // 회전시킬 벡터(테스트용으로 world forward 썼음)
            Vector3 v3RotatedDirection = v3Rotation * v3Direction;



            Instantiate(ItemManager.instance.GetItem(itemIndex),transform.position,transform.rotation).GetComponent<Rigidbody>().AddForce((Vector3.up * ItemDropPower + v3RotatedDirection).normalized*ItemDropPower, ForceMode.Impulse);
        }

    }
}
