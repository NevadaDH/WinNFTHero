using System.Diagnostics.Contracts;
using System;
using UnityEngine;

namespace Mao
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DG.Tweening;

    public class HexCell : MonoBehaviour
    {
        public int Index;

        public bool CanPutHero;

        public bool IsEnemy;

        public HexCoordinates coordinates;

        public Transform HeroObj
        {
            get
            {
                return transform.Find("hero");
            }
        }

        bool _isSelected = false;
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    if (value)
                    {
                        playFlash();
                    }
                    else
                    {
                        stopFlash();
                    }
                }
            }
        }

        GameObject meshObj;
        Tween flashTween;
        Material cellMat;

        public Material material;

        private int defaultLayer;
        float EffectRange1 = 0.0f;

        void Start()
        {
            showCell(true);
            MeshRenderer meshRenderer = this.GetComponentInChildren<MeshRenderer>();
            meshObj = meshRenderer.gameObject;
            cellMat = meshRenderer.sharedMaterial;
            meshObj.layer = CanPutHero ? 10 : 11;
            defaultLayer = meshObj.layer;
            showCell(false);
        }

        public void showCell(bool active)
        {
            GameObject foc = transform.Find("Focus").gameObject;
            foc.SetActive(active);
        }

        void playFlash(float duration = 1.5f)
        {
            if (flashTween != null && flashTween.IsPlaying())
            {
                return;
            }
            meshObj.layer = 4;
            EffectRange1 = 0f;
            flashTween = DOTween.To(() => EffectRange1, v => EffectRange1 = 1, 2.0f, duration).
            OnUpdate(() =>
            {
                material.SetFloat("_Distance", EffectRange1);
            })
            .OnComplete(() =>
            {
                EffectRange1 = 0.0f;
                material.SetFloat("_Distance", EffectRange1);
            });
            flashTween.SetLoops(-1);

        }

        void stopFlash()
        {
            flashTween.Kill(CanPutHero);
            flashTween = null;
            meshObj.layer = CanPutHero ? 10 : 11;

            cellMat.SetFloat("_Distance", 0);
        }

        public void AddHero(Transform hero)
        {
            // var curhero = transform.Find("hero");
            // if(curhero!=null && curhero!=hero){
            //    Destroy(curhero.gameObject);
            // }
            hero.name = "hero";
            hero.SetParent(transform);
            hero.DOLocalMove(Vector3.zero, 0.3f);
            if (IsEnemy)
            {
                hero.localEulerAngles = new Vector3(0, 180, 0);
            }
            //hero.localPosition = Vector3.zero;

        }

        public void RemoveHero()
        {
            var curhero = transform.Find("hero");
            if (curhero != null)
            {
                Destroy(curhero.gameObject);
            }
        }

        public void MoveHeroTo(HexCell cell)
        {
            var curhero = transform.Find("hero");
            if (curhero != null)
            {
                curhero.SetParent(cell.transform);
                curhero.DOLocalMove(Vector3.zero, 0.3f);
                //curhero.localPosition = Vector3.zero;
            }
        }

    }

}