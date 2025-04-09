using Godot;
using System;
using System.Threading.Tasks;

public partial class PreloadRegistry : Node
{
    public override void _Ready()
    {
        _ = LoadTilesetAsync();
    }

    public static class ControlNodes
    {
        public static PackedScene mainScene = GD.Load<PackedScene>("res://Nodes/main.tscn");

        public static PackedScene loadingScreenScene = GD.Load<PackedScene>("res://Nodes/Menus/loading_screen.tscn");

        public static PackedScene mainMenu = GD.Load<PackedScene>("res://Nodes/Menus/mainmenu.tscn");
        public static PackedScene submenuStart = GD.Load<PackedScene>("res://Nodes/Menus/submenuStart.tscn");
        public static PackedScene submenuContinueScene = GD.Load<PackedScene>("res://Nodes/Menus/submenuContinue.tscn");
        public static PackedScene submenuSettings = GD.Load<PackedScene>("res://Nodes/Menus/submenuSettings.tscn");
        
        public static PackedScene newGameLaunchScene = GD.Load<PackedScene>("res://Nodes/Menus/newGame_launch.tscn");
        public static PackedScene saveItemScene = GD.Load<PackedScene>("res://Nodes/Menus/save_item.tscn");
        
        public static PackedScene signInPopupScene = GD.Load<PackedScene>("res://Nodes/Menus/sign_in_popup.tscn");
        public static PackedScene popupScene = GD.Load<PackedScene>("res://Nodes/Menus/popup.tscn");

        
        public static PackedScene UIscene = GD.Load<PackedScene>("res://Nodes/Game/ui.tscn");
        public static PackedScene pauseMenuScene = GD.Load<PackedScene>("res://Nodes/Menus/pause_menu.tscn");
        public static PackedScene RespawnScene = GD.Load<PackedScene>("res://Nodes/Menus/respawn_screen.tscn");
    }
    public static class Game
    {
        public static class Entities
        {
            public static PackedScene playerScene = GD.Load<PackedScene>("res://Nodes/Game/player.tscn");

            public static PackedScene lightMeleeScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/light_meele.tscn");
            public static PackedScene HeavyMeleeScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/heavy_melee.tscn");
            public static PackedScene LightRangedScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/light_ranged.tscn");
            public static PackedScene HeavyRangedScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/heavy_ranged.tscn");

            public static PackedScene BossUndeadScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/Bosses/boss_undead.tscn");
            public static PackedScene BossMechScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/Bosses/boss_mech.tscn");

            public static PackedScene itemScene = GD.Load<PackedScene>("res://Nodes/Game/item.tscn");

        }
        public static class Projectiles
        {
            public static PackedScene basicProjectile = GD.Load<PackedScene>("res://Nodes/Game/basic_projectile.tscn");
            public static PackedScene chargeProjectile = GD.Load<PackedScene>("res://Nodes/Game/charge_projectile.tscn");
            public static PackedScene spellOracle = GD.Load<PackedScene>("res://Nodes/Game/spell_oracle.tscn");
            public static PackedScene casterProjectile = GD.Load<PackedScene>("res://Nodes/Game/Enemies/projectiles/caster_projectile.tscn");

            public static PackedScene laserBeamScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/projectiles/laser.tscn");
            public static PackedScene SpikeScene = GD.Load<PackedScene>("res://Nodes/Game/Enemies/projectiles/tile_spike.tscn");
        }
        public static class Maps
        {
            public static PackedScene gameScene = GD.Load<PackedScene>("res://Nodes/Maps/gameScene.tscn");
            public static PackedScene Map0Scene = GD.Load<PackedScene>("res://Nodes/Maps/map_0.tscn");
        }
        
    }
    public static class Assets
    {
        public static Resource cursor = GD.Load<Resource>("res://Assets/UI/Cursor/crosshair124.png");
        public static TileSet tileSet;
    }

    public static async Task LoadTilesetAsync()
    {
        if (Assets.tileSet != null) return;

        await Task.Delay(100);

        var resource = GD.Load<Resource>("res://Assets/Tilesheet/tranquil.res");
        GD.Print("Tileset loaded: " + !(resource != null));

        if (resource is TileSet tileset)
        {
            Assets.tileSet = tileset;
            GD.Print("Tileset assigned successfully");
        }
        else GD.PrintErr("Tileset failed to load");
    }
}
