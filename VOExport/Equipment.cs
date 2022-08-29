
using System.Collections.Generic;

public partial class EquipmentVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 部位  
    */
    public string part = "";
    
    /**
    * 名字  
    */
    public string name = "";
    
    /**
    * 描述  
    */
    public string desc = "";
    
    /**
    * icon  
    */
    public string icon = "";
    
    /**
    * 模型  
    */
    public string prefab = "";
    
    /**
    * 稀有度  
    */
    public uint rare = 0;
    
    /**
    * 武器类型  
    */
    public uint weapontype = 0;
    
    /**
    * 武器普攻  
    */
    public uint attackskill = 0;
    
    /**
    * tag克制属性  
    */
    public Protag protag = null;
    
    /**
    * 属性  
    */
    public Pro pro = null;
    
}



