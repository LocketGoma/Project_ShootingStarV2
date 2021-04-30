using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRay : MonoBehaviour
{
    [Header("basic")]
    public int raySticks = 100;
    public float searchReach = 5f;

    [Header("explosion")]
    public float explosePower = 30f;
    public float exploseSize = 5f;
    public ParticleSystem exploseParticle;

    Vector3[] vt;
    Ray[] ry;
    // Start is called before the first frame update
    void Start()
    {
        vt = new Vector3[raySticks];
        ry = new Ray[raySticks];
        MakeRays();

        Invoke("BulletClear", 3);
    }


    public void MakeRays() {
        for (int i = 0; i < raySticks; i++) {
            vt[i] = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            ry[i] = new Ray((vt[i] * MaxInVector3(gameObject.transform.localScale)) + gameObject.transform.position, vt[i]);
            //Debug.Log(vt[i]);
            //Debug.DrawRay(ry[i].Origin, ry[i].Direction * reach, new Color((vt[i].x+1)/2, (vt[i].y + 1) / 2, (vt[i].z + 1) / 2));
        }

    }
    public void Update() {


        for (int i = 0; i < raySticks; i++) {
            ry[i] = new Ray((vt[i] * MaxInVector3(gameObject.transform.localScale)) + gameObject.transform.position, vt[i]);
            Debug.DrawRay(ry[i].origin, ry[i].direction * searchReach, new Color((vt[i].x + 1) / 2, (vt[i].y + 1) / 2, (vt[i].z + 1) / 2));
            RaycastHit hit;
            if (Physics.Raycast(ry[i], out hit, searchReach) && !hit.collider.tag.Equals("Untagged")) {
                //Debug.Log("Ray hit : "+hit.collider.tag);
               // Debug.Log(gameObject.GetComponent<Rigidbody>().velocity);
               if (hit.collider.GetComponent<Rigidbody>() != null)
                    hit.collider.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Rigidbody>().velocity);
            }
        }
    }
    private float MaxInVector3(Vector3 vt) {
        float result;
        result = vt.x>vt.y?vt.x:vt.y;
        result = result>vt.z?result:vt.z;

        return result;
    }
    private void BulletClear() {
        Explose();        
    }
    private void Explose() {
        //파티클 자료 : https://docs.unity3d.com/kr/2018.4/Manual/PartSysExplosion.html

        //파티클 오브젝트가 생성이 안된 상태여서 "재생"이 되지 않았음.
        Instantiate(exploseParticle,transform.position,transform.rotation);

        if (exploseParticle != null)
            exploseParticle.Play();

        //if (exploseParticle.isEmitting) Debug.Log("Emit");
        //if (exploseParticle.isPlaying) Debug.Log("Play");
        //if (exploseParticle.isPaused) Debug.Log("Pause");
        //if (exploseParticle.isStopped) Debug.Log("Stop");        


        GetComponent<MeshRenderer>().material.color = Color.red;
        for (int i = 0; i < raySticks; i++) {
            //Debug.Log(ry[i].origin+">"+ry[i].direction);

            ry[i] = new Ray((vt[i] * MaxInVector3(gameObject.transform.localScale)) + gameObject.transform.position, vt[i]);
            Debug.DrawRay(ry[i].origin, ry[i].direction * exploseSize, new Color((vt[i].x + 1) / 2, (vt[i].y + 1) / 2, (vt[i].z + 1) / 2),3f);
            RaycastHit hit;
            if (Physics.Raycast(ry[i], out hit, exploseSize) && hit.collider.GetComponent<Rigidbody>()!= null) {                                
                hit.collider.GetComponent<Rigidbody>().AddForce(ry[i].direction * explosePower);
            }
        }
        Destroy(gameObject);
    }

}
