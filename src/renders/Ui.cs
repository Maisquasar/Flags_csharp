using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Raylib_cs.Color;
using Raylib_cs;

namespace Flags_csharp.src.renders
{
    class UIComponent
    {
        public virtual void Draw() { }
        public virtual void Update() { }
        public virtual bool IsClicked() { return false; }
        public virtual bool IsMouseOn() { return false; }
        public virtual bool Visible { get { return false; } set { } }
    }
    // --------Sprite-------- //
    class Sprite : UIComponent
    {
        protected Vector2 mPos;
        protected Vector2 mSize;
        protected Vector2 mOrigin;
        protected Texture2D mTexture = new Texture2D();
        protected Color mColor = WHITE;
        protected bool mShown = true;

        public Sprite() { }
        public Sprite(bool shown, Vector2 pos, Vector2 size, Color color, Vector2 origin = new Vector2())
        {
            mShown = shown;
            mPos = pos;
            mSize = size;
            mColor = color;
            mOrigin = origin;
        }
        ~Sprite() { }
        public override void Draw()
        {
            if (!Visible)
                return;
            if (mTexture.id == 0)
                DrawRectanglePro(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), new Vector2((float)mOrigin.X, (float)mOrigin.Y), 0, mColor);
            else
                DrawTexturePro(mTexture, new Rectangle(0, 0, (float)mTexture.width, (float)mTexture.height), new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), new Vector2((float)mOrigin.X, (float)mOrigin.Y), 0, mColor);
        }

        // ----------------------------- Setters / Getters ------------------------------ //
        public Vector2 Position { get { return mPos; } set { mPos = value; } }
        public Vector2 Size { get { return mSize; } set { mSize = value; } }
        public Vector2 Origin { get { return mOrigin; } set { mOrigin = value; } }
        public Texture2D Texture { get { return mTexture; } set { mTexture = value; } }
        public Color Color { get { return mColor; } set { mColor = value; } }
        public override bool Visible { get { return mShown; } set { mShown = value; } }
    };

    enum ButtonType
    {
        Basic,
        Toggle,
        Text,
        ToggleText,
    }

    class TexturedButton : Sprite
    {
        ButtonType mType;
        string mText;
        bool mToggled;
        Vector2 mTextOffSet;
        Color mTextColor;
        Color mHoveredColor = DARKGRAY;
        Color mToggledColor = GRAY;
        int mTextSize;
        public string Text { get { return mText; } set { mText = value; } }

        public TexturedButton(Vector2 pos, Vector2 size, Texture2D texture, ButtonType type, bool show = true)
        {
            mPos = pos;
            mSize = size;
            mTexture = texture;
            mType = type;
            mShown = show;
        }
        public void SetText(string text, Vector2 offset, int size, Color color)
        {
            mText = text;
            mTextOffSet = offset;
            mTextSize = size;
            mTextColor = color;
        }
        public override void Draw()
        {
            if (!Visible)
                return;
            DrawTexturePro(mTexture, new Rectangle(0, 0, mTexture.width, mTexture.height), new Rectangle((int)mPos.X, (int)mPos.Y, mSize.X, mSize.Y), new Vector2(), 0, mToggled ? mToggledColor : IsMouseOn() ? mHoveredColor : mColor);
            if (mType == ButtonType.Text || mType == ButtonType.ToggleText)
                DrawText(mText, (int)(mPos.X + mTextOffSet.X), (int)(mPos.Y + mTextOffSet.Y), mTextSize, mTextColor);
        }
        public override void Update()
        {
            if (!Visible)
                return;
            switch (mType)
            {
                case ButtonType.Toggle:
                case ButtonType.ToggleText:
                    {
                        if (IsClicked())
                            mToggled = !mToggled;
                    }
                    break;
                case ButtonType.Text:
                case ButtonType.Basic:
                    {
                        mToggled = false;
                        if (IsClicked())
                            mToggled = true;
                    }
                    break;
                default:
                    break;
            }
        }
        public override bool IsClicked()
        {
            if (IsMouseOn() && IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                return true;
            else
                return false;
        }
        public override bool IsMouseOn()
        {
            Rectangle rec = new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y);
            if (CheckCollisionPointRec(GetMousePosition(), rec))
                return true;
            else
                return false;
        }
        public void SetState(bool state) { mToggled = state; }

        public Color HoveredColor { get { return mHoveredColor; } set { mHoveredColor = value; } }
        public Color ToggledColor { get { return mToggledColor; } set { mToggledColor = value; } }
    }

    // --------- Button ----------- //
    class Button : Sprite
    {
        protected float mRoundness = 0;
        protected Color mHoveredColor;

        public Button() { }
        public Button(bool shown, Vector2 pos, Vector2 size, float roundness, Color color, Color hoverdColor, Texture2D texture = new Texture2D())
        {
            mShown = shown;
            mPos = pos;
            mSize = size;
            mRoundness = roundness;
            mColor = color;
            mHoveredColor = hoverdColor;
            mTexture = texture;
        }
        ~Button() { }
        public override void Draw()
        {
            if (!Visible)
                return;
            Color clickedColor = new Color(mHoveredColor.r, mHoveredColor.g, mHoveredColor.b, (mHoveredColor.a - 100));
            if (IsMouseOn())
            {
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, IsClicked() ? clickedColor : mHoveredColor);
            }
            else
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mColor);
        }
        public override void Update() { }

        public override bool IsClicked()
        {
            if (!Visible)
                return false;
            if (IsMouseOn() && IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                return true;
            else
                return false;
        }
        public override bool IsMouseOn()
        {
            Rectangle rec = new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y);
            if (CheckCollisionPointRec(GetMousePosition(), rec))
                return true;
            else
                return false;
        }

        public Color HoveredColor { get { return mHoveredColor; } set { mHoveredColor = value; } }
        public float Roundness { get { return mRoundness; } set { mRoundness = value; } }
    };

    // -----------Toggle Button ----------- //
    class ToggleButton : Button
    {
        bool mToggle;
        Color mToggledColor;
        public ToggleButton() { }
        public ToggleButton(Button btn, bool toggle)
        {
            mShown = btn.Visible;
            mPos = btn.Position;
            mSize = btn.Size;
            mRoundness = btn.Roundness;
            mColor = btn.Color;
            mHoveredColor = btn.HoveredColor;
            mToggle = toggle;
            mToggledColor = new Color(mHoveredColor.r, mHoveredColor.g, mHoveredColor.b, mHoveredColor.a - 100);
        }
        ~ToggleButton() { }
        public override void Draw()
        {
            if (!Visible)
                return;
            if (IsMouseOn())
            {
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, IsToggle() ? mToggledColor : mHoveredColor);
            }
            else
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mColor);
        }
        public override void Update()
        {
            if (!Visible)
                return;
            if (IsClicked())
            {
                ToggleState();
            }
        }
        public void ToggleState() { mToggle = !mToggle; }
        public void SetState(bool toggle) { mToggle = toggle; }
        public bool IsToggle()
        {
            return mToggle;
        }

        public Color ToggledColor { get { return mToggledColor; } set { mToggledColor = value; } }
    }

    // --------- Text ------------ //
    class Text : UIComponent
    {
        protected Vector2 mTextPos;
        protected string mText;
        protected float mTextSize;
        protected Color mTextColor;
        protected bool mTextShown = true;
        protected Font mTextFont = GetFontDefault();
        protected float mTextSpacing = 1;

        public Text() { }
        public Text(string text, Vector2 pos, float size, Color color, bool shown = true, float spacing = 1.5f, Font font = new Font())
        {
            mText = text;
            mTextPos = pos;
            mTextSize = size;
            mTextColor = color;
            mTextShown = shown;
            mTextSpacing = spacing;
            mTextFont = GetFontDefault();
        }
        ~Text() { }
        public override void Draw()
        {
            if (!Visible)
                return;
            DrawTextEx(mTextFont, mText, mTextPos, mTextSize, mTextSpacing, mTextColor);
        }
        public string Content { get { return mText; } set { mText = value; } }
        public override bool Visible { get { return mTextShown; } set { mTextShown = value; } }
        public Color TextColor { get { return mTextColor; } set { mTextColor = value; } }
        public Vector2 TextPosition { get { return mTextPos; } set { mTextPos = value; } }
        public float TextSize { get { return mTextSize; } set { mTextSize = value; } }
        public float TextSpacing { get { return mTextSpacing; } set { mTextSpacing = value; } }
        public Font TextFont { get { return mTextFont; } set { mTextFont = value; } }
    };

    // --------- Text Button ------------ //
    class TextButton : Button
    {
        protected Vector2 mTextOffSet;
        protected Vector2 mTextPos;
        protected string mText;
        protected float mTextSize;
        protected Color mTextColor;
        protected bool mTextShown = true;
        protected Font mTextFont = GetFontDefault();
        protected float mTextSpacing = 1;
        public TextButton() { }
        public TextButton(bool shown, Vector2 pos, Vector2 size, float roundness, Color color, Color colorWhenMouseOn,
               Vector2 TextOffSet, string text, Vector2 textPos, float textSize, Color textColor, bool textShown = true,
               float textRotation = 0, float textSpacing = 1, Texture2D texture = new Texture2D(), Font font = new Font())
        {
            mShown = shown;
            mPos = pos;
            mSize = size;
            mRoundness = roundness;
            mColor = color;
            mHoveredColor = colorWhenMouseOn;
            mTextOffSet = TextOffSet;
            mTextSize = textSize;
            mText = text;
            mTextPos = textPos;
            mTextColor = color;
            mTextShown = shown;
            mTextFont = font;
            mTextSpacing = textSpacing;
        }
        public TextButton(bool shown, Vector2 pos, Vector2 size, float roundness, Color color, Color colorWhenMouseOn, Vector2 TextOffSet, Text text)
        {
            mShown = shown;
            mPos = pos;
            mSize = size;
            mRoundness = roundness;
            mColor = color;
            mHoveredColor = colorWhenMouseOn;
            mTextOffSet = TextOffSet;
            mTextSize = text.TextSize;
            mText = text.Content;
            mTextPos = text.TextPosition;
            mTextColor = text.TextColor;
            mTextShown = text.Visible;
            mTextFont = text.TextFont;
            mTextSpacing = text.TextSpacing;
        }
        public TextButton(Button btn, Vector2 TextOffSet, string text, Vector2 pos, float size, Color color,
               bool shown = true, float rotation = 0, float spacing = 1, Font font = new Font())
        {
            mShown = btn.Visible;
            mPos = btn.Position;
            mSize = btn.Size;
            mRoundness = btn.Roundness;
            mColor = btn.Color;
            mHoveredColor = btn.HoveredColor;
            mTextOffSet = TextOffSet;
            mTextSize = size;
            mText = text;
            mTextColor = color;
            mTextShown = shown;
            mTextFont = font;
            mTextSpacing = spacing;
        }
        public TextButton(Vector2 pos, Vector2 size, Color color, Vector2 textOffSet, string text, Color textColor, int textSize)
        {
            mPos = pos;
            mSize = size;
            mColor = color;
            mHoveredColor = new Color((color.r - 20), (color.g - 20), (color.b - 20), color.a);
            mTextOffSet = textOffSet;
            mText = text;
            mTextColor = textColor;
            mTextSize = textSize;
        }
        ~TextButton() { }
        public override void Draw()
        {
            if (!Visible)
                return;
            if (IsMouseOn())
            {
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mHoveredColor);
            }
            else
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mColor);
            DrawTextEx(mTextFont, mText, (mPos + mTextOffSet), mTextSize, mTextSpacing, mTextColor);
        }
        public void SetTextSize(int textSize) { mTextSize = textSize; }
        public void SetTextOffSet(Vector2 textOffSet) { mTextOffSet = textOffSet; }
        public void SetTextColor(Color textColor) { mTextColor = textColor; }
        public void SetText(string text) { mText = text; }

        public float GetTextSize() { return mTextSize; }
        public Vector2 GetTextOffSet() { return mTextOffSet; }
        public Color GetTextColor() { return mTextColor; }
        public string GetText() { return mText; }
    };

    // ------------ Input Box ------------- //
    class InputBox : Sprite
    {
        int mLetterCount = 0;
        int mFrameCounter = 0;
        int mMaxInputs;
        string mText;
        int mTextSize;
        Vector2 mTextOffSet;
        Font mFont = GetFontDefault();
        Color mTextColor;
        bool clicked = false;

        public InputBox() { }
        public InputBox(Vector2 pos, Vector2 size, int maxInputs, Color color, Vector2 textOffSet, int textSize, Color textColor = new Color(), Font font = new Font())
        {
            mPos = pos;
            mSize = size;
            mMaxInputs = maxInputs;
            mColor = color;
            mTextOffSet = textOffSet;
            mTextSize = textSize;
            mTextColor = textColor;
            mText = "";
            mFont = GetFontDefault();
        }
        ~InputBox() 
        {
            SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
        }

        public override void Update()
        {
            if (!Visible)
                return;
            ++mFrameCounter;
            if (IsClicked())
                clicked = true;
            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && !IsClicked())
                clicked = false;
            // Check if more characters have been pressed on the same frame
            float tmp = GetCharPressed();
            if (clicked)
            {
                // NOTE: Only allow keys in range [32..125]
                if (((tmp >= 32 && tmp <= 125) || (tmp == 130)) && (mLetterCount < mMaxInputs))
                {
                    mText = new string(mText + (char)tmp);
                    mLetterCount++;
                }
                if (IsKeyDown(KeyboardKey.KEY_BACKSPACE) && ((mFrameCounter % 6) == 0))
                {
                    mLetterCount--;
                    if (mLetterCount < 0)
                        mLetterCount = 0;
                    if (mText.Count() != 0)
                        mText = new string(mText.Remove(mText.Count() - 1));
                }
                if (IsKeyPressed(KeyboardKey.KEY_ENTER))
                {
                    clicked = false;
                }
            }
        }
        public override void Draw()
        {
            if (!Visible)
                return;
            DrawRectangle((int)mPos.X, (int)mPos.Y, (int)mSize.X, (int)mSize.Y, mColor);
            DrawRectangleLines((int)mPos.X, (int)mPos.Y, (int)mSize.X, (int)mSize.Y, clicked ? RED : BLACK);
            DrawTextEx(mFont, mText, new Vector2((float)mPos.X + 5, (float)mPos.Y + 5), mTextSize, 1, BLACK);
            DrawText($"{mLetterCount }/{mMaxInputs}", (int)(mPos.X + mSize.X - MeasureText($"{mLetterCount}/{mMaxInputs}", mTextSize)), (int)(mPos.Y + mSize.Y + 20), mTextSize, BLACK);
            if (IsMouseOn())
            {
                if (((mFrameCounter / 20) % 2) == 0)
                    DrawText("|", (int)mPos.X + 8 + MeasureText(mText, mTextSize - 5), (int)mPos.Y + 12, mTextSize, MAROON);
            }
        }
        public override bool IsMouseOn()
        {
            Rectangle rec = new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y);
            if (CheckCollisionPointRec(GetMousePosition(), rec) && mShown)
            {
                SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);
                return true;
            }
            else
            {
                SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);
                return false;
            }
        }
        public override bool IsClicked()
        {
            if (IsMouseOn() && IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                return true;
            else
                return false;
        }
        public string GetText() { return mText; }
        public void ClearInput()
        {
            mText = new string("");
            mLetterCount = 0;
        }
        public void SetClicked(bool click) { clicked = click; }
    }
}


