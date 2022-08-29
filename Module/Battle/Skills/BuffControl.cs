using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mao
{
    public class BuffControl
    {
        /// <summary>石化</summary>
        public Material stone;

        ///<summary>添加Buff</summary>
        public void AddBuff(FighterData caster_fd, FighterData target_fd, BuffData bd)
        {
            //if (bd.type == BuffType.Poison || bd.type == BuffType.Shield) {
            //    ClearBuff(td, BuffType.Shield);
            //}

            BuffData newBd = bd; //new BuffData(bd);

            //for(int i = 0; i < td.buffList.Count; i++)
            //{
            //    if(td.buffList[i].id == newBd.id)
            //    {
            //        newBd.markLayer = td.buffList[i].markLayer + 1;
            //        newBd.add = Mathf.Clamp(td.buffList[i].add + 1, 0, newBd.addMax);
            //        DelBuff(td, td.buffList[i]);
            //        break;
            //    }
            //}
            target_fd.buffList.Add(newBd);
            target_fd.buffTypes.Add(newBd.type);
            newBd.maker = caster_fd;
        }
        
        /// <summary>刷新buff</summary>
        public void RefreshBuff(FighterData fd)
        {
            //if(fd.buffList.Count != 0)
            //{
            //    for(int i = 0; i < fd.buffList.Count; i++)
            //    {
            //        fd.buffList[i].CDFlow();
            //        if(fd.buffList[i].cdNow == 0)
            //        {
            //            DelBuff(fd, fd.buffList[i]);
            //            i--;
            //        }
            //    }
            //    BuffShow(fd);
            //}
        }

        /// <summary>删除Buff</summary>
        public void DelBuff(FighterData td, BuffData bd)
        {
            //if (bd.type == BuffType.Attr) 
            //{
            //    td.attrAdd [bd.attr] -= bd.value * (bd.add + 1);
            //}
            //else if (bd.type == BuffType.Shield) 
            //{
            //    td.attrAdd.Remove(AttributeType.shield);
            //}
            //ShowBuff(td, bd, false);
            bd.DelBuff();
            td.buffTypes.Remove(bd.type);
            td.buffList.Remove(bd);
        }

        /// <summary>触发buff</summary>
        public void TriggerBuff(FighterData fd)
        {
            //if(fd.buffList.Count != 0)
            //{
            //    for(int i = 0; i < fd.buffList.Count; i++)
            //   {
            //        if(fd.buffList[i].type == BuffType.Poison)
            //        {
            //            AniManager.Instance.BreakAct(fd.ani, Act.knockback);
                        //BattleManager.Instance.AddHurt(fd, fd.buffList[i].value);
            //        }
            //        else if(fd.buffList[i].type == BuffType.Burn)
            //        {
            //            AniManager.Instance.BreakAct(fd.ani, Act.knockback);
                        //BattleManager.Instance.AddHurt(fd, fd.buffList[i].value);
            //        }
            //    }
            //}
        }
        
        /// <summary>清除Buff 1增2减0清</summary>
        public void ClearBuff(FighterData fd)
        {
            for(int i = 0; i < fd.buffList.Count; i++)
            {
               DelBuff(fd, fd.buffList[i]);
               i--;
           }
            fd.buffList.Clear();
            //fd.attrAdd.Clear();
            fd.buffTypes.Clear();
        }

        /// <summary>清除Buff类型 </summary>
        //public void ClearBuff(FighterData fd, BuffType type)
        //{
        //    for (int i = 0; i < fd.buffList.Count; i++) 
        //    {
        //        if (fd.buffList [i].type == type) 
        //        {
        //            DelBuff (fd, fd.buffList [i]);
        //            i--;
        //        }
        //    }
        //}

        /// <summary>buff展示判断</summary>
        public void BuffShow(FighterData fd)
        {
            for(int i = 0; i < fd.buffList.Count; i++)
            {
                if(fd.buffList[i].effect == null && !fd.buffList[i].isShow)
                {
                    ShowBuff(fd, fd.buffList[i]);
                    fd.buffList[i].isShow = true;
                }
            }
            //fd.ani.enabled = !(fd.buffTypes.Contains(BuffType.Frozen) || fd.buffTypes.Contains(BuffType.Stone));
            //BattleManager.Instance.DoEvent(BattleManager.EVENT_BUFF, fd);
        }

        /// <summary>buff显示</summary>
        void ShowBuff(FighterData td, BuffData bd)
        {
            bd.showBuff(td);    
        }
    }
}
 
