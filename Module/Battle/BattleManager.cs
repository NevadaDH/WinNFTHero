using UnityEngine;
using Cosmos;
using System.Collections;
using System.Collections.Generic;

namespace Mao
{
    //TODO 战斗管理,负责调度创建战斗,战斗模块对外唯一接口
    //public class BattleManager : MonoSingleton<BattleManager> {
    public class BattleManager : MonoSingleton<BattleManager>
    {
        //IFSM<GameObject> fsm;
        public static BattleData battleData;
        public static Transform battleSite;
        public bool isBattleStart = false;
        public BattleTroop  battleTroop; 

        /// <summary>buff控制</summary>
        public static BuffControl buffControl;

        public static int cols = 7;
        public static int rows = 8;

        //public void Init()
        //{
        //    IFSMManager fsmManager = CosmosEntry.FSMManager;
        //    var states = new List<FSMState<GameObject>>();
        //    fsm = fsmManager.CreateFSM("BattleFSM", this.gameObject, false,states);
        //}

        public void Init()
        {
            Transform troopTr = battleSite.Find("Troop");
            Transform heroTr = battleSite.Find("Hero");
            int rows = 8, cols = 7;
            spawnSites(rows, cols, troopTr, heroTr);
            syncHeroSites();
            
            battleData = new BattleData();
            battleTroop = battleSite.GetComponent<BattleTroop>();
            battleTroop.Init();
            isBattleStart = false;
            buffControl = new BuffControl();

            //直接开始战斗
            connectToFight();
        }

        public void connectToFight()
        {
            battleTroop.connectToFight();
            isBattleStart = true;
        }

        /*
        创建蓝色方块(己方英雄区域)
        */
        Transform createBlueCell(Transform parent, Vector3 pos, string name)
        {
            Transform prefab = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Cells/blueCell"), false).transform;
			Transform creator = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity) as Transform;
			creator.gameObject.SetActive(true);
			//creator.eulerAngles = rot;
			creator.name = name;
			creator.SetParent(parent);
            return creator;
        }

