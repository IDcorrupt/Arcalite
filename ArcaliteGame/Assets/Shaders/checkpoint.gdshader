shader_type canvas_item;

uniform sampler2D palette_texture;
uniform sampler2D lookup_texture;

void fragment() {
    vec4 original_color = texture(TEXTURE, UV); // Sample the sprite texture
    
    if (original_color.a < 0.1) {
        discard; // Discard fully transparent pixels
    }

    vec4 indexed_color = texture(lookup_texture, original_color.rg); // Find the indexed color
    vec4 new_color = texture(palette_texture, vec2(indexed_color.r, 0.5)); // Get the new color from the palette

    // Preserve the original alpha channel
    COLOR = vec4(new_color.rgb, original_color.a);
}
