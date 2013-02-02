using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Allows specifying mouse movement in a ControlItem
/// </summary>
public enum MouseDirection { None, Horizontal, Vertical, Both }
/// <summary>
/// Allows specifying mouse button clicks or wheel movement in a ControlItem
/// </summary>
public enum MouseButton { None, LeftClick, RightClick, BothClick, MiddleClick, ScrollWheel };

/// <summary>
/// Defines the overall style of the ShowControls.  Docked appears at the top or bottom,
/// FullScreen takes up the entire screen with a shaded background.
/// </summary>
public enum ShowControlStyle { Docked, FullScreen };

/// <summary>
/// Defines whether the docked ShowControls should start at the top or
/// the bottom of the screen.  Has no effect on Fullscreen ShowControls.
/// </summary>
public enum ShowControlPosition { Top, Bottom };

/// <summary>
/// Defines the size of the docked ShowControls.  Small are half height,
/// allowing for more unobtrusive permanent displays.
/// Has no effect on Fullscreen ShowControls.
/// </summary>
public enum ShowControlSize { Normal, Small };

/// <summary>
/// Provides a way to create a completely custom display for your own
/// ControlItem.  It is used internally for the wasd/arrow key clusters.
/// </summary>
public class CustomDisplay
{
    public static CustomDisplay esdf = new CustomDisplay((Texture)Resources.Load("Textures/showControlsSpecialESDF", typeof(Texture)));
    public static CustomDisplay wasd = new CustomDisplay((Texture)Resources.Load("Textures/showControlsSpecialWASD", typeof(Texture)));
    public static CustomDisplay arrows = new CustomDisplay((Texture)Resources.Load("Textures/showControlsSpecialArrows", typeof(Texture)));
    public Texture customTexture;

    public CustomDisplay(Texture tex)
    {
        customTexture = tex;
    }
}

/// <summary>
/// Defines an individual control that is used by ShowControls.
/// It takes some combination of keyboard, MouseMovement, and MouseButton to
/// describe how it is invoked, and a text description that explains what the
/// control is.  Multiple constructors are available to ease creation.
/// 
/// Usually ControlItems are only used when passing in to a static ShowControls
/// creation method.
/// </summary>
public class ControlItem
{
    /// <summary>
    /// A list of keys that should use the "big" key instead of the small.
    /// Also has optional ToString() override to make some fit.
    /// </summary>
    public static Dictionary<KeyCode, string> BigKeys = new Dictionary<KeyCode, string>()
    {
        { KeyCode.Backspace, "Bksp" },
        { KeyCode.Delete, "Del" },
        { KeyCode.Tab, null },
        { KeyCode.Clear, null },
        { KeyCode.Return, null },
        { KeyCode.Pause, null },
        { KeyCode.Escape, "Esc" },
        { KeyCode.Space, null },
        { KeyCode.Keypad0, "NUM0" },
        { KeyCode.Keypad1, "NUM1" },
        { KeyCode.Keypad2, "NUM2" },
        { KeyCode.Keypad3, "NUM3" },
        { KeyCode.Keypad4, "NUM4" },
        { KeyCode.Keypad5, "NUM5" },
        { KeyCode.Keypad6, "NUM6" },
        { KeyCode.Keypad7, "NUM7" },
        { KeyCode.Keypad8, "NUM8" },
        { KeyCode.Keypad9, "NUM9" },
        { KeyCode.KeypadPeriod, "NUM." },
        { KeyCode.KeypadDivide, "NUM/" },
        { KeyCode.KeypadMultiply, "NUM*" },
        { KeyCode.KeypadMinus, "NUM-" },
        { KeyCode.KeypadPlus, "NUM+" },
        { KeyCode.KeypadEquals, "NUM=" },
        { KeyCode.KeypadEnter, "NUMEn" },
        { KeyCode.Home, null },
        { KeyCode.End, null },
        { KeyCode.PageUp, "PgUp" },
        { KeyCode.PageDown, "PgDn" },
        { KeyCode.Numlock, "NumLk" },
        { KeyCode.CapsLock, "CapsLk" },
        { KeyCode.LeftShift, "L Shift"},
        { KeyCode.LeftControl, "L Ctrl" },
        { KeyCode.RightControl, "R Ctrl" },
        { KeyCode.LeftAlt, "L Alt" },
        { KeyCode.RightAlt, "R Alt" },
        { KeyCode.LeftApple, "L Apple" },
        { KeyCode.RightApple, "R Apple" },
        { KeyCode.LeftWindows, "L Win" },
        { KeyCode.RightWindows, "R Win" },
        { KeyCode.AltGr, "Alt Gr" },
        { KeyCode.Help, null },
        { KeyCode.Print, null },
        { KeyCode.SysReq, null },
        { KeyCode.Break, null },
        { KeyCode.Menu, null }
    };

