using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace WhereIAm
{
    public class UICheckBox : UIText
    {
        public Color mainColor;
        static Texture2D checknoTexture = WhereIAm.instance.GetTexture("checkno");
        static Texture2D checkyesTexture = WhereIAm.instance.GetTexture("checkyes");
        public event EventHandler OnSelectedChanged;

        public float order = 0;
        string tooltip = "";
        string text = "";
        bool clickable = true;
        private bool selected = true;
        public bool Selected
        {
            get { return selected; }
            set
            {
                if (value != selected)
                {
                    selected = value;
                    if (OnSelectedChanged != null)
                    {
                        OnSelectedChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        public UICheckBox(string text, string tooltip, Color mainColor, bool clickable = true, float textScale = 1f, bool large = false) : base("", textScale, large)
        {
            this.text = "   " + text;
            this.tooltip = tooltip;
            this.mainColor = mainColor;
            this.clickable = clickable;
            SetText("   ");
            Recalculate();
        }

        public override void Click(UIMouseEvent evt)
        {
            if (clickable)
            {
                Selected = !Selected;
            }
            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle calculatedStyle = GetInnerDimensions();
            Vector2 position = new Vector2(calculatedStyle.X, calculatedStyle.Y - 2);

            spriteBatch.Draw(checknoTexture, position, null, Color.White, 0f, new Vector2(0, -1), 1f, SpriteEffects.None, 0f);
            if (Selected)
            {
                spriteBatch.Draw(checkyesTexture, position, null, Color.White, 0f, new Vector2(0, -1), 1f, SpriteEffects.None, 0f);
            }
            base.DrawSelf(spriteBatch);

            Utils.DrawBorderString(spriteBatch, text, position, mainColor, 1f, 0f, 0f, -1);

            if (IsMouseHovering && tooltip.Length > 0)
            {
                Main.HoverItem = new Item();
                Main.hoverItemName = tooltip;
            }
        }

        public override int CompareTo(object obj)
        {
            UICheckBox other = obj as UICheckBox;
            return order.CompareTo(other.order);
        }
    }
}
