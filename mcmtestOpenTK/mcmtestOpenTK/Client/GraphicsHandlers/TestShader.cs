using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class TestShader
    {
        public static int Program;
        public static void TryAShader()
        {
            Console.WriteLine("Try one");
            //string VS = "void main(){gl_FrontColor = gl_Color;gl_Position = ftransform();}";
            //string VS = "void main(){}";
            string VS = "void main(){gl_TexCoord[0] = gl_MultiTexCoord0;gl_Position = ftransform();}";
            //string VS = "uniform vec3 lightDir;varying float intensity;void main(){intensity = dot(lightDir,gl_Normal);gl_Position = ftransform();}";
            int VertexObject = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexObject, VS);
            GL.CompileShader(VertexObject);
            string VS_Info = GL.GetShaderInfoLog(VertexObject);
            int VS_Status = 0;
            GL.GetShader(VertexObject, ShaderParameter.CompileStatus, out VS_Status);

            Console.WriteLine("Status: " + VS_Status + ", info: /" + VS_Info + "/");

            Console.WriteLine("Try two");
            //string FS = "void main(){gl_FragColor = gl_Color/*vec4(0.4,0.4,0.8,1.0)*/;}";
            //string FS = "void main(){}";
            //string FS = "varying float intensity;void main(){vec4 color;if (intensity > 0.95)color = vec4(1.0,0.5,0.5,1.0);else if (intensity > 0.5)color = vec4(0.6,0.3,0.3,1.0);else if (intensity > 0.25)color = vec4(0.4,0.2,0.2,1.0);else color = vec4(0.2,0.1,0.1,1.0);gl_FragColor = color;}";
            //string FS = "uniform sampler2D tex;void main(){vec4 color = texture2D(tex,gl_TexCoord[0].st);gl_FragColor = color;}";
            string FS = "uniform sampler2D tex;void main(){vec4 color = texture2D(tex,gl_TexCoord[0].st);gl_FragColor = vec4(0, color[1], color[2], color[3]);}";
            int FragmentObject = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentObject, FS);
            GL.CompileShader(FragmentObject);
            string FS_Info = GL.GetShaderInfoLog(FragmentObject);
            int FS_Status = 0;
            GL.GetShader(FragmentObject, ShaderParameter.CompileStatus, out FS_Status);

            Console.WriteLine("Status: " + FS_Status + ", info: /" + FS_Info + "/");

            Console.WriteLine("Try three");
            Program = GL.CreateProgram();
            GL.AttachShader(Program, FragmentObject);
            GL.AttachShader(Program, VertexObject);

            Console.WriteLine("Try four");
            GL.LinkProgram(Program);
            GL.UseProgram(Program);
            Console.WriteLine("Yay");
        }
    }
}
