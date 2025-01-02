using Godot;
using System;

public partial class Settings : Panel
{
    VBoxContainer sfxOptionsList;
    HBoxContainer[] sfxOptions = new HBoxContainer[9];
    public override void _Ready()
    {
        sfxOptionsList = GetNode<VBoxContainer>("SFXOptionsList");

        for(int i=0; i < 9; i++){
            int tempIndex = i;
            sfxOptions[i] = sfxOptionsList.GetNode<HBoxContainer>($"SFXOptions{i+1}");
            sfxOptions[i].GetNode<Button>("PickSFXButton").Pressed += () => SetSFX(tempIndex);
            sfxOptions[i].GetNode<CheckBox>("SingularCheckBox").Toggled += (toggled) => CheckIfSingular(tempIndex, toggled);
            sfxOptions[i].GetNode<CheckBox>("LoopCheckBox").Toggled += (toggled) => CheckIfLooping(tempIndex, toggled);

        }
    }

    private void SetSFX(int index){
        // add file dialog reading here
    }
    private void CheckIfSingular(int index, bool toggled){
        //GD.Print($"{sfxOptions[index].GetNode<CheckBox>("SingleCheckBox").Name} toggled: {toggled}");
        // config file stuff

        sfxOptions[index].GetNode<CheckBox>("LoopCheckBox").Disabled = !toggled;
        sfxOptions[index].GetNode<CheckBox>("LoopCheckBox").ButtonPressed = false;

    }
    private void CheckIfLooping(int index, bool toggled){
        //GD.Print($"{sfxOptions[index].GetNode<CheckBox>("LoopCheckBox").Name} toggled: {toggled}");
        // config file stuff

        

    }
}
