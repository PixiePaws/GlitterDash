shader_type canvas_item;

uniform sampler2D SCREEN_TEXTURE : hint_screen_texture;

void fragment() {
	// Called for every pixel the material is visible on.
	vec4 color = texture(SCREEN_TEXTURE, SCREEN_UV);
	COLOR.rgb = vec3(color.r + color.g + color.b) / 3.0;
}

