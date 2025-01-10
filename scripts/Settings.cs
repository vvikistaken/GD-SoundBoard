using Godot;
using System;

public partial class Settings : Panel
{
    [Export]
    PackedScene sfxOptionsScene = GD.Load<PackedScene>("res://scenes/sfx_options.tscn");
    VBoxContainer sfxOptionsList;
    SfxOptions[] sfxOptions = new SfxOptions[9];
    Button leaveButton, applyButton;
    public override void _Ready()
    {
        ConfigFileHandler.LoadConfigFile();

        sfxOptionsList = GetNode<VBoxContainer>("ScrollContainer/SFXOptionsList");
        leaveButton = GetNode<Button>("Actions/LeaveButton");
        applyButton = GetNode<Button>("Actions/ApplyButton");

        applyButton.Pressed += SaveSettings;
        leaveButton.Pressed += () => {
            WindowScene window;
            if( (window = GetParentOrNull<WindowScene>()) is null){
                QueueFree();
            }
            else{
                window.Visible = false;
            }
        };

        CreateSfxOptionsList();
    }

    private void CreateSfxOptionsList(){
        foreach(SfxOptions sfxOption in sfxOptionsList.GetChildren()){
            sfxOption.QueueFree();
        }

        for(int i=0; i < 9; i++){
            SfxOptions sfxOptionsNode = sfxOptionsScene.Instantiate<SfxOptions>();
            sfxOptionsNode.Index = i+1;
            
            sfxOptions[i] = sfxOptionsNode;
            sfxOptionsList.AddChild(sfxOptionsNode);
        }
    }

    private void SaveSettings(){
        foreach(SfxOptions sfxOption in sfxOptions){
                ConfigFileHandler.ShowOption(sfxOption.Index);
            }
            ConfigFileHandler.SaveToConfigFile();
    }
}
