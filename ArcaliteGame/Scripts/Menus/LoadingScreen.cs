using Godot;
using System;

public partial class LoadingScreen : Control
{
    public override async void _Ready()
    {
        base._Ready();
        await PreloadRegistry.LoadTilesetAsync();
        MainNode main = GetParent() as MainNode;
        main.LoadingFinished();

    }
}
