
using System.Collections.Generic;

public partial class HeroVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 稀有度  
    */
    public ushort rare = 0;
    
    /**
    * 英雄名  
    */
    public string name = "";
    
    /**
    * 性格1  
    */
    public uint character1 = 0;
    
    /**
    * 性格2  
    */
    public uint character2 = 0;
    
    /**
    * 职业  
    */
    public uint job = 0;
    
    /**
    * 移动  
    */
    public uint move = 0;
    
    /**
    * 普攻  
    */
    public uint attack = 0;
    
    /**
    * 技能  
    */
    public uint skill = 0;
    
    /**
    * 属性  
    */
    public Pro pro = null;
    
    /**
    * 属性2  
    */
    public Pro pro2 = null;
    
    /**
    * 上阵属性  
    */
    public Protag protag = null;
    
    /**
    * 外观属性  
    */
    public object style = null;
    
}



