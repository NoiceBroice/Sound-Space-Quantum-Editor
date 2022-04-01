using System;
using System.Collections.Generic;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000018 RID: 24
	internal class Shader
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00005D4C File Offset: 0x00003F4C
		public Shader(string shaderName, params string[] uniforms)
		{
			this.ShaderName = shaderName;
			this.Init();
			this.RegisterUniformsSilent(new string[]
			{
				"transformationMatrix",
				"projectionMatrix",
				"viewMatrix"
			});
			this.RegisterUniforms(uniforms);
			Shader.Shaders.Add(this);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005DB0 File Offset: 0x00003FB0
		private void Init()
		{
			this.LoadShader(this.ShaderName);
			this._program = GL.CreateProgram();
			GL.AttachShader(this._program, this._vsh);
			GL.AttachShader(this._program, this._fsh);
			this.BindAttributes();
			GL.LinkProgram(this._program);
			GL.ValidateProgram(this._program);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00005E12 File Offset: 0x00004012
		private void BindAttributes()
		{
			this.BindAttribute(0, "position");
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00005E20 File Offset: 0x00004020
		private int GetUniformLocation(string uniform)
		{
			int result;
			if (this._uniforms.TryGetValue(uniform, out result))
			{
				return result;
			}
			return -1;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005E40 File Offset: 0x00004040
		private void RegisterUniforms(params string[] uniforms)
		{
			if (this._registered)
			{
				throw new Exception("Can't register uniforms twice, they need to be registered only once.");
			}
			this._registered = true;
			this.Bind();
			foreach (string text in uniforms)
			{
				if (this._uniforms.ContainsKey(text))
				{
					Console.WriteLine(string.Concat(new string[]
					{
						"Attemted to register uniform '",
						text,
						"' in shader '",
						this.ShaderName,
						"' twice"
					}));
				}
				else
				{
					int uniformLocation = GL.GetUniformLocation(this._program, text);
					if (uniformLocation == -1)
					{
						Console.WriteLine(string.Concat(new string[]
						{
							"Could not find uniform '",
							text,
							"' in shader '",
							this.ShaderName,
							"'"
						}));
					}
					else
					{
						this._uniforms.Add(text, uniformLocation);
					}
				}
			}
			this.Unbind();
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005F28 File Offset: 0x00004128
		private void RegisterUniformsSilent(params string[] uniforms)
		{
			this.Bind();
			foreach (string text in uniforms)
			{
				if (!this._uniforms.ContainsKey(text))
				{
					int uniformLocation = GL.GetUniformLocation(this._program, text);
					if (uniformLocation != -1)
					{
						this._uniforms.Add(text, uniformLocation);
					}
				}
			}
			this.Unbind();
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00005F84 File Offset: 0x00004184
		public void SetFloat(string uniform, float f)
		{
			int uniformLocation = this.GetUniformLocation(uniform);
			if (uniformLocation != -1)
			{
				GL.Uniform1(uniformLocation, f);
			}
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00005FA4 File Offset: 0x000041A4
		public void SetVector2(string uniform, Vector2 vec)
		{
			int uniformLocation = this.GetUniformLocation(uniform);
			if (uniformLocation != -1)
			{
				GL.Uniform2(uniformLocation, vec);
			}
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00005FC4 File Offset: 0x000041C4
		public void SetVector3(string uniform, Vector3 vec)
		{
			int uniformLocation = this.GetUniformLocation(uniform);
			if (uniformLocation != -1)
			{
				GL.Uniform3(uniformLocation, vec);
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00005FE4 File Offset: 0x000041E4
		public void SetVector4(string uniform, Vector4 vec)
		{
			int uniformLocation = this.GetUniformLocation(uniform);
			if (uniformLocation != -1)
			{
				GL.Uniform4(uniformLocation, vec);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00006004 File Offset: 0x00004204
		public void SetMatrix4(string uniform, Matrix4 mat)
		{
			int uniformLocation = this.GetUniformLocation(uniform);
			if (uniformLocation != -1)
			{
				GL.UniformMatrix4(uniformLocation, false, ref mat);
			}
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00006028 File Offset: 0x00004228
		public static void SetProjectionMatrix(Matrix4 mat)
		{
			for (int i = 0; i < Shader.Shaders.Count; i++)
			{
				Shader shader = Shader.Shaders[i];
				shader.Bind();
				shader.SetMatrix4("projectionMatrix", mat);
				shader.Unbind();
			}
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000606C File Offset: 0x0000426C
		public static void SetViewMatrix(Matrix4 mat)
		{
			for (int i = 0; i < Shader.Shaders.Count; i++)
			{
				Shader shader = Shader.Shaders[i];
				shader.Bind();
				shader.SetMatrix4("viewMatrix", mat);
				shader.Unbind();
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000060B0 File Offset: 0x000042B0
		private void BindAttribute(int attrib, string variable)
		{
			GL.BindAttribLocation(this._program, attrib, variable);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x000060C0 File Offset: 0x000042C0
		private void LoadShader(string shaderName)
		{
			string @string = File.ReadAllText("assets/shaders/" + shaderName + ".vsh");
			string string2 = File.ReadAllText("assets/shaders/" + shaderName + ".fsh");
			this._vsh = GL.CreateShader(ShaderType.VertexShader);
			this._fsh = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(this._vsh, @string);
			GL.ShaderSource(this._fsh, string2);
			GL.CompileShader(this._vsh);
			GL.CompileShader(this._fsh);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006147 File Offset: 0x00004347
		public void Bind()
		{
			GL.UseProgram(this._program);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00006154 File Offset: 0x00004354
		public void Unbind()
		{
			GL.UseProgram(0);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000615C File Offset: 0x0000435C
		public void Reload()
		{
			this.Destroy();
			this.Init();
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000616C File Offset: 0x0000436C
		public void Destroy()
		{
			this.Unbind();
			GL.DetachShader(this._program, this._vsh);
			GL.DetachShader(this._program, this._fsh);
			GL.DeleteShader(this._vsh);
			GL.DeleteShader(this._fsh);
			GL.DeleteProgram(this._program);
		}

		// Token: 0x060000BB RID: 187 RVA: 0x000061C4 File Offset: 0x000043C4
		public static void ReloadAll()
		{
			for (int i = 0; i < Shader.Shaders.Count; i++)
			{
				Shader.Shaders[i].Reload();
			}
		}

		// Token: 0x060000BC RID: 188 RVA: 0x000061F8 File Offset: 0x000043F8
		public static void DestroyAll()
		{
			for (int i = 0; i < Shader.Shaders.Count; i++)
			{
				Shader.Shaders[i].Destroy();
			}
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000622A File Offset: 0x0000442A
		public Shader Reloaded()
		{
			this.Reload();
			return this;
		}

		// Token: 0x0400006B RID: 107
		private static readonly List<Shader> Shaders = new List<Shader>();

		// Token: 0x0400006C RID: 108
		private int _vsh;

		// Token: 0x0400006D RID: 109
		private int _fsh;

		// Token: 0x0400006E RID: 110
		private int _program;

		// Token: 0x0400006F RID: 111
		private bool _registered;

		// Token: 0x04000070 RID: 112
		public readonly string ShaderName;

		// Token: 0x04000071 RID: 113
		private readonly Dictionary<string, int> _uniforms = new Dictionary<string, int>();
	}
}
