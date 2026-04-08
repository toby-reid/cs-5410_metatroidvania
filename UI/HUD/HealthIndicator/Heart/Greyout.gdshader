shader_type canvas_item;

uniform bool is_active = false;

void fragment() { // apparently Godot shaders will fail if you use Allman-style braces
	// Called for every pixel the material is visible on.
    vec4 tex_col = texture(TEXTURE, UV);
    if (is_active) {
        // Weighted dot-product for more "natural" greyscale;
        // human eye is much more perceptive to Green, soz
        float grey = dot(tex_col.rgb, vec3(0.299, 0.587, 0.114));
        COLOR = vec4(vec3(grey), tex_col.a);
    } else {
        COLOR = tex_col;
    }
}
