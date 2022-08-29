using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;
using Random = System.Random;
namespace Mao
{
    public class SkillFlow : MonoBehaviour
    {
        ///<summary>释放者</summary>
        protected FighterData myData;

        //当前服务器发来的战报数据
        protected Battle_Skill battle_Skill;

        //技能表数据
        protected SkillVO skillVO;

        private SkillChildVO skillChildVO;

        /// <summary>目标</summary>
	    protected List<FighterData> targetData;

        FighterData killtargetFD = null;

        ///<summary>技能数据</summary>
        protected SkillData skillData;

        //private Camera mainCamera;
        public delegate void skillFlowHandler();

        /// <summary>播放结束</summary>
        protected skillFlowHandler PlayEnd;

        protected SkillBuff skillValue;

        List<SkillBuffVO> skillBuffVOs;

        private bool isLastRound = false;

        /// <summary>是否暂停</summary>
        protected bool isPause
        {
            get
            {
                //return PauseManager.isPause;
                return false;
            }
        }

        /// <summary>初始化 带战场数据</summary>
        public void Init(FighterData mfd, List<FighterData> tfds, FighterData killFd, SkillData sd, Battle_Skill bs, SkillVO so,  SkillChildVO svo,  List<SkillBuffVO> buffs, bool islast, skillFlowHandler ed)
        {
            myData = mfd;
            PlayEnd = ed;
            skillData = sd;
            skillVO = so;
            skillChildVO = svo;
            targetData = tfds;
            battle_Skill = bs;
            isLastRound = islast;
            killtargetFD = killFd;   
            skillBuffVOs = buffs;

            //添加各种buff
            skillValue = new SkillBuff(myData, buffs);
        }

        public void Start()
        {
            StartCoroutine(DoFlow());
        }

        IEnumerator DoFlow()
        {
            yield return StartCoroutine(Release());
            yield return StartCoroutine(Function());
            yield return StartCoroutine(SummonFunction());
            yield return StartCoroutine(BuffFunction());
            yield return StartCoroutine(ControlFunction());
            yield return StartCoroutine(MeleeFunction());

            //显示释放者的buff和被攻击目标的buff
            ShowBuff();

            while (isPause){
                yield return null;
            }

            CheckDead(myData, targetData);

            if (PlayEnd != null)
                PlayEnd();

            if(isLastRound)
            {
                for(int i=0;  i<BattleManager.battleData.Herolist.Count; i++)
                {
                    FighterData fd = BattleManager.battleData.Herolist[i];
                    if(fd != null && fd.heroPro.hp > 0)
                       AniManager.Instance.DoAct(fd.ani, Act.Idle_battle); //切换为idle
                }
            }
            Destroy(this, 10);
        }

        void lookAt()
        {
            if(skillData.releasePart.moveAct)
            {
                var target = getTroopTarget(battle_Skill.pos);
                if(target != null)
                {
                    myData.mod.LookAt(target.position);
                    myData.mod.GetComponentInChildren<HeadBar>().repairRotation();
                }
            }
            else
            {
                FighterData td = getRandomTarget();
                if(td != null)
                {
                    var target = getTroopTarget(td.site);
                    if(target)
                    {
                        myData.mod.LookAt(target.position);
                        myData.mod.GetComponentInChildren<HeadBar>().repairRotation();
                    }
                }
            }
        }

        void setActF(SkillRelease ep)
        {
            if(ep.actF == Act.attack)
            {
                List<Act> acts = new List<Act>();
                acts.Add(Act.attack);
                acts.Add(Act.attack2);
                acts.Add(Act.attack3);
                Random random = new Random();
                List<Act> newList = new List<Act>();
                foreach (Act item in acts){
                   newList.Insert(random.Next(newList.Count + 1), item);
                }
                Act va = newList[0];
                ep.actF = va;
            }
            AniManager.Instance.DoAct(myData.ani, ep.actF);
        }

