using Godot;
using System;

public class UI : MarginContainer
{    
    private Slider seaLevelSlider;
    private Slider chaosLevelSlider;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.seaLevelSlider = (Slider) this.GetNode("WorldGeneratorPanel/Container/Content/SeaLevelSlider");
        this.chaosLevelSlider = (Slider) this.GetNode("WorldGeneratorPanel/Container/Content/ChaosLevelSlider");
    }
}
