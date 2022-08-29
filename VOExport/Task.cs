
using System.Collections.Generic;

public class TaskVO {
    /**
    * 主key  
    */
    public ushort id = 0;
    
    /**
    * 每日or每周任务  
    */
    public ushort type = 0;
    
    /**
    * 是否主动完成  
    */
    public ushort autofinish = 0;
    
    /**
    * 条件  
    */
    public List<O1LimitVO> condition = null;
    
    /**
    * 奖励  
    */
    public List<O1LimitVO> reward = null;
    
    /**
    * 该任务指向的npc的id  
    */
    public ushort npc = 0;
    
    /**
    * 图标  
    */
    public string icon = "";
    
    /**
    * 任务名称  
    */
    public string title = "";
    
    /**
    * 任务描述  
    */
    public string des = "";
    
}



