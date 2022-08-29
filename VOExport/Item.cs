
using System.Collections.Generic;

public partial class ItemVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 名字  
    */
    public string name = "";
    
    /**
    * icon  
    */
    public string icon = "";
    
    /**
    * 描述  
    */
    public string desc = "";
    
    /**
    * 稀有度  
    */
    public ushort rare = 0;
    
    /**
    * 最大堆叠数量  
    */
    public uint stack = 0;
    
    /**
    * 排序优先级  
    */
    public uint order = 0;
    
    /**
    * 跳转界面  
    */
    public string userinterface = "";
    
    /**
    * 额外属性  
    */
    public Proitem proitem = null;
    
}



