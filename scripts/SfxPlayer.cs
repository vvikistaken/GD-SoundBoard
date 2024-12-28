using Godot;
using System.IO;

public partial class SfxPlayer : AudioStreamPlayer
{
    private string CurrentFilePath = Directory.GetCurrentDirectory();
    public override void _Ready()
    {
        Finished += QueueFree;
    }
    public void PlaySFXviaFileName(string fileName)
    {
        Stream = LoadMP3($"res://sfx/{fileName}");
        Play();
    }
    public AudioStreamMP3 LoadMP3(string path)
    {
        using var file = Godot.FileAccess.Open(path, Godot.FileAccess.ModeFlags.Read);
        var sound = new AudioStreamMP3();
        sound.Data = file.GetBuffer((long)file.GetLength());
        return sound;
    }
}
