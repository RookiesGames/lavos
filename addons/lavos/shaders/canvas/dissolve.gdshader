shader_type canvas_item;

uniform float dissolve: hint_range(0.0, 1.0) = 0.0f;
uniform sampler2D noiseTexture;

void fragment() {
	vec4 originalColor = texture(TEXTURE, UV);
	vec4 noiseColor = texture(noiseTexture, UV);
	vec4 finalColor = originalColor;
	if (dissolve > noiseColor.r) {
		finalColor.a = 0.0;
	}
	COLOR = finalColor;
}
