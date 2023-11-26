#version 410 core
layout (location = 0) in vec3 aPosition;
layout (location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 uTransform;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

out vec3 ourColor;

void main()
{
    texCoord = aTexCoord;
    gl_Position = vec4(aPosition, 1.0);
}