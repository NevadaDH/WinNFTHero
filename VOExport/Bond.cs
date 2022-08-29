
using System.Collections.Generic;

public class BondVO {
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
    * 显示条件  
    */
    public List<O1LimitVO> showCondition = null;
    
    public List<BondChildVO> childs = null;
    
}



public class BondChildVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 级别  
    */
    public string part = "";
    
    /**
    * UID  
    */
    public ushort uid = 0;
    
    /**
    * 激活条件  
    */
    public List<O1LimitVO> condition = null;
    
    /**
    * 添加buff条件  
    */
    public Probuff buff = null;
    
    /**
    * 修改属性  
    */
    public Pro pro = null;
    
    /**
    * 修改属性  
    */
    public Protag protag = null;
    
}



