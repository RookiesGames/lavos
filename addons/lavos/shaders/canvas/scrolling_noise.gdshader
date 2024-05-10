shader_type canvas_item;

uniform vec2 offset = vec2(0.1);
uniform sampler2D noiseTexture: repeat_enable;

void fragment() {
	vec4 noiseColor = texture(noiseTexture, UV + offset * TIME);
	COLOR = noiseColor;	
}
