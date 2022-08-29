using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mao
{
    public class SkillDataBase
    {
        /// <summary>技能ID</summary>
        public int id;

        /// <summary>技能等级</summary>
        public int level;

        /// <summary>名称</summary>
        public string name = "";

        /// <summary>说明</summary>
        public string about = "";

        /// <summary>图标</summary>
        public string icon;
        
        /// <summary>CD</summary>
        public int cd;

        /// <summary>当前CD</summary>
        public int cdNow;

        /// <summary>特殊触发</summary>
       // public SkillSpecielType speciel;
       
        ///<summary>触发技能</summary>
        public int triggerSkill;

        /// <summary>重置CD</summary>
        public void CDRest(){
            cdNow = cd;
        }
        
        /// <summary>流动CD</summary>
        public void CDFlow(){
            cdNow--;
        }

        /// <summary>清空CD</summary>
        public void CDClear(){
            cdNow = 0;
        }
    }
}
