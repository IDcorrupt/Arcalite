using Godot;
using System;
using System.Text.RegularExpressions;

public partial class HotkeyRebindButton : Control
{
    Label label;
    Button button1;
    Button button2;
    SubmenuSettings SettingsNode;
    InputEventKey prevKey;
    InputEventMouseButton prevClick;
    InputEventKey prevKeyAlt;
    InputEventMouseButton prevClickAlt;
    [Export]
    string actionName = "";


    public override void _Ready()
    {
        SettingsNode = GetNode("/root/MainNode/MainMenu/Control") as SubmenuSettings;
        label = GetNode<Label>("HBox/Text");

        button1 = GetNode<Button>("HBox/Buttons/BindButton1");
        button2 = GetNode<Button>("HBox/Buttons/BindButton2");

        if (InputMap.ActionGetEvents(actionName).Count > 0)
        {
            InputEvent @event = InputMap.ActionGetEvents(actionName)[0];
            if (@event is InputEventKey eventkey) prevKey = eventkey;
            else if (@event is InputEventMouseButton eventbutton) prevClick = eventbutton;
            GD.Print(prevKey.PhysicalKeycode.ToString());
        }else prevKey = null;

        if (InputMap.ActionGetEvents(actionName + "-alt").Count > 0)
        {
            InputEvent @eventAlt = InputMap.ActionGetEvents(actionName + "-alt")[0];
            if (@eventAlt is InputEventKey eventkey) prevKeyAlt = eventkey;
            else if (@eventAlt is InputEventMouseButton eventbutton) prevClickAlt = eventbutton;
            GD.Print(prevKeyAlt.PhysicalKeycode.ToString());
        }else prevKeyAlt = null;

        //SetProcessUnhandledKeyInput(false);
        //SetProcessUnhandledInput(false);
        SetActionName();
        SetKeyText();
    }

    public override void _Process(double delta)
    {
        if (IsProcessingUnhandledKeyInput()) GD.Print("keyinput true");
        if (IsProcessingUnhandledInput()) GD.Print("input true");
    }


    public override void _UnhandledKeyInput(InputEvent @event)
    {
        if(@event is InputEventKey || @event is InputEventMouseButton) rebindActionKey(@event);
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
        button1.Text = ConfigFileHandler.settingChanges["controls"][actionName].ToString();
        button2.Text = ConfigFileHandler.settingChanges["controls"][actionName + "-alt"].ToString();
    }

    public void rebindActionKey(InputEvent @event)
    {
        GD.Print($"Rebind called for {actionName}");
        string none = "[NONE]";
        switch (Globals.buttontoggle)
        {
            case 1:
                InputMap.ActionEraseEvents(actionName);
                ConfigFileHandler.settingChanges["controls"][actionName] = "";
                if (@event is InputEventKey key && key.PhysicalKeycode != Key.Escape)
                {
                    //=======================
                    GD.Print($"Rebinding key: {actionName} from {(prevKey != null ? prevKey.PhysicalKeycode.ToString() : (prevClick != null ? prevClick.ButtonIndex : none))} to: {key.PhysicalKeycode.ToString()}");
                    //=======================
                    InputMap.ActionAddEvent(actionName, @event);
                    ConfigFileHandler.settingChanges["controls"][actionName] = key.PhysicalKeycode.ToString();
                    SettingsNode.isSaved = false;
                }
                else if (@event is InputEventMouseButton mouseButton)
                {
                    GD.Print($"Rebinding mouse button: {actionName} from: {(prevKey != null ? prevKey.PhysicalKeycode.ToString() : (prevClick != null ? prevClick.ButtonIndex : none))} to: {mouseButton.ButtonIndex}");
                    InputMap.ActionAddEvent(actionName, mouseButton);
                    ConfigFileHandler.settingChanges["controls"][actionName] = $"Mouse{mouseButton.ButtonIndex}";
                }
                else GD.Print($"Rebinding key: {actionName} from {(prevKey != null ? prevKey.PhysicalKeycode.ToString() : (prevClick != null ? prevClick.ButtonIndex : none))} to [NONE]");
                break;
            case 2:
                ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = "";
                InputMap.ActionEraseEvents(actionName + "-alt");
                if (@event is InputEventKey keyalt && keyalt.PhysicalKeycode != Key.Escape)
                {
                    //=======================
                    GD.Print($"Rebinding key: {actionName+"-alt"} from {(prevKeyAlt != null ? prevKeyAlt.PhysicalKeycode.ToString() : (prevClickAlt != null ? prevClickAlt.ButtonIndex : none))} to: {keyalt.PhysicalKeycode.ToString()}");
                    //=======================
                    InputMap.ActionAddEvent(actionName + "-alt", @event);
                    ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = keyalt.PhysicalKeycode.ToString();
                    SettingsNode.isSaved = false;
                }else if (@event is InputEventMouseButton mouseButtonAlt)
                {
                    GD.Print($"Rebinding mouse button: {actionName+"-alt"} from: {(prevKeyAlt != null ? prevKeyAlt.PhysicalKeycode.ToString() : (prevClickAlt != null ? prevClickAlt.ButtonIndex : none))} to: {mouseButtonAlt.ButtonIndex}");
                    InputMap.ActionAddEvent(actionName+"-alt", mouseButtonAlt);
                    ConfigFileHandler.settingChanges["controls"][actionName+"-alt"] = $"Mouse{mouseButtonAlt.ButtonIndex}";

                }
                else GD.Print($"Rebinding key: {actionName+"-alt"} from {(prevKeyAlt != null ? prevKeyAlt.PhysicalKeycode.ToString() : (prevClickAlt != null ? prevClickAlt.ButtonIndex : none))}to [NONE]");
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
                    hotkeyButtonExact.SetProcessUnhandledKeyInput(false);
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
                    hotkeyButtonExact.SetProcessUnhandledKeyInput(false);
                }
            }
            SetKeyText();
        }
    }
    public void Button2Toggled(bool toggled)
    {
        if (toggled)
        {
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
                    hotkeyButtonExact.SetProcessUnhandledKeyInput(false);
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
                    hotkeyButtonExact.SetProcessUnhandledKeyInput(false);
                }
            }
            SetKeyText();
        }
    }

    public void OnExiting()
    {
        GD.Print("bind tab exiting");
        if (!SettingsNode.isSaved)
        {
            GD.Print("bind tab resetting");
            InputMap.ActionEraseEvents(actionName);
            ConfigFileHandler.settingChanges["controls"][actionName] = "";
            InputMap.ActionAddEvent(actionName, prevKey);
            if (prevKey != null) ConfigFileHandler.settingChanges["controls"][actionName] = prevKey.PhysicalKeycode.ToString();
            InputMap.ActionEraseEvents(actionName + "-alt");
            ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = "";
            InputMap.ActionAddEvent(actionName + "-alt", prevKeyAlt);
            if (prevKey != null) ConfigFileHandler.settingChanges["controls"][actionName + "-alt"] = prevKeyAlt.PhysicalKeycode.ToString();

        }
    }
}