        ///<summary>释放阶段</summary>
        IEnumerator Release()
        {
            //设置朝向--如果是移动，lookAt 目标位置
            //如果是攻击动作, lookAt Targets
            lookAt();
            SkillRelease ep = skillData.releasePart;
            
            //前置--释放技能前置动作
            StartCoroutine(InstanceEffect(ep.effectNameF, myData, skillData.binder, ep.effectNameFDelay));

            //移动
            yield return StartCoroutine(Move());

            //后置--释放技能后置动作-随机一个攻击动作
            setActF(ep);

            //StartCoroutine(PlayAudio(skillData.audioPart.releaseAudio, skillData.audioPart.delayRelease));
            //var sawAnimState = myData.ani.GetCurrentAnimatorStateInfo(0);//读取当前动画事件的时间
            //yield return new WaitForSeconds(sawAnimState.length*0.45F);//动画执行完成后
            yield return StartCoroutine(WaitTime(0.5f));
            StartCoroutine(InstanceEffect(ep.effectNameB, myData, skillData.binder, ep.effectNameBDelay));
            //yield return StartCoroutine(WaitTime(ep.endTime));

            //技能动画释放完毕,切换会待机动作
            if(ep.actF != Act.none){
                AniManager.Instance.DoAct(myData.ani, Act.Idle_battle);
            }
        }

        ///<summary>移动</summary>
        IEnumerator Move()
        {
            if(skillData.releasePart.moveAct)
            {
                AniManager.Instance.DoAct(myData.ani, Act.move_2);
                Transform target = getTroopTarget(battle_Skill.pos);
                Vector3 ep = target.position;
                yield return StartCoroutine(MoveTo(myData.frame, myData.frame.position, ep, skillData.releasePart.moveTime));
                myData.site = battle_Skill.pos;
            }
        }
        
        //随机获得一个目标
        FighterData getRandomTarget()
        {
            if(killtargetFD != null)
               return killtargetFD;

            if(targetData != null && targetData.Count > 0)
            {
                if(targetData.Count == 1)
                  return targetData[0];

                SkillFunction fp = skillData.functionPart;
                List<FighterData> tds = new List<FighterData>();
                for(int i=0;i<targetData.Count;i++)
                tds.Add(targetData[i]);

                List<FighterData> newList = new List<FighterData>();
                Random random = new Random();
                foreach (FighterData item in tds){
                    newList.Insert(random.Next(newList.Count + 1), item);
                }
                FighterData td = tds[0];
                return td;
            }
            return null;
        }

        IEnumerator SummonFunction()
        {
            switch(skillData.skillKind)
            {
                case SkillKind.Summon_IceBox:
                {
                    Transform target = getTroopTarget(battle_Skill.pos);
                    Vector3 pos = target.position;
                    SkillFunction sp = skillData.functionPart;
                    StartCoroutine(summonIceBox("Battle/Effect/summon_icebox", pos));
                }
                break;

                case SkillKind.Summon_Monster:
                {
                    StartCoroutine(summonMonster());
                }
                break;
            }
            yield return null;
        }

        IEnumerator BuffFunction()
        {
            switch(skillData.skillKind)
            {
                case SkillKind.Buff_EnergyShield:
                {} //buffControl 处理
                break;

                case SkillKind.Buff_PlusShield:
                {}  //buffControl 处理
                break;

                //光之信标
                case SkillKind.Buff_Beaconoflight:{
                    //buffControl处理
                }
                break;

                //吸收罩 --buffControl处理
                case SkillKind.Buff_Hoodabsorption:
                {}
                break;

                //削弱诅咒 --buffControl处理
                case SkillKind.Buff_WeakenCurse: 
                break;

                case SkillKind.Buff_Grow:
                {
                    yield return StartCoroutine(Grow());

                    /*
                    SkillFunction sp = skillData.functionPart;
                    //获得血量最低的友军,放大
                    bool we = myData.we;
                    FighterData fd = BattleManager.Instance.battleTroop.FindLowestHpFighter(we);
                    if(fd != null)
                    {
                        float time = 5.0f;
                        StartCoroutine(Grow(fd, time));
                    }

                    //击退敌人几格
                    for(int i=0; i<targetData.Count; i++)
                    {
                        FighterData td = targetData[i];
                        float backtime = 0.5f;
                        int n = 2;
                        StartCoroutine(backTo(td, n, backtime));
                    }
                    yield return WaitTime(sp.flowTime);*/
                }
                break;

                //盾墙,举盾牌向前走3步
                case SkillKind.Buff_ShieldWall:
                {
                    /*
                    SkillFunction sp = skillData.functionPart;
                    float backtime = sp.flowTime;
                    int n = 3;
                    yield return StartCoroutine(backTo(myData, n, backtime));*/
                }
                break;
            }
            yield return null;
        }


