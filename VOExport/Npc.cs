
using System.Collections.Generic;

public partial class NpcVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 建筑名称  
    */
    public string name = "";
    
    /**
    * 跳转界面  
    */
    public string user_interface = "";
    
    /**
    * 旋转角度  
    */
    public float direction = 0;
    
    /**
    * 是否改变朝向  
    */
    public ushort change = 0;
    
    /**
    * 待机动作  
    */
    public string idle_motion = "";
    
}



