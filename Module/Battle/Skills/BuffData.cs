using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;

namespace Mao
{
    public class BuffData
    {
        /// <summary>buff释放者</summary>
        public FighterData maker;
        
        public int id;
        public int time; 
        /// <summary>buff类型</summary>
        public BuffType type;
        
        /// <summary>命中率</summary>
        //public float hit;
        
        /// <summary>属性</summary>
        //public AttributeType attr;
        
        /// <summary>数值</summary>
        public float value;
        
        /// <summary>是否是增益</summary>
        //public int target;
        
        /// <summary>触发类型</summary>
        //public SkillTriggerType trigger;
        
        /// <summary>目标自己？</summary>
        public bool isMe;
        
        /// <summary>标记层数</summary>
        //public int markLayer = 1;
        
        /// <summary>最大标记层</summary>
        //public int markLayerMax;
        
        /// <summary>标记替换Buff</summary>
        //public int markBuff;

        /// <summary>最大叠加层</summary>
        //public int addMax;
        
        /// <summary>叠加层</summary>
        //public int add;
        
        /// <summary>标记回合</summary>
        //public int markTurn;

        /// <summary>特效</summary>
        public Transform effect;

        /// <summary>是否展示</summary>
        public bool isShow;

        public SkillBuffVO skillBuffVO;


        /// <summary>生成副本</summary>
        public BuffData(SkillBuffVO vo)
        {
            this.skillBuffVO = vo;
            int id = vo.battle_Damage.buff.id;
            if(id == 101)
               type = BuffType.Stun;
            else if(id == 102)
               type = BuffType.Poison;
            else if(id == 201)
               type = BuffType.Shield;
            else if(id == 301)
               type = BuffType.InVincible;
            else if(id == 401)
               type = BuffType.ReBoundShield;
            else if(id == 501)
               type = BuffType.AddPhysical;
            else if(id == 601)
               type = BuffType.ReducePhysical;
            else if(id == 701)
               type = BuffType.DelayHurt;
            else if(id == 801)
               type = BuffType.GrowBig;
            else if(id == 901)
               type = BuffType.ReduceHit;
            else if(id == 1001)
               type = BuffType.ReducePhysicAndMagic;
            else if(id == 1101)
               type = BuffType.ManaCostIncreased;
            else if(id == 1201)
               type = BuffType.ReCoverEnergy;
            else if(id == 1301)
               type = BuffType.AddReduceSpeed;
            /*
            this.id = bd.id;
            this.buffVO = bd.buffVO;
            this.time = bd.time;
            this.type = bd.type;
            //this.hit = bd.hit;
            //this.level = bd.level;
            //this.attr = bd.attr;
            this.value = bd.value;
            //this.target = bd.target;
            //this.cd = bd.cd;
            //this.cdNow = bd.cd;
            this.isMe = bd.isMe;
            //this.markLayerMax = bd.markLayerMax;
            //this.markBuff = bd.markBuff;
            //this.markTurn = bd.markTurn;
            //this.icon = bd.icon;
            //this.add = bd.add;
            //this.addMax = bd.addMax;*/
        }

        public void showBuff(FighterData fd)
        {
           ushort id = skillBuffVO.battle_Damage.buff.id;
           float bufftime = skillBuffVO.buff_time;
           spawnBuffEffect(fd, bufftime);
           /*
           switch(type)
           {
              case BuffType.Stun:
              break;
              case BuffType.Poison:
              break;
              case BuffType.Shield:
              break;
              case BuffType.InVincible:
              break;
              case BuffType.ReBoundShield:
              break;
              case BuffType.AddPhysical:
              break;
              case BuffType.ReducePhysical:
              break;
              case BuffType.DelayHurt:
              break;
              case BuffType.GrowBig:
              break;
              case BuffType.ReduceHit:
              break;
              case BuffType.ReducePhysicAndMagic:
              break;
              case BuffType.ReCoverEnergy:
              break;
              case BuffType.AddReduceSpeed:
              break;
           }*/
        }

        void spawnBuffEffect(FighterData fd, float buffTime)
        {
            string path = "Battle/Effect/" + skillBuffVO.buffVO.effect;
            GameObject go = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(path), false);
            if(!go)
              return;
            Transform tf = go.transform;
            bool loop = true;
            ParticleSystem ps = tf.gameObject.GetComponent<ParticleSystem>();
            if(ps != null)
            {
               ParticleSystem.MainModule main = ps.main;
               loop = main.loop;
               main.loop = true;
            }

            Transform tf_effect = InstanceManager.Instance.Create(tf);
            if (tf_effect == null) return;
            effect = tf_effect;
            effect.SetParent(fd.frame);
            effect.localPosition = Vector3.zero;
            BuffLifeTime blf = effect.gameObject.AddComponent<BuffLifeTime>(); 

            float interval = 1.0f*skillBuffVO.buffVO.interval/1000;  //buff触发的间隔时间
            int intervalReduceHp = skillBuffVO.battle_Damage.damage; //间隔时间造成的伤害值
            blf.init(fd, this, buffTime, interval, intervalReduceHp, loop, null);
        }
        
        public void DelBuff()
        {
            InstanceManager.Instance.DeCreate(effect);
        }
    }
}