        IEnumerator ControlFunction()
        {
            switch(skillData.skillKind)
            {
                case SkillKind.Control_SmokeBomb:
                yield return StartCoroutine(Aoe_bomb());
                break;

                case SkillKind.Control_HitGround:
                DirectlyEffect();
                break;

                case SkillKind.Control_Throw:
                {
                    /*
                    SkillFunction fp = skillData.functionPart;
                    int pos = -1;
                    Transform target = getTroopTarget(pos);
                    Vector3 ep = target.position;

                    FighterData td = null; 
                    td.ani.enabled = false;

                    float throwtime = 0;
                    yield return StartCoroutine(MoveTo(td.frame,td.frame.position, ep, throwtime));
                    td.site = pos;
                    td.ani.enabled = true;*/
                    DirectlyEffect();
                }
                break;
            }
            yield return null;
        }
        
        IEnumerator MeleeFunction()
        {
            switch(skillData.skillKind)
            {
                case SkillKind.Melee_DamageStrike:
                break;

                case SkillKind.Melee_BrokenNail:
                {
                    StartCoroutine(PassSkill());
                    yield return WaitTime(skillData.functionPart.flyTime);
                }
                break;

                case SkillKind.Melee_Invisible:
                break;

                case SkillKind.Melee_SoulConnect:
                break;

                case SkillKind.Melee_SuckBlood:
                break;

                case SkillKind.Melee_Weapon:
                break;
            }
            yield return null;
        }

        ///<summary>释放技能,执行具体技能动作，前提是攻击目标不为空</summary>
        IEnumerator Function()
        {
            if(targetData != null && targetData.Count > 0)
            {
                switch(skillData.skillKind)
                {
                    case SkillKind.System_NormalAttack:
                    {
                        DirectlyEffect();
                    }
                    break;

                    case SkillKind.System_LongAttack:
                    yield return StartCoroutine(ExcuteFlyMove());
                    break;

                    ////Aoe////
                    case SkillKind.Aoe_XuanFenZhang:{
                        DirectlyEffect();
                    } 
                    break;

                    case SkillKind.Aoe_Cone:
                        DirectlyEffect();
                    break;

                    case SkillKind.Aoe_TimbBomb:
                    yield return StartCoroutine(Aoe_timeBomb());
                    break;

                    case SkillKind.Aoe_bomb:
                    yield return StartCoroutine(Aoe_bomb());
                    break;

                    case SkillKind.Aoe_rainArrows:
                    yield return StartCoroutine(Aoe_ArrowsRain());
                    break;

                    case SkillKind.Aoe_MagicBall:
                    yield return StartCoroutine(ExcuteFlyMove());
                    break;

                    case SkillKind.Aoe_FireWall:
                    {
                        SkillFunction fp = skillData.functionPart;
                        FighterData td = getRandomTarget();
                        spawnFireWall(td);
                        yield return StartCoroutine(WaitTime(fp.flyTime));
                        for(int i=0; i<targetData.Count; i++)
                        {
                            FighterData xtd = targetData[i];
                            Effect(xtd, 0);
                        }
                    }
                    break;

                    case SkillKind.Aoe_GroupBloodSucking:
                    yield return StartCoroutine(ExcuteFlyMove()); 
                    break;

                    case SkillKind.Aoe_TimeCross:
                    yield return StartCoroutine(ExcuteFlyMove()); 
                    break;

                    case SkillKind.Aoe_Virus:
                    yield return StartCoroutine(ExcuteFlyMove()); 
                    break;

                    /////Projectile////////
                    case SkillKind.Projectile_ShineBall:
                    yield return StartCoroutine(ExcuteFlyMove());
                    break;

                    case SkillKind.Projectile_back:
                    yield return StartCoroutine(Aoe_bomb());
                    break;

                    case SkillKind.Projectile_Penetrating_arrow:
                    StartCoroutine(PassSkill());
                    yield return WaitTime(skillData.functionPart.flyTime);
                    break;

                    case SkillKind.Projectile_shijianzhao:
                    yield return StartCoroutine(Aoe_bomb());
                    break;

                    case SkillKind.Projectile_TargetSniper:
                    DirectlyEffect();
                    break;

                    //////////Dash//////
                    case SkillKind.Dash_Flicker_Rotation:
                    DirectlyEffect();
                    break;
                }
            }
        }

