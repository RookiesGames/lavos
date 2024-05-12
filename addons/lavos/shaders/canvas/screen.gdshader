shader_type canvas_item;

uniform sampler2D screenTexture: hint_screen_texture, filter_nearest;

void fragment() {
	vec4 screenColor = texture(screenTexture, SCREEN_UV);
	// Apply effect	
	float grayscale = (screenColor.r + screenColor.g + screenColor.b) / 3.0;
	float alpha = COLOR.a;
	//
	COLOR = vec4(vec3(grayscale), screenColor.a);
}