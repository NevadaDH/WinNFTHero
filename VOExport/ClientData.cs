
using System.Collections.Generic;

public partial class HeroData:ProxyDataBase {
    public Dictionary<string, HeroItem> id = null;
    
}



public partial class HeroItem:ProxyDataBase {
    public ushort id = 0;
    
    public uint exp = 0;
    
    public uint exp_cnt = 0;
    
    public uint critical_cnt = 0;
    
    public ushort lv = 0;
    
    public ushort baptize_level = 0;
    
    public ushort baptize = 0;
    
    public Pro pro = null;
    
    public Pro proEnhance = null;
    
    public ushort state = 0;
    
    public ushort mineId = 0;
    
    public short pos = 0;
    
    public short pos_rank_atk = 0;
    
    public short pos_rank_def = 0;
    
    public short pos_match_atk = 0;
    
    public short pos_match_def = 0;
    
}



public partial class ChapterData:ProxyDataBase {
    public Dictionary<string, ChapterItem> id = null;
    
    public ushort chapter = 0;
    
}



public partial class ChapterItem:ProxyDataBase {
    public ushort id = 0;
    
    public ushort pass = 0;
    
}



public partial class MailData:ProxyDataBase {
    public Dictionary<string, MailItem> id = null;
    
}



public partial class MailItem:ProxyDataBase {
    public string guid = "";
    
    public ushort id = 0;
    
    public string title = "";
    
    public string type = "";
    
    public string content = "";
    
    public bool isGet = false;
    
    public bool isRead = false;
    
    public object param = null;
    
    public string time = "";
    
    public ushort sender = 0;
    
}



public partial class EquipData:ProxyDataBase {
    public Dictionary<string, EquipItem> id = null;
    
}



public partial class EquipItem:ProxyDataBase {
    public uint id = 0;
    
    public uint hero = 0;
    
    public Pro proEquip = null;
    
    public ushort level = 0;
    
    public uint enhance_cnt = 0;
    
    public uint critical_cnt = 0;
    
}



public partial class PvpRankData:ProxyDataBase {
    public uint rank = 0;
    
    public List<uint> match_list = null;
    
    public Dictionary<string, RankRecordItem> record_list = null;
    
}



public partial class RankHeroItem:ProxyDataBase {
    public uint id = 0;
    
    public uint lv = 0;
    
    public uint pos = 0;
    
    public List<uint> equip_list = null;
    
}



public partial class RankRecordItem:ProxyDataBase {
    public string guid = "";
    
    public string roleId = "";
    
    public string name = "";
    
    public uint face = 0;
    
    public uint fv = 0;
    
    public bool win = false;
    
    public bool attack = false;
    
    public string time = "";
    
    public int rank = 0;
    
}



public partial class MineData:ProxyDataBase {
    public Dictionary<string, MineItem> id = null;
    
}



public partial class MineItem:ProxyDataBase {
    public ushort id = 0;
    
    public List<HeroItem> heroList = null;
    
    public uint surplus = 0;
    
    public uint contribution = 0;
    
    public uint degree = 0;
    
}



public partial class TaskData:ProxyDataBase {
    public Dictionary<string, TaskItem> id = null;
    
    public ushort dailycount = 0;
    
    public ushort weekcount = 0;
    
    public bool giftState = false;
    
}



public partial class TaskItem:ProxyDataBase {
    public ushort id = 0;
    
    public ushort times = 0;
    
    public ushort state = 0;
    
    public ushort type = 0;
    
}



public partial class DItemData:ProxyDataBase {
    public Dictionary<string, DItemItem> runtimes = null;
    
}



public partial class DItemItem:ProxyDataBase {
    public string guid = "";
    
    public ushort id = 0;
    
    public Proitem pro = null;
    
    public uint count = 0;
    
}



public partial class ClientData {
    public string name = "";
    
    public uint icon = 0;
    
    public Res res = null;
    
    public HeroData hero = null;
    
    public ChapterData chapter = null;
    
    public MailData mail = null;
    
    public EquipData equip = null;
    
    public DItemData item = null;
    
    public TaskData task = null;
    
    public Day day = null;
    
    public Week week = null;
    
    public PvpRankData pvprank = null;
    
    public MineData mine = null;
    
}



