using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Rendering.Universal​;

using UnityEngine.SceneManagement;

namespace Mao
{
    using static VOManager;

    public enum FormationMode
    {
        None,
        PVE,
        PVP_Rank_Attack, //排位进攻
        PVP_Rank_Defend,//排位防守
        PVP_Rank_RePlay, //排位回放
    }

    public class FormationState : FSMState<Transform>
    {
        public FormationMode Mode = FormationMode.PVE;

        public int ChapterId;
        public int ChapterLevel;

        //最大上阵英雄
        public int MaxHero = 6;

        public int MapWidth = 7;
        public int MapHeight = 8;

        public RankComData rankComData;
        public override void Action(IFSM<Transform> fsm)
        { }

        public override void OnEnter(IFSM<Transform> fsm)
        {
            Utility.Debug.LogInfo("Enter FormationState");
            ViewManager.Instance.PlayFade(EFadeEffects.Black, (playFadeout) =>
            {
                SceneManager.LoadScene("combat1");
                var asyncOp = SceneManager.LoadSceneAsync("Formation",LoadSceneMode.Additive);
                asyncOp.completed += (AsyncOperation op) =>
                {
                    Init();
                    playFadeout();
                };
            });
        }

        void Init()
        {
            FormationController crtl = FormationController.Instance;
            FormationView view = ViewManager.Instance.ShowView<FormationView>();
            view.AddEventListener(FormationView.Event_OnClickSave, () =>
            {
                if (Mode == FormationMode.PVE)
                {
                    SavePVE();
                }
                else if (Mode == FormationMode.PVP_Rank_Defend)
                {
                    SavePVPDefend();
                }
                else if (Mode == FormationMode.PVP_Rank_Attack)
                {
                    SavePVPAttack(rankComData.roleId);
                }
            });

            crtl.Init(Mode, MaxHero, MapWidth, MapHeight);

            if (Mode == FormationMode.PVE)
            {
                ChapterMoudle module = ClassUtils.Instance<ChapterMoudle>();
                List<Chapter_monsterChildVO> monsters = module.GetMonsterInLevel(ChapterId, ChapterLevel);
                for (int i = 0; i < monsters.Count; i++)
                {
                    Chapter_monsterChildVO vo = monsters[i];
                    MonsterVO monsterVO = GetVO<MonsterVO>(vo.monster + "");
                    GameObject monster = crtl.CreateMonster(vo.monster);
                    int pos = MapWidth * MapHeight - vo.pos - 1;
                    crtl.AddToCell(monster, pos);
                }
            }
            else if(Mode == FormationMode.PVP_Rank_Attack)
            {
                List<BattleHero> battleHeroes = rankComData.battleHeros;
                for(int i=0; i<battleHeroes.Count; i++)
                {
                    BattleHero hero = battleHeroes[i];
                    GameObject monster = crtl.CreatePVPBattleHero((int)hero.id);
                    int pos = MapWidth * MapHeight - (int)hero.pos - 1;
                    crtl.AddToCell(monster, pos);
                }
            }
        }

        async System.Threading.Tasks.Task<Chapter_Fight_atk> requestPVE()
        {
            ChapterMoudle chapterMoudle = ClassUtils.Instance<ChapterMoudle>();
            //在发送开始战斗协议
            Chapter_Fight_req fight_req = chapterMoudle.fight_req;
            Chapter_Fight_atk chapter_Fight_atk = await MaoSocket.asyncToken(ChapterSocketConst.Chapter_Fight, fight_req) as Chapter_Fight_atk;
            return chapter_Fight_atk;
        }

        public async void SavePVE()
        {
            FormationController crtl = FormationController.Instance;
            string ret = await crtl.SaveFormation(Mode);
            if (string.IsNullOrEmpty(ret))
            {
                Chapter_Fight_atk chapter_Fight_atk = await requestPVE();
                if (chapter_Fight_atk == null)
                {
                    ViewManager.Instance.ShowMessageBox($"请求战斗失败");
                }
                else
                {
                    //string fightJson = Utility.Json.ToJson(chapter_Fight_atk,true);
                    //Utility.Debug.LogInfo(fightJson);

                    var battleState = AppEntry.Instance.GameFSM.GetState<BattlePlayState>();
                    battleState.Movie = chapter_Fight_atk.movie;
                    //AppEntry.Instance.GameFSM.ChangeState<BattleState>();
                    AppEntry.Instance.GameFSM.ChangeState<BattlePlayState>();
                }
            }
            else
            {
                ToastManager.Instance.ShowToast(ret);
            }
        }

        //pvp排位布阵保存
        public async void SavePVPDefend()
        {
            FormationController crtl = FormationController.Instance;
            string ret = await crtl.SaveFormation(FormationMode.PVP_Rank_Defend);
            if (string.IsNullOrEmpty(ret))
            {
                MainCityState state = AppEntry.Instance.GameFSM.GetState<MainCityState>();
                state.OpenViewName = "ArenaView";
                AppEntry.Instance.GameFSM.ChangeState<MainCityState>();
            }
            else
            {
                ToastManager.Instance.ShowToast(ret);
            }
        }

        public async void SavePVPAttack(string targetRole_id)
        {
            FormationController crtl = FormationController.Instance;
            rank_battle_rsp ret = await crtl.SavePVPAttackFormation(FormationMode.PVP_Rank_Attack, targetRole_id);
            if (ret.result == 1)
            {
                var res = await MaoSocket.asyncToken(3122, ret.mv) as CROSS_BATTLE_MOVIE;
                var movie = res.movie.ReadObject() as Battle_Movie;
                var battleState = AppEntry.Instance.GameFSM.GetState<BattlePlayState>();
                battleState.Movie = movie;
                AppEntry.Instance.GameFSM.ChangeState<BattlePlayState>();
            }
            else
            {
                ToastManager.Instance.ShowToast(ret.reason);
            }
        }

        public override void OnExit(IFSM<Transform> fsm)
        {
            //FormationController.Instance.clearGuys();
            ViewManager.Instance.CloseView<FormationView>();
        }

        public override void OnInitialization(IFSM<Transform> fsm)
        {
        }

        public override void OnTermination(IFSM<Transform> fsm)
        {
        }
    }
}
