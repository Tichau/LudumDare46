using Godot;

public class PlayerController : KinematicBody2D	
{
	private int speed = 64;
    private Vector2 velocity = new Vector2();

	private AnimatedSprite PlayerSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello from C# to Godot :)");

		this.PlayerSprite = this.GetNode("AnimatedSprite") as  AnimatedSprite;
		if (this.PlayerSprite == null)
		{
			GD.PrintErr("Can't retrieve player sprite");
		}
	}

	public override void _Process(float delta)
	{
	}

	public override void _PhysicsProcess(float delta)
    {
		this.velocity = new Vector2(0, 0);
		string animation = this.PlayerSprite.Animation;
		int frame = this.PlayerSprite.Frame;

		if (Input.IsActionPressed("game_down"))
        {
            this.velocity.y += speed;
			animation = "walk-front";
			
        }
        if (Input.IsActionPressed("game_up"))
        {
            this.velocity.y -= speed;
			animation = "walk-back";
        }
		if (Input.IsActionPressed("game_right"))
        {
            this.velocity.x += speed;
			animation = "walk-right";
        }
		if (Input.IsActionPressed("game_left"))
        {
            this.velocity.x -= speed;
			animation = "walk-left";
        }

		if (this.PlayerSprite.Animation != animation)
		{
			this.PlayerSprite.Animation = animation;
			this.PlayerSprite.Frame = frame;
		}

		if (this.velocity == Vector2.Zero && this.PlayerSprite.IsPlaying())
		{
			this.PlayerSprite.Stop();
		}
		else if (this.velocity != Vector2.Zero && !this.PlayerSprite.IsPlaying())
		{
			this.PlayerSprite.Play();
		}

        // var direction = this.GetGlobalMousePosition() - this.GlobalPosition;

        // // Don't move if too close to the mouse pointer
        // if (direction.Length() > 5)
        // {
        //     Rotation = direction.Angle();
        // }
            
		this.velocity = this.MoveAndSlide(this.velocity);
    }
}
