
using System.Collections.Generic;

public partial class ChapterVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    public List<ChapterChildVO> childs = null;
    
}



public partial class ChapterChildVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 难度  
    */
    public ushort level = 0;
    
    /**
    * 可上阵英雄数量  
    */
    public ushort count = 0;
    
    /**
    * 怪物关卡  
    */
    public string chapter = "";
    
    /**
    * 关卡名称  
    */
    public string name = "";
    
    /**
    * 条件  
    */
    public List<O1LimitVO> condition = null;
    
    /**
    * 奖励  
    */
    public List<O1LimitVO> reward = null;
    
    /**
    * 资源消耗  
    */
    public List<O1LimitVO> cost = null;
    
    /**
    * 地图  
    */
    public string map = "";
    
    /**
    * 奖励事件  
    */
    public List<CodeFunctionVO> module = null;
    
    /**
    * 基准难度  
    */
    public Pro pro = null;
    
}



