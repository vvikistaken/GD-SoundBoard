using Godot;
using System;

public partial class Settings : Panel
{
    [Export]
    PackedScene sfxOptionsScene = GD.Load<PackedScene>("res://scenes/sfx_options.tscn");
    VBoxContainer sfxOptionsList;
    Button leaveButton, applyButton;
    public override void _Ready()
    {
        ConfigFileHandler.LoadConfigFile();

        sfxOptionsList = GetNode<VBoxContainer>("ScrollContainer/SFXOptionsList");
        leaveButton = GetNode<Button>("Actions/LeaveButton");
        applyButton = GetNode<Button>("Actions/ApplyButton");

        applyButton.Pressed += SaveSettings;
        leaveButton.Pressed += ExitSettings;

        CreateSfxOptionsList();
    }

    private void CreateSfxOptionsList(){
        foreach(Node child in sfxOptionsList.GetChildren()){
            child.QueueFree();
        }

        for(int i=1; i <= 9; i++){
            SfxOptions sfxOptionsNode = sfxOptionsScene.Instantiate<SfxOptions>();
            sfxOptionsNode.Index = i;
            
            sfxOptionsList.AddChild(sfxOptionsNode);
        }
    }
    private void SaveSettings(){
        ConfigFileHandler.SaveToConfigFile(true);
    }
    private void ExitSettings(){
        WindowScene window;
        if( (window = GetParentOrNull<WindowScene>()) is null)
            QueueFree();
        else
            window.Visible = false;
    }
}
