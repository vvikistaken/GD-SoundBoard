using Godot;
using System;
using System.IO;

public partial class SfxPlayer : AudioStreamPlayer
{
    public bool Loop { get; set; } = false;
    public override void _Ready()
    {
        Finished += QueueFree;
    }
    public void PlaySFXviaFileName(string filePath)
    {
        AudioStream sound;
        if(filePath.Contains("\\") || filePath.Contains("/")){
            switch(Path.GetExtension(filePath)){
                case ".mp3":
                    sound = LoadExternalMP3(filePath);
                break;
                /*
                case ".wav":
                    sound = LoadExternalWAV(filePath);
                break;
                */
                default:
                    throw new Exception("Unsupported audio file format");
            }
        }
        else
            sound = LoadLocalMP3(filePath);

        Stream = sound;

        Play();
    }
    private AudioStream LoadLocalMP3(string fileName)
    {
        using var file = Godot.FileAccess.Open("res://resources/sfx/"+fileName, Godot.FileAccess.ModeFlags.Read);
        var mp3 = new AudioStreamMP3();
        mp3.Data = file.GetBuffer((long)file.GetLength());

        mp3.Loop = Loop;

        return mp3;
    }
    private AudioStream LoadExternalMP3(string path, bool result = false)
    {
        if(result) GD.Print("Loading external mp3 . . .");
        using var fs = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);
        byte[] fileData = new byte[fs.Length];
        fs.Read(fileData, 0, fileData.Length);

        var mp3 = new AudioStreamMP3();
        mp3.Data = fileData;

        mp3.Loop = Loop;

        if(result) GD.Print("Successfully loaded external mp3");
        return mp3;
    }
    // I give up for now, will do wav support in new godot versions
    private AudioStream LoadExternalWAV(string path, bool result = false)
    {
        if(result) GD.Print("External wav not implemented");
        using var fs = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);
        byte[] fileData = new byte[fs.Length];
        fs.Read(fileData, 0, fileData.Length);

        sbyte[] signedFileData = new sbyte[fileData.Length];
        for(int i = 0; i < fs.Length; i++){
            signedFileData[i] = (sbyte)(fileData[i]-128);
        }

        var wav = new AudioStreamWav();
        //wav.Data = signedFileData;

        return wav;
    }
}
