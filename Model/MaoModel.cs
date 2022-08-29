using System;
using System.Collections.Generic;

namespace Mao{
    public class ConditionRuntime {

        public string id;

        public string icon;

        public string name ;

        public string desc;

        public int rare;
        
        public int count;

        public int needcount;
    }


    public class MaoModel : BaseModel
    {
        public List<ConditionRuntime> getLimitRewards(object root, O1LimitVO limit)
        {
            List<ConditionRuntime> result = new List<ConditionRuntime>();

            object target = filterRuntime(root, DataConvert.tryGetValue(root, key), limit.filter);
            if (null == target) return null;

            foreach (var condition in limit.conditions)
            {
                // if (condition.opera == "+=")
                // {
                ConditionRuntime item = new ConditionRuntime();
                this.createConditionItem(item, root, target, limit, condition);
                result.Add(item);
                // }
            }

            return result;
        }

        virtual protected ConditionRuntime createConditionItem(ConditionRuntime item, object root, object target, O1LimitVO limit, O1ConditionVO condition)
        {
            item.id = condition.id;
            item.count = Convert.ToInt32(DataConvert.tryGetValue(target, condition.id)) ;
            item.needcount = Convert.ToInt32(condition.value);
            return item;
        }

        public static List<ConditionRuntime> getLimitRewards(object root, List<O1LimitVO> limits)
        {
            List<ConditionRuntime> result = new List<ConditionRuntime>();

            foreach (var limit in limits)
            {
                BaseModel model = null;
                models.TryGetValue(limit.module, out model);
                if (null != model)
                {
                    var list = (model as MaoModel).getLimitRewards(root, limit);
                    if (list.Count > 0)
                    {
                        result.InsertRange(result.Count, list);
                    }
                }
            }
            return result;

        }

    }
}
