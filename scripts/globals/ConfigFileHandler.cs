using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class ConfigFileHandler : Node
{
    private const string CONFIG_PATH = "user://", CONFIG_FILE_NAME = "config.cfg",
    CONFIG_FILE_PATH = CONFIG_PATH + CONFIG_FILE_NAME;
    public static string[] SFXFilePaths = new string[9];
    public static Dictionary<string, bool>[] SFXOptions = new Dictionary<string, bool>[9];

    public override void _Ready()
    {
        //CreateConfigFile();
        LoadConfigFile();
    }

    public static void LoadConfigFile(){
        // check if config file exists
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

            SFXFilePaths[i] = (string)config.GetValue($"SFX {i+1}", "FILE_PATH");
            SFXOptions[i] = new Dictionary<string, bool>{
                {"Singular", (bool)config.GetValue($"SFX {i+1}", "SINGULAR")},
                {"Loop", (bool)config.GetValue($"SFX {i+1}", "LOOP")}
            };
            GD.Print($"--------SFX{i+1}--------");
            GD.Print($"File path: {SFXFilePaths[i]}");
            GD.Print($"Singular: {SFXOptions[i]["Singular"]}, Loop: {SFXOptions[i]["Loop"]}");
            GD.Print("--------------------");
        }
    }
    // no longer for debugging only
    public static void SetSFXPath(int option, string value){
        if( !(option >= 1 && option <= 9) ){
            throw new Exception("Option must be between 0 and 8.");
        }
        ConfigFile config = new ConfigFile();

        var result = config.Load(CONFIG_FILE_PATH);
        if(result != Error.Ok){
            GD.PushError($"{result}. Cannot load config file while setting value.");
            return;
        }
        
        config.SetValue($"SFX {option}", "FILE_PATH", value);
        result = config.Save(CONFIG_FILE_PATH);
        if(result != Error.Ok){
            GD.PushError($"{result}. Cannot save config file while setting value.");
            return;
        }
        GD.Print($"Set SFX{option}|FILE_PATH to: {value}");
    }
    private static Error CreateConfigFile(){
        ConfigFile config = new ConfigFile();

        for(int i=0; i<9; i++){
            config.SetValue($"SFX {i+1}", "FILE_PATH", string.Empty);
            config.SetValue($"SFX {i+1}", "SINGULAR", false);
            config.SetValue($"SFX {i+1}", "LOOP", false);
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
}
