using Godot;
using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public partial class HotkeyRebindButton : Control
{
    Label label;
    Button button1;
    Button button2;
    SubmenuSettings SettingsNode;
    InputEventKey prevKey;
    InputEventKey prevKeyAlt;
    InputEventMouseButton prevClick;
    InputEventMouseButton prevClickAlt;
    [Export]
    string actionName = "";

    public override void _Ready()
    {
        SettingsNode = GetNode("../../../../../../../../../../settings") as SubmenuSettings;
        label = GetNode<Label>("HBox/Text");

        button1 = GetNode<Button>("HBox/Buttons/BindButton1");
        button2 = GetNode<Button>("HBox/Buttons/BindButton2");

        //get previous keys for reset
        if (InputMap.ActionGetEvents(actionName).Count > 0)
        {
            InputEvent @event = InputMap.ActionGetEvents(actionName)[0];
            if (@event is InputEventKey eventkey) prevKey = eventkey;
            else if (@event is InputEventMouseButton eventbutton) prevClick = eventbutton;
        }else
        {
            prevKey = null;
            prevClick = null;
        }
        if (InputMap.ActionGetEvents(actionName + "-alt").Count > 0)
        {
            InputEvent @eventAlt = InputMap.ActionGetEvents(actionName + "-alt")[0];
            if (@eventAlt is InputEventKey eventkey) prevKeyAlt = eventkey;
            else if (@eventAlt is InputEventMouseButton eventbutton) prevClickAlt = eventbutton;
        }else
        {
            prevKeyAlt = null;
            prevClickAlt = null;
        }

        SetProcessUnhandledKeyInput(false);
        SetActionName();
        SetKeyText();
    }

    public override void _Process(double delta)
    {
        if (IsProcessingUnhandledKeyInput())
        {
            button1.Disabled = true;
            button2.Disabled = true;
            listenMouseInput();
        }
        else
        {
            button1.Disabled = false;
            button2.Disabled= false;
        }
    }

    public void listenMouseInput()
    {
        foreach (string button in new string[5] {"Left", "Right", "Middle", "Xbutton1", "Xbutton2"})
        {
            bool buttonPressed = Input.IsMouseButtonPressed((MouseButton)Enum.Parse(typeof(MouseButton), button, true));
            if (buttonPressed)
            {
                rebindActionKey(new InputEventMouseButton()
                {
                    ButtonIndex = (MouseButton)Enum.Parse(typeof(MouseButton), button, true),
                    AltPressed = false,
                    CtrlPressed = false,
                    MetaPressed = false,
                    ShiftPressed = false,
                    Pressed = true
                });
                switch (Globals.buttontoggle)
                {
                    case 1:
                        button1.ButtonPressed = false;
                        Globals.buttontoggle = 0;
                        break;
                    case 2:
                        button2.ButtonPressed = false;
                        Globals.buttontoggle = 0;
                        break;

                    default:
                        break;
                }

            }
        }
    }

    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if(@event is InputEventKey) rebindActionKey(@event);
        switch (Globals.buttontoggle)
        {
            case 1:
                button1.ButtonPressed = false;
                Globals.buttontoggle = 0;
                break;
            case 2:
                button2.ButtonPressed = false;
                Globals.buttontoggle = 0;
                break;

            default:
                break;
        }
    }
    public void SetActionName()
    {
        label.Text = "Unassigned";
        switch (actionName)
        {
            case "move_left":
                label.Text = "Move left";
                break;
            case "move_right":
                label.Text = "Move right";
                break;
            case "move_jump":
                label.Text = "Move up / Jump";
                break;
            case "move_crouch":
                label.Text = "Move down / Crouch";
                break;
            case "move_dash":
                label.Text = "Dash";
                break;
            case "spell_oracle":
                label.Text = "Oracle";
                break;
            case "spell_slot1":
                label.Text = "Spellslot 1";
                break;
            case "spell_slot2":
                label.Text = "Spellslot 2";
                break;
            case "attack_normal":
                label.Text = "Primary attack";
                break;
            case "attack_charge":
                label.Text = "Charge attack";
                break;

            default:
                break;
        }
    }
    public void SetKeyText()
    {
        if (ConfigFileHandler.settingChanges["controls"][actionName].ToString().Contains("Xbutton"))
        {
            button1.Text = ConfigFileHandler.settingChanges["controls"][actionName].ToString().Replace("Xbutton", "Side");

        }else button1.Text = ConfigFileHandler.settingChanges["controls"][actionName].ToString();
        if(ConfigFileHandler.settingChanges["controls"][actionName + "-alt"].ToString().Contains("Xbutton"))
        {
            button2.Text = ConfigFileHandler.settingChanges["controls"][actionName + "-alt"].ToString().Replace("Xbutton", "Side");

        }else button2.Text = ConfigFileHandler.settingChanges["controls"][actionName + "-alt"].ToString();
    }
    public void rebindActionKey(InputEvent @event)
    {
        SettingsNode.rebinding = false;
        SettingsNode.rebindtimer = 2;
        switch (Globals.buttontoggle)
        {
            case 1:
                InputMap.ActionEraseEvents(actionName);
                ConfigFileHandler.settingChanges["controls"][actionName] = "";
                if (@event is InputEventKey key && key.PhysicalKeycode != Key.Escape)
                {
                    InputMap.ActionAddEvent(actionName, @event);
                    ConfigFileHandler.settingChanges["controls"][actionName] = key.PhysicalKeycode.ToString();
                    SettingsNode.isSaved = false;
                }
                else if (@event is InputEventMouseButton mouseButton)
                {
                    InputMap.ActionAddEvent(actionName, mouseButton);
                    ConfigFileHandler.settingChanges["controls"][actionName] = $"Mouse{mouseButton.ButtonIndex}";
                }
                break;
            case 2:
                ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = "";
                InputMap.ActionEraseEvents(actionName + "-alt");
                if (@event is InputEventKey keyalt && keyalt.PhysicalKeycode != Key.Escape)
                {
                    InputMap.ActionAddEvent(actionName + "-alt", @event);
                    ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = keyalt.PhysicalKeycode.ToString();
                    SettingsNode.isSaved = false;
                }else if (@event is InputEventMouseButton mouseButtonAlt)
                {
                    InputMap.ActionAddEvent(actionName+"-alt", mouseButtonAlt);
                    ConfigFileHandler.settingChanges["controls"][actionName+"-alt"] = $"Mouse{mouseButtonAlt.ButtonIndex}";

                }
                break;
            default:
                break;
        }
        SetProcessUnhandledKeyInput(false);
        SetKeyText();
        SetActionName();
    }
    public void Button1Toggled(bool toggled)
    {
        if (toggled)
        {
            SettingsNode.rebinding = toggled;
            Globals.buttontoggle = 1;
            button1.Text = "listening...";
            SetProcessUnhandledKeyInput(toggled);

            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = false;
                    hotkeyButton.button2.ToggleMode = false;
                    hotkeyButton.SetProcessUnhandledKeyInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button2.ToggleMode = false;
                }
            }
        }
        else
        {
            Globals.buttontoggle = 0;
            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = true;
                    hotkeyButton.button2.ToggleMode = true;
                    hotkeyButton.SetProcessUnhandledKeyInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button2.ToggleMode = true;
                }
            }
            SetKeyText();
        }
    }
    public void Button2Toggled(bool toggled)
    {
        if (toggled)
        {
            SettingsNode.rebinding = toggled;
            Globals.buttontoggle = 2;
            button2.Text = "listening...";
            SetProcessUnhandledKeyInput(toggled);

            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = false;
                    hotkeyButton.button2.ToggleMode = false;
                    hotkeyButton.SetProcessUnhandledKeyInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button1.ToggleMode = false;
                }
            }
        }
        else
        {
            Globals.buttontoggle = 0;
            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = true;
                    hotkeyButton.button2.ToggleMode = true;
                    hotkeyButton.SetProcessUnhandledKeyInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button1.ToggleMode = true;
                }
            }
            SetKeyText();
        }
    }

    public void OnExiting()
    {
        if (SettingsNode == null)
        {
            GD.PrintErr("SettingsNode is null");
            return;
        }
        if (!SettingsNode.isSaved)
        {
            InputMap.ActionEraseEvents(actionName);
            ConfigFileHandler.settingChanges["controls"][actionName] = "";
            if (prevKey != null)
            {
                InputMap.ActionAddEvent(actionName, prevKey);
                ConfigFileHandler.settingChanges["controls"][actionName] = prevKey.PhysicalKeycode.ToString();
            }
            InputMap.ActionEraseEvents(actionName + "-alt");
            ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = "";
            if (prevKeyAlt != null)
            {
                InputMap.ActionAddEvent(actionName + "-alt", prevKeyAlt);
                ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = prevKeyAlt.PhysicalKeycode.ToString();
            }

        }
    }
}
