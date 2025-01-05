using Godot;
using System;

public partial class SfxOptions : VBoxContainer
{
    [Export] 
    public int Index = 0;
    FileDialog fileDialog;
    Label label;
    LineEdit previewLineEdit;
    Button pickSFXButton;
    CheckBox singularCheckBox, loopCheckBox;

    public override void _Ready()
    {
        fileDialog = GetNode<FileDialog>("FileDialog");
        label = GetNode<Label>("Label");
        previewLineEdit = GetNode<LineEdit>("PathPreview/PreviewLineEdit");
        pickSFXButton = GetNode<Button>("PathPreview/PickSFXButton");
        singularCheckBox = GetNode<CheckBox>("Options/SingularCheckBox");
        loopCheckBox = GetNode<CheckBox>("Options/LoopCheckBox");

        fileDialog.FileSelected += SetPreviewPath;

        label.Text = $"Sound effect {Index}";
        pickSFXButton.Pressed += PickSFX;
        singularCheckBox.Toggled += CheckIfSingular;
        loopCheckBox.Toggled += CheckIfLooping;
    }
    public void LoadConfigFileValues(){
        previewLineEdit.Text = ConfigFileHandler.SFXFilePaths[Index-1];
        singularCheckBox.ButtonPressed = ConfigFileHandler.SFXOptions[Index-1]["Singular"];
        loopCheckBox.ButtonPressed = ConfigFileHandler.SFXOptions[Index-1]["Loop"];
    }
    // i love you copilot for this
    public void khhkfhopfhk()
    {
        GD.Print("khhkfhopfhk");
    }
    private void SetPreviewPath(string path)
    {
        previewLineEdit.Text = path;
        previewLineEdit.TooltipText = path;
        ConfigFileHandler.SFXFilePaths[Index-1] = path;

        ConfigFileHandler.ShowOption(Index);
    }
    private void PickSFX()
    {
        fileDialog.Visible = true;
    }
    private void CheckIfSingular(bool toggled){
        //GD.Print($"{singularCheckBox.Name} {Index} toggled: {toggled}");
        
        ConfigFileHandler.SFXOptions[Index-1]["Singular"] = toggled;

        loopCheckBox.Disabled = !toggled;
        loopCheckBox.ButtonPressed = false;

        ConfigFileHandler.ShowOption(Index);
    }
    private void CheckIfLooping(bool toggled){
        //GD.Print($"{loopCheckBox.Name} {Index} toggled: {toggled}");
        
        ConfigFileHandler.SFXOptions[Index-1]["Loop"] = toggled;
        ConfigFileHandler.ShowOption(Index);
    }
}
