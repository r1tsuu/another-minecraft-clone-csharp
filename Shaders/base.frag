#version 410 core
out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D texture0;
uniform bool enableFxaa;

void main()
{
    if (enableFxaa) {
        vec2 texelSize = 1.0 / textureSize(texture0, 0);

        vec3 rgbNW = texture(texture0, texCoord + vec2(-1.0, -1.0) * texelSize).xyz;
        vec3 rgbNE = texture(texture0, texCoord + vec2(1.0, -1.0) * texelSize).xyz;
        vec3 rgbSW = texture(texture0, texCoord + vec2(-1.0, 1.0) * texelSize).xyz;
        vec3 rgbSE = texture(texture0, texCoord + vec2(1.0, 1.0) * texelSize).xyz;
        vec3 rgbM = texture(texture0, texCoord).xyz;

        vec3 luma = vec3(0.299, 0.587, 0.114);
        float lumaNW = dot(rgbNW, luma);
        float lumaNE = dot(rgbNE, luma);
        float lumaSW = dot(rgbSW, luma);
        float lumaSE = dot(rgbSE, luma);
        float lumaM = dot(rgbM, luma);

        float lumaMin = min(lumaM, min(min(lumaNW, lumaNE), min(lumaSW, lumaSE)));
        float lumaMax = max(lumaM, max(max(lumaNW, lumaNE), max(lumaSW, lumaSE)));

        vec2 dir = vec2(clamp((lumaNW + lumaNE - lumaSW - lumaSE) * 0.25 / (lumaMax - lumaMin) + 0.5, 0.0, 1.0), 
                        clamp((lumaNW + lumaSW - lumaNE - lumaSE) * 0.25 / (lumaMax - lumaMin) + 0.5, 0.0, 1.0));

        float offset = 1.0 / textureSize(texture0, 0).x;
        vec3 rgbA = 0.5 * (texture(texture0, texCoord + dir * vec2(-offset, -offset) * texelSize).xyz +
                          texture(texture0, texCoord + dir * vec2(-offset, offset) * texelSize).xyz);
        vec3 rgbB = 0.5 * (texture(texture0, texCoord + dir * vec2(offset, -offset) * texelSize).xyz +
                          texture(texture0, texCoord + dir * vec2(offset, offset) * texelSize).xyz);

        vec3 finalColor = rgbM + (clamp(2.0 * rgbM - rgbA - rgbB, 0.0, 1.0) - rgbM) * smoothstep(0.0, 1.0 / 8.0, lumaMax - lumaMin);

        FragColor = vec4(finalColor, 1.0);
    } else {
        // If FXAA is disabled, just sample the texture without modification
        FragColor = texture(texture0, texCoord);
    }
}