    /// <summary>
    /// Defines custom strings for some of the small keys
    /// </summary>
    public static Dictionary<KeyCode, string> SmallKeys = new Dictionary<KeyCode, string>()
    {
        { KeyCode.UpArrow, "\u2191" },
        { KeyCode.DownArrow, "\u2193" },
        { KeyCode.LeftArrow, "\u2190" },
        { KeyCode.RightArrow, "\u2192" },
        { KeyCode.Insert, "Ins" },
        { KeyCode.Alpha0, "0" },
        { KeyCode.Alpha1, "1" },
        { KeyCode.Alpha2, "2" },
        { KeyCode.Alpha3, "3" },
        { KeyCode.Alpha4, "4" },
        { KeyCode.Alpha5, "5" },
        { KeyCode.Alpha6, "6" },
        { KeyCode.Alpha7, "7" },
        { KeyCode.Alpha8, "8" },
        { KeyCode.Alpha9, "9" },
        { KeyCode.Exclaim, "!" },
        { KeyCode.DoubleQuote, "\"" },
        { KeyCode.Hash, "#" },
        { KeyCode.Dollar, "$" },
        { KeyCode.Ampersand, "&" },
        { KeyCode.Quote, "'" },
        { KeyCode.LeftParen, "(" },
        { KeyCode.RightParen, ")" },
        { KeyCode.Asterisk, "*" },
        { KeyCode.Plus, "+" },
        { KeyCode.Comma, "," },
        { KeyCode.Minus, "-" },
        { KeyCode.Period, "." },
        { KeyCode.Slash, "/" },
        { KeyCode.Colon, ":" },
        { KeyCode.Semicolon, ";" },
        { KeyCode.Less, "<" },
        { KeyCode.Greater, ">" },
        { KeyCode.Question, "?" },
        { KeyCode.At, "@" },
        { KeyCode.LeftBracket, "[" },
        { KeyCode.Backslash, "\\" },
        { KeyCode.RightBracket, "]" },
        { KeyCode.Caret, "^" },
        { KeyCode.Underscore, "_" },
        { KeyCode.BackQuote, "`" },
    };

    /// <summary>
    /// The text description for this control, shown to the right of whatever
    /// key/mouse icons are needed.
    /// </summary>
    public string description;

    public KeyCode[] keys = null;
    public MouseButton button = MouseButton.None;
    public MouseDirection direction = MouseDirection.None;
    public CustomDisplay custom = null;

