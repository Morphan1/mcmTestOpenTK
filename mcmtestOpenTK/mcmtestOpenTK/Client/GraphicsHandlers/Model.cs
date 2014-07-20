﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mcmtestOpenTK.Shared.Util;
using mcmtestOpenTK.Shared;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace mcmtestOpenTK.Client.GraphicsHandlers
{
    public class Model
    {
        /// <summary>
        /// All currently loaded models.
        /// </summary>
        public static List<Model> LoadedModels;

        /// <summary>
        /// Prepares the model system.
        /// </summary>
        public static void Init()
        {
            LoadedModels = new List<Model>();
        }

        public static Model LoadModel(string filename)
        {
            try
            {
                filename = FileHandler.CleanFileName(filename);
                if (!FileHandler.Exists("models/" + filename + ".obj"))
                {
                    ErrorHandler.HandleError("Cannot load model, file '" +
                        TextStyle.Color_Standout + "models/" + filename + ".obj" + TextStyle.Color_Error +
                        "' does not exist.");
                    return null;
                }
                return FromString(filename, FileHandler.ReadText("models/" + filename + ".obj"));
            }
            catch (Exception ex)
            {
                ErrorHandler.HandleError("Failed to load model from filename '" +
                    TextStyle.Color_Standout + "models/" + filename + ".obj" + TextStyle.Color_Error + "'", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the texture object for a specific texture name.
        /// </summary>
        /// <param name="texturename">The name of the texture</param>
        /// <returns>A valid texture object</returns>
        public static Model GetModel(string modelname)
        {
            modelname = FileHandler.CleanFileName(modelname);
            for (int i = 0; i < LoadedModels.Count; i++)
            {
                if (LoadedModels[i].Name == modelname)
                {
                    return LoadedModels[i];
                }
            }
            Model Loaded = LoadModel(modelname);
            if (Loaded == null)
            {
                Loaded = new Model(modelname);
                // TODO: Default (cube or origin mark) model!
            }
            LoadedModels.Add(Loaded);
            return Loaded;
        }

        /// <summary>
        /// loads a model from a .obj file string
        /// </summary>
        /// <param name="name">The name of the model</param>
        /// <param name="data">The .obj file string</param>
        /// <returns>A valid model</returns>
        public static Model FromString(string name, string data)
        {
            Model result = new Model(name);
            ModelMesh currentMesh = null;
            string[] lines = data.Replace("\r", "").Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains('#'))
                {
                    int index = lines[i].IndexOf('#');
                    if (index == 0)
                    {
                        continue;
                    }
                    lines[i] = lines[i].Substring(0, index);
                }
                string[] args = lines[i].Split(' ');
                if (args.Length <= 1)
                {
                    continue;
                }
                switch (args[0])
                {
                    case "mtllib":
                        break; // TODO: Maybe enable materials?
                    case "usemtl":
                        break;
                    case "s": // TODO: What is this?
                        break;
                    case "o":
                        currentMesh = new ModelMesh(args[1]);
                        result.Meshes.Add(currentMesh);
                        break;
                    case "v":
                        currentMesh.Vertices.Add(new Location(Utilities.StringToDouble(args[1]),
                            Utilities.StringToDouble(args[2]), Utilities.StringToDouble(args[3])));
                        break;
                    case "f":
                        if (args.Length == 4)
                        {
                            currentMesh.Faces.Add(new ModelFace(Utilities.StringToInt(args[1]),
                                Utilities.StringToInt(args[2]), Utilities.StringToInt(args[3])));
                        }
                        else
                        {
                            currentMesh.Faces.Add(new ModelFace(Utilities.StringToInt(args[1]),
                                Utilities.StringToInt(args[2]), Utilities.StringToInt(args[3])));
                            currentMesh.Faces.Add(new ModelFace(Utilities.StringToInt(args[3]),
                                Utilities.StringToInt(args[4]), Utilities.StringToInt(args[1])));
                        }
                        break;
                    default:
                        SysConsole.Output(OutputType.WARNING, "Invalid model key '" + args[0] + "'");
                        break;
                }
            }
            return result;
        }

        public Model(string _name)
        {
            Name = _name;
            Meshes = new List<ModelMesh>();
        }

        /// <summary>
        /// The name of  this model.
        /// </summary>
        public string Name;

        /// <summary>
        /// All the meshes this model has.
        /// </summary>
        public List<ModelMesh> Meshes;

        /// <summary>
        /// Draws the model.
        /// </summary>
        /// <param name="loc">The location to draw at</param>
        /// <param name="rot">The rotation to draw with</param>
        /// <param name="scale">The scale to draw with</param>
        public void Draw(Location loc, Location rot, Location scale)
        {
            GL.PushMatrix();
            GL.Translate(loc.X, loc.Y, loc.Z);
            GL.Scale(scale.X, scale.Y, scale.Z);
            GL.Rotate(rot.X, 0, 0, 1);
            GL.Rotate(rot.Y, 0, 1, 0);
            GL.Rotate(rot.Z, 1, 0, 0);
            for (int i = 0; i < Meshes.Count; i++)
            {
                for (int x = 0; x < Meshes[i].Faces.Count; x++)
                {
                    SimpleRenderer.RenderCustomPlane(Meshes[i].Vertices[Meshes[i].Faces[x].L1 - 1],
                        Meshes[i].Vertices[Meshes[i].Faces[x].L2 - 1], Meshes[i].Vertices[Meshes[i].Faces[x].L3 - 1]);
                }
            }
            GL.PopMatrix();
        }
    }

    public class ModelMesh
    {
        /// <summary>
        /// The name of this mesh.
        /// </summary>
        public string Name;

        public ModelMesh(string _name)
        {
            Name = _name;
            Vertices = new List<Location>();
            Faces = new List<ModelFace>();
        }

        /// <summary>
        /// Alll the mesh's vertices.
        /// </summary>
        public List<Location> Vertices;

        /// <summary>
        /// All the mesh's faces.
        /// </summary>
        public List<ModelFace> Faces;
    }

    public class ModelFace
    {
        public ModelFace(int _l1, int _l2, int _l3)
        {
            L1 = _l1;
            L2 = _l2;
            L3 = _l3;
        }

        public int L1;
        public int L2;
        public int L3;
    }
}