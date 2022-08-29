using System;
using System.Collections.Generic;
using Cosmos;
using UnityEngine;
using foundation;

namespace Mao
{
    public enum EFadeEffects
    {
        Black
    }

    public class ViewManager : Singleton<ViewManager>
    {
        private string PACKAGE_ROOT_PATH = "FUI/";

        Dictionary<string, ViewBase>
            _views = new Dictionary<string, ViewBase>();

        Stack<ViewBase> _viewStack = new Stack<ViewBase>();

        public void Init(string lang)
        {
            UnityEngine.Object.DontDestroyOnLoad(StageCamera.main.gameObject);

            if (!string.IsNullOrEmpty(lang) && !lang.Equals("en"))
            {
                UIPackage.branch = lang;
            }

            UIConfig.l10nFunc = L10N.L;
           
        }

        public T ShowView<T>(bool isWindow = false, bool isCenter = true)
            where T : ViewBase, new()
        {
            System.Type viewType = typeof(T);
            string viewName = viewType.Name;
            return ShowView(viewName, isWindow, isCenter) as T;
        }

        public ViewBase ShowView(string viewName, bool isWindow = false, bool isCenter = true)
        {
            ViewBase view = null;
            if (_views.ContainsKey(viewName))
            {
                view = _views[viewName];
                return view;
            }

            view = CreateView(viewName);
            _views.Add(view.name, view);
            if (isWindow)
            {
                Window win = new Window();
                win.contentPane = view;
                win.modal = true;
                Root.inst.ShowWindow(win);
                if (isCenter)
                {

                    win.x = (Root.inst.width - win.width) / 2;
                    win.y = (Root.inst.height - win.height) / 2;
                }
            }
            else
            {
                FairyGUI.Root.inst.AddChild(view);
            }

            return view;
        }

        ViewBase CreateView(string viewName)
        {
            Type viewType = System.Type.GetType("Mao." + viewName);
            ViewAttribute viewAttr =
                (ViewAttribute)
                Attribute.GetCustomAttribute(viewType, typeof(ViewAttribute));

            if (UIPackage.GetByName(viewAttr.PkgName) == null)
            {
                var package = UIPackage.AddPackage(PACKAGE_ROOT_PATH + viewAttr.PkgName);

                foreach (var depend in package.dependencies)
                {
                    UIPackage.AddPackage(PACKAGE_ROOT_PATH + depend["name"]);
                }

            }

            ViewBase view = UIPackage.CreateObject(viewAttr.PkgName, viewAttr.ResName) as ViewBase;
            view.name = viewName;
            return view;
        }

        public T CreateView<T>()
            where T : ViewBase, new()
        {
            System.Type viewType = typeof(T);
            string viewName = viewType.Name;
            return CreateView(viewName) as T;
        }



        public T GetView<T>() where T : ViewBase, new()
        {
            System.Type viewType = typeof(T);
            string viewName = viewType.Name;
            T view = null;
            if (_views.ContainsKey(viewName))
            {
                view = _views[viewName] as T;
                return view;
            }
            return null;
        }

        
        public void CloseView<T>(bool dispose = true)
            where T : ViewBase, new()
        {
            string viewName = typeof(T).Name;
            if (_views.ContainsKey(viewName))
            {
                ViewBase view = _views[viewName];
                this.CloseView(view, dispose);
            }
        }

        public void CloseView(ViewBase view, bool dispose = true)
        {
            string viewName = view.name;
            if (_views.ContainsKey(viewName))
            {
                if (view.parent.GetType() == typeof(Window))
                {
                    Window win = view.parent as Window;
                    win.Hide();
                }
                view.Dispose();
                _views.Remove(viewName);
                Facade.EVT.SimpleDispatch("closePanelByNpc", viewName);
            }
        }

        public void CloseAll(bool dispose = true)
        {
            var views = new List<ViewBase>(_views.Values);
            foreach (var item in views)
            {
                CloseView(item);
            }
        }

