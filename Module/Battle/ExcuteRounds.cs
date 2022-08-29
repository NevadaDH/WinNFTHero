using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;

namespace Mao 
{
    using static VOManager;

    public class SkillBuffVO
    {
        //读表数据
        public BuffVO buffVO;
        //读表数据
        public List<Effect_buffVO> effect_BuffVOs;

        //服务器传递数据
        public Battle_Damage battle_Damage;
        public float buff_time; //buff持续时间
    }
    
    public class ExcuteRounds : MonoBehaviour
    {
        public int allRounds = 0;
        public int curRound = 0;
        public bool isPause
        {
            get
            {
                //return PauseManager.isPause;
                return false;
            }
        }

        public void startRounds()
        {
            StartCoroutine(ExcuteSkills());
        }

        public float getMoveTime(int i, Battle_Skill skill, List<Battle_Skill> newSkills)
        {
            float movetime = 0;
            if(skill.id == "1001" && i<newSkills.Count-1)  //如果当前技能是普通移动
            {
                for(int j=i+1; j<newSkills.Count; j++)
                {
                    Battle_Skill jskill = newSkills[j];
                    if(jskill.time != skill.time)
                    {
                        movetime = jskill.time - skill.time;
                        break;
                    }      
                }
            }
            return movetime;
        }

        public void processSkillPower(Battle_Skill skill)
        {
            FighterData fd = BattleManager.Instance.battleTroop.FindFighter(skill.target);
            if(fd != null)
            {
                int time = skill.time;
                int power = skill.power;
                fd.powerBar.flushPower(power, time);
            }
        }

        private List<SkillBuffVO> processBuff(Battle_Skill skill, float lastTime)
        {
             List<SkillBuffVO> buffs = null;
            if(skill.damages != null)
            {
                buffs = new List<SkillBuffVO>();
                foreach(Battle_Damage battle_Damage in skill.damages)
                {
                    if(battle_Damage.buff != null)
                    {
                        SkillBuffVO skillBuffVO = new SkillBuffVO();

                        //set bufftime
                        float bufftime = battle_Damage.buff.time;
                        bufftime -= lastTime;
                        skillBuffVO.buff_time = 1.0f*bufftime/1000;

                        //set battle_Damage
                        skillBuffVO.battle_Damage = battle_Damage;

                        //set buffVO
                        BuffVO buffVO = GetVO<BuffVO>(battle_Damage.buff.id);
                        skillBuffVO.buffVO = buffVO;
                        
                        //set effect_BuffVOs
                        if(buffVO != null && buffVO.hit != null && buffVO.hit.Count > 0)
                        {
                            List<Effect_buffVO> effect_BuffVOs = new List<Effect_buffVO>();
                            foreach(string hit in buffVO.hit)
                            {
                                Effect_buffVO vo = GetVO<Effect_buffVO>(hit); 
                                if(vo != null)
                                {
                                    effect_BuffVOs.Add(vo);       
                                } 
                            }
                            skillBuffVO.effect_BuffVOs = effect_BuffVOs;
                        }
                        buffs.Add(skillBuffVO);
                    }
                }
            }
            return buffs;
        }

        IEnumerator ExcuteSkills()
        {
            List<Battle_Skill> skills = BattleManager.Instance.battleTroop.battleRounds;
            List<Battle_Skill> newSkills = new List<Battle_Skill>();
            
            foreach (var skill in skills)
            {
                if(skill != null)
                   newSkills.Add(skill);
            }

            float lastTime=0;
            allRounds = newSkills.Count;
            curRound = 0;

            for(int i=0; i<newSkills.Count; i++)
            {
                Battle_Skill skill = newSkills[i];
                float time = skill.time;
                float timeF = time / 1000;    
                //float movetime = getMoveTime(i, skill, newSkills);
                yield return StartCoroutine(WaitTime((timeF-lastTime)));
                List<SkillBuffVO> skillBuffs = processBuff(skill, lastTime);
                StartCoroutine(ExcuteSkill(skill, curRound, skillBuffs));
                processSkillPower(skill);
                lastTime = timeF;   
                curRound += 1;
            }

            yield return StartCoroutine(WaitTime(3.0F));
            for(int i=0;  i<BattleManager.battleData.Herolist.Count; i++)
            {
                FighterData fd = BattleManager.battleData.Herolist[i];
                AniManager.Instance.DoAct(fd.ani, Act.Idle_battle); //切换为idle
            }
            
            BattleResultView view = Mao.ViewManager.Instance.ShowView<BattleResultView>();
            int result = BattleManager.Instance.battleTroop.battleResult;
            if(result == 1)
              view.showWin();
            else
              view.showLose();
        }

