
using System.Collections.Generic;

public class HerolevelVO {
    /**
    * 等级  
    */
    public ushort id = 0;
    
    /**
    * 需要经验  
    */
    public uint exp = 0;
    
    /**
    * 需要资源  
    */
    public List<O1LimitVO> require = null;
    
    /**
    * 点一次需要的钻石或者WIN  
    */
    public uint heo = 0;
    
    /**
    * 最小次数  
    */
    public ushort min = 0;
    
    /**
    * 最大次数  
    */
    public ushort max = 0;
    
    /**
    * 暴击几率  
    */
    public float probability = 0;
    
    /**
    * 属性  
    */
    public Pro pro = null;
    
}



