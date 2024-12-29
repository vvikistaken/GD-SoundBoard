using Godot;
using System;
using System.IO;

public partial class SoundBoard : Panel
{
    [Export]
    PackedScene sfxPlayerScene = GD.Load<PackedScene>("res://scenes/sfx_player.tscn");
    Node audio;
    Button[] sfxButtons = new Button[9];
    Button muteButton, settingsButton;
    public override void _Ready()
    {
        audio = GetNode<Node>("Audio");

        for(int i = 0; i < sfxButtons.Length; i++){
            int tempIndex = i;
            sfxButtons[i] = GetNode<Button>($"SFX/SFXButton{i+1}");
            sfxButtons[i].Pressed += () => OnSFXButtonPressed(tempIndex);
        }

        muteButton = GetNode<Button>("Options/MuteButton");
        muteButton.Pressed += MuteCurrentAudios;

        settingsButton = GetNode<Button>("Options/SettingsButton");
        settingsButton.Pressed += OpenSettings;
    }
    public override void _Input(InputEvent @event)
    {
        // sfx buttons
        if(Input.IsActionJustPressed("sfx1")) OnSFXButtonPressed(0);
        if(Input.IsActionJustPressed("sfx2")) OnSFXButtonPressed(1);
        if(Input.IsActionJustPressed("sfx3")) OnSFXButtonPressed(2);
        if(Input.IsActionJustPressed("sfx4")) OnSFXButtonPressed(3);
        if(Input.IsActionJustPressed("sfx5")) OnSFXButtonPressed(4);
        if(Input.IsActionJustPressed("sfx6")) OnSFXButtonPressed(5);
        if(Input.IsActionJustPressed("sfx7")) OnSFXButtonPressed(6);
        if(Input.IsActionJustPressed("sfx8")) OnSFXButtonPressed(7);
        if(Input.IsActionJustPressed("sfx9")) OnSFXButtonPressed(8);
        // other buttons
        if(Input.IsActionJustPressed("mute")) MuteCurrentAudios();
        if(Input.IsActionJustPressed("settings")) OpenSettings();
    }

    private void OnSFXButtonPressed(int index){
        //GD.Print($"Current button: {sfxButtons[index].Name}");
        sfxButtons[index].GrabFocus();

        var sfxPlayer = sfxPlayerScene.Instantiate<SfxPlayer>();
        audio.AddChild(sfxPlayer);
        sfxPlayer.PlaySFXviaFileName("Library of Ruina SFX - Click Dice Roll Finger Snap.mp3");
    }
    private void MuteCurrentAudios(){
        muteButton.GrabFocus();

        foreach(SfxPlayer sfxPlayer in audio.GetChildren()){
            sfxPlayer.QueueFree();
        }
    }
    private void OpenSettings(){
        settingsButton.GrabFocus();

        GD.Print("Settings button pressed");
    }
    // config file
    private void CheckConfigFile(){
        if( !File.Exists("res://resources/config.json")){
            var config = new ConfigFile();
            config.SetValue("SFX", "1", Path.Combine(Directory.GetCurrentDirectory(), "resources", "sfx", "Library of Ruina SFX - Click Dice Roll Finger Snap.mp3"));
        }
        
    }
    
}
