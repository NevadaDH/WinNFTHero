using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mao 
{
  public class BattleData
  {
    ///<summary>英雄</summary>
    public List<FighterData> Herolist = new List<FighterData>();
        
    public float bloodMissingTime = 0.1F;
  }
}