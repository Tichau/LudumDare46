using Godot;

public class WorldGeneratorDebugPanel : Panel
{
    private WorldMap worldMap;
    private LineEdit seedEdit;
    private Slider seaLevelSlider;
    private Slider chaosLevelSlider;

    public override void _Ready()
    {
        this.worldMap = (WorldMap) this.GetNode("/root/Game/World");
        Debug.Assert(this.worldMap);
        this.worldMap.Connect("GeneratorSettingsChanged", this, nameof(this.OnGeneratorSettingsChanged));

        this.seedEdit = (LineEdit) this.GetNode("Content/SeedEdit");
        Debug.Assert(this.seedEdit);
        this.seaLevelSlider = (Slider) this.GetNode("Content/SeaLevelSlider");
        Debug.Assert(this.seaLevelSlider);
        this.chaosLevelSlider = (Slider) this.GetNode("Content/ChaosLevelSlider");
        Debug.Assert(this.chaosLevelSlider);
        
        this.OnGeneratorSettingsChanged();
    }

    public void OnGeneratorSettingsChanged()
    {
        var settings = this.worldMap.GeneratorSettings;

        this.seedEdit.Text = settings.Seed.ToString();
        this.seaLevelSlider.Value = settings.SeaLevel;
        this.chaosLevelSlider.Value = settings.ChaosLevel;
    }

    public void OnSeedEditTextChanged(string text)
    {
        if (int.TryParse(text, out int seed))
        {
            var settings = this.worldMap.GeneratorSettings;
            settings.Seed = seed;
            this.worldMap.GeneratorSettings = settings;
        }
    }

    public void OnSeaLevelSliderValueChanged(float value)
    {
        var settings = this.worldMap.GeneratorSettings;
        settings.SeaLevel = value;
        this.worldMap.GeneratorSettings = settings;
    }

    public void OnChaosLevelSliderValueChanged(float value)
    {
        var settings = this.worldMap.GeneratorSettings;
        settings.ChaosLevel = value;
        this.worldMap.GeneratorSettings = settings;
    }
}
