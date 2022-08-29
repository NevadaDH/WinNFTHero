using UnityEngine;
using System.Collections;
using Cosmos;

namespace Mao 
{
	public class AniManager : Singleton<AniManager>
	{
		/// <summary>动作float切换间隔时间</summary>
		public const float ANI_FLOAT_DELTATIME = 0.2F;

		public void Init()
		{}
		
		public void DoAct(Animator ani, Act n)
		{
			if(ani != null && n != Act.none)
			{
				ani.Play(n.ToString(), 0, 0);
			}
		}

		public void DoActHalf(Animator ani, Act n)
		{
			if(ani != null && n != Act.none)
			{
				ani.Play(n.ToString(), 0, 0.5F);
			}
		}

		public void DoAct(Animator ani, string n)
		{
			if(ani != null)
			{
				ani.Play(n, 0, 0);
			}
		}

		public void DoAct(Animator ani, Act n, bool b)
		{
			if(ani != null)
			{
				ani.SetBool(n.ToString(), b);
			}
		}

		public void DoAct(Animator ani, Act n, float v)
		{
			ani.SetFloat(n.ToString(), v);
		}

		public void BreakAct(Animator ani, Act n)
		{
			if(ani != null && ani.enabled)
			{
				if(n == Act.knockback && ani.GetBool(Act.stun.ToString()))
				{
					return;
				}
				ani.Play(n.ToString(), 0, 0);
			}
		}

		public void BreakAct(Animator ani, string n)
		{
			if(ani != null)
			{
				ani.Play(n, 0, 0);
			}
		}

		public void ChangeSpeed(Animator ani, float v)
		{
			ani.speed = v;
		}
	}
}