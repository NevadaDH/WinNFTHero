
using System.Collections.Generic;

public class EquipenhanceVO {
    /**
    * 升星等级  
    */
    public ushort id = 0;
    
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
    * 升2阶的几率  
    */
    public float probability = 0;
    
    /**
    * 属性  
    */
    public Pro pro = null;
    
}