    /// <summary>
    /// Creates a ControlItem invoked by a single keystroke.
    /// <example><code>new ControlItem("Jump!", KeyCode.Space);</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode key)
    {
        this.description = description;
        this.keys = new KeyCode[1] { key };
    }
    /// <summary>
    /// Creates a ControlItem invoked by multiple keys.
    /// <example><code>new ControlItem("Find", new[] { KeyCode.LeftControl, KeyCode.F });</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode[] keys)
    {
        this.description = description;
        this.keys = keys;
    }
    /// <summary>
    /// Creates a ControlItem invoked by moving the mouse.
    /// <example><code>new ControlItem("Look around", MouseDirection.Both);</code></example>
    /// </summary>
    public ControlItem(string description, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
    }
    /// <summary>
    /// Creates a ControlItem invoked by clicking a mouse button or scrolling the wheel.
    /// <example><code>new ControlItem("Fire your weapon", MouseButton.LeftClick);</code></example>
    /// </summary>
    public ControlItem(string description, MouseButton button)
    {
        this.description = description;
        this.button = button;
    }
    /// <summary>
    /// Creates a ControlItem invoked by clicking & dragging.
    /// 
    /// <example><code>new ControlItem("Create a box selection", MouseDirection.Both, MouseButton.LeftClick);</code></example>
    /// </summary>
    public ControlItem(string description, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
    }
    /// <summary>
    /// Creates a ControlItem invoked by a combination of keystroke &amp; mouse movement.
    /// 
    /// <example><code>new ControlItem("Look around while zoomed", KeyCode.LeftControl, MouseDirection.Both);</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode key, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
        this.keys = new KeyCode[1] { key };
    }
    /// <summary>
    /// Creates a ControlItem invoked by a combination of keystroke &amp; mouse button.
    /// 
    /// <example><code>new ControlItem("Invoke 'Open With...' in Windows", KeyCode.LeftShift, MouseButton.RightClick);</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode key, MouseButton button)
    {
        this.description = description;
        this.button = button;
        this.keys = new KeyCode[1] { key };
    }
    /// <summary>
    /// Creates a ControlItem invoked by a combination of keystroke &amp; mouse dragging.
    /// 
    /// <example><code>new ControlItem("Add a box selection", KeyCode.LeftShift, MouseDirection.Both, MouseButton.LeftClick);</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode key, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
        this.keys = new KeyCode[1] { key };
    }
    /// <summary>
    /// Creates a ControlItem invoked by a combination of multiple keystrokes &amp; mouse movement.
    /// 
    /// <example><code>new ControlItem("Look around while REALLY zoomed", new[] { KeyCode.LeftControl, KeyCode.LeftShift}, MouseDirection.Both);</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode[] keys, MouseDirection direction)
    {
        this.description = description;
        this.direction = direction;
        this.keys = keys;
    }
    /// <summary>
    /// Creates a ControlItem invoked by a combination of multiple keystrokes &amp; mouse click.
    /// 
    /// <example><code>new ControlItem("Select an additional edge loop in Blender", new[] { KeyCode.LeftShift, KeyCode.LeftAlt }, MouseButton.LeftClick });</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode[] keys, MouseButton button)
    {
        this.description = description;
        this.button = button;
        this.keys = keys;
    }
    /// <summary>
    /// Creates a ControlItem invoked by a combination of multiple keystrokes &amp; mouse dragging.
    /// 
    /// <example><code>new ControlItem("Convoluted nondescript example", new[] { KeyCode.LeftControl, KeyCode.LeftShift}, MouseDirection.Both, MouseButton.MiddleClick);</code></example>
    /// </summary>
    public ControlItem(string description, KeyCode[] keys, MouseDirection direction, MouseButton button)
    {
        this.description = description;
        this.direction = direction;
        this.button = button;
        this.keys = keys;
    }
    /// <summary>
    /// Creates a ControlItem that uses a custom display.
    /// 
    /// <example><code>new ControlItem("Move around", CustomDisplay.wasd);</code></example>
    /// </summary>
    public ControlItem(string description, CustomDisplay custom)
    {
        this.description = description;
        this.custom = custom;
    }

    public override string ToString()
    {
        string msg="";
        if (keys != null)
        {
            foreach(KeyCode key in keys)
                msg += string.Format("[{0}]", key);
            if(button != MouseButton.None)
                msg += " + ";
        }
        if (direction != MouseDirection.None)
            msg += string.Format("[dir {0}]", direction.ToString("G"));
        if(button != MouseButton.None)
            msg += string.Format("[but {0}]", button.ToString("G"));

        msg += string.Format(": {0}", description);
        return msg;
    }
}

/// <summary>
/// The main class for creating & invoking the tools to
/// show controls to the user.  Rather than a public constructor, use
/// CreateDocked or CreateFullScreen to create a ShowControls item.
/// Supply any number of ControlItems to those functions to define the
/// controls you want to show.
/// 
/// Show, Hide, and Toggle are used to control the actual display
/// </summary>
public class ShowControls : MonoBehaviour {

    /// <summary>
    /// The GUISkin that is used when displaying controls.  Box is used
    /// for the text description, and two customStyles are also used.
    /// The first is for the top & bottom title of the fullscreen display,
    /// the second for the face of key widgets.
    /// </summary>
    public GUISkin gui;

    /// <summary>
    /// The set of controls that will be shown.  It's normally populated
    /// when you CreateDocked or CreateFullScreen, but you can work with it
    /// manually to dynamically add/remove additional ControlItems as needed.
    /// </summary>
    public ArrayList controls;

    /// <summary>
    /// If <c>true</c>, the GameObject this ShowControls is attached to
    /// will be automatically destroyed when it's done being shown.
    /// Used for "one shot" popup displays.
    /// </summary>
    public bool destroyWhenDone = true;

