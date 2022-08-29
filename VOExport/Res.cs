
using System.Collections.Generic;

public partial class Pro:ProxyDataBase {
    /**
    * 力量
    */
    public uint str = 0;
    
    /**
    * 体质
    */
    public uint vit = 0;
    
    /**
    * 敏捷
    */
    public uint dex = 0;
    
    /**
    * 智力
    */
    public uint wis = 0;
    
    /**
    * 幸运
    */
    public uint luk = 0;
    
    /**
    * 魅力
    */
    public uint cha = 0;
    
    /**
    * 生命值
    */
    public float hp = 0;
    
    /**
    * 物理攻击力
    */
    public float atk = 0;
    
    /**
    * 物理防御
    */
    public float def = 0;
    
    /**
    * 魔法攻击力
    */
    public float mAtk = 0;
    
    /**
    * 魔防
    */
    public float mDef = 0;
    
    /**
    * 速度
    */
    public float speed = 0;
    
    /**
    * 暴击率
    */
    public float crit = 0;
    
    /**
    * 暴击伤害
    */
    public float critHurt = 0;
    
    /**
    * 能量初始值
    */
    public ushort power = 0;
    
    /**
    * 能量上限
    */
    public int powerMax = 0;
    
    /**
    * 能量回复速度
    */
    public float powerSpeed = 0;
    
    /**
    * 命中
    */
    public float hit = 0;
    
    /**
    * 闪避
    */
    public float dodge = 0;
    
    /**
    * 增伤
    */
    public float damageUp = 0;
    
    /**
    * 减伤
    */
    public float damageDown = 0;
    
    /**
    * 算力
    */
    public float hashRate = 0;
    
    /**
    * 护盾
    */
    public float hudun = 0;
    
    /**
    * 性格
    */
    public uint character = 0;
    
    /**
    * 职业
    */
    public uint job = 0;
    
}



public partial class Proskill:ProxyDataBase {
    /**
    * 目标数量
    */
    public ushort targetNum = 0;
    
    /**
    * 附加真实伤害
    */
    public ushort additionRealDamage = 0;
    
    /**
    * 攻击次数
    */
    public ushort damageCount = 0;
    
    /**
    * 物攻系数
    */
    public float phyFactor = 0;
    
    /**
    * 魔攻系数
    */
    public float magFactor = 0;
    
}



public partial class Res:ProxyDataBase {
    /**
    * 名字
    */
    public string name = "";
    
    /**
    * 头像
    */
    public uint face = 0;
    
    /**
    * 玩家体力
    */
    public uint energy = 0;
    
    /**
    * 玩家体力上限
    */
    public uint energyMax = 0;
    
    /**
    * 体力回复数量
    */
    public ushort energyRecoveryNum = 0;
    
    /**
    * 体力回复时间(分)
    */
    public uint energyRecoveryTime = 0;
    
    /**
    * 下一次体力恢复的时间
    */
    public double nextEnergyTime = 0;
    
    /**
    * win
    */
    public double win = 0;
    
    /**
    * 钻石
    */
    public double heo = 0;
    
    /**
    * 古钱
    */
    public double gold = 0;
    
    /**
    * 不知道有没有用
    */
    public double money = 0;
    
    /**
    * 玩家战力
    */
    public uint fv = 0;
    
}



public partial class Profilter:ProxyDataBase {
    /**
    * 距离
    */
    public ushort range = 0;
    
    /**
    * 角度
    */
    public ushort angle = 0;
    
    /**
    * 宽度
    */
    public ushort width = 0;
    
    /**
    * 1敌方2己方3自己4全体
    */
    public ushort target = 0;
    
}



public partial class Proconst:ProxyDataBase {
    /**
    * 物理防御常数
    */
    public ushort def = 0;
    
    /**
    * 魔法防御常数
    */
    public ushort mdef = 0;
    
}



public partial class Probuff:ProxyDataBase {
    /**
    * 伤害加成
    */
    public float damageAdd = 0;
    
    /**
    * 伤害减免
    */
    public float damageSub = 0;
    