        //直线攻击--比如一个攻击特效，直接直线飞行岛目标位置
        //1:束缚光球(projectile) 2:禁魔球(aoe)
        IEnumerator ExcuteFlyMove()
        {
            FighterData td = getRandomTarget();
            SkillFunction fp = skillData.functionPart;
            if(fp.effectFly != null)
            {
                Vector3 startP = new Vector3(myData.frame.position.x, myData.frame.position.y+0.5F, myData.frame.position.z);
                Vector3 endP = new Vector3(td.frame.position.x, td.frame.position.y+0.5F, td.frame.position.z);
                yield return StartCoroutine(FlyMoveTo(skillData.functionPart.effectFly.name,
                                                        skillData.functionPart.effectFly,
                                                        startP, endP,skillData.functionPart.flyTime));   
                for(int i=0; i<targetData.Count; i++)
                {
                    FighterData xtd = targetData[i];
                    Effect(xtd, 0);
                }
            }
            else
            {
                //yield return StartCoroutine(WaitTime(fp.endTime));
                for (int i = 0; i < targetData.Count; i++)
                {
                    Effect(targetData[i], 0);
                }
            } 
        }

        void DirectlyEffect()
        {
            if(targetData != null && targetData.Count > 0)
            {
                //yield return StartCoroutine(WaitTime(fp.endTime));
                for(int i = 0; i < targetData.Count; i++)
                {
                    Effect(targetData[i], 0);
                }
            }
        }

        IEnumerator Aoe_timeBomb()
        {
            SkillFunction fp = skillData.functionPart;
            FighterData td = getRandomTarget();
            bool stun = true;
            float afterDurationExplosion = 1.0f;
            FlyAreaBomb(td.frame.position, stun, afterDurationExplosion);
            yield return StartCoroutine(WaitTime(fp.flyTime)); 
        }

        //区域炸弹
        IEnumerator Aoe_bomb()
        {
            SkillFunction fp = skillData.functionPart;
            FighterData td = getRandomTarget();
            bool stun = false;
            float afterDurationExplosion = 0.0f;
            FlyAreaBomb(td.frame.position, stun, afterDurationExplosion);
            yield return StartCoroutine(WaitTime(fp.flyTime));
        }

        IEnumerator Aoe_ArrowsRain()
        {
            SkillFunction fp = skillData.functionPart;
            FighterData td = getRandomTarget();
            Random random = new Random();
            float time = 0.0f;
            
            bool stun = false;
            float afterDurationExplosion = 0.0f;
            for(int k=0; k<4; k++)
            {
                for(int i=0; i<6; i++)
                {
                    sss_rain(td, stun, afterDurationExplosion);
                }
                float val = (float)random.NextDouble()/10;
                time += val;
                yield return WaitTime(val);
            }
            time= fp.flyTime - time;
            time = (time >= 0 ? time : 0);
            yield return StartCoroutine(WaitTime(time)); 
            for(int i=0; i<targetData.Count; i++)
            {
                FighterData xtd = targetData[i];
                Effect(xtd, 0);
            }
        }

