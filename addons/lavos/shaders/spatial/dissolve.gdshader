shader_type spatial;

uniform float dissolve: hint_range(0.0, 1.0) = 0.0f;
uniform vec4 topColor: source_color;
uniform vec4 bottomColor: source_color;
uniform sampler2D noiseTexture;

void fragment() {
	vec4 originalColor = topColor;
	vec4 noiseColor = texture(noiseTexture, UV);
	vec4 finalColor = originalColor;
	if (dissolve > noiseColor.r) {
		finalColor.xyz = bottomColor.xyz;
		finalColor.a = 1.0;
	}
	ALBEDO = finalColor.xyz;
	ALPHA = finalColor.a;
}
