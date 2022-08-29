
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Mao
{


    public class AvatarManager : MonoSingleton<AvatarManager>
    {
        public RuntimeAnimatorController ControllerMainCity;
        public RuntimeAnimatorController ControllerBattle;

        Dictionary<string,QCostume.CostumeSetting> _settings = new Dictionary<string,QCostume.CostumeSetting>();
        public struct Avatar{
            //皮肤颜色
            public string SkinColor;
            //发型或帽子
            public string TopHead;
            //嘴鼻
            public string MouthNose;
            //眼睛 + 眉毛
            public string Eyebrow;

            public string Face;
            //上身
            public string Top;
            //下身
            public string Bottom;
            //手或手套
            public string Hand;
            //脚
            public string Foot;
            //面具或眼镜
            public string Mask;
            //配件
            public string Accessory;
        }


        public struct Equipments{
            public string Cap; //头部
            public string Upper;//上衣
            //帽子
            public string Bottom; //裤子
            //衣服
            public string Weapon; //武器
            //裤子
            public string Glasses; //眼镜
            //手套
            public string Gloves;//手套
            //鞋子
            public string Shoes;//鞋子

            public string Ornament;//饰品

        }

	    private const int COMBINE_TEXTURE_MAX = 512;
	    private const string COMBINE_DIFFUSE_TEXTURE = "_MainTex";

        private void Start() {

        }

        public void Init(){
            
        }

        public GameObject CreateHead(Avatar avatar,Equipments equips){
            var go = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo("Avatar/Face/Face_04"),true);
            // var attachObj = new GameObject("headicon3d");
            CostumeBuilder costumeBuilder = go.AddComponent<CostumeBuilder>();
            CostumeItem[] costumeItems = new CostumeItem[]{
                new CostumeItem("Avatar/Eye/Eye_01","Eyebrow"),
                //new CostumeItem(avatar.Eyebrow,"Eyebrow"),
                // new CostumeItem(string.IsNullOrEmpty(equips.Cap)?avatar.TopHead:equips.Cap,"TopHead"),
                // new CostumeItem(string.IsNullOrEmpty(equips.Glasses)?avatar.Mask:equips.Glasses,"Mask"),
            };

            //  SkinnedMeshItem[] costumeSkinnedItems = new SkinnedMeshItem[]{
            //         new SkinnedMeshItem(avatar.Eyes,"Eyeball"),
            //         new SkinnedMeshItem(avatar.MouthNose,"MouthNose"),
            //         new SkinnedMeshItem(avatar.Eyebrow,"Eyebrow")
            // };

            foreach (var item in costumeItems)
            {
                costumeBuilder.Add(item);
            }

            costumeBuilder.Build(false);
                        
            return go;
        }

        public GameObject CreateAvatar(bool isBattle,Avatar avatar,Equipments equips){
            string skinPath = "Avatar/Hero_Skin";
            var go = CosmosEntry.ResourceManager.LoadPrefab(new AssetInfo(skinPath),true);
            go.GetComponent<Animator>().runtimeAnimatorController = isBattle? ControllerBattle:ControllerMainCity;

            BuiildAvatar(go,avatar,equips);
           
            return go;
        }

        public  Transform Find(Transform tf, string name, bool includeInactive = false)
        {
            Transform[] childTF = tf.GetComponentsInChildren<Transform>(includeInactive);

            for (int i = 0; i < childTF.Length; ++i)
            {
                if (childTF[i].name == name)
                    return childTF[i];
            }

            return null;
        }

        public void BuiildAvatar(GameObject go,Avatar avatar,Equipments equips){
            CostumeBuilder costumeBuilder = go.GetComponentInChildren<CostumeBuilder>();
            costumeBuilder.defaultItems.Clear();
            //身体
            CostumeItem[] costumeItems = new CostumeItem[]{
                new CostumeItem(avatar.Accessory,"Accessory"),
                new CostumeItem(string.IsNullOrEmpty(equips.Cap)?avatar.TopHead:equips.Cap,"TopHead"),
                new CostumeItem(string.IsNullOrEmpty(equips.Upper)?avatar.Top:equips.Upper,"Top"),
                new CostumeItem(string.IsNullOrEmpty(equips.Bottom)?avatar.Bottom:equips.Bottom,"Bottom"),
                new CostumeItem(string.IsNullOrEmpty(equips.Gloves)?avatar.Hand:equips.Gloves,"Hand"),
                new CostumeItem(string.IsNullOrEmpty(equips.Shoes)?avatar.Foot:equips.Shoes,"Foot"),
                new CostumeItem(string.IsNullOrEmpty(equips.Glasses)?avatar.Mask:equips.Glasses,"Mask"),
                new CostumeItem(equips.Ornament,"Ornament")
                //new CostumeItem(equips.Weapon,"Weapon"),
            };

            foreach (var item in costumeItems)
            {
                costumeBuilder.Add(item);
            }

            costumeBuilder.Build(false);

            var head = CreateHead(avatar,equips);
            head.transform.SetParent(Find(go.transform,"Bip001 Head"),false) ;
        }

        public void ChangeEquip(GameObject go, string mask, string equip){
            CostumeBuilder costumeBuilder = go.GetComponentInChildren<CostumeBuilder>();
            
            if(costumeBuilder==null){
                Debug.LogWarning("ChangeEquip failed CostumeBuilder missing");
                return ;
            }

            CostumeItem cosItem = new CostumeItem(equip,mask);
            costumeBuilder.RemoveMask(mask);
            costumeBuilder.Add(cosItem);
            costumeBuilder.Build();
        }

    }
    
}