        //穿透
        IEnumerator PassSkill()
        {
            SkillFunction fp = skillData.functionPart;
            for(int i=0; i<targetData.Count; i++)
            {
                StartCoroutine(passOne(targetData[i]));
            }
            yield return null;
        }

        IEnumerator passOne(FighterData td)
        {
            SkillFunction fp = skillData.functionPart;
            Transform effect = InstanceManager.Instance.Create(name, fp.effectFly);
            effect.position = myData.mod.position;
            Vector3 velocity = (td.frame.position-effect.position).normalized * 25;
            effect.LookAt(td.frame.position);

            float t = 0;
            while (t < fp.flyTime)
            {
                if (!isPause)
                {
                    t += Time.deltaTime;
                    effect.Translate(velocity*Time.deltaTime, Space.World);
                }
                yield return null;
            }
            InstanceManager.Instance.DeCreate(effect);
            Effect(td, 0, fp.hurtkind);
        }

        void sss_rain(FighterData td, bool stun, float afterDurationExplosion)
        {
            Random random = new Random();
            SkillFunction fp = skillData.functionPart;
            Vector3 pos = td.frame.position;
            pos.y += 10;
            pos.y += random.Next(3,6);
            pos.x += random.Next(-3,3);
            pos.z += random.Next(-3,3);

            string name = fp.effectFly.name;
            Transform effectFly = fp.effectFly;
            Transform tf = InstanceManager.Instance.Create(name, effectFly);
            Vector3 startP = new Vector3(pos.x, pos.y, pos.z);
            tf.position = startP;
            AreaBomb bomb = tf.GetComponent<AreaBomb>();
            bomb.targetPos = new Vector3(pos.x, 0, pos.z);
            bomb.speed = fp.flySpeed;
            bomb.excute(spawnRainBoomExplsiodEff, stun, afterDurationExplosion);
        }

        void spawnFireWall(FighterData td)
        {
            SkillFunction fp = skillData.functionPart;
            int site = td.site;
            int cols = 7;
            int row = site / cols;
            int c = site% cols;
            for(int i=0; i<cols; i++)
            {
                int newSite = row * cols + i;
                Transform target = getTroopTarget(newSite);
                if(target)
                {
                    string name = fp.effectFly.name;
                    Transform effectFly = fp.effectFly;
                    Transform tf = InstanceManager.Instance.Create(name, effectFly);
                    tf.position = target.position;
                    StartCoroutine(DelayRemoveEffect(tf, 1.0f)); 
                }
            }
        }

        void FlyAreaBomb(Vector3 pos, bool stun, float afterDurationExplosion)
        {
            SkillFunction fp = skillData.functionPart;
            string name = fp.effectFly.name;
            Transform effectFly = fp.effectFly;
            Transform tf = InstanceManager.Instance.Create(name, effectFly);
            Vector3 startP = new Vector3(myData.frame.position.x, myData.frame.position.y + 3.5F, myData.frame.position.z + 0.3F);
            tf.position = startP;
            AreaBomb bomb = tf.GetComponent<AreaBomb>();
            bomb.targetPos = pos;//td.frame.position;
            bomb.speed = fp.flySpeed;
            bomb.excute(spawnAreaBombExplsiod, stun, afterDurationExplosion);
        }

        ///IEnumerator Grow(FighterData fd, float time)
        //{
        //    SkillFunction fp = skillData.functionPart;
        //    fd.mod.localScale = new Vector3(4, 4, 4);
        //    yield return WaitTime(time);
        //    fd.mod.localScale = new Vector3(fd.oldSx, fd.oldSy, fd.oldSz);
        //}

