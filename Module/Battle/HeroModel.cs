using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mao
{
    public class HeroModel : MonoBehaviour
    {

        private GameObject mySelf;
        private GameObject Site;

        private SkinnedMeshRenderer[] mySkinMeshRender;
        private MeshRenderer[] myMeshRender;
        private int DefaultLayer;

        // Start is called before the first frame update
        void Awake()
        {
            //  mySelf = this.gameObject;
            if (BattleManager.battleSite != null)
                Site = BattleManager.battleSite.gameObject;
            // mySkinMeshRender = mySelf.GetComponentsInChildren<SkinnedMeshRenderer>();
            // myMeshRender = mySelf.GetComponentsInChildren<MeshRenderer>();
            //DefaultLayer = mySelf.layer;

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void xuHan()
        {
            UnityEngine.Debug.Log("鼠标按下时");
            BattleManager.battleSite.gameObject.GetComponentInChildren<Vacuity>().PlaySceneEffect();
        }

        public void outLine()
        {
            // foreach (var render in mySkinMeshRender)
            // {
            //     render.gameObject.layer = 12;
            // }
            // foreach (var meshRender in myMeshRender)
            // {
            //     meshRender.gameObject.layer = 12;
            // }
        }

        public void cancelOutLine()
        {
            // foreach (var render in mySkinMeshRender)
            // {
            //     render.gameObject.layer = 0;
            // }
            // foreach (var meshRender in myMeshRender)
            // {
            //     meshRender.gameObject.layer = 0;
            // }
        }

        public void pauseEffect()
        {
            Site.GetComponentInChildren<Vacuity>().PauseSceneEffect();
            mySelf.layer = DefaultLayer;
        }
    }

}