    /// <summary>
    /// For docked controls, defines how long the controls should be shown.
    /// Set to -1 for infinite duration, which will only finish when
    /// <c>Hide</c> or <c>Toggle</c> is invoked.
    /// Unused for Fullscreen controls.
    /// </summary>
    public float showDuration = 3;
    /// <summary>
    /// Technically keys like Left Shift & Right Shift are differnt keys.
    /// If this is <c>true</c>, the Left/Right parts will be hidden
    /// and it will just show "Shift".
    /// </summary>
    public bool hideLeftRightOnModifierKeys = true;
    /// <summary>
    /// Pauses the game when the Fullscreen controls are displayed.
    /// </summary>
    /// <remarks>
    /// The pausing is accomplished by setting <c>Time.timeScale=0</c>.
    /// <c>FixedUpdate</c> functions will not be called, but <c>Update</c>
    /// functions WILL, so your code may need to test <c>(Time.TimeScale==0)</c>
    /// to detect whether the game is paused and suspend its actions.
    /// </remarks>
    public bool pauseOnDisplay = false;
    /// <summary>
    /// Defines how long the docked controls take to slide in & out.
    /// Set to 0 to have no sliding.  Unused with Fullscreen controls.
    /// </summary>
    public float slideSpeed = .25f;

    /// <summary>
    /// Defines an x & y offset for the docked controls.  It's often useful
    /// to create a single right-justified control by using
    /// <c>xOffset=Screen.width/2</c>.
    /// Unused with Fullscreen controls.
    /// </summary>
    public int offsetX = 0, offsetY = 0;

    /// <summary>
    /// Specifies the overall style of the ShowControls.  Docked appears at the top or bottom,
    /// FullScreen takes up the entire screen with a shaded background.
    /// </summary>
    public ShowControlStyle style = ShowControlStyle.Docked;
    /// <summary>
    /// Specifies whether the docked ShowControls should start at the top or
    /// the bottom of the screen.  Has no effect on Fullscreen ShowControls.
    /// </summary>
    public ShowControlPosition position = ShowControlPosition.Top;

    private ShowControlSize _size = ShowControlSize.Normal;
    /// <summary>
    /// Specifies the size of the docked ShowControls.  Small are half height,
    /// allowing for more unobtrusive permanent displays.
    /// Has no effect on Fullscreen ShowControls.
    /// </summary>
    public ShowControlSize size
    {
        get { return _size; }
        set
        {
            if (value == ShowControlSize.Normal)
                verticalSize = texSize;
            else
                verticalSize = texSize / 2;
            _size = value;
        }
    }

    /// <summary>
    /// Defines the key shown in the "Press X to continue" message at the bottom.
    /// </summary>
    /// <remarks>
    /// This doesn't perform any of the showing or hiding actions, that will need
    /// done by your code.  This only controls what key is displayed on the
    /// screen.
    /// </remarks>
    public KeyCode fullscreenClearKey = KeyCode.Tab;

    /// <summary>
    /// Defines the message that is shown at the top of the
    /// Fullscreen ShowControls.
    /// </summary>
    public string fullscreenTitle = "Controls";
    /// <summary>
    /// Defines the distance between the top of the screen and the start of the
    /// ControlItem widgets in a Fullscreen ShowControls.
    /// </summary>
    public int fullscreenTitleHeight = 100;
    /// <summary>
    /// Defines the text to the left of the fullscreen clear key in the
    /// "Press X to continue" message at the bottom of Fullscreen Controls.
    /// </summary>
    public string fullscreenMessageLeft = "Press ";
    /// <summary>
    /// Defines the text to the right of the fullscreen clear key in the
    /// "Press X to continue" message at the bottom of Fullscreen Controls.
    /// </summary>
    public string fullscreenMessageRight = " to continue";

    public Texture keyBaseSmall;
    public Texture keyBaseLarge;
    public Texture mouseBase;
    public Texture mouseLeftClick;
    public Texture mouseMiddleClick;
    public Texture mouseRightClick;
    public Texture mouseWheel;
    public Texture mouseHorizontal;
    public Texture mouseVertical;
    public Texture mouseHorizontalAndVertical;
    public Texture plus;

    private const int texSize = 64;

