
using System.Collections.Generic;

public class HeroenhanceVO {
    /**
    * 洗练等级  
    */
    public ushort id = 0;
    
    /**
    * 洗练度经验  
    */
    public ushort enhanceNum = 0;
    
    /**
    * 需要资源  
    */
    public List<O1LimitVO> require = null;
    
    /**
    * 点一次需要的钻石或者WIN  
    */
    public uint heo = 0;
    
    /**
    * 属性条目最小值  
    */
    public ushort min = 0;
    
    /**
    * 属性条目最大值  
    */
    public ushort max = 0;
    
    /**
    * 属性  
    */
    public Pro pro = null;
    
}



