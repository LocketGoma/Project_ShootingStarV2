using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAmmo : MonoBehaviour
{
    [Range(0.5f, 5.0f)]
    [SerializeField] private float ammoLifeTime;
    public ParticleSystem exploseParticle;
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

        //이펙트 실행하고
        if (exploseParticle != null)
        {
            Instantiate(exploseParticle, transform.position, transform.rotation);
            exploseParticle.Play();
        }

        //삭제
        Destroy(gameObject);
    }



}