        //弹窗
        // ViewManager.Instance.ShowMessageBox("这是一个弹窗",(view)=>{
        //     Utility.Debug.LogInfo("ClickOK");
        //     view.CloseSelf();
        // });
        public void ShowMessageBox(
            string content,
            Action<MessageBox> clickOK = null
        )
        {
            MessageBox view = this.ShowView<MessageBox>(true);
            view.Content = content;
            view.HasCancel = false;
            view.OnClickOK = clickOK;
        }

        public void ShowToast(string content)
        {
            Toast view = this.ShowView<Toast>(true);
            view.Label = content;
        }

        // ViewManager.Instance.ShowConfirmBox("这是一个弹窗",(view)=>{
        //      Utility.Debug.LogInfo("ClickOK");
        //      view.CloseSelf();
        //  },(view)=>{
        //      Utility.Debug.LogInfo("ClickCancel");
        //      view.CloseSelf();
        //  });
        public void ShowConfirmBox(
            string content,
            Action<MessageBox> clickOK,
            Action<MessageBox> clickCancel
        )
        {
            MessageBox view = this.ShowView<MessageBox>(true);
            view.Content = content;
            view.HasCancel = true;
            view.OnClickOK = clickOK;
            view.OnClickCancel = clickCancel;
        }

        public void PlayFade(EFadeEffects type, Action<Action> afterFadein)
        {
            string comName = "";
            switch (type)
            {
                case EFadeEffects.Black:
                    comName = "FadeEffect_Black";
                    break;

            }
            Component com = UIPackage.CreateObject("common", comName).asCom;

            Root.inst.AddChild(com);
            Transition fadeIn = com.GetTransition("fadein");
            Transition fadeOut = com.GetTransition("fadeout");
            fadeIn.Play(() =>
            {
                afterFadein(() =>
                {
                    Root.inst.AddChild(com);
                    fadeOut.Play(() =>
                    {
                        com.Dispose();
                    });
                });
            });
        }

        public void bindingView(ViewBase view, Component ui)
        {
            System.Type viewType = view.GetType();
            var properties = viewType.GetProperties();
            var fields = viewType.GetFields();
            view.width = ui.actualWidth;
            view.height = ui.actualHeight;
            foreach (var property in properties)
            {
                if (!property.IsDefined(typeof(BindingAttribute), false))
                    continue;
                BindingAttribute binding =
                    (BindingAttribute)
                    Attribute
                        .GetCustomAttribute(property,
                        typeof(BindingAttribute));

                UIObject obj;
                if (ui.GetChildByPath(binding.Path) != null)
                {
                    obj = ui.GetChildByPath(binding.Path);
                }
                else
                {
                    obj = UIPackage.CreateObject("common", binding.Path);
                }
                if (obj == null)
                {

                    Utility
                        .Debug
                        .LogWarning(string
                            .Format("{0} Field {1} Binding Failed Path:{2}",
                            viewType.Name,
                            property.Name,
                            binding.Path));
                    continue;
                }
                property.SetValue(view, obj);
            }

            foreach (var field in fields)
            {
                if (!field.IsDefined(typeof(BindingAttribute), false))
                    continue;

                BindingAttribute binding =
                    (BindingAttribute)
                    Attribute
                        .GetCustomAttribute(field,
                        typeof(BindingAttribute));

                UIObject obj = null;
                if (ui.GetChildByPath(binding.Path) != null)
                {
                    obj = ui.GetChildByPath(binding.Path);
                }
                else
                {
                    obj = UIPackage.CreateObject("common", binding.Path);
                }

                if (obj == null)
                {

                    Utility
                        .Debug
                        .LogWarning(string
                            .Format("{0} Field {1} Binding Failed Path:{2}",
                            viewType.Name,
                            field.Name,
                            binding.Path));
                    continue;
                }
                field.SetValue(view, obj);
            }
        }
    }
}
