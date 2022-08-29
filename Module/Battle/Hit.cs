using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Hit : MonoBehaviour
{
    private GameObject c;
    private Material material;
    private SkinnedMeshRenderer[] m;
    private MeshRenderer[] s;

    public Vector4 xForward = new Vector4(0, 0, 0, 0);
    private List<Material> q = new List<Material>();

    float hitDistance = 0.0f;

    float DissolveClip = 1.0f;
 
    void Start()
    {
        c = this.gameObject;
    }

    private List<Material> getMaterials(){
       if(m==null){
            m = c.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var i in m)
            {
                q.Add(i.material);
                //q.Add(i);
            }
        }
        if(s==null){
            s = c.GetComponentsInChildren<MeshRenderer>();
            foreach (var j in s)
            {
                q.Add(j.material);
            }
        }
        return q;
    }

    public void PlayHitEffect(float duration = 0.5f)
    {
       
        hitDistance = 0f;
    }
    public void PlayDeadEffect(float duration = 4.5f)
    {
        DissolveClip = 1.0f;
    }

    void Update()
    {

    }
}


