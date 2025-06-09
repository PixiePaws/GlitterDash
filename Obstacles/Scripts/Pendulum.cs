using Godot;
using System;

public partial class Pendulum : RigidBody2D
{
    private Node _rootNode;
    private StaticBody2D _pendulumHolder;
    private CollisionShape2D _ball;
    private float _previousRotation;
    private float _startingRotation;
    private float _maxRotation = 0.0f;
    private float _minRotation = 0.0f;
    private Vector2 _anchorPoint;
    private Vector2 _ballCenterOfMass;
    private float _armLength;
    private float _centerOfMassHeight;
    private float Gravity;
    private float PendulumInertia;
    private bool MovingClockwise = true;
    public override void _Ready()
    {
        Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
        _pendulumHolder = GetNode<StaticBody2D>("/root/PendulumHolder");
        if (_pendulumHolder == null)
        {
            GD.Print("Pendulum holder is null");
        }
        _ball = GetNode<CollisionShape2D>("CollisionShape2D2");
        if (_ball == null)
        {
            GD.Print("Ball is null");
        }
        _anchorPoint = _pendulumHolder.GlobalPosition;
        _ballCenterOfMass = _ball.GlobalPosition;
        _armLength = (_ballCenterOfMass - _anchorPoint).Length();
        RotationDegrees = -90.0f;
        _startingRotation = RotationDegrees;
        CallDeferred(nameof(CalculateInertia));
    }
    public override void _PhysicsProcess(double delta)
    {
        //UpdateRotation();
        UpdateRotation();
    }
    public void CalculateInertia()
    {
        PendulumInertia = GetPendulumInertia();
    }

    public void AddAngularVelocity(bool MovingClockwise)
    {
        GD.Print("Adding Angular Velocity");
        
        GD.Print($"Pendulum inertia : {PendulumInertia}");
        float NeededAngularVelocity = calculateNeededAngularVelocity();
        GD.Print($"Needed angular velocity : {NeededAngularVelocity}");
        float TorqueI = PendulumInertia * NeededAngularVelocity;
        GD.Print($"Torque impulse : {TorqueI}");
        if (MovingClockwise)
        {
            GD.Print("Clockwise");
            ApplyTorqueImpulse(-32000);
        }
        if (!MovingClockwise)
        {
            GD.Print("Clockwise");
            ApplyTorqueImpulse(32000);
            //GD.Print(TorqueI);
        }
    }
    public float calculateNeededAngularVelocity()
    {
        GD.Print("Calculating needed AngularVelocity");
        float Height = Mathf.Abs(GetCenterOfMassHeight(_anchorPoint, _ballCenterOfMass));
        GD.Print($"Gravity : {Gravity}");
        GD.Print($"Center of mass height : {Height}");
        GD.Print($"arm length : {_armLength}");
        float NeededAngularVelocity = Mathf.Sqrt(2 * Gravity * Height) / _armLength;
        
        return NeededAngularVelocity;
    }
    public void PrintMinMaxRotation()
    {
        GD.Print($"Min rotation: {_minRotation}");
        GD.Print($"Max rotation : {_maxRotation}");
    }
    public void UpdateCenterOfMassPosition(CollisionShape2D Ball)
    {
        if (Ball != null)
        {
            _ballCenterOfMass = Ball.GlobalPosition;
            GD.Print($"Center of mass position : {_ballCenterOfMass}");
        }
        else
        {
            GD.Print("Cannot update center of mass position, ball is null");
        }
    }
    public float GetCenterOfMassHeight(Vector2 Anchor, Vector2 CenterOfMass)
    {
        GD.Print("Getting center of mass height");
        UpdateCenterOfMassPosition(_ball);
        float MassHeight = Anchor.Y - CenterOfMass.Y;
        GD.Print($"Anchor Y : {Anchor.Y} Center of mass Y : {CenterOfMass.Y}");
        GD.Print($"Mass height : {MassHeight}");
        return MassHeight;
    }
    public void UpdateRotation()
    {
        //GD.Print("Updating rotation");
        float CurrentRotation = Rotation;
        if (_previousRotation >= 0 && CurrentRotation <= 0)
        {

            GD.Print(_previousRotation);
            GD.Print(CurrentRotation);
            GD.Print("Counter-clockwise");
            AddAngularVelocity(true);
        }
        else if (_previousRotation <= 0 && CurrentRotation >= 0)
        {
            GD.Print(_previousRotation);
            GD.Print(CurrentRotation);
            GD.Print("Clockwise");
            AddAngularVelocity(false);
        }
        _previousRotation = Rotation;
        //GD.Print("Updated previous rotation");
    }
    public float GetPendulumInertia()
    {
        return 1.0f / PhysicsServer2D.BodyGetDirectState(this.GetRid()).InverseInertia;
    }
}