        /*
        创建红色方块(敌人区域)
        */
        Transform createRedCell(Transform parent, Vector3 pos, string name)
        {
            Transform prefab = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Cells/RedCell"), false).transform;
			Transform creator = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity) as Transform;
			creator.gameObject.SetActive(true);
			//creator.eulerAngles = rot;
			creator.name = name;
			creator.SetParent(parent);
            return creator;
        }

        /*
        创建英雄区域
        */
        Transform createHeroCell(Transform parent, Vector3 pos, string name)
        {
            Transform prefab = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Battle/Cells/heroCell"), false).transform;
			Transform creator = UnityEngine.Object.Instantiate(prefab, pos, Quaternion.identity) as Transform;
			creator.gameObject.SetActive(true);
			//creator.eulerAngles = rot;
			creator.name = name;
			creator.SetParent(parent);
            return creator;
        }

        /*
        创建所有方块
        */
        public void spawnSites(int rows, int cols, Transform parentTroop, Transform parentHero)
        {
            float startX = -10, startZ = -8;
            float stepX = 3.0f, stepZ = 2.5f;
            int index = 0;
            for(int row = 0; row < rows; row++)
            {
                float tx = 0.0f;
                int isBlue = 1;
                if(row % 2 == 0)
                    tx = 0;
                else 
                    tx =-1.5f;
                if(row >= rows/2)
                     isBlue = 0;

                for(int col = 0; col < cols; col++)
                {
                    Vector3 pos = new Vector3(startX + col * stepX + tx, 0, startZ + row * stepZ);
                    string name = index + "";
                    index += 1;
                    if(isBlue == 1)
                    {
                        Transform cell = createBlueCell(parentTroop, pos, name);
                    }
                    else
                    {
                        Transform cell = createRedCell(parentTroop, pos, name);
                    }
                    createHeroCell(parentHero, pos, name);
                }
            }
        }

        //42, 43, 44, 45,  46,  47,  48,
        //35, 36, 27, 38,  39,  40,  41,
        //28, 29, 30, 31,  32,  33,  34,
        //21, 22, 23, 24,  25,  26,  27,
        //14, 15, 16, 17,  18,  19,  20,
        //7,  8,  9,  10,  11,  12,  13,
        //0,  1,  2,  3,   4,   5,   6,

        /*同步英雄方块 和 troops方块*/
        public void syncHeroSites()
        {
            Transform troopTr = battleSite.Find("Troop");
            Transform heroTr = battleSite.Find("Hero");
            int cnt = troopTr.childCount;
            for(int i=0;  i<cnt; i++)
            {
                Transform tc = troopTr.GetChild(i);
                Transform hc = heroTr.GetChild(i);
                hc.position = tc.position;
            }
        }
        
        ///<summary>添加伤害</summary>
        public void AddHurt(FighterData fd, Battle_Damage bd)
        {
            if(bd == null)
              return;
            
            fd.heroPro.hp = bd.hp;
            //血条同步
            fd.headBar.addHurt((int)bd.hp);
        }
        
        public bool buffHurt(FighterData fd, int damage)
        {
            fd.heroPro.hp -= damage;
            //血条同步
            fd.headBar.addHurt((int)fd.heroPro.hp); 
            if(fd.heroPro.hp <= 0)
            {
                Dead(fd);
                return true;
            }
            return false; 
        }
        public void triggerShieldEnergy(Vector3 pos)
        {
            StartCoroutine(shieldEnergy_trigger(pos));
        }

        IEnumerator shieldEnergy_trigger(Vector3 pos)
        {
            Transform prefab = null;
             //rot = myData.mod.eulerAngles;
            Transform effect = InstanceManager.Instance.Create(prefab.name, prefab);
            effect.position = pos;
            //ParticleSystem sys = effect.GetComponent<ParticleSystem>();
            yield return new WaitForSeconds(3.0F);
            InstanceManager.Instance.DeCreate(effect);
        }


        ///<summary>具体死亡 </summary>
        public void Dead(FighterData fd)
        {
            if(fd.isDead)
              return;

            bool find = false;
            List<FighterData> lst = BattleManager.battleData.Herolist;
            for(int i=0; i<lst.Count; i++)
            {
                FighterData xd = lst[i];
                if(xd.fid == fd.fid)
                {
                    find = true;
                    break;
                }
            }
            if(!find)
            return;
            
            buffControl.ClearBuff(fd);
            fd.isDead = true; 
            print("dead fid:" + fd.fid);
            //DoEvent(EVENT_DEAD, fd);
            AniManager.Instance.DoAct(fd.ani, Act.dead);
            BattleManager.battleData.Herolist.Remove(fd);
            float deadTime = 2.0F;
            PlayDeadEffect(fd, deadTime);
            //fd.frame.position = new Vector3(fd.frame.position.x, 1.2F, fd.frame.position.z);
            //fd.mod.localEulerAngles = new Vector3(-90, fd.mod.eulerAngles.y, 0);
            fd.headBar.hide();
            fd.powerBar.hide();
            StartCoroutine(DeadDisappear(fd));
        }
        void PlayDeadEffect(FighterData td, float duration)
        {
            //Hit hit = td.mod.GetComponent<Hit>();
            Dissolve hit = td.mod.GetComponent<Dissolve>();
            if (hit != null)
            {
                //hit.PlayDeadEffect(duration);
                hit.PlayDeadEffect();
            }
        }

        /// <summary>死亡消失</summary>
        IEnumerator DeadDisappear(FighterData fd)
        {
            yield return new WaitForSeconds(3.0F);
            //AniManager.Instance.DoAct(fd.ani, Act.die, false);
            BattleManager.Instance.battleTroop.SiteDeCreate(fd.frame);
        }
    }
}


