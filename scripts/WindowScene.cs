using Godot;
using System;

public partial class WindowScene : Window
{
	[Export]
	public PackedScene ContainedScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InstantiateScene();

		CloseRequested += () => Visible = false;
		VisibilityChanged += InstantiateScene;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void InstantiateScene(){
		if(ContainedScene is null){
			GD.PrintErr($"{Name} error | ContainedScene is null");
			return;
		}
		switch(Visible){
			case true:
				var contained = ContainedScene.Instantiate();

				Title = contained.Name;
				AddChild(contained);
			break;
			case false:
				if(GetChildCount() > 0)
					GetChild(0).QueueFree();
			break;
		}
		
	}
}
