using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Cosmos;

namespace Mao 
{
		/// <summary>实例化管理器</summary>
	public class InstanceManager : Singleton<InstanceManager>
	{
		private Dictionary<string, List<Transform>> usePool = new Dictionary<string, List<Transform>>();
		private Transform Pool;

		public void Init()
		{
			usePool.Clear();
			Pool = new GameObject().transform;
			Pool.name = "InstancePool";
			//SceneManager.sceneLoaded -= OnSceneLoaded;
			//SceneManager.sceneLoaded += OnSceneLoaded;
		}

		void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
		{
			//usePool.Clear();
			//Pool = new GameObject().transform;
			//Pool.name = "InstancePool";
		}

		/// <summary>创建</summary>
		public Transform Create(string n, Transform tf)
		{
			return CreateOne(n, Vector3.zero, Vector3.zero, tf);
		}

		/// <summary>创建</summary>
		public Transform Create(string n, Vector3 pos, Vector3 rot, Transform tf)
		{
			return CreateOne(n, pos, rot, tf);
		}
		
		/// <summary>创建</summary>
		public Transform Create(Transform tf)
		{
			return CreateOne(tf.name, Vector3.zero, Vector3.zero, tf);
		}

		/// <summary>创建</summary>
		public Transform Create(Transform tf, Vector3 pos, Vector3 rot)
		{
			return CreateOne(tf.name, pos, rot, tf);
		}

		/// <summary>创建</summary>
		private Transform CreateOne(string n, Vector3 pos, Vector3 rot, Transform tf)
		{
			Transform creator = null;
			creator = UnityEngine.Object.Instantiate(tf, pos, Quaternion.identity) as Transform;
			creator.gameObject.SetActive(true);
			creator.eulerAngles = rot;
			creator.name = n+"";
			creator.SetParent(Pool);
			return creator;
		}

		/// <summary>创建隐藏</summary>
		public Transform CreateLimit(string n, Vector3 pos, Vector3 rot, float time, Transform tf)
		{
			Transform xtf = CreateOne(n, pos, rot, tf);
			if (xtf == null) return null;
			DeCreate(xtf, time);
			return xtf;
		}

		/// <summary>删除</summary>
		public void DeCreate(Transform tf, float time = 0)
		{
			if(tf != null)
			{
			    tf.gameObject.SetActive(false);
				tf.SetParent(Pool);
			}
		}

		IEnumerator DelayDeCreate(Transform tf, float time)
		{
			yield return new WaitForSeconds(time);
			if(tf != null)
			{
				tf.gameObject.SetActive(false);
				tf.SetParent(Pool);
			}
		}

		/// <summary>清空</summary>
		public void Clear()
		{
			usePool.Clear();
			foreach(Transform child in Pool)
			{
				//Destroy(child.gameObject);
			}
		}
	}
}
