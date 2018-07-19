using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WhereIAm
{
    class FunctionCheck : UIState
    {
        public static bool visible = false;
        public UIPanel checklistPanel;
        private Vector2 offset;
        private bool dragging;
        private float panelWidth = 200f;
        private float panelHeight = 110f;
        private static Color textColor = Color.SandyBrown;

        public UICheckBox biomeBox;
        public static bool biome = true;
        public UICheckBox timerBox;
        public static bool timer = true;
        public UICheckBox playerPosBox;
        public static bool playerPos = true;
        public UICheckBox FPSBox;
        public static bool showFPS = true;


        public override void OnInitialize()
        {
            checklistPanel = new UIPanel();
            checklistPanel.SetPadding(10f);
            checklistPanel.Left.Set((Main.screenWidth - panelWidth) /2, 0f);
            checklistPanel.Top.Set((Main.screenHeight - panelHeight) /2, 0f);
            checklistPanel.Width.Set(panelWidth, 0f);
            checklistPanel.Height.Set(panelHeight, 0f);
            checklistPanel.BackgroundColor = new Color(73, 94, 171);
            checklistPanel.OnMouseDown += new MouseEvent(DragOn);
            checklistPanel.OnMouseUp += new MouseEvent(DragOff);
            Append(checklistPanel);

            biomeBox = new UICheckBox("显示环境", "", textColor, true, 1f, false);
            biomeBox.OnSelectedChanged += (object o, EventArgs e) =>
            {
                biome = !biome;
            };
            checklistPanel.Append(biomeBox);

            timerBox = new UICheckBox("显示时间", "", textColor, true, 1f, false);
            timerBox.Top.Set(25f, 0f);
            timerBox.OnSelectedChanged += (object o, EventArgs e) =>
            {
                timer = !timer;
            };
            checklistPanel.Append(timerBox);

            playerPosBox = new UICheckBox("显示坐标", "", textColor, true, 1f, false);
            playerPosBox.Top.Set(50f, 0f);
            playerPosBox.OnSelectedChanged += (object o, EventArgs e) =>
            {
                playerPos = !playerPos;
            };
            checklistPanel.Append(playerPosBox);

            FPSBox = new UICheckBox("显示帧数", "", textColor, true, 1f, false);
            FPSBox.Top.Set(75f, 0f);
            FPSBox.OnSelectedChanged += (object o, EventArgs e) =>
            {
                showFPS = !showFPS;
            };
            checklistPanel.Append(FPSBox);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 vector = new Vector2(Main.mouseX, Main.mouseY);

            if (checklistPanel.ContainsPoint(vector))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                checklistPanel.Left.Set(vector.X - offset.X, 0f);
                checklistPanel.Top.Set(vector.Y - offset.Y, 0f);
                Recalculate();
            }
        }

        //鼠标拖拽窗口
        //Code learning from Fargo:https://github.com/Fargowilta/Fargowiltas
        private void DragOn(UIMouseEvent evt, UIElement listeningElement)
        {
            offset = new Vector2(evt.MousePosition.X - checklistPanel.Left.Pixels, evt.MousePosition.Y - checklistPanel.Top.Pixels);
            dragging = true;
        }
        private void DragOff(UIMouseEvent evt, UIElement listeningElement)
        {
            Vector2 mousePosition = evt.MousePosition;
            dragging = false;
            checklistPanel.Left.Set(mousePosition.X - offset.X, 0f);
            checklistPanel.Top.Set(mousePosition.Y - offset.Y, 0f);
            Recalculate();
        }

    }
}