        IEnumerator ExcuteSkill(Battle_Skill skill, int curRound,  List<SkillBuffVO> buffs)
        {
            if(skill.id == "")
            {
                FighterData fd = BattleManager.Instance.battleTroop.FindFighter(skill.target);
                if(fd != null){
                    AniManager.Instance.DoAct(fd.ani, Act.Idle_battle); //切换为idle
                }
            }
            else
            {
                SkillChildVO skillChildVO = GetVO<SkillChildVO>(skill.id);
                SkillVO skillVO = GetVO<SkillVO>(skillChildVO.id);
                if(skillVO != null)
                {
                    sExcuteSkill(skill, skillVO, skillChildVO, buffs, curRound);
                }
            }
            yield return null;
        }
        
        void sExcuteSkill(Battle_Skill skill, SkillVO skillVO, SkillChildVO skillChildVO, List<SkillBuffVO> buffs, int round)
        {
            //获得技能释放者
            FighterData fd = BattleManager.Instance.battleTroop.FindFighter(skill.target);
            if(fd != null)
            {
                //获取技能攻击目标集群
                List<FighterData> tds = null;
                if(skill.damages != null)
                {
                    tds = new List<FighterData>();
                    for(int i=0; i<skill.damages.Count; i++)
                    {
                        FighterData td = BattleManager.Instance.battleTroop.FindFighter(skill.damages[i].target);
                        if(td != null)
                        {
                            td.battle_Damage = skill.damages[i];
                            tds.Add(td);
                        }
                    }
                }
                
                FighterData killtargetFD = null;
                if(skill.killtarget >= 0)
                {
                    killtargetFD = BattleManager.Instance.battleTroop.FindFighter(skill.killtarget); 
                }

                fd.site = skill.pos;
                var gm = fd.frame.gameObject;
                string skillname = skillVO.name;
                SkillFlow  flow = gm.AddComponent<SkillFlow>();
                //if(fd.we){
                    //UnityEngine.Debug.Log("skill.id" + skill.id+"  skill.time = "+skill.time);
                //}
                switch(skill.id)
                {
                    case "11": //"移动":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0;
                        
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.none;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = true;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime =  skillChildVO.time/1000.0F;
                        releasePart.moveType = SkillMoveType.Move; 
                        fd.attackData = new SkillData(functionPart, releasePart);
                    }
                    break;

                    case "31"://"瞬移":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0;
                        
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.none;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = true;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = skillChildVO.time/1000.0F;
                        releasePart.moveType = SkillMoveType.DashMove;

                        fd.attackData = new SkillData(functionPart, releasePart);
                    }
                    break;

                    case "41"://"冲锋":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0;
                        
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.none;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = true;//Act.leap;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = skillChildVO.time/1000.0F;
                        releasePart.moveType = SkillMoveType.RushMove;
                        fd.attackData = new SkillData(functionPart, releasePart);
                    }
                    break;

                    case "51"://"跳跃":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.none;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = true;//Act.leap;
                        releasePart.haveMoveAct = true;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = skillChildVO.time/1000.0F;
                        releasePart.moveType = SkillMoveType.JumpMove;
                        fd.attackData = new SkillData(functionPart, releasePart);
                    }
                    break;