        IEnumerator Grow()
        {
            SkillFunction fp = skillData.functionPart;
            for(int i=0; i<targetData.Count; i++)
            {
                FighterData xtd = targetData[i];
                xtd.mod.localScale = new Vector3(4, 4, 4);
            }

            yield return WaitTime(fp.flyTime);
            for(int i=0; i<targetData.Count; i++)
            {
                FighterData xtd = targetData[i];
                Effect(xtd, 0);
            }

            yield return WaitTime(5.0f-fp.flyTime);
            for(int i=0; i<targetData.Count; i++)
            {
                FighterData xtd = targetData[i];
                xtd.mod.localScale = new Vector3(xtd.oldSx, xtd.oldSy, xtd.oldSz);
            }
        }

        //后退几格
        IEnumerator backTo(FighterData fd, int n, float backtime)
        {
            SkillFunction fp = skillData.functionPart;
            int site = myData.site;
            int cols = BattleManager.cols;
            int rows = BattleManager.rows;
            int row = site / cols;
            int c = site % cols;
            bool we = fd.we;
            if(we)
            {
                row += n; //从外向里走3格
                if(row >= rows/2-1)
                    row = rows/2-1;
            }
            else
            {
                row -= n; //从里向外走3格
                if(row <= rows/2)
                    row = rows/2; 
            }
            int newSite = row*cols+c;
            Transform target = getTroopTarget(newSite);
            if(target)
            {
                AniManager.Instance.DoAct(myData.ani, Act.move_2);
                Vector3 ep = target.position;
                yield return StartCoroutine(MoveTo(myData.frame, myData.frame.position, ep, backtime));
                myData.site = newSite;
            }
        }

        IEnumerator DelayRemoveEffect(Transform tf, float duration)
        {
            yield return StartCoroutine(WaitTime(duration));    
            InstanceManager.Instance.DeCreate(tf);     
        }

        void spawnRainBoomExplsiodEff(Vector3 pos, bool stun, float afterDurationExplosion)
        {
            SkillFunction part = skillData.functionPart;
            Transform tf = InstanceManager.Instance.Create(part.exploid.name, part.exploid);
            Vector3 startP = pos;
            tf.position = startP;
            StartCoroutine(killExplosion(tf));
        }

        void spawnAreaBombExplsiod(Vector3 pos, bool needStun, float duration)
        {
            StartCoroutine(areaBombExplsiod(pos, needStun, duration));          
        }

        IEnumerator areaBombExplsiod(Vector3 pos, bool needStun, float duration)
        {
             //如果有炫晕
            if(needStun)
            {
                foreach(FighterData fd in targetData)
                {
                    //AniManager.Instance.DoAct(myData.ani, Act.stun);
                }
            }
            yield return StartCoroutine(WaitTime(duration));
            SkillFunction part = skillData.functionPart;
            Transform tf = InstanceManager.Instance.Create(part.exploid.name, part.exploid);
            Vector3 startP = pos;
            tf.position = startP;
            for(int i=0; i<targetData.Count; i++)
            {
               FighterData td = targetData[i];
               Effect(td, 0);
            }
            //yield return StartCoroutine(WaitTime(1.0F));
            StartCoroutine(killExplosion(tf));   
        }

        IEnumerator killExplosion(Transform tf)
        {
            yield return StartCoroutine(WaitTime(3.0F));
            InstanceManager.Instance.DeCreate(tf);
        }

