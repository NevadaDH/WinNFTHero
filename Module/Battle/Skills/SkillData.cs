
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;

namespace Mao
{
    public struct SkillRelease
    {
        public Transform effectNameF;
        public float effectNameFDelay;
        public Act actF;
        public SkillMoveType moveType;
        public float moveDelay;
        public float moveTime;

        public float endTime;
        public Transform effectNameB;
        public float effectNameBDelay;
        public Act actB;

        public bool moveAct;
    }

    public struct SkillFunction
    {
        /// <summary>连击次数</summary>
        public int combo;
        public List<float> deltaTime;
        public Transform effectFly; //飞行动画
        public Transform exploid;   //受击效果

        public float flySpeed;
        public float flyTime;
        public float flowTime;   //技能发生总的时间
        //public string effectAll;
        //public float effectAllDelay;
        public float endTime;
        public ShowHurtKind hurtkind;
    }
    /// <summary>功能阶段</summary>

    public struct SkillEffect
    {
        public List<string> effectName;
        public float endTime;
        public bool waitCamera;
    }
    /// <summary>结果阶段</summary>

    public struct SkillAudio
    {
        public string releaseAudio;
        public float delayRelease;
        public string functionAudio;
        public float delayFunction;
        public string effectAudio;
        public float delayEffect;
    }

    public class SkillData : SkillDataBase
    {
        ///<summary>buff列表</summary>
        public List<BuffData> buffList;


        /// <summary>目标类型</summary>
        public SkillTargetType targetType;

        /// <summary>目标范围</summary>
        public SkillTargetRange targetRange;

        public SkillKind  skillKind;
        /// <summary>buff释放者</summary>
        public FighterData maker;

        /// <summary>buff类型</summary>
        public BuffType type;
        
        /// <summary>命中率</summary>
        public float hit;
        
        /// <summary>属性</summary>
        public AttributeType attr;

        /// <summary>特殊触发</summary>
	    public SkillSpecielType speciel;

        public Transform specialEffect;

        /// <summary>数值</summary>
        public float value;
        
        /// <summary>是否是增益</summary>
        public int target;
        
        /// <summary>触发类型</summary>
        //public SkillTriggerType trigger;
        
        /// <summary>目标自己？</summary>
        public bool isMe;
        
        /// <summary>标记层数</summary>
        public int markLayer = 1;
        
        /// <summary>最大标记层</summary>
        public int markLayerMax;
        
        /// <summary>标记替换Buff</summary>
        public int markBuff;

        /// <summary>最大叠加层</summary>
        public int addMax;
        
        /// <summary>叠加层</summary>
        public int add;
        
        /// <summary>标记回合</summary>
        public int markTurn;

        /// <summary>特效</summary>
        public Transform effect;

        /// <summary>是否展示</summary>
        public bool isShow;

        public SkillRelease releasePart;

        /// <summary>功能阶段</summary>
        public SkillFunction functionPart;

        /// <summary>结果阶段</summary>
        public SkillEffect effectPart;

        /// <summary>相机路径</summary>
        public SkillAudio audioPart;


        public string binder = "";
        public bool haveNextSkill = false;

        public SkillData(string strRelease, string strFunciton, string strAudio = "")
        {
            InitData();

            if(strRelease != ""){
                ParseRelease(strRelease);
            }

            if(strFunciton != ""){
                ParseFunction(strFunciton);
            }

            if(strAudio != ""){
                ParseAudio(strAudio);
            }
        }

        /// <summary>初始战斗数据</summary>
        private void InitData()
        {
            releasePart.effectNameF = null;
            releasePart.effectNameB = null;

            functionPart.effectFly = null;
            functionPart.exploid = null;
            functionPart.hurtkind = ShowHurtKind.None;
            functionPart.combo = 0;

            audioPart.releaseAudio = "";
            audioPart.effectAudio = "";
            audioPart.functionAudio = "";
            return;
        }

