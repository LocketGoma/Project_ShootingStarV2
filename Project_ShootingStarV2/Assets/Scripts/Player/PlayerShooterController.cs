using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooterController : MonoBehaviour
{
    enum eToolState
    {
        Idle,Handgun,Rifle,Throwable
    };
    [SerializeField] private eToolState toolState;
    [SerializeField] private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float fScroll = Input.GetAxisRaw("Mouse ScrollWheel");
    }
}