        ///<summary>特效生成</summary>
        //攻击结束，产生攻击结束特效-类似爆炸，打击，碰撞特效，被攻击目标播放hurt动画
        //计算伤害
        //如果技能是特殊技能，被攻击目标还会被踢飞等效果
        void Effect(FighterData td, int combo, ShowHurtKind hurtkind = ShowHurtKind.None)
        {
            //print("Effect special:" + skillData.speciel + "--skillid--" + battle_Skill.id +  "--time:" + skillChildVO.time);
            Battle_Damage bd = td.battle_Damage;

            int skill_id = int.Parse(battle_Skill.id);
            bool isSkill = false;
            if(skill_id > 61)
                isSkill = true;

            FloatTextManager.Instance.Swpan(bd.damage + "", BattleManager.battleSite, td.frame.position, isSkill);

            //播放击中特效
            PlayHitEffect(myData.frame.forward, td);
            SkillEffect effectPart = skillData.effectPart;
            //skillValue.AddHurt(myData, fd, combo, index);
            BattleManager.Instance.AddHurt(td, bd);
            if(hurtkind == ShowHurtKind.NormalHurt){
                //AniManager.Instance.DoAct(td.ani, Act.knockback);                
            }

            //StartCoroutine(PlayAudio(skillData.audioPart.effectAudio, skillData.audioPart.delayEffect));
            if (effectPart.effectName != null && effectPart.effectName.Count != 0){
            }

            if (skillData.functionPart.combo == combo)
            {
                if(skillData.speciel == SkillSpecielType.HitFly){
                    StartCoroutine(HitFly(td));
                }
                else if(skillData.speciel == SkillSpecielType.AddDun){
                    StartCoroutine(InstanceEffect(skillData.specialEffect, td, skillData.binder));
                }
            }
        }

        Transform getTroopTarget(int pos)
        {
            var target = BattleManager.battleSite.Find("Troop/" + pos);
            return target;
        }

        /// <summary>移动前往</summary>
        IEnumerator MoveTo(Transform tf, Vector3 start, Vector3 end, float time)
        {
            float t = 0;
            while (t < 1)
            {
                if (!isPause)
                {
                    t += Time.deltaTime / time;
                    tf.position = Vector3.Lerp(start, end, t);
                }
                yield return null;
            }
            tf.position = end;
        }

