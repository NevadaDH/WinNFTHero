

using UnityEngine;
using System.IO;
using Cosmos;
using foundation;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;

namespace Mao
{
    public class VOManager
    {
        static Dictionary<Type, object> _VOTypes = new Dictionary<Type, object>();

        //VO索引 key:type+field+fieldvalue  value:id
        static Dictionary<string, string> _VOIndex = new Dictionary<string, string>();

        //初始化索引
        static Dictionary<Type, string[]> BuildVOIndex = new Dictionary<Type, string[]>(){
            //{typeof(SkillVO),new string[]{"uid"}}
        };


        static public IEnumerator LoadAll()
        {
            _VOTypes.Clear();
            var assets = Resources.LoadAll<TextAsset>("GameConfig");
            var type = typeof(GameConfig);
            foreach (var asset in assets)
            {
                try
                {
                    TextAsset textAsset = asset as TextAsset;
                    var target = AmfHelper.read_object(textAsset.bytes) as Dictionary<string, object>;
                    //Utility.Debug.LogInfo(asset.name);
                    var info = type.GetField(asset.name);
                    var convalue = ObjectDictToVODict<object>(target, info.FieldType);
                    info.SetValue(null, convalue);
                    _VOTypes.Add(info.FieldType.GenericTypeArguments[1], convalue);
                }
                catch (System.Exception e)
                {
                    Utility.Debug.LogWarning("LoadFailed " + asset.name);
                    Utility.Debug.LogWarning(e.Message + e.StackTrace);
                }

                yield return new WaitForSeconds(0.01f);
            }


            {
                Dictionary<string, SkillVO> skills = _VOTypes[typeof(SkillVO)] as Dictionary<string, SkillVO>;
                Dictionary<string, SkillChildVO> skillChilds = new Dictionary<string, SkillChildVO>();
                foreach (var item in skills)
                {
                    foreach (var child in item.Value.childs)
                    {
                        if(!skillChilds.TryAdd("" + child.uid, child)){
                            Utility.Debug.LogWarning($"SkillVO duplicate uid ${child.uid}");
                        }
                    }
                }
                _VOTypes.Add(typeof(SkillChildVO), skillChilds);
            }

            {
                Dictionary<string, ModuleVO> modulevVos = _VOTypes[typeof(ModuleVO)] as Dictionary<string, ModuleVO>;
                Dictionary<string, ModuleChildVO> moduleChildVos = new Dictionary<string, ModuleChildVO>();
                foreach (var item in modulevVos)
                {
                    foreach (var child in item.Value.childs)
                    {
                        moduleChildVos.Add($"{item.Key}_{child.id}", child);
                    }
                }
                _VOTypes.Add(typeof(ModuleChildVO), moduleChildVos);
            }

            buildIndex();
        }

        static void buildIndex()
        {
            _VOIndex.Clear();
            foreach (KeyValuePair<Type, string[]> entry in BuildVOIndex)
            {
                if (!_VOTypes.ContainsKey(entry.Key))
                {
                    Utility.Debug.LogWarning("buildIndex Failed " + entry.Key);
                    continue;
                }
                IDictionary vos = _VOTypes[entry.Key] as IDictionary;
                foreach (var fieldName in entry.Value)
                {
                    var fieldInfo = entry.Key.GetField(fieldName);
                    if (fieldInfo == null)
                    {
                        Utility.Debug.LogWarning($"buildIndex Failed {entry.Key} fieldName: {fieldName}");
                        continue;
                    }
                    var idField = entry.Key.GetField("id");
                    foreach (var vo in vos.Values)
                    {
                        string id = idField.GetValue(vo).ToString();
                        string v = fieldInfo.GetValue(vo).ToString();
                        string idx = entry.Key.Name + fieldName + v;
                        _VOIndex.Add(idx, id);
                    }
                }

            }
        }

        static public T ObjectDictToVODict<T>(Dictionary<string, object> value, Type type = null)
        {
            if (null == type) type = typeof(T);
            var dict = Activator.CreateInstance(type);
            var add = type.GetMethod("Add");
            var objs = new object[] { "", null };
            foreach (KeyValuePair<string, object> entry in value)
            {
                objs[0] = entry.Key;
                objs[1] = entry.Value;
                add.Invoke(dict, objs);
            }

            return (T)dict;
        }

        static public bool TryGetVO<T>(int key, out T v){
            return TryGetVO<T>("" + key,out v);
        }

        static public bool TryGetVO<T>(uint key, out T v){
            return TryGetVO<T>("" + key,out v);
        }

        static public bool TryGetVO<T>(string key, out T v)
        {
            Type t = typeof(T);
            if (!_VOTypes.ContainsKey(t))
            {
                v = default(T);
                return false;
            }

            Dictionary<string, T> dict = _VOTypes[t] as Dictionary<string, T>;
            if (!dict.ContainsKey(key))
            {
                v = default(T);
                return false;
            }
            v = dict[key];
            return true;
        }

        static public T GetVO<T>(string key)
        {
            Type t = typeof(T);
            if (!_VOTypes.ContainsKey(t))
            {
                throw new Exception("can't found vo type");
            }

            Dictionary<string, T> dict = _VOTypes[t] as Dictionary<string, T>;
            if (!dict.ContainsKey(key))
            {
                throw new Exception($"[{t.Name}] not key:[{key}]");
            }
            return dict[key];
        }

        static public T GetVO<T>(int key)
        {
            return GetVO<T>("" + key);
        }

        static public T GetVO<T>(ushort key)
        {
            return GetVO<T>("" + key);
        }


        //根据索引查找
        static public T GetByIndex<T>(string fieldName, object value)
        {
            Type t = typeof(T);
            if (!_VOTypes.ContainsKey(t))
            {
                throw new Exception("can't found vo type");
            }

            Dictionary<string, T> dict = _VOTypes[t] as Dictionary<string, T>;
            string idx = t.Name + fieldName + value;
            if (_VOIndex.ContainsKey(idx))
            {
                string id = _VOIndex[idx];
                return dict[id];
            }
            else
            {
                var field = t.GetField(fieldName);
                foreach (T element in dict.Values)
                {
                    if (field.GetValue(element).Equals(value))
                    {
                        return element;
                    }
                }
            }
            Utility.Debug.LogError(string.Format("idx:{0} not found", idx));
            return default(T);
            //throw new Exception(string.Format("idx:{0} not found",idx));
        }

    }
}