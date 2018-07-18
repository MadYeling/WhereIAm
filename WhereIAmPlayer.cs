using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace WhereIAm
{
    public class WhereIAmPlayer : ModPlayer
    {
        public override void OnEnterWorld(Player player)
        {
            if (WhereIAm.hasLeveled)
            {
                string check = Language.GetTextValue("Mods.WhereIAm.check");
                Main.NewText("检测到Leveled, Where I Am功能已自动关闭", Color.Cyan);
            }
        }
    }
}
