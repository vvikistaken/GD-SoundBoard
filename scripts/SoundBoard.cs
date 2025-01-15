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
        ConfigFileHandler.ShowOptions();

        audio = GetNode<Node>("Audio");

        for(int i = 0; i < sfxButtons.Length; i++){
            int tempIndex = i;
            sfxButtons[i] = GetNode<Button>($"SFX/SFXButton{tempIndex+1}");
            sfxButtons[i].ToggleMode = ConfigFileHandler.SFXOptions[i]["Singular"];

            sfxButtons[i].Pressed += () => OnSFXButtonPressed(tempIndex);
            sfxButtons[i].Toggled += (bool toggled) => OnSFXButtonToggled(tempIndex, toggled);
        }

        muteButton = GetNode<Button>("Options/MuteButton");
        muteButton.Pressed += MuteCurrentAudios;

        settingsButton = GetNode<Button>("Options/SettingsButton");
        settingsButton.Pressed += OpenSettings;

    }
    public override void _Input(InputEvent @event)
    {
        // sfx buttons
        if(Input.IsActionJustPressed("sfx1")){
            if(sfxButtons[0].ToggleMode) OnSFXButtonToggled(0, !sfxButtons[0].ButtonPressed);
            else OnSFXButtonPressed(0);
        }
        if(Input.IsActionJustPressed("sfx2")){
            if(sfxButtons[1].ToggleMode) OnSFXButtonToggled(1, !sfxButtons[1].ButtonPressed);
            else OnSFXButtonPressed(1);
        }
        if(Input.IsActionJustPressed("sfx3")){
            if(sfxButtons[2].ToggleMode) OnSFXButtonToggled(2, !sfxButtons[2].ButtonPressed);
            else OnSFXButtonPressed(2);
        }
        if(Input.IsActionJustPressed("sfx4")){
            if(sfxButtons[3].ToggleMode) OnSFXButtonToggled(3, !sfxButtons[3].ButtonPressed);
            else OnSFXButtonPressed(3);
        }
        if(Input.IsActionJustPressed("sfx5")){
            if(sfxButtons[4].ToggleMode) OnSFXButtonToggled(4, !sfxButtons[4].ButtonPressed);
            else OnSFXButtonPressed(4);
        }
        if(Input.IsActionJustPressed("sfx6")){
            if(sfxButtons[5].ToggleMode) OnSFXButtonToggled(5, !sfxButtons[5].ButtonPressed);
            else OnSFXButtonPressed(5);
        }
        if(Input.IsActionJustPressed("sfx7")){
            if(sfxButtons[6].ToggleMode) OnSFXButtonToggled(6, !sfxButtons[6].ButtonPressed);
            else OnSFXButtonPressed(6);
        }
        if(Input.IsActionJustPressed("sfx8")){
            if(sfxButtons[7].ToggleMode) OnSFXButtonToggled(7, !sfxButtons[7].ButtonPressed);
            else OnSFXButtonPressed(7);
        }
        if(Input.IsActionJustPressed("sfx9")){
            if(sfxButtons[8].ToggleMode) OnSFXButtonToggled(8, !sfxButtons[8].ButtonPressed);
            else OnSFXButtonPressed(8);
        }
        // other buttons
        if(Input.IsActionJustPressed("mute")) MuteCurrentAudios();
        if(Input.IsActionJustPressed("settings")) OpenSettings();
    }

    private void OnSFXButtonPressed(int index){
        if(sfxButtons[index].ToggleMode) return;
        //GD.Print($"Current button: {sfxButtons[index].Name}");
        sfxButtons[index].CallDeferred("grab_focus");

        var sfxPlayer = sfxPlayerScene.Instantiate<SfxPlayer>();

        audio.AddChild(sfxPlayer);
        sfxPlayer.PlaySFXviaFileName(ConfigFileHandler.SFXFilePaths[index]);
    }
    private void OnSFXButtonToggled(int index, bool toggled){
        //GD.Print($"Current button: {sfxButtons[index].Name}, toggled: {toggled}");
        sfxButtons[index].CallDeferred("grab_focus");
        sfxButtons[index].SetPressedNoSignal(toggled);

        var sfxPlayer = sfxPlayerScene.Instantiate<SfxPlayer>();
        if(toggled){
            sfxPlayer.Name = $"Audio{index}";
            sfxPlayer.Loop = ConfigFileHandler.SFXOptions[index]["Loop"];

            audio.AddChild(sfxPlayer);
            sfxPlayer.PlaySFXviaFileName(ConfigFileHandler.SFXFilePaths[index]);
        }
        else{
            foreach(SfxPlayer sfx in audio.GetChildren()){
                if(sfx.Name == $"Audio{index}")
                    sfx.QueueFree();
            }
        }
    }
    private void MuteCurrentAudios(){
        muteButton.CallDeferred("grab_focus");

        foreach(Button button in sfxButtons){
            if(button.ToggleMode)
                button.ButtonPressed = false;
        }
        foreach(SfxPlayer sfxPlayer in audio.GetChildren()){
            sfxPlayer.QueueFree();
        }
    }
    private void OpenSettings(){
        settingsButton.CallDeferred("grab_focus");

        WindowScene settings;
        if( (settings = GetWindowScene()) is null){
            GD.PrintErr($"{Name} error | There is no WindowScene");
        }
        else{
            settings.Visible = !settings.Visible;
        }

    }
    private WindowScene GetWindowScene(){
        foreach(var child in GetChildren()){
            if(child is WindowScene)
                return child as WindowScene;
        }
        return null;
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
