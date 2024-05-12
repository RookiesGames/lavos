shader_type canvas_item;

uniform sampler2D maskTexture;

void fragment() {
	vec4 maskColor = texture(maskTexture, UV);
	vec4 originalColor = texture(TEXTURE, UV);
	
	if (originalColor.a > 0.0) {
		COLOR.a = maskColor.r;
	}
}
