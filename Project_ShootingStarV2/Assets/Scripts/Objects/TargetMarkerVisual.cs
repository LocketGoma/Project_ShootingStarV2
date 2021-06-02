using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMarkerVisual : MonoBehaviour
{
    [SerializeField] private float angle;
    private float timer;

    private void Start()
    {
        StartCoroutine("BigAndSmall");
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(angle, transform.forward);
    }

    IEnumerator BigAndSmall()
    {
        while (true)
        {
            timer += Time.deltaTime * angle;
            transform.localScale = Vector3.one * (Mathf.Sin(timer)+5)/5;

            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
