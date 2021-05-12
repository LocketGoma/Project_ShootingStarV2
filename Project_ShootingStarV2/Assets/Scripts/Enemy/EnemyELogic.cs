using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyELogic : MonoBehaviour
{

    [Range (0,180)]
    [SerializeField] private float rotateSpeed;
    private float fRotate;
    // Start is called before the first frame update
    void Start()
    {
        fRotate = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(fRotate);
        fRotate += Time.deltaTime * 180.0f * rotateSpeed;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0.0f, fRotate, 0.0f);

    }
}
