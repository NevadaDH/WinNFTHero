
using System.Collections.Generic;

public partial class MineVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    public List<MineChildVO> childs = null;
    
}



public partial class MineChildVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 等级id  
    */
    public ushort mine_id = 0;
    
    /**
    * 矿点名称  
    */
    public string name = "";
    
    /**
    * 解锁条件  
    */
    public List<O1LimitVO> condition = null;
    
    /**
    * 该矿点等级  
    */
    public ushort level = 0;
    
    /**
    * 该矿点等级系数  
    */
    public float coefficient = 0;
    
    /**
    * 质押花费  
    */
    public uint spend = 0;
    
    /**
    * 矿点质押英雄最低等级  
    */
    public ushort herolevel = 0;
    
    /**
    * 最大人数  
    */
    public ushort heronum = 0;
    
    /**
    * 该矿点的总奖励份数  
    */
    public uint totalbonus = 0;
    
    /**
    * 每份奖励内容  
    */
    public List<O1LimitVO> reward = null;
    
    /**
    * 日程类型  
    */
    public ushort type = 0;
    
    /**
    * 时间  
    */
    public string time = "";
    
}



