using Godot;
using System.IO;

public partial class SfxPlayer : AudioStreamPlayer
{
    public override void _Ready()
    {
        Finished += QueueFree;
    }
    public void PlaySFXviaFileName(string filePath)
    {
        AudioStream sound;
        if(filePath.Contains("\\") || filePath.Contains("/"))
            sound = LoadExternalMP3(filePath);
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
        return mp3;
    }
    private AudioStream LoadExternalMP3(string path)
    {
        using var fs = new FileStream(path, FileMode.Open, System.IO.FileAccess.Read);
        byte[] fileData = new byte[fs.Length];
        fs.Read(fileData, 0, fileData.Length);

        var mp3 = new AudioStreamMP3();
        mp3.Data = fileData;
        return mp3;
    }
}
