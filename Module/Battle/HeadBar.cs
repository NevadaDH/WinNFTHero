using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mao
{
    public class HeadBar : MonoBehaviour
    {
        Camera mainCamera;
        public int hp;
        int hpMax;
        public GProgressBar inBar;
        public GProgressBar outBar;

        float t = 0;
        bool beHurt = false;

        int hurt_cnt = 0;


        bool block = false;

        float block_dt = 0;

        public static float HeadInBarMissingTime = 0.56F;
        public static float HeadOutBarMissingTime = 0.12F;

        void Awake()
        {
            //panel.ui.GetChild("sign").asLoader.url = "ui://HeadBar/task";

            mainCamera = Camera.main;
            UIPanel panel = GetComponent<UIPanel>();
            inBar = panel.ui.GetChild("inBar").asProgress;
            outBar = panel.ui.GetChild("outBar").asProgress;
            inBar.asProgress.value = 100;
            outBar.asProgress.value = 100;
        }

        // Start is called before the first frame update
        void Start()
        {
            transform.forward = mainCamera.transform.forward;
            //outBar.visible = false;
        }

        public void setBlood(int hpVal, bool we)
        {
            hpMax = hp = hpVal;
        }

        public void setBar(bool we)
        {
         //   return;
            if (we)
            {
                outBar.GetChild("bar").asLoader.url = UIPackage.GetItemURL("HeadBar", "blue");
            }
            else
            {
                outBar.GetChild("bar").asLoader.url = UIPackage.GetItemURL("HeadBar", "red");
            }
        }

        public void setName(string name)
        {
            //UIPanel panel = GetComponent<UIPanel>();
            //panel.ui.GetChild("name").text = name;
        }

        public bool isDead()
        {
            if (hp <= 0)
                return true;
            return false;
        }

        // Update is called once per frame
        void Update()
        {
            //transform.forward = mainCamera.transform.forward;
            //Camera.main.transform.LookAt(transform);
             if (beHurt)
             {
                 t += Time.deltaTime;
                 if (t >= 0.2)
                 {
                     float val = 1.0F * hp / hpMax;
                     //Debug.Log("update inBar Val:" + val);
                     //inBar.asProgress.value = val*100;
                     inBar.TweenValue(val * 100, HeadInBarMissingTime);
                     t = 0;
                     beHurt = false;
                     //Debug.Log("update inBar Pro:" +  inBar.asProgress.value);
                 }
             }
        }

        public void addHurt(int xhp)
        {
            hp = xhp;
            float val = 1.0F * hp / hpMax;
            //outBar.asProgress.value = val * 100;
            outBar.TweenValue(val * 100, HeadOutBarMissingTime);
            //inBar.TweenValue(val * 100, GameDataX.HeadBarBloodMissingTime);
            //记录被受击,过0.5秒，开始inBar, 如果0.5秒再次被攻击，则时间继续从0开始计算，直到0.5秒结束
            beHurt = true;
            t = 0;
            //Debug.Log("add Hurt：" + hurt_cnt++);
        }
        
        public void hide()
        {
            inBar.visible = false;
            outBar.visible = false;
            //inBar.TweenFade(0, 0.1F);
            //outBar.TweenFade(0, 0.1F);
        }

        public void repairRotation()
        {
            transform.forward = mainCamera.transform.forward;
        }
    }
}