                    case "21"://"普攻":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.3F;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.attack;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.System_NormalAttack;
                    }
                    break;

                    case "21011"://冰柱  
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;
                        
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.none;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;
                        releasePart.moveDelay = 0;
                        //releasePart.moveTime = skillChildVO.time/1000.0F;
                        //releasePart.moveType = SkillMoveType.DashMove;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Summon_IceBox;
                    }
                    break;
                    
                    case "61"://"远程普攻":
                    {
                        /*
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/bomb2"), false).transform;
                        functionPart.exploid = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Hit_C White"), false).transform;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;                        
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.System_LongAttack;*/

                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/longAttackBomb"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Explosion_B_Smoke+Text"), false).transform;
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 18;
                        //functionPart.linekind = LineKind.bigBomb;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Aoe_bomb;
                    }
                    break;

                    case "35011": //群体吸血
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Magical_Source-吸血"), false).transform;
                        functionPart.exploid = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Explosion_群体吸血"), false).transform;

                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Aoe_GroupBloodSucking;
                    }
                    break;

                    case "40011": //时光交错
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Magical_Source-时光交错"), false).transform;
                        functionPart.exploid = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Explosion_time"), false).transform;

                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;
                        //functionPart.linekind = LineKind.Aoe;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Aoe_TimeCross;
                    }
                    break;

                    case "15011": //禁魔球
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Magical_Source"), false).transform;
                        functionPart.exploid = null;

                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;
                        //functionPart.linekind = LineKind.magicBall;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.skillKind = SkillKind.Aoe_MagicBall;
                        fd.attackData.speciel = SkillSpecielType.AddDun;
                        fd.attackData.binder = "near";
                        fd.attackData.specialEffect =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_PickupDiamond2"), false).transform;
                    }
                    break;

                    case "7011"://"箭雨":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/bomb3"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_Big_Splash (No Collision)"), false).transform;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 80;
                        //functionPart.linekind = LineKind.arrowRain;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.skillKind = SkillKind.Aoe_rainArrows;
                    }
                    break;
                    
                    case "14011":  //加盾
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.cast;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Buff_PlusShield;
                        fd.attackData.speciel = SkillSpecielType.AddDun;
                        fd.attackData.specialEffect = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_PickupDiamond2"), false).transform;
                        fd.attackData.binder = "near";
                    }
                    break;

                    case "36011": //成长
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Magical_Source"), false).transform;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;
                        //functionPart.linekind = LineKind.Grow;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.cast;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.skillKind = SkillKind.Buff_Grow;
                    }
                    break;
                    
                    case "34011": //"火墙":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX4 Fire"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX3_Fire_Explosion"), false).transform;
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.skillKind = SkillKind.Aoe_FireWall;
                    }
                    break;
                    
                    case "20011": //"束缚光球":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX3_Hit_Light_C_Air"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX3_Fire_Explosion"), false).transform;

                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Projectile_ShineBall;
                        fd.attackData.targetRange = SkillTargetRange.Pass;
                    }
                    break;

                    case "33011": //"穿刺箭":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/jian"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX3_Fire_Explosion"), false).transform;

                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;
                        //functionPart.linekind = LineKind.passThrough;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Pass;
                        fd.attackData.skillKind = SkillKind.Projectile_Penetrating_arrow;
                    }
                    break;
                    
                    case "22011": //"击飞炸弹":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/bomb2"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX3_Fire_Explosion"), false).transform;

                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 30;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.One;
                        fd.attackData.speciel = SkillSpecielType.HitFly;
                        fd.attackData.skillKind = SkillKind.Projectile_back;
                    }
                    break;
 
                    case "11011": //削弱诅咒
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.exploid = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 65;
                        //functionPart.linekind = LineKind.arrowRain;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/Smoke"), false).transform;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.binder = "far";
                        fd.attackData.skillKind = SkillKind.Buff_WeakenCurse;
                    }
                    break;

                    case "10011": //"烟雾弹":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        functionPart.exploid = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 65;
                        //functionPart.linekind = LineKind.arrowRain;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/Smoke"), false).transform;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.skillKind = SkillKind.Control_SmokeBomb;
                        fd.attackData.binder = "far";
                    }
                    break;

                    case "5011"://"炸弹":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/Atomic bomb"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Explosion_Smoke_big"), false).transform;

                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 15;
                        //functionPart.linekind = LineKind.bigBomb;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.skillKind = SkillKind.Aoe_bomb;
                    }
                    break;

                    case "2011": //"砸地":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0.5F;
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.leap;
                        releasePart.effectNameB = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_Big_Splash (No Collision)"), false).transform;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.speciel = SkillSpecielType.HitFly;
                        fd.attackData.skillKind = SkillKind.Control_HitGround;
                    }
                    break;

                    case "37011": //吸收罩
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.cast;
                        releasePart.effectNameB =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_PickupDiamond2"), false).transform;;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;
                        
                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.binder = "near";
                        fd.attackData.skillKind = SkillKind.Buff_Hoodabsorption;
                    }
                    break;

                    case "8011": //能量护盾
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.cast;
                        releasePart.effectNameB =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_PickupDiamond2"), false).transform;;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.binder = "near";
                        fd.attackData.skillKind = SkillKind.Buff_EnergyShield;
                    }
                    break;

                    case "1012":
                    case "1011"://"光之信标":
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.cast;
                        releasePart.effectNameB =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX2_PickupDiamond2"), false).transform;;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.binder = "near";
                        fd.attackData.skillKind = SkillKind.Buff_Beaconoflight;
                    }
                    break;

                    case "3011"://"旋风斩": 
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Tornado"), false).transform;
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;
                        //functionPart.linekind = LineKind.bigBomb;
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.circel;
                        releasePart.effectNameF = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX3_Vortex_Ground"), false).transform;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.targetRange = SkillTargetRange.Group;
                        fd.attackData.binder = "near_none";
                        fd.attackData.skillKind = SkillKind.Aoe_XuanFenZhang;
                    }
                    break;

                    //Dash===瞬间
                    case "31011"://"旋转":  
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = null;
                        //functionPart.effectAll = "";
                        functionPart.combo = 0;
                        functionPart.flyTime = 0;
                        functionPart.deltaTime = null;
                        //functionPart.effectAllDelay = 0;
                        functionPart.endTime = 0;
                        
                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.attack;
                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = true;//Act.leap;
                        releasePart.haveMoveAct = true;
                        releasePart.moveDelay = 0;
                        releasePart.moveTime = skillChildVO.time/1000.0F;
                        releasePart.moveType = SkillMoveType.DashMove;
                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Dash_Flicker_Rotation;
                    }
                    break;

                    default:
                    {
                        SkillFunction functionPart = new SkillFunction();
                        functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/SkillPrefab/Atomic bomb"), false).transform;
                        functionPart.exploid =  CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Effect/CFX_Explosion_Smoke_big"), false).transform;
                        functionPart.combo = 0;
                        functionPart.flyTime = skillChildVO.time/1000.0F;
                        functionPart.deltaTime = null;
                        functionPart.endTime = 0.5F;
                        functionPart.flySpeed = 15;
                        //functionPart.linekind = LineKind.bigBomb;

                        SkillRelease releasePart = new SkillRelease();
                        releasePart.actB = Act.none;
                        releasePart.actF = Act.dart;

                        releasePart.effectNameB = null;
                        releasePart.effectNameBDelay = 0;
                        releasePart.effectNameF = null;
                        releasePart.effectNameFDelay = 0;

                        releasePart.moveAct = false;//Act.none;
                        releasePart.haveMoveAct = false;

                        releasePart.moveDelay = 0;
                        releasePart.moveTime = 0;
                        releasePart.moveType = SkillMoveType.None;

                        fd.attackData = new SkillData(functionPart, releasePart);
                        fd.attackData.skillKind = SkillKind.Aoe_bomb;
                    } 
                    break;
                }

                if(flow != null)
                {
                    bool islastR = false;
                    if(round == allRounds -1)
                      islastR = true;
                    flow.Init(fd, tds, killtargetFD, fd.attackData, skill, skillVO, skillChildVO, buffs, islastR, null);
                }
            }
        }

        /// <summary>等待时间</summary>
        IEnumerator WaitTime(float s)
        {
            float t = 0;
            while (t < s)
            {
                if (!isPause)
                {
                    t += Time.deltaTime;
                }
                yield return null;
            }
        }
    }
}











