using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using mcmtestOpenTK.Shared;
using mcmtestOpenTK.Client.CommonHandlers;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class Shader
    {
        /// <summary>
        /// A full list of currently loaded shaders.
        /// </summary>
        public static List<Shader> LoadedShaders;

        /// <summary>
        /// A generic shader with no modifications.
        /// </summary>
        public static Shader Generic;

        /// <summary>
        /// A test shader that removes the red color value.
        /// </summary>
        public static Shader AntiRed;

        /// <summary>
        /// Starts or restarts the shader system.
        /// </summary>
        public static void InitShaderSystem()
        {
            // Dispose existing shaders
            if (LoadedShaders != null)
            {
                for (int i = 0; i < LoadedShaders.Count; i++)
                {
                    LoadedShaders[i].Remove();
                    i--;
                }
            }
            // Reset shader list
            LoadedShaders = new List<Shader>();
            // Pregenerate a few needed shader
            Generic = CreateGeneric("generic");
            LoadedShaders.Add(Generic);
            // Preload a few common shaders
            AntiRed = GetShader("test/antired");
        }

        /// <summary>
        /// Gets the shader object for a specific shader name.
        /// </summary>
        /// <param name="shadername">The name of the shader</param>
        /// <returns>A valid shader object</returns>
        public static Shader GetShader(string shadername)
        {
            shadername = FileHandler.CleanFileName(shadername);
            for (int i = 0; i < LoadedShaders.Count; i++)
            {
                if (LoadedShaders[i].Name == shadername)
                {
                    return LoadedShaders[i];
                }
            }
            Shader Loaded = LoadShader(shadername);
            if (Loaded == null)
            {
                Loaded = new Shader();
                Loaded.Name = shadername;
                Loaded.Internal_Program = Generic.Internal_Program;
                Loaded.LoadedProperly = false;
            }
            LoadedShaders.Add(Loaded);
            return Loaded;
        }

        /// <summary>
        /// Loads a shader from file.
        /// </summary>
        /// <param name="filename">The name of the file to use</param>
        /// <returns>The loaded shader, or null if it does not exist</returns>
        public static Shader LoadShader(string filename)
        {
            try
            {
                filename = FileHandler.CleanFileName(filename);
                if (!FileHandler.Exists("shaders/" + filename + ".vs"))
                {
                    ErrorHandler.HandleError("Cannot load shader, file '" +
                        TextStyle.Color_Standout + "shaders/" + filename + ".vs" + TextStyle.Color_Error +
                        "' does not exist.");
                    return null;
                }
                if (!FileHandler.Exists("shaders/" + filename + ".fs"))
                {
                    ErrorHandler.HandleError("Cannot load shader, file '" +
                        TextStyle.Color_Standout + "shaders/" + filename + ".fs" + TextStyle.Color_Error +
                        "' does not exist.");
                    return null;
                }
                string VS = FileHandler.ReadText("shaders/" + filename + ".vs");
                string FS = FileHandler.ReadText("shaders/" + filename + ".fs");
                uint Program = CompileToProgram(VS, FS);
                Shader created = new Shader();
                created.Name = filename;
                created.LoadedProperly = true;
                created.Internal_Program = Program;
                return created;
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Failed to load texture from filename '" +
                    TextStyle.Color_Standout + "textures/" + filename + ".png" + TextStyle.Color_Error + "'", ex);
                return null;
            }
        }

        /// <summary>
        /// Creates a generic shader object.
        /// </summary>
        /// <returns>A generic no-change shader object.</returns>
        public static Shader CreateGeneric(string name)
        {
            string VS = "void main(){gl_TexCoord[0] = gl_MultiTexCoord0;gl_Position = ftransform();}";
            string FS = "uniform sampler2D tex;void main()" +
            "{vec4 color = texture2D(tex,gl_TexCoord[0].st);" +
            "gl_FragColor = color;}";
            uint Program = CompileToProgram(VS, FS);
            Shader generic = new Shader();
            generic.Name = name;
            generic.LoadedProperly = true;
            generic.Internal_Program = Program;
            return generic;
        }

        /// <summary>
        /// Compiles a VertexShader and FragmentShader to a usable shader program.
        /// </summary>
        /// <param name="VS">The input VertexShader code</param>
        /// <param name="FS">The input FragmentShader code</param>
        /// <returns>The internal OpenGL program ID</returns>
        public static uint CompileToProgram(string VS, string FS)
        {
            int VertexObject = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(VertexObject, VS);
            GL.CompileShader(VertexObject);
            string VS_Info = GL.GetShaderInfoLog(VertexObject);
            int VS_Status = 0;
            GL.GetShader(VertexObject, ShaderParameter.CompileStatus, out VS_Status);
            if (VS_Status != 1)
            {
                throw new Exception("Error creating VertexShader. Error status: " + VS_Status + ", info: " + VS_Info);
            }
            int FragmentObject = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(FragmentObject, FS);
            GL.CompileShader(FragmentObject);
            string FS_Info = GL.GetShaderInfoLog(FragmentObject);
            int FS_Status = 0;
            GL.GetShader(FragmentObject, ShaderParameter.CompileStatus, out FS_Status);
            if (FS_Status != 1)
            {
                throw new Exception("Error creating VertexShader. Error status: " + FS_Status + ", info: " + FS_Info);
            }
            int Program = GL.CreateProgram();
            GL.AttachShader(Program, FragmentObject);
            GL.AttachShader(Program, VertexObject);
            GL.LinkProgram(Program);
            return (uint) Program;
        }

        /// <summary>
        /// The name of the shader
        /// </summary>
        public string Name;

        /// <summary>
        /// The internal OpenGL ID for the shader program.
        /// </summary>
        public uint Internal_Program;

        /// <summary>
        /// Whether the shader loaded properly.
        /// </summary>
        public bool LoadedProperly = false;

        /// <summary>
        /// Removes the shader from the system.
        /// </summary>
        public void Remove()
        {
            if (GL.IsProgram(Internal_Program))
            {
                GL.DeleteProgram(Internal_Program);
            }
            LoadedShaders.Remove(this);
        }
    }
}
