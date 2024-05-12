shader_type canvas_item;

uniform vec4 color: source_color = vec4(1.0);
uniform float percent: hint_range(0.0, 1.0, 0.1);

void fragment() {
	vec4 originalColor = texture(TEXTURE, UV);
	vec4 mixedColor = mix(originalColor, color, percent);
	COLOR = vec4(mixedColor.rgb, originalColor.a);
}
