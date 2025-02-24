using Godot;
using System;
using System.Threading.Tasks;

public partial class TilesetLoader : Node
{
    public static TileSet LoadedTileset { get; private set; }

    public override void _Ready()
    {
        _ = LoadTilesetAsync();
    }

    public static async Task LoadTilesetAsync()
    {
        if (LoadedTileset != null) return;

        await Task.Delay(100);

        var resource = ResourceLoader.Load("res://Assets/Tilesheet/tranquil.res");
        GD.Print("Tileset loaded: "+!(resource != null));

        if (resource is TileSet tileset)
        {
            LoadedTileset = tileset;
            GD.Print("Tileset assigned successfully");
        } else GD.PrintErr("Tileset failed to load");
    }
}
