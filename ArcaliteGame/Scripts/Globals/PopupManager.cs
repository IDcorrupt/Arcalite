using Godot;
using System;

public partial class PopupManager : Node
{
	//custom instancing construct for popups
	public static void CreatePopup(Node Parent, string popupScenePath, Action<bool> onPopupClosedCallback)
	{
		var popup = (Node)GD.Load<PackedScene>(popupScenePath).Instantiate();

		Parent.AddChild(popup);

		//popup.Connect("PopupClosed", popup, (bool result) => { onPopupClosedCallback(result); } );
    }

}
