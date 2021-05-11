using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//아이템 리스트 보관용 매니저
public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private GameObject[] items;
    private int itemListSize;

    public int ItemListSize { get { return itemListSize; } }

    // Start is called before the first frame update
    void Start()
    {
        itemListSize = items.Length;
    }

    public GameObject GetItem(int index)
    {
        if (index > itemListSize || index < 0)
        {
            return null;
        }
        return items[index];
    }

}
