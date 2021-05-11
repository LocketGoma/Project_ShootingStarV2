using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static ControlManager instance;
    public enum InputMode
    {
        PC, Mobile
    }

    public InputMode controlMode;

    [SerializeField] private GameObject joystickGroup;


    // Start is called before the first frame update
    void Start()
    {
        joystickGroup.SetActive(controlMode == InputMode.Mobile);        
    }


}
