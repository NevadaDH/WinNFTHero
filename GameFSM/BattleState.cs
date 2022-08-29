using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Rendering.Universal​;
using UnityEngine.SceneManagement;

namespace Mao
{
    public class BattleState : FSMState<Transform>
    {
        public System.Action playFadeoutAction;
        public delegate void SceneLoadedHandler();
        public SceneLoadedHandler onSceneLoaded;

        public override void Action(IFSM<Transform> fsm)
        {
        }
        public override void OnEnter(IFSM<Transform> fsm)
        {
            Utility.Debug.LogInfo("Enter BattleState");
            ViewManager.Instance.PlayFade(EFadeEffects.Black,(playFadeout)=>{

                SceneManager.UnloadSceneAsync("Formation");
                //SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);

                playFadeoutAction = playFadeout;
                var asyncOp = SceneManager.LoadSceneAsync("Battle", LoadSceneMode.Additive);
                asyncOp.completed += OnSceneLoaded;
                
                //CosmosEntry.SceneManager.LoadSceneAsync(new SceneInfo("Battle", false),()=>{
                //    playFadeout();
                //});
            });
        }

        private void OnSceneLoaded ( AsyncOperation op ) {
              if(playFadeoutAction != null)
                playFadeoutAction();
        }

        public override void OnExit(IFSM<Transform> fsm)
        {
            ViewManager.Instance.CloseView<BattleView>();
        }
        public override void OnInitialization(IFSM<Transform> fsm)
        {
        }
        public override void OnTermination(IFSM<Transform> fsm)
        {
        }
    }
}
