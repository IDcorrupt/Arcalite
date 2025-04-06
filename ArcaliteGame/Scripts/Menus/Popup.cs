using Godot;
using System;
using System.Net;
using System.Net.Mime;
using System.Reflection.Metadata;

public partial class Popup : Control
{

	private Node Parent;
	private string popuptype;
	private RichTextLabel title;
	private RichTextLabel content;
	private Button confirm;
	private Button cancel;

    public void SetMessageType(string type)
	{
		popuptype = type;
	}

	public void OnCancelPressed()
	{
		Globals.PopupResult = false;
		Globals.PopupOpen = false;
		QueueFree();
	}

	public void OnConfirmPressed()
	{
		if(popuptype!= "invalidSettings")
		{
			Globals.PopupResult = true;
		}
        Globals.PopupOpen = false;
        QueueFree();
    }



    public override void _Ready()
	{
		Globals.PopupOpen= true;
		title = GetNode<RichTextLabel>("BackGround/Margin/Panel/VBox/Title");
        content = GetNode<RichTextLabel>("BackGround/Margin/Panel/VBox/Content");
        confirm = GetNode<Button>("BackGround/Margin/Panel/VBox/HBox/Confirm");
        cancel = GetNode<Button>("BackGround/Margin/Panel/VBox/HBox/Cancel");

		Parent = GetParent();

		//determine the type of popup
		switch (popuptype)
		{
			//when changes were not saved
			case "nosave":
                title.Text = "[center]Changes not saved[/center]";
				content.Text = "[center]The changes you made to your settings have not been saved. Do you want to exit without saving?[/center]";
				confirm.Text = "Yes";
				cancel.Text = "No";
                break;

			case "invalidSettings":
				title.Text = "[center]Invalid settings[/center]";
				content.Text = "[center]A setting the config.ini file cannot be applied to your game, thus your settings will be reset to default.[/center]";
				confirm.Text="Understood";
				cancel.QueueFree();
                break;
			case "filecorrupt":
				title.Text = "[center]config file error[/center]";
				content.Text = "[center]Config file import failed, resetting settings[/center]";
				confirm.Text = "Understood";
				cancel.QueueFree();
				break;
            default:
				break;
		}
    }

}
