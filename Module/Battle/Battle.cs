using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cosmos;
namespace Mao
{
    public class Battle : MonoBehaviour
    {
        public Transform TroopSite;
        public Transform HeroSite;
        public Transform BattleSite;
        
        ///<summary>英雄</summary>
        public List<FighterData> Herolist = new List<FighterData>();

        void Awake()
        {
            BattleManager.battleSite = BattleSite;
            BattleTroop.RolePrefab = RolePrefab;
            BattleTroop.MonsterPrefab = MonsterPrefab;
        }

        // Start is called before the first frame update
        void Start()
        {
            BattleManager.Instance.Init();
            //Mao.ViewManager.Instance.ShowView<BattleView>();
        }
        
        // Update is called once per frame
        void Update(){  
        }
    }
}