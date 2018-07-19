using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace WhereIAm
{
    public class WhereIAm : Mod
    {
        public static bool hasLeveled = false;
        private string biome = "UNKNOWN ERR";
        private string time = "UNKNOWN ERR";
        private string position = "X: " + Main.player[Main.myPlayer].position.X + " Y: " + Main.player[Main.myPlayer].position.Y;
        private string fps = "FPS:" + Main.frameRate;

        public override void Load()
        {
            if (ModLoader.GetLoadedMods().Contains("Leveled"))
            {
                hasLeveled = true;
            }
            SetTranslation();
            //Title();
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            if (!hasLeveled)
            {
                if (!Main.gameMenu && Main.player[Main.myPlayer].active)
                {
                    biome = CheckBiome();
                    time = CheckTime();

                    Utils.DrawBorderString(Main.spriteBatch, biome, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(biome).X * 0.75f) - 2, 30), Color.White, 1.5f);
                    Utils.DrawBorderString(Main.spriteBatch, time, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(time).X * 0.75f) - 2, 60), Color.White, 1.5f);
                    Utils.DrawBorderString(Main.spriteBatch, position, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(position).X * 0.75f) - 2, 90), Color.White, 1.5f);

                    //Debug Messages
                    ///*
                    //string test2 = "a: ";
                    //Main.spriteBatch.DrawString(Main.fontMouseText, test2, new Vector2(Main.screenWidth / 2 - (int)(Main.fontMouseText.MeasureString(test2).X * 0.75f) - 2, 120), new Color.White, 0f, new Vector2(0f, 0f), 1.5f, SpriteEffects.None, 1f);
                    //*/
                }
            }
            if (!Main.gameMenu && Main.player[Main.myPlayer].active)
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
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "白天");

            text = CreateTranslation("night");
            text.SetDefault("Night");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "夜晚");

            text = CreateTranslation("TowerNebula");
            text.SetDefault("Nebula Pillar");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "星云柱");

            text = CreateTranslation("TowerSolar");
            text.SetDefault("Solar Pillar");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "日耀柱");

            text = CreateTranslation("TowerStardust");
            text.SetDefault("Stardust Pillar");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "星尘柱");

            text = CreateTranslation("TowerVortex");
            text.SetDefault("Vortex Pillar");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "星璇柱");

            text = CreateTranslation("meteor");
            text.SetDefault("Meteor");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "陨石");

            text = CreateTranslation("glowshroom");
            text.SetDefault("Glowshroom");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "发光蘑菇地");

            text = CreateTranslation("underCrimson");
            text.SetDefault("Underground Crimson");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "血腥之地 (地下)");

            text = CreateTranslation("crimson");
            text.SetDefault("Crimson");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "血腥之地");

            text = CreateTranslation("underCorrupt");
            text.SetDefault("Underground Corruption");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "腐化之地 (地下)");

            text = CreateTranslation("corrupt");
            text.SetDefault("Corruption");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "腐化之地");

            text = CreateTranslation("hallowedCavern");
            text.SetDefault("Hallowed Cavern");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "神圣之地 (岩石洞穴层)");

            text = CreateTranslation("underHallow");
            text.SetDefault("Underground Hallow");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "神圣之地 (地下)");

            text = CreateTranslation("hallowedDesert");
            text.SetDefault("Hallowed Desert");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "沙漠 (神圣化)");

            text = CreateTranslation("hallow");
            text.SetDefault("Hallow");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "神圣之地");

            text = CreateTranslation("ocean");
            text.SetDefault("Ocean");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "海洋");

            text = CreateTranslation("underSnow");
            text.SetDefault("Underground Ice");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "地下冰原");

            text = CreateTranslation("snow");
            text.SetDefault("Snow");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "雪地");

            text = CreateTranslation("underDesert");
            text.SetDefault("Underground Desert");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "沙漠 (地下)");

            text = CreateTranslation("desert");
            text.SetDefault("Desert");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "沙漠");

            text = CreateTranslation("dungeon");
            text.SetDefault("Dungeon");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "地牢");

            text = CreateTranslation("temple");
            text.SetDefault("Lihzahrd Temple");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "丛林神庙");

            text = CreateTranslation("underJungle");
            text.SetDefault("Underground Jungle");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "丛林 (地下)");

            text = CreateTranslation("jungle");
            text.SetDefault("Jungle");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "丛林");

            text = CreateTranslation("underworld");
            text.SetDefault("Underworld");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "地狱");

            text = CreateTranslation("cavern");
            text.SetDefault("Cavern");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "岩石洞穴");

            text = CreateTranslation("space");
            text.SetDefault("Space");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "太空");

            text = CreateTranslation("forest");
            text.SetDefault("Forest");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "森林");

            text = CreateTranslation("underground");
            text.SetDefault("Underground");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "地下");

            text = CreateTranslation("sky");
            text.SetDefault("Sky");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "天空");

            text = CreateTranslation("unknown");
            text.SetDefault("Unknown");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "未知");

            text = CreateTranslation("check");
            text.SetDefault("Detected Leveled, Part of the function of Where I Am has been automatically disabled");
            AddTranslation(text);
            text.AddTranslation(GameCulture.Chinese, "检测到Leveled, Where I Am部分功能已自动关闭");
        }
    }
}
