using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using foundation;
using foundation.events;
namespace Mao
{
    public class Hero : MonoBehaviour
    {
        private GameObject mySelf;
        private GameObject Site;

        private SkinnedMeshRenderer[] mySkinMeshRender;
        private MeshRenderer[] myMeshRender;
        private int DefaultLayer;
        private Vector3 screenPoint;
        private Vector3 offset;
        public Transform sHeadBar;
        public Transform sPoweBar;
        private Vector3 originalPos;

        private BondModule bondModule = ClassUtils.Instance<BondModule>();
        //英雄唯一Id
        public int fid;

        public bool we = true;

        int lastSite = -1;

        void Start()
        {
            mySelf = this.gameObject;
            Site = GameObject.FindGameObjectWithTag("Site");
            mySkinMeshRender = mySelf.GetComponentsInChildren<SkinnedMeshRenderer>();
            myMeshRender = mySelf.GetComponentsInChildren<MeshRenderer>();
            DefaultLayer = mySelf.layer;
        }

/*
        void OnMouseEnter()
        {
            //Debug.Log("鼠标进入该对象区域时");
        }

        void OnMouseExit()
        {
            //Debug.Log("鼠标离开该模型区域时");
        }

        void OnMouseDown()
        {
            if (BattleManager.Instance.isBattleStart)
                return;

            if (we)
            {
                if (mySkinMeshRender.Length == 0 || myMeshRender.Length == 0)
                {
                    mySkinMeshRender = mySelf.GetComponentsInChildren<SkinnedMeshRenderer>();
                    myMeshRender = mySelf.GetComponentsInChildren<MeshRenderer>();
                }

                UnityEngine.Debug.Log("鼠标按下时");
                Site.GetComponentInChildren<Vacuity>().PlaySceneEffect();
                foreach (var render in mySkinMeshRender)
                {
                    render.gameObject.layer = 12;
                }
                foreach (var meshRender in myMeshRender)
                {
                    meshRender.gameObject.layer = 12;
                }
                screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
                originalPos = gameObject.transform.position;
            }
        }

        void OnMouseDrag()
        {
            if (BattleManager.Instance.isBattleStart)
                return;

            if (we)
            {
                // BattleManager.instance.DisabelAllSiteShine();
                print("鼠标拖动");
                //Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                //Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
                //transform.position = new Vector3(cursorPosition.x, 0, cursorPosition.z); //cursorPosition;

                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                //鼠标转移的3d空间坐标值
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
                //改变鼠标的3D空间坐标
                curPosition.z += 0.5F;
                transform.position = curPosition;

                Vector3 touch = new Vector3(Input.mousePosition.x - 8, Input.mousePosition.y - 16, screenPoint.z);
                Vector3 worldpos = Camera.main.ScreenToWorldPoint(touch);

                bool isIn = isInBounds(touch);
                if (isIn)
                {
                    //int site = GetNearstTroopSite(transform.position, worldpos);
                    int site = BattleManager.Instance.battleTroop.GetPlaceTroopSite(new Vector3(touch.x, touch.y, 0));
                    if (site != lastSite)
                    {
                        lastSite = site;
                        BattleManager.Instance.battleTroop.DisabelAllSiteShine();
                        BattleManager.Instance.battleTroop.ShineSite(lastSite);
                        print("lastsite:" + lastSite);
                    }
                }
            }
        }

        void OnMouseUp()
        {
            Site.GetComponentInChildren<Vacuity>().PauseSceneEffect();
            if (BattleManager.Instance.isBattleStart)
                return;

            if (we)
            {
                //Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                //Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
                //transform.position = new Vector3(cursorPosition.x, 0, cursorPosition.z); //cursorPosition;


                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                //鼠标转移的3d空间坐标值
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);
                //改变鼠标的3D空间坐标
                curPosition.z += 0.5F;
                transform.position = curPosition;


                Vector3 touch = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
                Vector3 worldpos = Camera.main.ScreenToWorldPoint(touch);

                bool isIn = isInBounds(touch);
                if (isIn)
                {
                    int site = BattleManager.Instance.battleTroop.GetPlaceTroopSite(new Vector3(touch.x, touch.y, 0));
                    //if (site >= 0)
                    //int site = GetNearstTroopSite(transform.position, worldpos);
                    if (site >= 0)
                    {
                        if (!BattleManager.Instance.battleTroop.checkIsSiteHaveHero(site))
                        {
                            BattleManager.Instance.battleTroop.moveToSite(fid, site);
                        }
                        else //switch site
                        {
                            int ret = BattleManager.Instance.battleTroop.switchSite(fid, site);
                            if (ret == -1)
                            {
                                gameObject.transform.position = originalPos;
                            }
                            //transform.position = originalPos;
                        }
                    }
                    else
                    {
                          BattleManager.Instance.battleTroop.removeHeroFromMap(fid);
                    }
                }
                else
                {
                    BattleManager.Instance.battleTroop.removeHeroFromMap(fid);
                    int cnt = BattleManager.Instance.battleTroop.getIsOnMapHeroCnt();
                    if (cnt == 0)
                    {
                        bondModule.destroyMyBondData();
                    }
                    else
                    {
                        //bondModule.InitBondData();
                    }

                   // Facade.EVT.SimpleDispatch("refreshBondWindoLst");
                }
                lastSite = -1;
                BattleManager.Instance.battleTroop.DisabelAllSiteShine();
                foreach (var render in mySkinMeshRender)
                {
                    render.gameObject.layer = DefaultLayer;
                }
                foreach (var meshRender in myMeshRender)
                {
                    meshRender.gameObject.layer = DefaultLayer;
                }

            }
        }

        bool isInBounds(Vector3 mousePosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity);//哦这个就相当于把1左移一个int
            if (hitArr.Length > 0)
            {
                for (int i = 0; i < hitArr.Length; i++)
                {
                    string name = hitArr[i].collider.gameObject.name;
                    UnityEngine.Debug.Log("射到了：" + name);
                    if (name == "Bounds")
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        int GetNearstTroopSite(Vector3 curPos, Vector3 touch)
        {
            float minDis = 1000000;
            int site = -1;
            var node = BattleManager.battleSite.Find("Troop");
            var childcount = node.childCount;
            for (int i = 0; i <= 27; i++)
            {
                Transform trans = node.GetChild(i);
                float dist = Vector3.Distance(curPos, trans.position);
                if (minDis > dist)
                {
                    minDis = dist;
                    site = i;
                }
            }
            return site;
        }*/
    }
}