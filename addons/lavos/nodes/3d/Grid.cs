using Godot;
using Lavos.Core;

namespace Lavos.Nodes;

public sealed partial class Grid : Node3D
{
    const string KEY_POS = "pos";

    [Export] float _elementSize;

    int _width;
    int _height;
    Node3D[] _grid;

    #region Node

    public override void _Ready()
    {
        NodeTree.PinNodeByType<Grid>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNodeByType<Grid>();
    }

    #endregion

    public void ClearGrid()
    {
        _width = 0;
        _height = 0;
        _grid = null;
        this.RemoveChildren();
    }

    public void GenerateGrid(Dimensions dimensions)
    {
        _width = dimensions.Width;
        _height = dimensions.Height;
        _grid = new Node3D[dimensions.Width * dimensions.Height];
        //
        for (var h = 0; h < dimensions.Height; ++h)
        {
            for (var w = 0; w < dimensions.Width; ++w)
            {
                var node = this.AddNode<Node3D>($"grid_row{h:00}_col{w:00}");
                node.SetMeta(KEY_POS, new Vector2(h, w));
                node.Position = new Vector3(w * _elementSize, 0f, h * _elementSize);
                _grid[(h * dimensions.Width) + w] = node;
            }
        }
    }

    public Node3D GetGridNodeAtIndex(int index) => _grid[index];
    public Node3D GetGridNodeAtPosition(int x, int y) => _grid[(y * _width) + x];
    public Vector3 GetGridCenter() => new(_width * _elementSize * 0.5f, 0f, _height * _elementSize * 0.5f);
    public Vector2 GetGridNodePositionAtIndex(int index) => GetGridNodeAtIndex(index).GetMeta(KEY_POS).AsVector2();
}
