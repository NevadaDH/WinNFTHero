
using System.Collections.Generic;

public partial class ModuleVO {
    /**
    * 模块  
    */
    public string module = "";
    
    public List<ModuleChildVO> childs = null;
    
}



public partial class ModuleChildVO {
    /**
    * 模块  
    */
    public string module = "";
    
    /**
    * 属性  
    */
    public string id = "";
    
    /**
    * 类型  
    */
    public string type = "";
    
    /**
    * 属性名  
    */
    public string name = "";
    
    /**
    * center同步数据维护  
    */
    public uint center = 0;
    
    /**
    * 效果  
    */
    public List<List<object>> effect = null;
    
    /**
    * 战力计算权重  
    */
    public float fv = 0;
    
    /**
    * 属性名  
    */
    public object value = null;
    
    /**
    * icon  
    */
    public string icon = "";
    
}