        /// <summary>播放音效</summary>
        IEnumerator PlayAudio(string n, float delay)
        {
            if (n != "")
            {
                yield return new WaitForSeconds(delay);
                //AudioManager.instance.PlayAudio(n);
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

        void PlayHitEffect(Vector3 forward, FighterData td)
        {
            Hit hit = td.mod.GetComponent<Hit>();
            if (hit != null)
            {
                //var f = fd.frame.transform.forward;
                hit.xForward = new Vector4(forward.x, forward.y, forward.z, 0);
                hit.PlayHitEffect();
            }
        }
        
        /// <summary>判断是否死亡</summary>
        public void CheckDead(FighterData killer, List<FighterData> targetData)
        {
            if(targetData != null)
            {
                for (int i = 0; i < targetData.Count; i++)
                {
                    if (targetData[i] != null && targetData[i].heroPro.hp <= 0)
                    {
                        Dead(targetData[i]);
                    }
                }    
            }

            if (killer != null &&  killer.heroPro.hp <= 0)
            {
                Dead(killer);
            }
        }

        void Dead(FighterData td)
        {
            BattleManager.Instance.Dead(td);
        }

        //<summary>buff展示</summary>
        void ShowBuff()
        {
            BattleManager.buffControl.BuffShow(myData);
            if(targetData != null)
            {
                for (int i = 0; i < targetData.Count; i++){
                   BattleManager.buffControl.BuffShow(targetData[i]);
                }
            }
            //BattleManager.buffControl.TriggerBuff(myData);
            //if(skillValue.DoTriggle(SkillTriggerType.Poison)){
            //    StartCoroutine(AddGame(SkillTriggerType.Poison, skillValue.comboTime));
            //}
        }
        
        /// <summary>飞行连击</summary>
        IEnumerator FlyCombo(int combo, FighterData fd)
        {
            Vector3 startP = new Vector3(myData.frame.position.x, myData.frame.position.y+0.5F, myData.frame.position.z+0.3F);
            Vector3 endP = new Vector3(fd.frame.position.x, fd.frame.position.y+0.5F, fd.frame.position.z-0.1F);
            yield return StartCoroutine(FlyMoveTo(skillData.functionPart.effectFly.name,
                                                  skillData.functionPart.effectFly,
                                                  startP,endP,skillData.functionPart.flyTime));
            Effect(fd, combo);
        }

        /// <summary>飞行前往</summary>
        IEnumerator FlyMoveTo(string name, Transform prefab, Vector3 start, Vector3 end, float time)
        {
            Transform tf = InstanceManager.Instance.Create(name, prefab);
            if (tf == null) yield break;
            tf.position = start;
            tf.LookAt(end);
            float t = 0;
            while (t < 1)
            {
                if (!isPause)
                {
                    t += Time.deltaTime / time;
                    tf.position = Vector3.Lerp(start, end, t);
                }
                yield return null;
            }
            tf.position = end;
            InstanceManager.Instance.DeCreate(tf);
        }
        
        ///<summary>击飞</summary>
        IEnumerator HitFly(FighterData fd)
        {
            AniManager.Instance.DoAct(fd.ani, Act.knockback);                
            float time = 0.2F;
            fd.frame.DOMoveY(fd.frame.position.y + 2.5F*3, time).SetEase(Ease.OutElastic);
            yield return StartCoroutine(WaitTime(time));
            time = 0.1F;
            fd.frame.DOMoveY(fd.frame.position.y - 2.5F*3, time).SetEase(Ease.OutElastic);
            yield return StartCoroutine(WaitTime(time));
        }

        /// <summary>实例特效</summary>
        IEnumerator InstanceEffect(Transform tf, FighterData fd, string binder = "", float delay = 0)
        {
            if (tf == null)
                yield break;
            yield return new WaitForSeconds (delay);

            Transform bind = null;
            Vector3 pos = fd.frame.position;
            Vector3 rot = fd.frame.eulerAngles;

            if(binder.Contains("near")) //绑定到释放者
            {
                bind = myData.mod;
                pos = myData.mod.position;
                rot = myData.mod.eulerAngles;
                pos = new Vector3(pos.x, pos.y+1.5F, pos.z);
            }
            
            if(binder.Contains("near_none"))//绑定到释放着无之向
            {
                bind = myData.mod;
                pos = myData.mod.position;
                rot = tf.eulerAngles;
            }
            else if(binder.Contains("far")) //绑定到目标
            {
                FighterData td = targetData[0];
                bind = td.mod;
                pos = td.mod.position;
                rot = td.mod.eulerAngles;
                pos = new Vector3(pos.x, pos.y+1.5F, pos.z);
            }

            //rot = myData.mod.eulerAngles;
            Transform effect = InstanceManager.Instance.Create(tf.name, pos, rot, tf);
            if(bind != null)
            {
                Transform ss = bind.Find(effect.name);
                if(ss != null){
                   InstanceManager.Instance.DeCreate(ss);
                }
                effect.SetParent(bind);
            }

            //ParticleSystem sys = effect.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(5.0F);
            InstanceManager.Instance.DeCreate(effect);
        }

        //冰柱
        IEnumerator summonIceBox(string effectName, Vector3 pos)
        {
            Vector3 rot = new Vector3(0,0,0);
            Transform tf = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(effectName), false).transform;
            Transform effect = InstanceManager.Instance.Create(tf.name, pos, rot, tf);
            yield return new WaitForSeconds(2.0F);
            InstanceManager.Instance.DeCreate(effect);
        }

        IEnumerator summonMonster()
        {
            bool find = false;
            List<FighterData> list =  BattleManager.battleData.Herolist;
            for(int i=0; i<list.Count; i++)
            {
                if(!list[i].we && list[i].isSummon)
                {
                    find = true;
                    break;
                }
            }

            if(!find)
            {
                Battle_Hero hero = null;
                float angleY = 180;
                bool we = false;
                BattleManager.Instance.battleTroop.processAddFighter(hero, angleY, we, true);      
            }
            yield return null;
        }
    }   
}

