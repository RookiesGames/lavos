shader_type canvas_item;

uniform vec2 tiling = vec2(1.0, 1.0);
uniform vec2 offset = vec2(0.0, 1.0);
uniform float speed: hint_range(0.1, 0.5, 0.05) = 0.5;

void vertex() {
	UV = UV * tiling + offset * TIME * speed;	
}
