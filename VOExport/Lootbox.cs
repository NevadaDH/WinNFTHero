
using System.Collections.Generic;

public partial class LootboxVO {
    /**
    * 盲盒id  
    */
    public uint id = 0;
    
    /**
    * 类型  
    */
    public uint type = 0;
    
    /**
    * icon  
    */
    public string icon = "";
    
    /**
    * 描述  
    */
    public string desc = "";
    
    /**
    * 内容  
    */
    public List<O1LimitVO> exchangeNum = null;
    
}



