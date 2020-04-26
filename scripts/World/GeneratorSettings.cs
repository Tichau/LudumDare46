namespace World
{
    public struct GeneratorSettings
    {
        [Godot.Export]
        public int Seed;
        
        [Godot.Export]
        public float SeaLevel;
        public float ChaosLevel;
    }
}
