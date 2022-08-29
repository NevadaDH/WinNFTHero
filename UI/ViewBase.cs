
using Cosmos;
using Cosmos.UI;
using UnityEngine;
using foundation;
using foundation.events;
using System;
using System.Reflection;

namespace Mao
{
    public class ViewBase : GComponent
    {
        MiniDispatcher dispatcher = new MiniDispatcher();
        protected GLoader _bgLoader;
        public ViewBase()
        {

        }

        override public void ConstructFromResource()
        {
            base.ConstructFromResource();
            ViewManager.Instance.bindingView(this, this.asCom);

            onAddedToStage.Add(OnShown);
            onRemovedFromStage.Add(OnClosed);
            OnInit();
        }

        override public void Dispose()
        {
            onAddedToStage.Remove(OnShown);
            onRemovedFromStage.Remove(OnClosed);

            OnDispose();
            base.Dispose();
        }

        public virtual void OnInit()
        {
            EvtAttribute.registerEvent(this, 0);
       
        }

        //增加背景蒙版，防止点击人物寻路，暂时无调用
        public virtual void setBackground()
        {
            if(_bgLoader == null)
            {
                _bgLoader = new GLoader();
                var stage = Stage.inst;
                _bgLoader.SetSize(stage.width, stage.height);
                _bgLoader.touchable = false;
                AddChildAt(_bgLoader,0);
            }
        }
        public virtual void OnDispose()
        {

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        protected virtual void OnShown()
        {
        }

        protected virtual void OnClosed()
        {
            EvtAttribute.removeEvent(this);
            RedDotManager.Instance.CheckMailRedDot();
        }

        public void CloseSelf()
        {
            ViewManager.Instance.CloseView(this);

        }
    }

}