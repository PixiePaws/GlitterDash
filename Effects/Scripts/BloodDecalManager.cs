using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class BloodDecalManager : Node2D
{
    [Export] public MultiMeshInstance2D MultiMeshInstance;
    [Export] public Texture2D BloodTexture;
    [Export] public int MaxDecals = 5000;

    private MultiMesh _MultiMesh;
    private int _DecalCount = 0;
    public static BloodDecalManager Instance { get; private set; }
  
    public override void _Ready()
    {
        // For debugging
        // GD.Print("Is BloodTexture null? ", BloodTexture == null);
        // GD.Print("MultiMeshInstance: ", MultiMeshInstance);
        
        Instance = this;
        if (MultiMeshInstance.Multimesh == null)
        {
            MultiMeshInstance.Multimesh = new MultiMesh
            {
                TransformFormat = MultiMesh.TransformFormatEnum.Transform2D,
                InstanceCount = MaxDecals
            };
        }

        _MultiMesh = MultiMeshInstance.Multimesh;
        _MultiMesh.InstanceCount = MaxDecals;
        MultiMeshInstance.ZIndex = 100;
        MultiMeshInstance.Texture = BloodTexture;

        //Debugging
        // GD.Print("MultiMeshInstance. Multimesh = ", MultiMeshInstance.Multimesh);
        // _MultiMesh.SetInstanceTransform2D(0, new Transform2D(0, new Vector2(300, 200)));
    }

    // This whole part is just for debugging
    // public override void _Process(double delta)
    // {
    //     if (Input.IsActionJustPressed("tester"))
    //     {
    //         AddDecal(new Vector2(300, 250)); // Pick a position you know is visible
    //         GD.Print("Manual decal added with key press.");
    //     }
    // }


    public void AddDecal(Vector2 position)
    {
        Transform2D xform = new Transform2D(0, position); //0 is rotation as we want that always the same

        int index = _DecalCount % MaxDecals;
        _MultiMesh.SetInstanceTransform2D(index, xform);
        _DecalCount++;
        //GD.Print("Adding decal at ", position, " to index ", index, ". The current decal count is ", _DecalCount);
    }
//add multimesh instance count to 0 to reset it between levels. might not work but idk. if not then find a way to destroy the instance and recreate it on new map
}