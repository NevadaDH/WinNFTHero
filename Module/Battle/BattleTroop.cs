using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using foundation;
using Random = System.Random;

namespace Mao
{
    using static VOManager;
    public class BattleTroop : MonoBehaviour
    {
        public Transform battleSite;
        public static Transform RolePrefab;
        public static Transform MonsterPrefab;
        //private Dictionary<string, int> namesToSites = new Dictionary<string, int>();
        private BattleData battleData;
        public int battleResult;
        public List<Battle_Skill> battleRounds;
        
        //Start is called before the first frame update
        void Start()
        {}

        public void Init(){
            battleData = BattleManager.battleData;
        }

        public async void connectToFight()
        {
            List<Hero_put_req> lst = new List<Hero_put_req>();
            ChapterMoudle chapterMoudle = ClassUtils.Instance<ChapterMoudle>();
            //在发送开始战斗协议
            Chapter_Fight_req fight_req = chapterMoudle.fight_req;
            Chapter_Fight_atk chapter_Fight_atk = await MaoSocket.asyncToken(ChapterSocketConst.Chapter_Fight, fight_req) as Chapter_Fight_atk;
            battleResult = chapter_Fight_atk.result;
            prepareBattleData(chapter_Fight_atk.movie);
            PrepareBattle();
        }

        /*
        解析战报信息，英雄A数据，英雄B数据
        */
        public void prepareBattleData(Battle_Movie movie)
        {
            processBattleHeroes(movie.A.heros, 0, true);
            processBattleHeroes(movie.B.heros, 180, false);
            battleRounds = movie.skill;
        }

        /*在已有的英雄数据修复,增加英雄Pro属性*/
        public void processBattleHeroes(List<Battle_Hero> heroes, float angleY, bool we)
        {
            for (int i = 0; i < heroes.Count; i++)
            {
                Battle_Hero hero = heroes[i];
                processAddFighter(hero, angleY, we); 
            }
        }

        public void processAddFighter(Battle_Hero hero, float angleY, bool we, bool isSummon = false)
        {
            int site = hero.pos;  //英雄站位
            int rid = hero.id;   //英雄模型id
            int guid = hero.guid; //英雄唯一ID
            FighterData fd = AddFighter(guid, site, rid, hero.pro, we);
            fd.isSummon = isSummon;
            Transform siteTrans = battleSite.Find("Hero/" + fd.site);
            siteTrans.gameObject.SetActive(true);
            CreateMod(fd, siteTrans, angleY, we);
        }

        ///<summary>准备战斗</summary>
        public void PrepareBattle()
        {
            Transform node = battleSite.Find("Troop");
            node.gameObject.SetActive(false);
            StartCoroutine(BattleBegin());
        }

        ///<summary>战斗开始</summary>
        IEnumerator BattleBegin()
        {
            yield return new WaitForSeconds(1.0F);
            TurnBegin();
        }

        void TurnBegin()
        {
            ExcuteRounds ex = battleSite.GetComponent<ExcuteRounds>();
            ex.startRounds();
        }

        /// <summary>添加英雄</summary>
        FighterData AddFighter(int guid, int site, int rid, Pro pro, bool we)
        {
            FighterData fd = new FighterData(guid, site, rid, pro, we);
            fd.prefab = rid.ToString();
            battleData.Herolist.Add(fd);
            return fd;
        }

        FighterData RemoveFighter(int fid)
        {
            for (int i = 0; i < battleData.Herolist.Count; i++)
            {
                FighterData fd = battleData.Herolist[i];
                if (fd.fid == fid)
                {
                    battleData.Herolist.Remove(fd);
                    return fd;
                }
            }
            return null;
        }

