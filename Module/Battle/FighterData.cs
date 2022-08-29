using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mao
{
    public class FighterData : HeroBase
    {
    /// <summary>战斗ID</summary>
        public int fid;

        public float oldSx;
        public float oldSy;
        public float oldSz;

        public bool isSummon = false; //是否是召唤出来的
        public bool isDead = false;

        /// <summary>战斗位置</summary>
        public int site;

        /// <summary>是否是友方</summary>
        public bool we;

        /// <summary>血条位置</summary>
        public Transform lifePosition;

        /// <summary>身体</summary>
        public Transform body;

        /// <summary>当前生命值</summary>
        public float hp;
        
        /// <summary>当前生命值</summary>
        public float hpMax;

        /// <summary>初始位置</summary>
        public Vector3 position;

        /// <summary>框架</summary>
        public Transform frame;

        public Vector3 troopSitePosition;

        /// <summary>动画</summary>
        public Animator ani;

        public HeadBar headBar;

        public PowerBar powerBar;

        public Hero hero;

        public Battle_Damage battle_Damage;

        //英雄属性
        public Pro heroPro;

        /// <summary>攻击</summary>
        public SkillData attackData;

        /// <summary>拥有的buff类型</summary>
	    public List<BuffType> buffTypes = new List<BuffType>();

        /// <summary>技能</summary>
        public List<SkillData> skillDatas = new List<SkillData>();

        /// <summary>Buff数据</summary>
	    public List<BuffData> buffList = new List<BuffData>();

        ///<summary>加成属性</summary>
	    public Dictionary<AttributeType, float> attrAdd = new Dictionary<AttributeType, float>();

        ///<summary>初始化2</summary>
        /*
            site: 位置id
            rid: 模型id
            we:正方还是友方
        */
        public FighterData(int fid, int site, int rid, Pro proVal, bool we)
        {
            this.fid = fid;
            this.rid = rid;
            this.we = we;
            this.heroPro = proVal;
            this.site = site;
        }
    }
}
