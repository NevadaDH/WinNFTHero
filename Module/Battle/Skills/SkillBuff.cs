using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Mao
{
	public class SkillBuff
	{
		private FighterData myData;
		List<SkillBuffVO> skillBuffVOs;
		public SkillBuff(FighterData m, List<SkillBuffVO> svos)
		{
			myData = m;
			skillBuffVOs = svos;
			if(svos != null)
			{
				for(int i=0; i<svos.Count; i++)
				{
					SkillBuffVO vo = svos[i];
					FighterData td = BattleManager.Instance.battleTroop.FindFighter(vo.battle_Damage.buff.target);
					BuffData bd = new BuffData(vo);
					BattleManager.buffControl.AddBuff(myData, td, bd);
				}
			}
		}
	}
}
