shader_type canvas_item;

uniform float speed = 1.0;
uniform int frameCount = 1;
uniform int tilesetSize = 128;

void fragment()
{
	float time = fract(TIME*speed);
	
	int index = int(time * float(frameCount));
	
	vec2 uv = UV + vec2(float(tilesetSize * index) * TEXTURE_PIXEL_SIZE.x, 0.0);

	vec4 color = texture(TEXTURE, uv);
	
	COLOR = color;
}
