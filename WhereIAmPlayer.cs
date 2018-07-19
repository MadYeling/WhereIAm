using Terraria.GameInput;
using Terraria.ModLoader;

namespace WhereIAm
{
    public class WhereIAmPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (WhereIAm.openUI.JustPressed)
            {
                FunctionCheck.visible = !FunctionCheck.visible;
            }
        }

    }
}
