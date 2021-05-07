using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffect : MonoBehaviour
{
    public GameObject muzzleEffect;

    // Update is called once per frame
    
    public void EffectPlay()
    {
        if (muzzleEffect)
        {            
            muzzleEffect.GetComponent<ParticleSystem>().Play();
        }
    }


}
