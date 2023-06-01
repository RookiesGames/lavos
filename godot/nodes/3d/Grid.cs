using Godot;

namespace Lavos.Nodes;

public sealed partial class Grid : Node3D
{
    [Export]
    float _elementSize;

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

    public void GenerateGrid(int width, int height)
    {
        _width = width;
        _height = height;
        _grid = new Node3D[width * height];
        //
        for (var h = 0; h < height; ++h)
        {
            for (var w = 0; w < width; ++w)
            {
                var node = this.AddNode<Node3D>($"grid_row{h:00}_col{w:00}");
                node.Position = new Vector3(w * _elementSize, 0f, h * _elementSize);
                _grid[h * width + w] = node;
            }
        }
    }

    public Node3D GetGridNodeAtIndex(int index) => _grid[index];
    public Node3D GetGridNodeAtPosition(int x, int y) => _grid[y * _width + x];
    public Vector3 GetGridCenter() => new Vector3(_width * _elementSize * 0.5f, 0f, _height * _elementSize * 0.5f);
}
