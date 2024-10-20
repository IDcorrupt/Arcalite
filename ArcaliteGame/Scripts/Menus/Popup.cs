using Godot;
using System;
using System.Net;
using System.Net.Mime;
using System.Reflection.Metadata;

public partial class Popup : Control
{
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
		GD.Print("popup deny");
		Globals.PopupResult = false;
		QueueFree();
	}

	public void OnConfirmPressed()
	{
        GD.Print("popup accept");
		Globals.PopupResult = true;
        QueueFree();
    }



    public override void _Ready()
	{
		title = GetNode<RichTextLabel>("BackGround/Margin/Panel/VBox/Title");
        content = GetNode<RichTextLabel>("BackGround/Margin/Panel/VBox/Content");
        confirm = GetNode<Button>("BackGround/Margin/Panel/VBox/HBox/Confirm");
        cancel = GetNode<Button>("BackGround/Margin/Panel/VBox/HBox/Cancel");
		switch (popuptype)
		{
			case "nosave":
                title.Text = "[center]Changes not saved[/center]";
				content.Text = "[center]The changes you made to your settings have not been saved. Do you want to exit without saving?[/center]";
				confirm.Text = "Yes";
				cancel.Text = "No";
                break;

            default:
				break;
		}
    }

}