        /// <summary>创建模型</summary>
        Transform CreateMod(FighterData fd, Transform site, float angleY, bool we)
        {
            fd.frame = site;
            fd.frame.gameObject.SetActive(true);
            Transform heroPrefab = null;
            GameObject avatarObj = null;
            if (we)
            {
                heroPrefab = RolePrefab;
                AvatarManager.Avatar avatar = new AvatarManager.Avatar();
                avatar.Top = "Avatar/Top/Top_01";
                avatar.TopHead = RandomHat();
                avatar.Bottom = "Avatar/Bottom/Bottom_01";
                avatar.Hand = "Avatar/Hand/Hand_01";
                avatar.Foot = "Avatar/Foot/Foot_01";
                //avatar.Mask = "Avatar/Mask/Mask_01";

                AvatarManager.Equipments equips = new AvatarManager.Equipments();
                //equips.Clothes = "Avatar/Cloths/Cloths01";
                avatarObj = AvatarManager.Instance.CreateAvatar(true, avatar, equips);
                avatarObj.name = "Hero";
            }
            else
            {
                heroPrefab = MonsterPrefab;
            }

            Transform temp = InstanceManager.Instance.Create(fd.prefab, fd.frame.position, fd.frame.eulerAngles, heroPrefab);
            temp.gameObject.SetActive(true);
            //node.eulerAngles = rot;
            temp.localEulerAngles = new Vector3(0, angleY, 0);

            if (avatarObj)
            {
                avatarObj.transform.parent = temp;
                avatarObj.transform.localPosition = Vector3.zero;
                avatarObj.transform.localScale = Vector3.one;
            }

            fd.mod = temp;
            fd.mod.gameObject.AddComponent<Dissolve>();
            fd.ani = temp.GetComponentInChildren<Animator>();
            fd.hero = temp.GetComponent<Hero>();
            fd.hero.fid = fd.fid;
            fd.hero.we = fd.we;

            fd.oldSx = fd.mod.localScale.x;
            fd.oldSy = fd.mod.localScale.y;
            fd.oldSz = fd.mod.localScale.z;

            fd.powerBar = fd.hero.sPoweBar.GetComponent<PowerBar>();

            fd.headBar = fd.hero.sHeadBar.GetComponent<HeadBar>();
            fd.headBar.setName(fd.fid + "");
            fd.headBar.setBar(fd.we);

            if (fd.heroPro != null)
            {
                fd.headBar.setBlood((int)fd.heroPro.hp, fd.we);
            }
            fd.mod.SetParent(fd.frame);

            AniManager.Instance.DoAct(fd.ani, Act.Idle_battle);
            
            return temp;
        }

        private string RandomHat()
        {
            List<string> acts = new List<string>();
            acts.Add("Avatar/TopHead/TopHead_02");
            acts.Add("Avatar/TopHead/TopHead_03");
            acts.Add("Avatar/TopHead/TopHead_05");
            Random random = new Random();
            List<string> newList = new List<string>();
            foreach (string item in acts)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            string va = newList[0];
            return va;
        }

        public FighterData FindFighter(int fid)
        {
            FighterData lastFighter = null;
            for (int i = 0; i < battleData.Herolist.Count; i++)
            {
                FighterData fd = battleData.Herolist[i];
                if (fd.fid == fid)
                {
                    lastFighter = fd;
                    break;
                }
            }
            return lastFighter;
        }

        //获取血量最低的fighter
        public FighterData FindLowestHpFighter(bool we)
        {
            FighterData findFd = null;
            float hp = 0;
            for(int i=0; i<battleData.Herolist.Count; i++)
            {
                FighterData fd = battleData.Herolist[i];
                if(fd.we == we)
                {
                    if(fd.heroPro.hp > hp)
                    {
                        findFd = fd;
                        hp = fd.heroPro.hp;
                    }
                }
            }
            return findFd;
        }



        /// <summary>清扫战场</summary>
        public void ClearBattle()
        {
            var node = battleSite.Find("Hero");
            var childcount = node.childCount;
            for (int i = 0; i < childcount; i++)
            {
                SiteDeCreate(node.GetChild(i));
            }
            battleData.Herolist.Clear();
        }

        /// <summary>站位清除</summary>
        public void SiteDeCreate(Transform temp)
        {
            for (int i = 0; i < temp.childCount; i++)
            {
                Transform child = temp.GetChild(i);
                if (child.name != "Shadow" && child.name != "lifePosition" && child.name != "Focus" && child.name != "Power")
                {
                    InstanceManager.Instance.DeCreate(child);
                    i--;
                }
            }
            temp.gameObject.SetActive(false);
        }
    }
}
