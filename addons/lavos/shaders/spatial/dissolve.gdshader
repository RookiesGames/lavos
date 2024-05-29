shader_type spatial;

uniform float dissolve: hint_range(0.0, 1.0) = 0.0f;
uniform vec4 color: source_color;
uniform sampler2D noiseTexture;

void fragment() {
	vec4 originalColor = color;
	vec4 noiseColor = texture(noiseTexture, UV);
	vec4 finalColor = originalColor;
	if (dissolve > noiseColor.r) {
		finalColor.a = 0.0;
	}
	ALBEDO = finalColor.xyz;
	ALPHA = finalColor.a;
}
