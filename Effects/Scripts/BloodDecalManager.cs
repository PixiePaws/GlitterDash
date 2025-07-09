using Godot;
using System;

public partial class BloodDecalManager : Node2D
{
    [Export] public MultiMeshInstance2D MultiMeshInstance;
    [Export] public Texture2D BloodTexture;
    [Export] public int MaxDecals = 500;

    private MultiMesh _MultiMesh;
    private int _DecalCount = 0;
    public static BloodDecalManager Instance { get; private set; }

    public override void _Ready()
    {
        GD.Print("Is BloodTexture null? ", BloodTexture == null);
        GD.Print("MultiMeshInstance: ", MultiMeshInstance);
        Instance = this;

        _MultiMesh = new MultiMesh
        {
            TransformFormat = MultiMesh.TransformFormatEnum.Transform2D,
            InstanceCount = MaxDecals
        };

        MultiMeshInstance.Multimesh = _MultiMesh;
        MultiMeshInstance.Texture = BloodTexture;
    }

    public void AddDecal(Vector2 position)
    {
        Transform2D xform = new Transform2D();
        xform.Origin = position;

        int index = _DecalCount % MaxDecals;
        _MultiMesh.SetInstanceTransform2D(index, xform);

        _DecalCount++;
    }
}
