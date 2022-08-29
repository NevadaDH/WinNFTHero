
using System.Collections.Generic;
using Mao;

public partial class ClientData
{
    public List<EquipItem> GetHeroEquips(int heroId)
    {
        List<EquipItem> ret = new List<EquipItem>();
        foreach (var item in this.equip.id)
        {
            EquipItem equip = item.Value;
            if (equip.hero == heroId)
            {
                ret.Add(equip);
            }
        }
        return ret;
    }

    public List<EquipItem> GetHeroEquips(int heroId, string partFilter)
    {
        List<EquipItem> ret = GetHeroEquips(heroId);
        foreach (var equip in ret)
        {
            EquipmentVO vo;
            if (!VOManager.TryGetVO<EquipmentVO>(equip.id, out vo))
            {
                continue;
            }

            if (vo.part.Equals(partFilter))
            {
                ret.Add(equip);
            }
        }
        return ret;
    }

    public AvatarManager.Avatar GetAvatar(int heroId){
        AvatarManager.Avatar avatar = new AvatarManager.Avatar();
        HeroVO heroVO;
        if(VOManager.TryGetVO<HeroVO>(heroId,out heroVO)){
            HeroStyle style = heroVO.GetHeroStyle();
            avatar.TopHead = "Avatar/TopHead/TopHead_"+style.hair.ToString("D2");
            avatar.Top = "Avatar/Top/Top_"+style.shirt.ToString("D2"); 
            avatar.Bottom = "Avatar/Bottom/Bottom_01";
            avatar.Hand = "Avatar/Hand/Hand_01";
            avatar.Foot = "Avatar/Foot/Foot_"+style.socks.ToString("D2"); 
            avatar.Face = "Avatar/Face/Face_"+style.face.ToString("D2"); 
            avatar.Eyebrow = "Avatar/Eye/Eye_"+style.brow.ToString("D2"); 
        }else{

            avatar.Top = "Avatar/Top/Top_01";
            avatar.TopHead = "Avatar/TopHead/TopHead_05";
            avatar.Bottom = "Avatar/Bottom/Bottom_01";
            avatar.Hand = "Avatar/Hand/Hand_01";
            avatar.Foot = "Avatar/Foot/Foot_01";
        }
        
        return avatar;
    }

    public AvatarManager.Equipments GetAvatarEquipments(int heroId)
    {
        AvatarManager.Equipments ret = new AvatarManager.Equipments();
        List<EquipItem> equips = GetHeroEquips(heroId);
        foreach (var equip in equips)
        {
            EquipmentVO vo;
            if (!VOManager.TryGetVO<EquipmentVO>(equip.id, out vo))
            {
                continue;
            }
            EQUIP_TYPE type = (EQUIP_TYPE)int.Parse(vo.part);
            switch (type)
            {
                case EQUIP_TYPE.BOTTOM:
                    ret.Bottom = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.CAP:
                    ret.Cap = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.GLASSES:
                    ret.Glasses = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.GLOVES:
                    ret.Gloves = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.ORNAMENT:
                    ret.Ornament = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.SHOES:
                    ret.Shoes = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.UPPER:
                    ret.Upper = "Avatar/"+vo.prefab;
                    break;
                case EQUIP_TYPE.WEAPON:
                    ret.Weapon = "Avatar/"+vo.prefab;
                    break;
            }

        }
        return ret;
    }

    public EquipItem GetWeapon(int heroId)
    {
        List<EquipItem> ret = GetHeroEquips(heroId, "4");
        return ret.Count > 0 ? ret[0] : null;
    }

}