    /// <summary>
    /// The vertical size/offset actually being used, which may be
    /// modified if size=ShowControlSize.small
    /// </summary>
    private int verticalSize = texSize;

    /// <summary>
    /// offsets in the array of custom styles in the GUISkin
    /// </summary>
    private static int TITLE_STYLE = 0;
    private static int KEYBOARD_STYLE = 1;
    /// <summary>
    /// Created dynamically based off title style
    /// </summary>
    private GUIStyle fullscreenBottomLeftStyle;
    private GUIStyle fullscreenBottomRightStyle;

    /// <summary>
    /// make this configurable someday?
    /// </summary>
    private static int NUM_COLUMNS = 2;

    /// <summary>
    /// Internal tracking of whether we're showing at all.  controls
    /// that have had Hide() will still have doShow=true if they're
    /// still sliding.
    /// </summary>
    private bool doShow = false;
    /// <summary>
    /// tracking of when various things start & stop
    /// </summary>
    private float showStart = -1, showStop = -1, savedTimeScale;

    /// <summary>
    /// Creates a new ShowControls for a single control that will be docked.
    /// </summary>
    public static ShowControls CreateDocked(ControlItem control)
    {
        return CreateDocked(new[] { control });
    }
    /// <summary>
    /// Creates a new ShowControls for a multiple controls that will be docked.
    /// </summary>
    public static ShowControls CreateDocked(ControlItem[] controls)
    {
        GameObject prefab = (GameObject)Resources.Load("DefaultShowControls");
        GameObject obj = (GameObject)Instantiate(prefab);
        ShowControls sc = obj.GetComponent<ShowControls>();
        sc.controls = new ArrayList(controls);
        return sc;
    }
    /// <summary>
    /// Creates a new ShowControls for a single control that will shown fullscreen.
    /// </summary>
    public static ShowControls CreateFullscreen(ControlItem item)
    {
        return CreateFullscreen(new[] { item });
    }
    /// <summary>
    /// Creates a new ShowControls for a multiple controls that will shown fullscreen.
    /// </summary>
    public static ShowControls CreateFullscreen(ControlItem[] controls)
    {
        GameObject prefab = (GameObject)Resources.Load("DefaultShowControls");
        GameObject obj = (GameObject)Instantiate(prefab);
        ShowControls sc = obj.GetComponent<ShowControls>();
        sc.controls = new ArrayList(controls);
        sc.style = ShowControlStyle.FullScreen;
        sc.slideSpeed = -1;
        sc.showDuration = -1;
        sc.pauseOnDisplay = true;
        return sc;
    }

