using Godot;

namespace Mojang.Api.Skins.Godot.src.Camera;
public partial class OrbitCamera : Camera3D
{

    private float zoomSpeed = 0.1f; // Geschwindigkeit des Zooms, anpassbar
    private float minZoomDistance = 2.0f; // Minimale Entfernung zur Kamera
    private float maxZoomDistance = 10.0f; // Maximale Entfernung zur Kamera

    [Export]
    public float RotationSpeed = 0.005f;


    [Export]

    private Node3D target;

    private Vector3 originalOffset;
    private float minY = -1.0f; // Minimaler Y-Rotationswinkel
    private float maxY = 1.0f;  // Maximaler Y-Rotationswinkel

    public override void _Ready()
    {
        originalOffset = GlobalTransform.Origin - target.GlobalTransform.Origin;
        LookAt(target.GlobalTransform.Origin, Vector3.Up);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (target is null) return;


        if (@event is InputEventMouseMotion mouseMotion && Input.IsMouseButtonPressed(MouseButton.Left))
        {
            Transform3D currentTransform = GlobalTransform;
            currentTransform.Origin = target.GlobalTransform.Origin;

            currentTransform = currentTransform.Rotated(Vector3.Up, -mouseMotion.Relative.X * RotationSpeed);
            currentTransform = currentTransform.Rotated(currentTransform.Basis.X.Normalized(), -mouseMotion.Relative.Y * RotationSpeed);

            currentTransform.Origin += currentTransform.Basis.Z.Normalized() * originalOffset.Length();

            GlobalTransform = currentTransform;
            LookAt(target.GlobalTransform.Origin, Vector3.Up);
        }

        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                originalOffset = originalOffset.Normalized() * Mathf.Max(originalOffset.Length() - zoomSpeed, minZoomDistance);
            }
            else if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown)
            {
                originalOffset = originalOffset.Normalized() * Mathf.Min(originalOffset.Length() + zoomSpeed, maxZoomDistance);
            }

            if (mouseButtonEvent.ButtonIndex == MouseButton.WheelDown || mouseButtonEvent.ButtonIndex == MouseButton.WheelUp)
            {
                Transform3D currentTransform = GlobalTransform;
                currentTransform.Origin = target.GlobalTransform.Origin + originalOffset;
                GlobalTransform = currentTransform;
                LookAt(target.GlobalTransform.Origin, Vector3.Up);
            }
        }



    }

}

