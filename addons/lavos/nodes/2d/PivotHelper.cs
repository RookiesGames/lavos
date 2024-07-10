using Godot;

namespace Lavos.Nodes.Nodes2D;

[Tool]
public partial class PivotHelper : Control
{
	[Export]
	Vector2 _range;

	public override void _EnterTree()
	{
		PivotOffset = new Vector2(Size.X * _range.X, Size.Y * _range.Y);
	}

	public override void _Process(double delta)
	{
		PivotOffset = new Vector2(Size.X * _range.X, Size.Y * _range.Y);
	}
}
