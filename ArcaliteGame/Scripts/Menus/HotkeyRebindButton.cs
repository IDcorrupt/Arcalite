using Godot;
using System;
using System.Text.RegularExpressions;

public partial class HotkeyRebindButton : Control
{
    private int buttontoggle = 0;
    Label label;
    Button button1;
    Button button2;
    SubmenuSettings SettingsNode;
    
    [Export]
    string actionName = "";


    public override void _Ready()
    {
        SettingsNode = GetNode("/root/MainNode/MainMenu/Control") as SubmenuSettings; 
        SetProcessUnhandledKeyInput(false);
        label = GetNode<Label>("HBox/Text");
        button1 = GetNode<Button>("HBox/Buttons/BindButton1");
        
        button2 = GetNode<Button>("HBox/Buttons/BindButton2");

        SetActionName();
        SetKeyText2();
    }


    public override void _UnhandledKeyInput(InputEvent @event)
    {
        rebindActionKey(@event);
        switch (buttontoggle)
        {
            case 1:
                button1.ButtonPressed = false;
                buttontoggle = 0;
                break;
            case 2:
                button2.ButtonPressed = false;
                buttontoggle = 0;
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

    public void SetKeyText2()
    {
        button1.Text = ConfigFileHandler.settingChanges["controls"][actionName].ToString();
        button2.Text = ConfigFileHandler.settingChanges["controls"][actionName+"-alt"].ToString();
    }
    public void SetKeyText()
    {
        Godot.Collections.Array<Godot.InputEvent> actionEvents = InputMap.ActionGetEvents(actionName);
        Godot.Collections.Array<Godot.InputEvent> actionEventsAlt = InputMap.ActionGetEvents(actionName+"-alt");
        InputEvent actionEvent;
        InputEvent actionEventAlt;
        if (actionEvents.Count != 0)
        {
            actionEvent = actionEvents[0];
        }else
        {
            actionEvent = null;
        }
        if (actionEventsAlt.Count != 0)
        {
            actionEventAlt = actionEventsAlt[0];
        }
        else
        {
            actionEventAlt = null;
        }
        
        string actionKeycode = "";
        string actionKeycodeAlt = "";
        if (actionEvent is InputEventKey keyEvent)
        {
            actionKeycode = OS.GetKeycodeString(keyEvent.PhysicalKeycode);
            GD.Print(actionKeycode);
        }
        if (actionEventAlt is InputEventKey keyEventAlt)
        {
            actionKeycodeAlt = OS.GetKeycodeString(keyEventAlt.PhysicalKeycode);
            GD.Print(actionKeycodeAlt);

        }
        button1.Text = actionKeycode;
        button2.Text = actionKeycodeAlt;
    }
    
    public void rebindActionKey(InputEvent @event)
    {
        switch (buttontoggle)
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
                break;
            case 2:
                ConfigFileHandler.settingChanges["controls"][actionName+"-alt"] = "";
                InputMap.ActionEraseEvents(actionName + "-alt");
                if(@event is InputEventKey keyalt && keyalt.PhysicalKeycode != Key.Escape)
                {
                    InputMap.ActionAddEvent(actionName + "-alt", @event);
                    ConfigFileHandler.settingChanges["controls"][actionName+"-alt"] = keyalt.PhysicalKeycode.ToString();
                    SettingsNode.isSaved = false;
                }
                break;
            default:
                break;
        }
        SetProcessUnhandledKeyInput(false);
        SetKeyText2();
        SetActionName();
    }
    public void Button1Toggled(bool toggled)
    {
        if (toggled)
        {
            buttontoggle = 1;
            button1.Text = "listening...";
            SetProcessUnhandledKeyInput(toggled);

            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if(item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = false;
                    hotkeyButton.button2.ToggleMode = false;
                    hotkeyButton.SetProcessUnhandledInput(false);
                }else if(item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button2.ToggleMode = false;
                    hotkeyButtonExact.SetProcessUnhandledInput(false);
                }
            }
        }
        else 
        {
            buttontoggle = 0;
            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = true;
                    hotkeyButton.button2.ToggleMode = true;
                    hotkeyButton.SetProcessUnhandledInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button2.ToggleMode = true;
                    hotkeyButtonExact.SetProcessUnhandledInput(false);
                }
            }
            SetKeyText2(); 
        }
    }
    public void Button2Toggled(bool toggled)
    {
        if (toggled)
        {
            buttontoggle = 2;
            button2.Text = "listening...";
            SetProcessUnhandledKeyInput(toggled);

            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = false;
                    hotkeyButton.button2.ToggleMode = false;
                    hotkeyButton.SetProcessUnhandledInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button1.ToggleMode = false;
                    hotkeyButtonExact.SetProcessUnhandledInput(false);
                }
            }
        }
        else
        {
            buttontoggle = 0;
            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = true;
                    hotkeyButton.button2.ToggleMode = true;
                    hotkeyButton.SetProcessUnhandledInput(false);
                }
                else if (item is HotkeyRebindButton hotkeyButtonExact)
                {
                    hotkeyButtonExact.button1.ToggleMode = true;
                    hotkeyButtonExact.SetProcessUnhandledInput(false);
                }
            }
            SetKeyText2();
        }
    }
}