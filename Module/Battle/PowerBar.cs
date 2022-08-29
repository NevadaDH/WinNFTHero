
using UnityEngine;
using FairyGUI;

namespace Mao
{
    public class PowerBar : MonoBehaviour
    {
        Camera mainCamera;
   
        public int unitPower;
        public float power = 0;
        public float time = 0;

        public GProgressBar bar;

        public static float HeadInBarMissingTime = 0.56F;

        void Awake()
        {
            //panel.ui.GetChild("sign").asLoader.url = "ui://HeadBar/task";
            mainCamera = Camera.main;
            UIPanel panel = GetComponent<UIPanel>();
            bar = panel.ui.GetChild("power").asProgress;
            bar.value = 0;
        }

        // Start is called before the first frame update
        void Start()
        {
            transform.forward = mainCamera.transform.forward;
            //outBar.visible = false;
        }

        public void flushPower(int p, int t)
        {
            if(unitPower == p)
              return;

            unitPower = p;
            power = p;
            time = t;
            power -= time;
            time = 0;
            power /= 1000.0f;
            
        }

        public void setBar(bool we)
        {
            bar.GetChild("bar").asLoader.url = UIPackage.GetItemURL("HeadBar", "blue");
        }

        public void hide()
        {
            bar.visible = false;
            //inBar.TweenFade(0, 0.1F);
            //outBar.TweenFade(0, 0.1F);
        }

        //Update is called once per frame
        void Update()
        {
            if(time < power)
            {
               time += Time.deltaTime;
               float val = 1.0F * time / power;
               bar.value = val*100;
            }
        }

        public void repairRotation()
        {
            transform.forward = mainCamera.transform.forward;
        }
    }

}