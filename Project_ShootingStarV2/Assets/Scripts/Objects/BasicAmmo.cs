using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAmmo : MonoBehaviour
{
    [Range(0.5f, 5.0f)]
    [SerializeField] private float ammoLifeTime;
    private float nowTime;


    private void Update()
    {
        nowTime += Time.deltaTime;

        if (nowTime > ammoLifeTime)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        //대미지 주고

        Destroy(gameObject);
    }



}
