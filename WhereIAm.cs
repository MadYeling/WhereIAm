using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace WhereIAm
{
    public class WhereIAm : Mod
    {
        public static bool hasLeveled = false;
        private string biome = "";
        private string time = "";
        private string position = "";
        private string fps = "";

        //For UI
        internal static ModHotKey openUI;
        internal FunctionCheck functionCheck;
        public UserInterface userInterface;
        internal static WhereIAm instance;

        public override void Load()
        {
            instance = this;
            openUI = RegisterHotKey("功能开关", "Z");

            //UI
            if (!Main.dedServ)
            {
                userInterface = new UserInterface();
                functionCheck = new FunctionCheck();
                FunctionCheck.visible = false;
                userInterface.SetState(functionCheck);
            }

            SetTranslation();
            //Title();
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (!Main.gameMenu && Main.player[Main.myPlayer].active)
            {
                biome = CheckBiome();
                time = CheckTime();
                position = "X: " + Main.player[Main.myPlayer].position.X + " Y: " + Main.player[Main.myPlayer].position.Y;
                fps = "FPS:" + Main.frameRate;

                CheckSet();
            }


            //Draw UI
            if (FunctionCheck.visible)
            {
                userInterface.Update(Main._drawInterfaceGameTime);
                functionCheck.Draw(Main.spriteBatch);
            }
        }

        internal void CheckSet()
        {
            int high = 30;
            if (FunctionCheck.biome)
            {
                Utils.DrawBorderString(Main.spriteBatch, biome, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(biome).X * 0.75f) - 2, high), Color.White, 1.5f);
                high += 30;
            }

            if (FunctionCheck.timer)
            {
                Utils.DrawBorderString(Main.spriteBatch, time, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(time).X * 0.75f) - 2, high), Color.White, 1.5f);
                high += 30;
            }

            if (FunctionCheck.playerPos)
            {
                Utils.DrawBorderString(Main.spriteBatch, position, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(position).X * 0.75f) - 2, high), Color.White, 1.5f);
            }

            if (FunctionCheck.showFPS)
            {
                Utils.DrawBorderString(Main.spriteBatch, fps, new Vector2(20, Main.screenHeight - 30), Color.White, 1.5f);
            }
        }

        /*
        //change title
        internal void Title()
        {
            string[] title = new string[]
            {
                "01",
                "02"
            };
            if (!Main.dedServ)
            {
                Main.instance.Window.Title = title[new Random().Next(title.Length)];
            }
        }
        */

        internal string CheckTime()
        {
            if (Main.dayTime)
            {
                return Language.GetTextValue("Mods.WhereIAm.day");
            }
            return Language.GetTextValue("Mods.WhereIAm.night");
        }

        internal string CheckBiome()
        {
            //一些关于玩家位置的判断
            //每个方块大小为16像素
            //玩家坐标以像素为单位

            //玩家是否在地底 (以bgm变化为标准, 有误差)
            bool isUnderground = Main.player[Main.myPlayer].position.Y > Main.worldSurface * 16.0 + Main.screenHeight / 2;
            bool isUndergroundWhereever = Main.player[Main.myPlayer].position.Y > Main.worldSurface * 16.0;

            //玩家中心的背景墙是否为丛林神庙砖墙
            bool isTemple = Main.tile[(int)(Main.player[Main.myPlayer].Center.X / 16f), (int)(Main.player[Main.myPlayer].Center.Y / 16f)].wall == WallID.LihzahrdBrickUnsafe;

            //玩家是否在地下岩石洞穴层
            bool isRockLayer = (double)Main.player[Main.myPlayer].position.Y > Main.rockLayer * 16.0;

            //not finished yet
            bool isSky = false;


            //我知道这段写的很烂, 我也想改啊但是改不来啊!
            //判断的顺序很重要, 一些小环境要放在大环境前面, 这样可以覆盖大环境
            if (Main.player[Main.myPlayer].ZoneTowerNebula)
            {
                return Language.GetTextValue("Mods.WhereIAm.TowerNebula");
            }

            if (Main.player[Main.myPlayer].ZoneTowerSolar)
            {
                return Language.GetTextValue("Mods.WhereIAm.TowerSolar");
            }

            if (Main.player[Main.myPlayer].ZoneTowerStardust)
            {
                return Language.GetTextValue("Mods.WhereIAm.TowerStardust");
            }

            if (Main.player[Main.myPlayer].ZoneTowerVortex)
            {
                return Language.GetTextValue("Mods.WhereIAm.TowerVortex");
            }

            if (Main.player[Main.myPlayer].ZoneMeteor)
            {
                return Language.GetTextValue("Mods.WhereIAm.meteor");
            }

            if (Main.player[Main.myPlayer].ZoneGlowshroom)
            {
                return Language.GetTextValue("Mods.WhereIAm.glowshroom");
            }

            if (Main.player[Main.myPlayer].ZoneCrimson)
            {
                if (isUnderground && Main.hardMode)
                {
                    return Language.GetTextValue("Mods.WhereIAm.underCrimson");
                }
                else
                {
                    return Language.GetTextValue("Mods.WhereIAm.crimson");
                }
            }

            if (Main.player[Main.myPlayer].ZoneCorrupt)
            {
                if (isUnderground && Main.hardMode)
                {
                    return Language.GetTextValue("Mods.WhereIAm.underCorrupt");
                }
                else
                {
                    return Language.GetTextValue("Mods.WhereIAm.corrupt");
                }
            }

            if (Main.player[Main.myPlayer].ZoneHoly)
            {
                if (isUnderground)
                {
                    if (isRockLayer)
                    {
                        return Language.GetTextValue("Mods.WhereIAm.hallowedCavern");
                    }
                    return Language.GetTextValue("Mods.WhereIAm.underHallow");
                }
                else
                {
                    if (Main.sandTiles > 1000)
                    {
                        return Language.GetTextValue("Mods.WhereIAm.hallowedDesert");
                    }
                    return Language.GetTextValue("Mods.WhereIAm.hallow");
                }
            }

            if (Main.player[Main.myPlayer].ZoneBeach)
            {
                return Language.GetTextValue("Mods.WhereIAm.ocean");
            }

            if (Main.player[Main.myPlayer].ZoneSnow)
            {
                if (isUnderground)
                {
                    return Language.GetTextValue("Mods.WhereIAm.underSnow");
                }
                return Language.GetTextValue("Mods.WhereIAm.snow");
            }

            if (Main.player[Main.myPlayer].ZoneDesert)
            {
                if (isUnderground)
                {
                    return Language.GetTextValue("Mods.WhereIAm.underDesert");
                }
                return Language.GetTextValue("Mods.WhereIAm.desert");
            }

            if (Main.player[Main.myPlayer].ZoneDungeon)
            {
                return Language.GetTextValue("Mods.WhereIAm.dungeon");
            }

            if (Main.player[Main.myPlayer].ZoneJungle)
            {
                if (isUnderground)
                {
                    if (isTemple)
                    {
                        return Language.GetTextValue("Mods.WhereIAm.temple");
                    }
                    return Language.GetTextValue("Mods.WhereIAm.underJungle");
                }
                return Language.GetTextValue("Mods.WhereIAm.jungle");
            }

            if (Main.player[Main.myPlayer].ZoneUnderworldHeight)
            {
                return Language.GetTextValue("Mods.WhereIAm.underworld");
            }

            if (isRockLayer)
            {
                return Language.GetTextValue("Mods.WhereIAm.cavern");
            }

            if (Main.player[Main.myPlayer].ZoneSkyHeight)
            {
                return Language.GetTextValue("Mods.WhereIAm.space"); ;
            }

            //Fuck, that's not forest
            if (Main.player[Main.myPlayer].ZoneOverworldHeight)
            {
                return Language.GetTextValue("Mods.WhereIAm.forest");
            }

            if (isUndergroundWhereever)
            {
                return Language.GetTextValue("Mods.WhereIAm.underground");
            }

            if (isSky)
            {
                return Language.GetTextValue("Mods.WhereIAm.sky");
            }

            return Language.GetTextValue("Mods.WhereIAm.unknown");
        }

        public void SetTranslation()
        {
            ModTranslation text = CreateTranslation("day");
            text.SetDefault("Day");
            text.AddTranslation(GameCulture.Chinese, "白天");
            AddTranslation(text);

            text = CreateTranslation("night");
            text.SetDefault("Night");
            text.AddTranslation(GameCulture.Chinese, "夜晚");
            AddTranslation(text);

            text = CreateTranslation("TowerNebula");
            text.SetDefault("Nebula Pillar");
            text.AddTranslation(GameCulture.Chinese, "星云柱");
            AddTranslation(text);

            text = CreateTranslation("TowerSolar");
            text.SetDefault("Solar Pillar");
            text.AddTranslation(GameCulture.Chinese, "日耀柱");
            AddTranslation(text);

            text = CreateTranslation("TowerStardust");
            text.SetDefault("Stardust Pillar");
            text.AddTranslation(GameCulture.Chinese, "星尘柱");
            AddTranslation(text);

            text = CreateTranslation("TowerVortex");
            text.SetDefault("Vortex Pillar");
            text.AddTranslation(GameCulture.Chinese, "星璇柱");
            AddTranslation(text);

            text = CreateTranslation("meteor");
            text.SetDefault("Meteor");
            text.AddTranslation(GameCulture.Chinese, "陨石");
            AddTranslation(text);

            text = CreateTranslation("glowshroom");
            text.SetDefault("Glowshroom");
            text.AddTranslation(GameCulture.Chinese, "发光蘑菇地");
            AddTranslation(text);

            text = CreateTranslation("underCrimson");
            text.SetDefault("Underground Crimson");
            text.AddTranslation(GameCulture.Chinese, "血腥之地 (地下)");
            AddTranslation(text);

            text = CreateTranslation("crimson");
            text.SetDefault("Crimson");
            text.AddTranslation(GameCulture.Chinese, "血腥之地");
            AddTranslation(text);

            text = CreateTranslation("underCorrupt");
            text.SetDefault("Underground Corruption");
            text.AddTranslation(GameCulture.Chinese, "腐化之地 (地下)");
            AddTranslation(text);

            text = CreateTranslation("corrupt");
            text.SetDefault("Corruption");
            text.AddTranslation(GameCulture.Chinese, "腐化之地");
            AddTranslation(text);

            text = CreateTranslation("hallowedCavern");
            text.SetDefault("Hallowed Cavern");
            text.AddTranslation(GameCulture.Chinese, "神圣之地 (岩石洞穴层)");
            AddTranslation(text);

            text = CreateTranslation("underHallow");
            text.SetDefault("Underground Hallow");
            text.AddTranslation(GameCulture.Chinese, "神圣之地 (地下)");
            AddTranslation(text);

            text = CreateTranslation("hallowedDesert");
            text.SetDefault("Hallowed Desert");
            text.AddTranslation(GameCulture.Chinese, "沙漠 (神圣化)");
            AddTranslation(text);

            text = CreateTranslation("hallow");
            text.SetDefault("Hallow");
            text.AddTranslation(GameCulture.Chinese, "神圣之地");
            AddTranslation(text);

            text = CreateTranslation("ocean");
            text.SetDefault("Ocean");
            text.AddTranslation(GameCulture.Chinese, "海洋");
            AddTranslation(text);

            text = CreateTranslation("underSnow");
            text.SetDefault("Underground Ice");
            text.AddTranslation(GameCulture.Chinese, "地下冰原");
            AddTranslation(text);

            text = CreateTranslation("snow");
            text.SetDefault("Snow");
            text.AddTranslation(GameCulture.Chinese, "雪地");
            AddTranslation(text);

            text = CreateTranslation("underDesert");
            text.SetDefault("Underground Desert");
            text.AddTranslation(GameCulture.Chinese, "沙漠 (地下)");
            AddTranslation(text);

            text = CreateTranslation("desert");
            text.SetDefault("Desert");
            text.AddTranslation(GameCulture.Chinese, "沙漠");
            AddTranslation(text);

            text = CreateTranslation("dungeon");
            text.SetDefault("Dungeon");
            text.AddTranslation(GameCulture.Chinese, "地牢");
            AddTranslation(text);

            text = CreateTranslation("temple");
            text.SetDefault("Lihzahrd Temple");
            text.AddTranslation(GameCulture.Chinese, "丛林神庙");
            AddTranslation(text);

            text = CreateTranslation("underJungle");
            text.SetDefault("Underground Jungle");
            text.AddTranslation(GameCulture.Chinese, "丛林 (地下)");
            AddTranslation(text);

            text = CreateTranslation("jungle");
            text.SetDefault("Jungle");
            text.AddTranslation(GameCulture.Chinese, "丛林");
            AddTranslation(text);

            text = CreateTranslation("underworld");
            text.SetDefault("Underworld");
            text.AddTranslation(GameCulture.Chinese, "地狱");
            AddTranslation(text);

            text = CreateTranslation("cavern");
            text.SetDefault("Cavern");
            text.AddTranslation(GameCulture.Chinese, "岩石洞穴");
            AddTranslation(text);

            text = CreateTranslation("space");
            text.SetDefault("Space");
            text.AddTranslation(GameCulture.Chinese, "太空");
            AddTranslation(text);

            text = CreateTranslation("forest");
            text.SetDefault("Forest");
            text.AddTranslation(GameCulture.Chinese, "森林");
            AddTranslation(text);

            text = CreateTranslation("underground");
            text.SetDefault("Underground");
            text.AddTranslation(GameCulture.Chinese, "地下");
            AddTranslation(text);

            text = CreateTranslation("sky");
            text.SetDefault("Sky");
            text.AddTranslation(GameCulture.Chinese, "天空");
            AddTranslation(text);

            text = CreateTranslation("unknown");
            text.SetDefault("Unknown");
            text.AddTranslation(GameCulture.Chinese, "未知");
            AddTranslation(text);

            //Has a bug, requires to reload
            //text = CreateTranslation("functionToggles");
            //text.SetDefault("Function Toggles");
            //text.AddTranslation(GameCulture.Chinese, "功能开关");
            //AddTranslation(text);

            //Has a bug, can not be translated
            //text = CreateTranslation("displayBiome");
            //text.SetDefault("Display Biome");
            //text.AddTranslation(GameCulture.Chinese, "显示环境");
            //AddTranslation(text);

            //text = CreateTranslation("displayTime");
            //text.SetDefault("Display Time");
            //text.AddTranslation(GameCulture.Chinese, "显示时间");
            //AddTranslation(text);

            //text = CreateTranslation("displayPosition");
            //text.SetDefault("Display Position");
            //text.AddTranslation(GameCulture.Chinese, "显示坐标");
            //AddTranslation(text);

            //text = CreateTranslation("displayFPS");
            //text.SetDefault("Display FPS");
            //text.AddTranslation(GameCulture.Chinese, "显示帧数");
            //AddTranslation(text);
        }
    }
}
