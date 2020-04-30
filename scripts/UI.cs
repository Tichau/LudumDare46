using Godot;
using System;

public class UI : Container
{    
    private Panel worldGeneratorDebugPanel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.worldGeneratorDebugPanel = (Panel) this.GetNode("WorldGeneratorPanel");
        Debug.Assert(this.worldGeneratorDebugPanel);
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("debug_toggleMenu"))
        {
            this.worldGeneratorDebugPanel.Visible = !this.worldGeneratorDebugPanel.Visible;
        }
    }
}
