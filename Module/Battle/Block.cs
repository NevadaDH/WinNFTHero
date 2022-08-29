using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Block : MonoBehaviour
{
    private GameObject c;
    public Tween myTween;

    public Material material;

    private int a;
    float EffectRange1 = 0.0f;



    void Start()
    {
        c = this.gameObject;
        a = c.layer;

        c.layer = 10;
    }

    public void PlayBlockEffect(float duration = 1.5f)
    {



        c.layer = 4;
        EffectRange1 = 0f;

        myTween = DOTween.To(() => EffectRange1, v => EffectRange1 = v, 2.0f, duration).
        OnUpdate(() =>
        {
            material.SetFloat("_Distance", EffectRange1);
        })
        .OnComplete(() =>
        {
            EffectRange1 = 0.0f;
            material.SetFloat("_Distance", EffectRange1);
        });

        myTween.SetLoops(-1);

    }

    public void StopBlockEffect(bool Stop)
    {
        myTween.Kill(Stop);
        c.layer = 10;
        material.SetFloat("_Distance", 0);
    }

    void Update()
    {

    }
}


