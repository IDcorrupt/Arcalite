using Godot;
using System;
using System.Text.RegularExpressions;

public partial class HotkeyRebindButton : Control
{
    private int buttontoggle = 0;
    Label label;
    Button button1;
    Button button2;
    [Export]
    string actionName = "";


    public override void _Ready()
    {
        SetProcessUnhandledKeyInput(false);
        label = GetNode<Label>("HBox/Text");
        button1 = GetNode<Button>("HBox/Buttons/BindButton1");
        
        button2 = GetNode<Button>("HBox/Buttons/BindButton2");

        SetActionName();
        SetKeyText();
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
            case "game_left":
                label.Text = "Move left";
                break;
            case "game_right":
                label.Text = "Move right";
                break;
            case "game_jump":
                label.Text = "Move up / Jump";
                break;
            case "game_crouch":
                label.Text = "Move down / Crouch";
                break;
            case "game_dash":
                label.Text = "Dash";
                break;
            case "game_oracle":
                label.Text = "Oracle";
                break;
            case "game_spellslot1":
                label.Text = "Spellslot 1";
                break;
            case "game_spellslot2":
                label.Text = "Spellslot 2";
                    break;
            case "primary_attack":
                label.Text = "Primary attack";
                break;
            case "charge_attack":
                label.Text = "Charge attack";
                break;

            default:
                break;
        }
    }

    public void SetKeyText()
    {
        Godot.Collections.Array<Godot.InputEvent> actionEvents = InputMap.ActionGetEvents(actionName);

        //2 kbm inputs && 1 controller input
        InputEvent actionEvent1 = actionEvents[0];
        InputEvent actionEvent2= actionEvents[1];
        string actionKeycode1 = "";
        string actionKeycode2 = "";
        if (actionEvent1 is InputEventKey keyEvent1)
        {
            actionKeycode1 = OS.GetKeycodeString(keyEvent1.PhysicalKeycode);
            GD.Print(actionKeycode1);
        }
        if (actionEvent2 is InputEventKey keyEvent2)
        {
            actionKeycode2 = OS.GetKeycodeString(keyEvent2.PhysicalKeycode);
            GD.Print(actionKeycode2);
        }
        button1.Text = actionKeycode1;
        button2.Text = actionKeycode2;      
    }
    
    public void rebindActionKey(InputEvent @event)
    {

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
                    hotkeyButton.SetProcessUnhandledInput(false);
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
                    hotkeyButton.SetProcessUnhandledInput(false);
                }
            }
            SetKeyText(); 
        }
    }
    public void Button2Toggled(bool toggled)
    {
        if (toggled)
        {
            buttontoggle = 2;
            button1.Text = "listening...";
            SetProcessUnhandledKeyInput(toggled);

            foreach (Node item in GetTree().GetNodesInGroup("hotkeyButton"))
            {
                if (item is HotkeyRebindButton hotkeyButton && hotkeyButton.actionName != actionName)
                {
                    hotkeyButton.button1.ToggleMode = false;
                    hotkeyButton.SetProcessUnhandledInput(false);
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
                    hotkeyButton.SetProcessUnhandledInput(false);
                }
            }
            SetKeyText();
        }
    }
}
