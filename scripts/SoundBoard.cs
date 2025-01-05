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
        ConfigFileHandler.LoadConfigFile();
        // for debugging only
        ConfigFileHandler.SetSFXPath(1, @"C:\Users\cebda\Music\sfx\Windows - connect.mp3");
        ConfigFileHandler.SetSFXPath(2, @"C:\Users\cebda\Music\sfx\Windows - disconnect.mp3");
        ConfigFileHandler.SetSFXPath(3, @"C:\Users\cebda\Music\インソムニア (INSOMNIA) Eve Music Video.mp3");
        ConfigFileHandler.SetSFXPath(4, @"C:\Users\cebda\Music\Homelander Theme The Boys.mp3");

        ConfigFileHandler.ShowOptions();

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
        sfxPlayer.PlaySFXviaFileName(ConfigFileHandler.SFXFilePaths[index]);
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
    /* 
    private void OnUnfocusedKeyPressed(int vkCode){
        switch((KeyCode)vkCode){
            case KeyCode.Num1:
                OnSFXButtonPressed(0);
            break;
            case KeyCode.Num2:
                OnSFXButtonPressed(1);
            break;
            case KeyCode.Num3:
                OnSFXButtonPressed(2);
            break;
            case KeyCode.Num4:
                OnSFXButtonPressed(3);
            break;
            case KeyCode.Num5:
                OnSFXButtonPressed(4);
            break;
            case KeyCode.Num6:
                OnSFXButtonPressed(5);
            break;
            case KeyCode.Num7:
                OnSFXButtonPressed(6);
            break;
            case KeyCode.Num8:
                OnSFXButtonPressed(7);
            break;
            case KeyCode.Num9:
                OnSFXButtonPressed(8);
            break;
            case KeyCode.Num0:
                MuteCurrentAudios();
            break;
        }
    }
    */
}