        public void ParseRelease(string str)
        {
            //string ss_move = "effectNameF,effectNameDelay|actF|moveType|moveDelay|moveTime|moveAct|effectNameB,effectNameBDelay|actB|endTime";
            //"前置特效名称,特殊生存的延迟时间|前置动作|移动类型|移动前等待时间|移动时间|是否有移动动作|后置特效名称,特效延迟时间|后置动作|结束等待时间
            char[] split_one= {','};
            string[] temp = str.Split('|');
            int n = -1;
            n++;
            string[] tempE = temp [n].Split (split_one);
            string assertName = tempE[0];
            releasePart.effectNameF = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(assertName), false).transform;
            if(tempE.Length > 1)
                releasePart.effectNameFDelay = float.Parse(tempE[1]);
            n++;releasePart.actF = (Act)byte.Parse(temp[n]);
            n++;releasePart.moveType = (SkillMoveType)byte.Parse(temp[n]);
            if(releasePart.moveType != SkillMoveType.None)
            {
                n++;releasePart.moveDelay = float.Parse(temp[n]);
                n++;releasePart.moveTime = float.Parse(temp[n]);
                n++;releasePart.moveAct = (int.Parse(temp[n]) == 1);
                n++;
                string[] tempD = temp [n].Split (split_one);
                assertName = tempD[0];
                releasePart.effectNameB = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(assertName), false).transform;
                if(tempD.Length > 1)
                    releasePart.effectNameBDelay = float.Parse(tempD[1]);
                n++;releasePart.actB = (Act)byte.Parse(temp[n]);
            }
            n++;releasePart.endTime = float.Parse(temp[n]);
        }

        /// <summary>解析功能阶段</summary>
        public void ParseFunction(string str)
        {
            //string ss_function = "combo|effectFly|flyTime|exploid|flySpeed|hurtkind";
            //combo|攻击特效或武器|技能时间|攻击特效结束的爆炸特效|攻击特效的飞行速度|被攻击目标的伤害播放动画，没有就写0
            char[] split_one= {','};
            int n = -1;
            string[] temp = str.Split('|');
            n++;functionPart.combo = int.Parse(temp[n]);
            n++;string assetName = temp[n];
            functionPart.effectFly = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(assetName), false).transform; 
            //if(functionPart.effectFly != null){}
            n++;functionPart.flyTime = float.Parse(temp[n]);
            n++;functionPart.flowTime = float.Parse(temp[n]);
            n++; assetName = temp[n];
            functionPart.exploid = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(assetName), false).transform; 
            n++;functionPart.flySpeed = float.Parse(temp[n]);
            n++;functionPart.hurtkind = (ShowHurtKind)byte.Parse(temp[n]);

            //n++;
            //string[] tempX = temp[n].Split (split_one);
            //functionPart.effectAll = tempX[0];
            //if (tempX.Length > 1)
            //    functionPart.effectAllDelay = float.Parse (tempX [1]);
            //n++;functionPart.endTime = float.Parse(temp[n]);
        }

        /// <summary>解析音频</summary>
        public void ParseAudio(string str)
        {
            //string ss = "技能动作准备释放的音效|延迟时间|技能特效发出去的音效｜延迟时间｜技能攻击到目标位置的音效|延迟时间";
            if(str != "")
            {
                int n = -1;
                string[] temp = str.Split('|');
                n++; audioPart.releaseAudio = temp[n];
                n++; audioPart.delayRelease = float.Parse(temp[n]);
                n++; audioPart.functionAudio = temp[n];
                n++; audioPart.delayFunction = float.Parse(temp[n]);
                n++; audioPart.effectAudio = temp[n];
                n++; audioPart.delayEffect = float.Parse(temp[n]);
            }
        }
    }
}*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;

namespace Mao
{
    public struct SkillRelease
    {
        public Transform effectNameF;
        public float effectNameFDelay;
        public Act actF;
        public SkillMoveType moveType;
        public bool haveMoveAct;
        public float moveDelay;
        public float moveTime;

        public float endTime;
        public Transform effectNameB;
        public float effectNameBDelay;
        public Act actB;

        public bool moveAct;
    }

    public struct SkillFunction
    {
        /// <summary>连击次数</summary>
        public int combo;
        public List<float> deltaTime;
        public Transform effectFly; //飞行动画
        public Transform exploid;   //受击效果
        
        public float flySpeed;
        public float flyTime;
        //public string effectAll;
        //public float effectAllDelay;
        public float endTime;
        public ShowHurtKind hurtkind;
    }
    /// <summary>功能阶段</summary>

    public struct SkillEffect
    {
        public List<string> effectName;
        public float endTime;
        public bool waitCamera;
    }
    /// <summary>结果阶段</summary>

