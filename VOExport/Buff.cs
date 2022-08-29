
using System.Collections.Generic;

public class BuffVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 类型  
    */
    public string tag = "";
    
    /**
    * buff名字  
    */
    public string name = "";
    
    /**
    * 描述  
    */
    public string desc = "";
    
    /**
    * 叠加层数  
    */
    public ushort stack = 0;
    
    /**
    * buff持续时间  
    */
    public uint time = 0;
    
    /**
    * 特效  
    */
    public string effect = "";
    
    /**
    * 生命周期[命中]  
    */
    public List<string> hit = null;
    
    /**
    * 生效间隔时间  
    */
    public uint interval = 0;
    
    /**
    * 生命周期[持续]  
    */
    public List<string> tick = null;
    
    /**
    * 生命周期[结束]  
    */
    public List<string> end = null;
    
}



