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
                Main.NewText(check, Color.Cyan);
            }
        }
    }
}
