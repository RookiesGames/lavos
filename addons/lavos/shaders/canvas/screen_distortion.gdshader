shader_type canvas_item;

uniform float distortionStrength: hint_range(-1.0, 1.0, 0.01) = 0.01;
uniform vec2 topOffset = vec2(0.1);
uniform vec2 bottomOffset = vec2(-0.1);
uniform sampler2D topNoiseTexture: repeat_enable;
uniform sampler2D bottomNoiseTexture: repeat_enable;
uniform sampler2D screenTexture: hint_screen_texture, filter_nearest;

void fragment() {
	vec4 topNoiseColor = texture(topNoiseTexture, UV + topOffset * TIME);
	vec4 bottomNoiseColor = texture(bottomNoiseTexture, UV + bottomOffset * TIME);
	float noise = (topNoiseColor.r * bottomNoiseColor.r) * 0.25 * distortionStrength;
	vec4 finalColor = texture(TEXTURE, UV + noise);
	COLOR = finalColor;	
}