    /**
    * 不能动
    */
    public ushort stun = 0;
    
    /**
    * 持续掉血
    */
    public float dot = 0;
    
    /**
    * 无敌
    */
    public ushort wudi = 0;
    
}



public partial class Protag:ProxyDataBase {
    /**
    * 性格1数量
    */
    public ushort character1 = 0;
    
    /**
    * 性格2数量
    */
    public ushort character2 = 0;
    
    /**
    * 性格3数量
    */
    public ushort character3 = 0;
    
    /**
    * 性格4数量
    */
    public ushort character4 = 0;
    
    /**
    * 性格5数量
    */
    public ushort character5 = 0;
    
    /**
    * 性格6数量
    */
    public ushort character6 = 0;
    
    /**
    * 性格7数量
    */
    public ushort character7 = 0;
    
    /**
    * 性格8数量
    */
    public ushort character8 = 0;
    
    /**
    * 性格9数量
    */
    public ushort character9 = 0;
    
    /**
    * 性格10数量
    */
    public ushort character10 = 0;
    
    /**
    * 职业1数量
    */
    public ushort job1 = 0;
    
    /**
    * 职业2数量
    */
    public ushort job2 = 0;
    
    /**
    * 职业3数量
    */
    public ushort job3 = 0;
    
    /**
    * 职业4数量
    */
    public ushort job4 = 0;
    
    /**
    * 职业5数量
    */
    public ushort job5 = 0;
    
    /**
    * 职业6数量
    */
    public ushort job6 = 0;
    
    /**
    * 职业7数量
    */
    public ushort job7 = 0;
    
    /**
    * 职业8数量
    */
    public ushort job8 = 0;
    
    /**
    * 职业9数量
    */
    public ushort job9 = 0;
    
    /**
    * 职业10数量
    */
    public ushort job10 = 0;
    
    /**
    * 性格1减伤
    */
    public float character1sub = 0;
    
    /**
    * 性格2减伤
    */
    public float character2add = 0;
    
    /**
    * 性格3减伤
    */
    public float character3add = 0;
    
    /**
    * 性格4减伤
    */
    public float character4add = 0;
    
    /**
    * 性格5减伤
    */
    public float character5add = 0;
    
    /**
    * 性格6减伤
    */
    public float character6add = 0;
    
    /**
    * 性格7减伤
    */
    public float character7add = 0;
    
    /**
    * 性格8减伤
    */
    public float character8add = 0;
    
    /**
    * 性格9减伤
    */
    public float character9add = 0;
    
    /**
    * 性格10减伤
    */
    public float character10add = 0;
    
    /**
    * 职业1加伤
    */
    public float job1add = 0;
    
    /**
    * 职业2加伤
    */
    public float job2add = 0;
    
    /**
    * 职业3加伤
    */
    public float job3add = 0;
    
    /**
    * 职业4加伤
    */
    public float job4add = 0;
    
    /**
    * 职业5加伤
    */
    public float job5add = 0;
    
    /**
    * 职业6加伤
    */
    public float job6add = 0;
    
    /**
    * 职业7加伤
    */
    public float job7add = 0;
    
    /**
    * 职业8加伤
    */
    public float job8add = 0;
    
    /**
    * 职业9加伤
    */
    public float job9add = 0;
    
    /**
    * 职业10加伤
    */
    public float job10add = 0;
    
}



public partial class Day:ProxyDataBase {
    /**
    * 每日登录
    */
    public ushort login = 0;
    
    /**
    * 每日PVE
    */
    public ushort chapterwin = 0;
    
    /**
    * 每日完成任务数量
    */
    public ushort task = 0;
    
}



public partial class Week:ProxyDataBase {
    /**
    * 每周登录
    */
    public ushort login = 0;
    
    /**
    * 每周PVE
    */
    public ushort chapterwin = 0;
    
    /**
    * 每周完成任务数量
    */
    public ushort task = 0;
    
    /**
    * 当前是今年第几周
    */
    public ushort week = 0;
    
}



public partial class Proitem:ProxyDataBase {
    /**
    * 道具额外属性
    */
    public string code = "";
    
}



