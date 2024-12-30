using Godot;
using System;
using System.IO;

public partial class ConfigFileHandler : Node
{
    public const string CONFIG_PATH = "user://", CONFIG_FILE_NAME = "config.cfg",
    CONFIG_FILE_PATH = CONFIG_PATH + CONFIG_FILE_NAME;
    public static string[] SFXFilePaths = new string[9];

    public override void _Ready()
    {
        SetConfigFileValue(1, @"C:\Users\cebda\Music\sfx\Library of Ruina SFX - Click Dice Roll Finger Snap.mp3");
        SetConfigFileValue(2, @"C:\Users\cebda\Music\sfx\Limbus Company SFX - Staggered.mp3");
        SetConfigFileValue(3, @"C:\Users\cebda\Music\sfx\Library of Ruina - RoundStart.mp3");
        SetConfigFileValue(4, @"C:\Users\cebda\Music\Kether battle 3.mp3");
        LoadConfigFile();
    }

    public static void LoadConfigFile(){
        if( !Godot.FileAccess.FileExists(CONFIG_FILE_PATH)){
            GD.Print("Config file not found. Creating a new one...");

            var result = CreateConfigFile();
            if(result == Error.Ok) 
                GD.Print($"Config file successfully created.");
            else
                GD.PushError($"{result}. Cannot create config file.");
        }
        else{
            GD.Print("Config file found.");
        }

        for(int i=0; i<9; i++){
            ConfigFile config = new ConfigFile();

            var result = config.Load(CONFIG_FILE_PATH);
            if(result != Error.Ok){
                GD.PushError($"{result}. Cannot load config file.");
                return;
            }

            SFXFilePaths[i] = (string)config.GetValue("SFX_FILE_PATHS", $"OPTION {i+1}");
            GD.Print($"SFXFilePaths[{i}]: {SFXFilePaths[i]}");
        }
    }
    private static Error CreateConfigFile(){
        ConfigFile config = new ConfigFile();

        for(int i=0; i<9; i++){
            config.SetValue("SFX_FILE_PATHS", $"OPTION {i+1}", string.Empty);
        }

        var result = config.Save(CONFIG_FILE_PATH);
        return result;
    }
    // for debugging purposes
    private void DeleteConfigFile(){
        if( Godot.FileAccess.FileExists(CONFIG_FILE_PATH)){
            var result = DirAccess.RemoveAbsolute(CONFIG_FILE_PATH);
            if(result == Error.Ok) 
                GD.Print($"Config file successfully deleted.");
            else
                GD.PushError($"{result}. File doesn't exist.");
        }
    }
    // for debugging purposes
    private void SetConfigFileValue(int option, string value){
        if( !(option >= 1 && option <= 9) ){
            throw new Exception("Option must be between 0 and 8.");
        }
        ConfigFile config = new ConfigFile();

        var result = config.Load(CONFIG_FILE_PATH);
        if(result != Error.Ok){
            GD.PushError($"{result}. Cannot load config file while setting value.");
            return;
        }
        
        config.SetValue("SFX_FILE_PATHS", $"OPTION {option}", value);
        result = config.Save(CONFIG_FILE_PATH);
        if(result != Error.Ok){
            GD.PushError($"{result}. Cannot save config file while setting value.");
            return;
        }
        GD.Print($"Set OPTION{option} to: {value}");
    }
}
