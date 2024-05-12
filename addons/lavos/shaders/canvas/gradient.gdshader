shader_type canvas_item;

uniform sampler2D gradient;

void fragment() {
	vec4 originalColor = texture(TEXTURE, UV);
	float grayscale = (originalColor.r + originalColor.g + originalColor.b) / 3.0;
	vec4 gradientColor = texture(gradient, vec2(grayscale));
	COLOR = vec4(gradientColor.rgb, originalColor.a);
}
