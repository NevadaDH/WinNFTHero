using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mao
{
    public class BattlePlayState : FSMState<Transform>
    {
        public Battle_Movie Movie;

        public override void Action(IFSM<Transform> fsm)
        {
        }
        public override void OnEnter(IFSM<Transform> fsm)
        {
            Utility.Debug.LogInfo("Enter BattlePlayState");
            ViewManager.Instance.PlayFade(EFadeEffects.Black, (playFadeout) =>
            {
                //布阵阶段跳转过来的情况可以不用加载战斗场景直接卸载
                if(SceneManager.GetSceneByName("Formation").IsValid()){
                    SceneManager.UnloadSceneAsync("Formation");
                }else{
                    SceneManager.LoadScene("combat1");
                }

                var asyncOp = SceneManager.LoadSceneAsync("BattlePlayer", LoadSceneMode.Additive);
                asyncOp.completed += (AsyncOperation op) =>
                {
                    Init();
                    playFadeout();
                };
            });
        }

        void Init()
        {
           BattlePlayer.BattlePlayer.Instance.Play(Movie);
        }

        public override void OnExit(IFSM<Transform> fsm)
        {

        }

        public override void OnInitialization(IFSM<Transform> fsm)
        {
        }

        public override void OnTermination(IFSM<Transform> fsm)
        {
        }
    }
}
