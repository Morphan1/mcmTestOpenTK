using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class CMultShader: Shader
    {
        /// <summary>
        /// The location of the color input.
        /// </summary>
        public int colorloc = 0;

        public CMultShader()
        {
            string VS = "void main(){gl_TexCoord[0] = gl_MultiTexCoord0;gl_Position = ftransform();}";
            string FS = "uniform sampler2D tex;uniform vec4 mult_color;void main()" +
            "{vec4 color = texture2D(tex,gl_TexCoord[0].st);gl_FragColor = vec4(color" +
            "[0] * mult_color[0], color[1] * mult_color[1], color[2] * mult_color[2], color[3] * mult_color[3]);}";
            Internal_Program = CompileToProgram(VS, FS);
            Original_Program = Internal_Program;
            LoadedProperly = true;
            Name = "colormultiplier";
            colorloc = GL.GetUniformLocation(Internal_Program, "mult_color");
        }

        /// <summary>
        /// Sets the color in use by the shader.
        /// </summary>
        /// <param name="c">The color to use.</param>
        public void SetColor(Color c)
        {
            GL.Uniform4(colorloc, c);
        }
    }
}
