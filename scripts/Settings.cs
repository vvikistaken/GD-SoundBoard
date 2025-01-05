using Godot;
using System;

public partial class Settings : Panel
{
    [Export]
    PackedScene sfxOptionsScene = GD.Load<PackedScene>("res://scenes/sfx_options.tscn");
    VBoxContainer sfxOptionsList;
    SfxOptions[] sfxOptions = new SfxOptions[9];
    public override void _Ready()
    {
        ConfigFileHandler.LoadConfigFile();

        sfxOptionsList = GetNode<VBoxContainer>("ScrollContainer/SFXOptionsList");

        CreateSfxOptionsList();
    }
    public override void _ExitTree()
    {
        ConfigFileHandler.SaveToConfigFile();
    }

    private void CreateSfxOptionsList(){
        foreach(SfxOptions sfxOption in sfxOptionsList.GetChildren()){
            sfxOption.QueueFree();
        }

        for(int i=0; i < 9; i++){
            SfxOptions sfxOptionsNode = sfxOptionsScene.Instantiate<SfxOptions>();
            sfxOptionsNode.Index = i+1;
            sfxOptionsNode.LoadConfigFileValues();
            
            sfxOptions[i] = sfxOptionsNode;
            sfxOptionsList.AddChild(sfxOptionsNode);
        }
    }
}
