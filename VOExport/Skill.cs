
using System.Collections.Generic;

public partial class SkillVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 类型  
    */
    public string tag = "";
    
    /**
    * 技能名  
    */
    public string name = "";
    
    /**
    * 等级  
    */
    public ushort level = 0;
    
    /**
    * 图标  
    */
    public string icon = "";
    
    /**
    * 描述  
    */
    public string desc = "";
    
    /**
    * 动作  
    */
    public string anim = "";
    
    public List<SkillChildVO> childs = null;
    
}



public partial class SkillChildVO {
    /**
    * ID  
    */
    public ushort id = 0;
    
    /**
    * 唯一id  
    */
    public uint uid = 0;
    
    /**
    * 技能持续时间  
    */
    public uint time = 0;
    
    /**
    * 自身buff  
    */
    public ushort selfBuff = 0;
    
    /**
    * 目标buff  
    */
    public ushort targetBuff = 0;
    
    /**
    * 筛选目标  
    */
    public object profilter = null;
    
    /**
    * 属性  
    */
    public object proskill = null;
    
}



