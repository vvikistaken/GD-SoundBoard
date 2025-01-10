using Godot;
using System;
using System.Collections.Generic;
using System.IO;

public partial class ConfigFileHandler : Node
{
    private const string CONFIG_PATH = "user://", CONFIG_FILE_NAME = "config.cfg",
    CONFIG_FILE_PATH = CONFIG_PATH + CONFIG_FILE_NAME;
    private static ConfigFile config = new ConfigFile();
    public static string[] SFXFilePaths = new string[9];
    public static Dictionary<string, bool>[] SFXOptions = new Dictionary<string, bool>[9];

    public override void _Ready()
    {
        //CreateConfigFile();
    }

    public static void LoadConfigFile(bool printResult = false){
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
            if(printResult) ShowOption(i+1);
        }
    }
    public static void SaveToConfigFile(bool printSaved = false){
        GD.Print("Saving to config file...");
        
        for(int i=0; i<9; i++){
            config.SetValue($"SFX {i+1}", "FILE_PATH", SFXFilePaths[i]);
            config.SetValue($"SFX {i+1}", "SINGULAR", SFXOptions[i]["Singular"]);
            config.SetValue($"SFX {i+1}", "LOOP", SFXOptions[i]["Loop"]);
        }

        var result = config.Save(CONFIG_FILE_PATH);
        if(result != Error.Ok){
            GD.PushError($"{result}. Cannot save config file.");
            return;
        }
        if(printSaved) ShowOptions();

        GD.Print("Config file successfully saved.");
    }
    public static void ShowOptions(){
        for(int i=0; i<9; i++){
            GD.Print($"--------SFX{i+1}--------");
            GD.Print($"File path: {SFXFilePaths[i]}");
            GD.Print($"Singular: {SFXOptions[i]["Singular"]}, Loop: {SFXOptions[i]["Loop"]}");
            GD.Print("--------------------");
        }
    }
    public static void ShowOption(int index){
        if( !(index >= 1 && index <= 9) ){
            throw new Exception("Index must be between 0 and 8.");
        }
        GD.Print($"--------SFX{index}--------");
        GD.Print($"File path: {SFXFilePaths[index-1]}");
        GD.Print($"Singular: {SFXOptions[index-1]["Singular"]}, Loop: {SFXOptions[index-1]["Loop"]}");
        GD.Print("--------------------");
    }
    // again for debugging purposes, kinda
    public static void SetSFXPath(int option, string value){
        if( !(option >= 1 && option <= 9) ){
            throw new Exception("Option must be between 0 and 8.");
        }

        LoadConfigFile();
        
        config.SetValue($"SFX {option}", "FILE_PATH", value);
        var result = config.Save(CONFIG_FILE_PATH);
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
