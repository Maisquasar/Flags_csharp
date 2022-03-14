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
    // --------Sprite-------- //
    class Sprite
    {
        protected Vector2 mPos;
        protected Vector2 mSize;
        protected Vector2 mOrigin;
        protected Color mColor;
        protected bool mShown;
        protected List<Texture2D> mTexture = new List<Texture2D>();

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
        public void Draw()
        {

            if ((int)mTexture.Count() <= 0)
                DrawRectanglePro(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), new Vector2((float)mOrigin.X, (float)mOrigin.Y), 0, mColor);
            else
                DrawTexturePro(mTexture[0], new Rectangle(0, 0, (float)mTexture[0].width, (float)mTexture[0].height), new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), new Vector2((float)mOrigin.X, (float)mOrigin.Y), 0, mColor);
        }
        // ----------------------------- Setters ------------------------------ //
        public void SetPos(Vector2 pos) { mPos = pos; }                         //
        public void SetSize(Vector2 size) { mSize = size; }                     //
        public void SetColor(Color color) { mColor = color; }                   //
        public void SetVisibility(bool shown) { mShown = shown; }               //
        public void SetTexture(Texture2D texture) { mTexture.Clear(); mTexture.Add(texture); }    //
        public void SetOrigin(Vector2 origin) { mOrigin = origin; }             //
        // ----------------------------- Getters ------------------------------ //
        public Vector2 GetPos() { return mPos; }                                //
        public Vector2 GetSize() { return mSize; }                              //
        public Color GetColor() { return mColor; }                              //
        public bool GetVisibility() { return mShown; }                          //
        public Texture2D GetTexture() { return mTexture[0]; }                   //
        // -------------------------------------------------------------------- //
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
        bool isOn;
        Vector2 mTextOffSet;
        Color mTextColor;
        int mTextSize;
        public string Text { get { return mText; } set { mText = value; } }

        public TexturedButton(Vector2 pos, Vector2 size, Texture2D texture, ButtonType type, bool show = true)
        {
            mPos = pos;
            mSize = size;
            mTexture.Add(texture);
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
        public new void Draw()
        {
            DrawTexturePro(mTexture[0], new Rectangle(0, 0, mTexture[0].width, mTexture[0].height), new Rectangle((int)mPos.X, (int)mPos.Y, mSize.X, mSize.Y), new Vector2(), 0, isOn ? DARKGRAY :  IsMouseOn() ? GRAY : WHITE);
            if (mType == ButtonType.Text || mType == ButtonType.ToggleText)
                DrawText(mText, (int)(mPos.X + mTextOffSet.X), (int)(mPos.Y + mTextOffSet.Y), mTextSize, mTextColor);
        }
        public void Update()
        {
            switch (mType)
            {
                case ButtonType.Toggle:
                case ButtonType.ToggleText:
                    {
                        if (IsClicked())
                            isOn = !isOn;
                    }
                    break;
                case ButtonType.Text:
                case ButtonType.Basic:
                    {
                        isOn = false;
                        if (IsClicked())
                            isOn = true;
                    }
                    break;
                default:
                    break;
            }
        }
        public bool IsClicked()
        {
            if (IsMouseOn() && IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                return true;
            else
                return false;
        }
        public bool IsMouseOn()
        {
            Rectangle rec = new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y);
            if (CheckCollisionPointRec(GetMousePosition(), rec))
                return true;
            else
                return false;
        }
        public void setState(bool state) { isOn = state; }
    }

    // --------- Button ----------- //
    class Button : Sprite
    {
        protected float mRoundness = 0;
        protected Color mColorWhenMouseOn;

        public Button() { }
        public Button(bool shown, Vector2 pos, Vector2 size, float roundness, Color color, Color colorWhenMouseOn, Texture2D texture = new Texture2D())
        {
            mShown = shown;
            mPos = pos;
            mSize = size;
            mRoundness = roundness;
            mColor = color;
            mColorWhenMouseOn = colorWhenMouseOn;
            mTexture.Add(texture);
        }
        ~Button() { }
        public new virtual void Draw()
        {
            Color clickedColor = new Color(mColorWhenMouseOn.r, mColorWhenMouseOn.g, mColorWhenMouseOn.b, (mColorWhenMouseOn.a - 100));
            if (IsMouseOn())
            {
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, IsClicked() ? clickedColor : mColorWhenMouseOn);
            }
            else
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mColor);
        }
        public virtual void Update() { }

        public bool IsClicked()
        {
            if (IsMouseOn() && IsMouseButtonReleased(MouseButton.MOUSE_LEFT_BUTTON))
                return true;
            else
                return false;
        }
        public bool IsMouseOn()
        {
            Rectangle rec = new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y);
            if (CheckCollisionPointRec(GetMousePosition(), rec))
                return true;
            else
                return false;
        }
        public float GetRoundness() { return mRoundness; }
        public Color GetColorWhenMouseOn() { return mColorWhenMouseOn; }
    };



    // -----------Toggle Button ----------- //
    class ToggleButton : Button
    {
        bool mOn;
        public ToggleButton() { }
        public ToggleButton(Button btn, bool On)
        {
            mShown = btn.GetVisibility();
            mPos = btn.GetPos();
            mSize = btn.GetSize();
            mRoundness = btn.GetRoundness();
            mColor = btn.GetColor();
            mColorWhenMouseOn = btn.GetColorWhenMouseOn();
            mOn = On;
        }
        ~ToggleButton() { }                    //
        public new void Draw()
        {
            Color clickedColor = new Color(mColorWhenMouseOn.r, mColorWhenMouseOn.g, mColorWhenMouseOn.b, mColorWhenMouseOn.a - 100);
            if (IsMouseOn())
            {
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, IsOn() ? clickedColor : mColorWhenMouseOn);
            }
            else
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mColor);
        }
        public new void Update()
        {
            if (IsClicked())
            {
                toggleState();
            }
        }
        public void toggleState() { mOn = !mOn; }
        public void SetState(bool on) { mOn = on; }
        public bool IsOn()
        {
            return mOn;
        }
    }


    class Text
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
        public void Draw()
        {
            DrawTextEx(mTextFont, mText, mTextPos, mTextSize, mTextSpacing, mTextColor);
        }
        public void SetText(string text) { mText = text; }
        public void SetTextVisibility(bool shown) { mTextShown = shown; }
        public void SetTextColor(Color textColor) { mTextColor = textColor; }
        public void SetTextPos(Vector2 textPos) { mTextPos = textPos; }

        public Vector2 GetTextPos() { return mTextPos; }
        public string GetText() { return mText; }
        public float GetTextSize() { return mTextSize; }
        public Color GetTextColor() { return mTextColor; }
        public bool GetTextVisibility() { return mTextShown; }
        public Font GetTextFont() { return mTextFont; }
        public float GetTextSpacing() { return mTextSpacing; }
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
            mColorWhenMouseOn = colorWhenMouseOn;
            mTextOffSet = TextOffSet;
            mTextSize = textSize;
            mText = text;
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
            mColorWhenMouseOn = colorWhenMouseOn;
            mTextOffSet = TextOffSet;
            mTextSize = text.GetTextSize();
            mText = text.GetText();
            mTextColor = text.GetTextColor();
            mTextShown = text.GetTextVisibility();
            mTextFont = text.GetTextFont();
            mTextSpacing = text.GetTextSpacing();
        }
        public TextButton(Button btn, Vector2 TextOffSet, string text, Vector2 pos, float size, Color color,
               bool shown = true, float rotation = 0, float spacing = 1, Font font = new Font())
        {
            mShown = btn.GetVisibility();
            mPos = btn.GetPos();
            mSize = btn.GetSize();
            mRoundness = btn.GetRoundness();
            mColor = btn.GetColor();
            mColorWhenMouseOn = btn.GetColorWhenMouseOn();
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
            mColorWhenMouseOn = new Color((color.r - 20), (color.g - 20), (color.b - 20), color.a);
            mTextOffSet = textOffSet;
            mText = text;
            mTextColor = textColor;
            mTextSize = textSize;
        }
        ~TextButton() { }
        public new void Draw()
        {
            Color clickedColor = new Color(mColorWhenMouseOn.r, mColorWhenMouseOn.g, mColorWhenMouseOn.b, (mColorWhenMouseOn.a - 100));
            if (IsMouseOn())
            {
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, IsClicked() ? clickedColor : mColorWhenMouseOn);
            }
            else
                DrawRectangleRounded(new Rectangle((float)mPos.X, (float)mPos.Y, (float)mSize.X, (float)mSize.Y), mRoundness, 1, mColor);
            DrawTextEx(mTextFont, mText, (mPos + mTextOffSet), mTextSize, mTextSpacing, mTextColor);
        }
        public new void Update() { }
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
        ~InputBox() { }
        public void Update()
        {
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
        public new void Draw()
        {
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
        public bool IsMouseOn()
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
        public bool IsClicked()
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


