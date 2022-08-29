using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Mao
{
    public class BuffLifeTime : MonoBehaviour
    {
        public delegate void triggerHandler(Vector3 pos);
        public triggerHandler playEnd;
        FighterData myData;
        BuffData _bd;
        bool loop = true;
        bool _needInterVal = false;
        float _t;
        float _interval; //间隔时间
        int _intervalReduceHp; //间隔持续掉血多少
        void Start()
        {}
        public void init(FighterData fd, BuffData bd, float time,  float interval = 0, int intervalReduceHp = 0, bool lp = true, triggerHandler playEnd = null)
        {
            myData = fd;
            _bd = bd;
            loop = lp;
            this._interval = interval;
            this._intervalReduceHp = intervalReduceHp;
            if(_interval > 0)
               _needInterVal = true;
               
            StartCoroutine(remove(fd, bd, time, playEnd));
        }
        
        IEnumerator remove(FighterData fd, BuffData bd, float time, triggerHandler playEnd)
        {
            yield return StartCoroutine(WaitTime(time));    
            removeEffect();
            Destroy(this);
        }

        void removeEffect()
        {
            myData.buffTypes.Remove(_bd.type);
            myData.buffList.Remove(_bd);

            if(playEnd != null)
                playEnd(transform.position);
            
            InstanceManager.Instance.DeCreate(_bd.effect);

            ParticleSystem ps = _bd.effect.GetComponent<ParticleSystem>();
            if(ps != null)
            {
               ParticleSystem.MainModule main = ps.main;
               main.loop = loop;
            }
        }

        void Update()
        {
            if(_needInterVal)
            {
                _t += Time.deltaTime;
                if(_t >= _interval)
                {
                    _t = 0;
                    removeBlood();
                }
            }
        }

        void removeBlood()
        {
            bool isDead = BattleManager.Instance.buffHurt(myData, _intervalReduceHp);
            if(isDead)
            {
                _needInterVal = false;
                BattleManager.Instance.Dead(myData);
                removeEffect();
                Destroy(this);
            }
        }

        /// <summary>等待时间</summary>
        IEnumerator WaitTime(float s)
        {
            float t = 0;
            while (t < s)
            {
                //if (!isPause)
                {
                    t += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
}
