using Godot;
using System;

public class Camera : Camera2D
{
    [Export]
    public float maxZoom = 50f;

    [Export]
    public float minZoom = 1f;

    [Export]
    public float zoomSpeed = 0.5f;


    public override void _Process(float delta)
    {
        if (Input.IsActionJustReleased("debug_dezoom"))
        {
            float zoomRatio = Mathf.Clamp(this.Zoom.x * (1 + zoomSpeed), minZoom, maxZoom);
            this.Zoom = new Vector2(zoomRatio, zoomRatio);
        }
        else if (Input.IsActionJustReleased("debug_zoom"))
        {
            float zoomRatio = Mathf.Clamp(this.Zoom.x / (1 + zoomSpeed), minZoom, maxZoom);
            this.Zoom = new Vector2(zoomRatio, zoomRatio);
        }
    }
}
