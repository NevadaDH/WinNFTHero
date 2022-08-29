using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity​Engine.Rendering.Universal​;

namespace Mao
{
    public class MainCityState : FSMState<Transform>
    {
        public string OpenViewName = "";

        public override void Action(IFSM<Transform> fsm)
        {
        }
        public override void OnEnter(IFSM<Transform> fsm)
        {
            Utility.Debug.LogInfo("Enter MainCityState");

            ViewManager.Instance.PlayFade(EFadeEffects.Black,(playFadeout)=>{
                CosmosEntry.SceneManager.LoadSceneAsync(new SceneInfo("Map_MainCity", false),()=>{
                    ViewManager.Instance.CloseView<LoginLoadingView>();
                    ViewManager.Instance.ShowView<NavigateView>();
                    if(!string.IsNullOrEmpty(OpenViewName)){
                        ViewManager.Instance.ShowView(OpenViewName,true);
                        OpenViewName = "";
                    }
                    playFadeout();
                });
            });
        }
        public override void OnExit(IFSM<Transform> fsm)
        {
            ViewManager.Instance.CloseView<NavigateView>();
            
        }
        public override void OnInitialization(IFSM<Transform> fsm)
        {
        }
        public override void OnTermination(IFSM<Transform> fsm)
        {
        }

    }
}

