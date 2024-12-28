using Godot;
using System;

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
    }
    private void OnSFXButtonPressed(int index){
        //GD.Print($"Current button: {sfxButtons[index].Name}");
        var sfxPlayer = sfxPlayerScene.Instantiate<SfxPlayer>();
        audio.AddChild(sfxPlayer);
        sfxPlayer.PlaySFXviaFileName("Library of Ruina SFX - Click Dice Roll Finger Snap.mp3");
    }
    private void MuteCurrentAudios(){
        foreach(SfxPlayer sfxPlayer in audio.GetChildren()){
            sfxPlayer.QueueFree();
        }
    }
}