    /// <summary>
    /// Causes this ShowControls to display.  If this style is ShowControlsStyle.Docked
    /// and duration!=0, it will begin sliding.  Has no effect if this is already displaying.
    /// </summary>
    public void Show()
    {
        if (IsShown)
            return;

        doShow = true;
        showStart = Time.time;
        showStop = -1;
        if (pauseOnDisplay)
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            AudioListener.pause = true;
        }
        if (style == ShowControlStyle.FullScreen)
        {
            fullscreenBottomLeftStyle = new GUIStyle(gui.customStyles[TITLE_STYLE]);
            fullscreenBottomLeftStyle.alignment = TextAnchor.MiddleRight;
            fullscreenBottomRightStyle = new GUIStyle(gui.customStyles[TITLE_STYLE]);
            fullscreenBottomRightStyle.alignment = TextAnchor.MiddleLeft;
        }
    }
    /// <summary>
    /// Hide() indicates we want to stop showing.  If this style is ShowControlsStyle.Docked
    /// and duration!=0, it will begin sliding.  Has no effect if this is already hiding / hidden.
    /// </summary>
    public void Hide()
    {
        if (!IsShown || showStop != -1)
            return;
        if (style == ShowControlStyle.FullScreen || slideSpeed == -1)
        {
            Finished();
            return;
        }
        if (showStop != -1)
            return;
        showStop = Time.time;
    }

    /// <summary>
    /// Shows the control if it's hidden, or hides the control if it's shown.
    /// </summary>
    public void Toggle()
    {
        if (doShow && showStop == -1)
            Hide();
        else
            Show();
    }

    /// <summary>
    /// Finished() is called when we are really done displaying,
    /// either because we're instantaneously toggling off, or because
    /// we're sliding and have finished the slide.
    /// </summary>
    private void Finished()
    {
        doShow = false;
        if (pauseOnDisplay)
        {
            Time.timeScale = savedTimeScale;
            AudioListener.pause = false;
        }
        if (destroyWhenDone)
            Destroy(gameObject);
    }

    /// <summary>
    /// Invoked by Unity3D to actually display the controls.
    /// </summary>
    public void OnGUI()
    {
        if (!doShow)
            return;

        /* if we'd already started hiding, see if we're done */
        if (showStop != -1 && Time.time > showStop + slideSpeed)
        {
            Finished();
            return;
        }

        /* if we've got a max duration, haven't begun stopping yet, and
         * are past the time that we want to stop, hide. */ 
        if (showDuration != -1 && showStop == -1 && Time.time > showStart + showDuration)
        {
            Hide();
            // if we're sliding, we keep showing.  If not, we're done
            if (doShow)
                DisplayControls();
            return;
        }

        // normal case, just display.
        DisplayControls();
    }

    private void DisplayControls()
    {
        int x=0, y, slideOffset=0;
        if (style == ShowControlStyle.FullScreen)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            GUI.Label(new Rect(0, 0, Screen.width, fullscreenTitleHeight), fullscreenTitle, gui.customStyles[TITLE_STYLE]);

            /* 3 things are used on the bottom - "Press " label on the left side,
             * widget for the clear key in the center, and " to continue" label
             * on the right side. */
            // TODO adjust width of bottom labels based on relative sizes
            int PADDING = 5;
                    
            int labelPos = Screen.height - fullscreenTitleHeight;
            int labelWidth = (Screen.width / 2) - (texSize / 2 + (2*PADDING));
            Rect r = new Rect(0, labelPos, labelWidth, fullscreenTitleHeight);
            GUI.Label(r, fullscreenMessageLeft, fullscreenBottomLeftStyle);

            int yPos = labelPos + (fullscreenTitleHeight - texSize) / 2;
            r = new Rect(labelWidth + PADDING, yPos, texSize, texSize);
            ShowKey(r, fullscreenClearKey);

            r = new Rect(labelWidth + texSize + PADDING * 2, labelPos, labelWidth, fullscreenTitleHeight);
            GUI.Label(r, fullscreenMessageRight, fullscreenBottomRightStyle);

            y = fullscreenTitleHeight;
        }
        else
        {
            if (Time.time < showStart + slideSpeed)
            {
                int slideTotal = texSize * Mathf.CeilToInt((float)controls.Count / NUM_COLUMNS);
                float slidePercent = (slideSpeed - (Time.time - showStart)) / (float)slideSpeed;
                slideOffset = (int)(slidePercent * slideTotal);
            }
            if (showStop != -1)
            {
                int slideTotal = texSize * Mathf.CeilToInt((float)controls.Count / NUM_COLUMNS);
                float slidePercent = (Time.time - showStop) / (float)slideSpeed;
                slideOffset = (int)(slidePercent * slideTotal);
            }

            if (position == ShowControlPosition.Top)
                y = 0 - slideOffset;
            else
                y = Screen.height - verticalSize + slideOffset;
        }

        ShowAllControls(x + offsetX, y + offsetY);
    }

    private void ShowAllControls(int x, int y)
    {
        int tmpX;
        bool shiftRight = false;
        foreach (ControlItem control in controls)
        {
            if (shiftRight)
                tmpX = Screen.width / 2 + x;
            else
                tmpX = x;

            ShowControl(control, tmpX, y);

            if (shiftRight)
            {
                if (position == ShowControlPosition.Top)
                    y += verticalSize + 5;
                else
                    y -= verticalSize + 5;
                shiftRight = false;
            }
            else
            {
                shiftRight = true;
            }
        }
    }
    private void ShowControl(ControlItem control, int x, int y)
    {
        Rect texRect = new Rect(x, y, texSize, texSize);
        Rect labelRect;

        if (control.custom != null)
        {
            ShowCustom(texRect, control.custom);
            texRect.x += texSize;
        }
        // draw each of the keys
        if (control.keys != null)
        {
            foreach (KeyCode key in control.keys)
            {
                ShowKey(texRect, key);
                texRect.x += texSize;
            }
        } 

        // draw the mouse, if necessary
        if (control.button != MouseButton.None || control.direction != MouseDirection.None)
        {
            // if we already showed keys, also show a plus between keys & mouse
            if (control.keys != null)
            {
                Rect plusRect = new Rect(texRect.x, texRect.y + (texSize *.375f), texSize / 4, texSize / 4);
                GUI.DrawTexture(plusRect, plus);
                texRect.x += texSize / 4;
            }
            ShowMouse(texRect, control.direction, control.button);
            texRect.x += texSize;
        }
        // put the text description in the leftover space
        labelRect = new Rect(texRect.x, y, (Screen.width / 2) - (texRect.x - x), verticalSize);
        GUI.Box(labelRect, control.description, gui.box);
    }

    private void ShowCustom(Rect texRect, CustomDisplay custom)
    {
        if (size == ShowControlSize.Normal)
        {
            GUI.DrawTexture(texRect, custom.customTexture);
        }
        else
        {
            Rect groupRect = new Rect(texRect.x, texRect.y, texSize, verticalSize);
            GUI.BeginGroup(groupRect);
            GUI.DrawTexture(new Rect(0, -verticalSize / 2, texSize, texSize), custom.customTexture, ScaleMode.ScaleToFit);
            GUI.EndGroup();
        }
    }

    private void ShowKey(Rect texRect, KeyCode key)
    {
        Texture tex = null;
        string label = null;
        if (ControlItem.BigKeys.ContainsKey(key))
        {
            tex = keyBaseLarge;
            label = ControlItem.BigKeys[key];
            if (label == null)
                label = key.ToString();
            else
            {
                if (hideLeftRightOnModifierKeys &&
                    (label.StartsWith("R ") || label.StartsWith("L ")))
                    label = label.Substring(2);
            }
        }
        else
        {
            tex = keyBaseSmall;
            if (ControlItem.SmallKeys.ContainsKey(key))
                label = ControlItem.SmallKeys[key];
            else
                label = key.ToString();
        }
        if (size == ShowControlSize.Normal)
        {
            GUI.DrawTexture(texRect, tex);
        }
        else
        {
            Rect groupRect = new Rect(texRect.x, texRect.y, texSize, verticalSize);
            GUI.BeginGroup(groupRect);
            GUI.DrawTexture(new Rect(0, -verticalSize/2, texSize, texSize), tex, ScaleMode.ScaleToFit);
            GUI.EndGroup();
        }
        Rect labelRect = new Rect(texRect.x, texRect.y, texSize, verticalSize - 15);
        GUI.Label(labelRect, label, gui.customStyles[KEYBOARD_STYLE]);
    }
    private void ShowMouse(Rect texRect, MouseDirection direction, MouseButton button)
    {
        GUI.DrawTexture(texRect, mouseBase);
        switch (button)
        {
            case MouseButton.None:
                break;
            case MouseButton.LeftClick:
                GUI.DrawTexture(texRect, mouseLeftClick); break;
            case MouseButton.RightClick:
                GUI.DrawTexture(texRect, mouseRightClick); break;
            case MouseButton.BothClick:
                GUI.DrawTexture(texRect, mouseLeftClick);
                GUI.DrawTexture(texRect, mouseRightClick); break;
            case MouseButton.MiddleClick:
                GUI.DrawTexture(texRect, mouseMiddleClick); break;
            case MouseButton.ScrollWheel:
                GUI.DrawTexture(texRect, mouseWheel); break;
            default:
                Debug.LogError("Unsupported MouseButton " + button);
                return;
        }
        switch (direction)
        {
            case MouseDirection.None:
                break;
            case MouseDirection.Horizontal:
                GUI.DrawTexture(texRect, mouseHorizontal); break;
            case MouseDirection.Vertical:
                GUI.DrawTexture(texRect, mouseVertical); break;
            case MouseDirection.Both:
                GUI.DrawTexture(texRect, mouseHorizontalAndVertical); break;
            default:
                Debug.LogError("Unsupported MouseDirection " + direction);
                return;
        }
    }

    /// <summary>
    /// Indicates whether or not the controls are currently shown.
    /// Setting to true is the same as calling Show(), setting false
    /// is the same as calling Hide().
    /// </summary>
    public bool IsShown
    {
        get { return doShow; }
        set
        {
            if (value)
            {
                if (!IsShown)
                    Show();
            }
            else
            {
                if (IsShown)
                    Hide();
            }
        }
    }
}