    public struct SkillAudio
    {
        public string releaseAudio;
        public float delayRelease;
        public string functionAudio;
        public float delayFunction;
        public string effectAudio;
        public float delayEffect;
    }

    public class SkillData : SkillDataBase
    {
        ///<summary>buff列表</summary>
        public List<BuffData> buffList;


        /// <summary>目标类型</summary>
        public SkillTargetType targetType;

        /// <summary>目标范围</summary>
        public SkillTargetRange targetRange;

        public SkillKind  skillKind;
        /// <summary>buff释放者</summary>
        public FighterData maker;

        /// <summary>buff类型</summary>
        public BuffType type;
        
        /// <summary>命中率</summary>
        public float hit;
        
        /// <summary>属性</summary>
        public AttributeType attr;

        /// <summary>特殊触发</summary>
	    public SkillSpecielType speciel;

        public Transform specialEffect;

        /// <summary>数值</summary>
        public float value;
        
        /// <summary>是否是增益</summary>
        public int target;
        
        /// <summary>触发类型</summary>
        //public SkillTriggerType trigger;
        
        /// <summary>目标自己？</summary>
        public bool isMe;
        
        /// <summary>标记层数</summary>
        public int markLayer = 1;
        
        /// <summary>最大标记层</summary>
        public int markLayerMax;
        
        /// <summary>标记替换Buff</summary>
        public int markBuff;

        /// <summary>最大叠加层</summary>
        public int addMax;
        
        /// <summary>叠加层</summary>
        public int add;
        
        /// <summary>标记回合</summary>
        public int markTurn;

        /// <summary>特效</summary>
        public Transform effect;

        /// <summary>是否展示</summary>
        public bool isShow;

        public SkillRelease releasePart;

        /// <summary>功能阶段</summary>
        public SkillFunction functionPart;

        /// <summary>结果阶段</summary>
        public SkillEffect effectPart;

        /// <summary>相机路径</summary>
        public SkillAudio audioPart;


        public string binder = "";
        public bool haveNextSkill = false;

        public SkillData(SkillFunction function, SkillRelease release)
        {
            InitData();
            ParseFunction(function);
            ParseRelase(release);
        }

        ///<summary>初始战斗数据</summary>
        private void InitData()
        {
            releasePart.effectNameF = null;
            releasePart.effectNameB = null;

            functionPart.effectFly = null;
            functionPart.combo = 0;
            functionPart.flyTime = 0;
            functionPart.deltaTime = null;
            functionPart.endTime = 0;
            
            audioPart.releaseAudio = "";
            audioPart.effectAudio = "";
            audioPart.functionAudio = "";
            return;
        }

        
        public void ParseRelase(SkillRelease release)
        {
            releasePart.effectNameF = release.effectNameF;
            releasePart.effectNameFDelay = release.effectNameFDelay;
            releasePart.effectNameB = release.effectNameB;
            releasePart.effectNameBDelay = release.effectNameBDelay;
            releasePart.actF = release.actF;
            releasePart.moveType = release.moveType;
            if(releasePart.moveType != SkillMoveType.None)
            {
                releasePart.moveDelay = release.moveDelay;
                releasePart.moveTime = release.moveTime; 
                releasePart.moveAct = release.moveAct;
                releasePart.haveMoveAct = release.haveMoveAct;
                releasePart.effectNameB = release.effectNameB;
                releasePart.effectNameBDelay = release.effectNameBDelay;
                releasePart.actB = release.actB;
            }
            releasePart.endTime = release.endTime;
        }

                
        public void ParseFunction(SkillFunction func)
        {
            functionPart.combo = func.combo;
            functionPart.hurtkind = func.hurtkind;
            
            if(functionPart.combo != 0)
            {
                functionPart.deltaTime = new List<float>();
                if(func.deltaTime != null)
                {
                    for(int i = 0; i < func.deltaTime.Count; i++)
                    {
                        functionPart.deltaTime.Add(func.deltaTime[i]);
                    }
                }
            }

            functionPart.exploid = func.exploid;
            functionPart.effectFly = func.effectFly;
            if(functionPart.effectFly != null)
            {
                functionPart.flyTime = func.flyTime;
            }
            functionPart.endTime = func.endTime; 
            functionPart.flySpeed = func.flySpeed;
        }
    }
}

