shader_type canvas_item;

void fragment() {
	vec4 originalColor = texture(TEXTURE, UV);
	float grayscale = (originalColor.r + originalColor.g + originalColor.b) / 3.0;
	COLOR = vec4(vec3(grayscale), originalColor.a);
}
