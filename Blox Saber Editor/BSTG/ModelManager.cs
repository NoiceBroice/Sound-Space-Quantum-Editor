using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200000F RID: 15
	internal class ModelManager : IDisposable
	{
		// Token: 0x0600005F RID: 95 RVA: 0x0000406C File Offset: 0x0000226C
		public void RegisterModel(string name, float[] vertexes, float[] normals, float[] uvs, bool centered = true)
		{
			Model value = ModelManager.LoadModelToVao(vertexes, normals, uvs, centered);
			this._models.Add(name, value);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004094 File Offset: 0x00002294
		public Model GetModel(string name)
		{
			Model result;
			this._models.TryGetValue(name, out result);
			return result;
		}

		// Token: 0x06000061 RID: 97 RVA: 0x000040B4 File Offset: 0x000022B4
		public static Model LoadModelToVao(float[] vertexes, float[] normals, float[] uVs, bool centered = true)
		{
			int vao = ModelManager.CreateVao();
			int buffer = ModelManager.StoreDataInAttribList(0, 3, vertexes);
			ModelManager.UnbindVao();
			return new Model(buffer, vertexes, vao, centered);
		}

		// Token: 0x06000062 RID: 98 RVA: 0x000040DC File Offset: 0x000022DC
		private static int CreateVao()
		{
			int num = GL.GenVertexArray();
			ModelManager.VaOs.Add(num);
			GL.BindVertexArray(num);
			return num;
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00004101 File Offset: 0x00002301
		private static void UnbindVao()
		{
			GL.BindVertexArray(0);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x0000410C File Offset: 0x0000230C
		private static int StoreDataInAttribList(int attrib, int coordSize, float[] data)
		{
			int num = GL.GenBuffer();
			ModelManager.VbOs.Add(num);
			GL.BindBuffer(BufferTarget.ArrayBuffer, num);
			GL.BufferData<float>(BufferTarget.ArrayBuffer, 4 * data.Length, data, BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer(attrib, coordSize, VertexAttribPointerType.Float, false, 0, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			return num;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004168 File Offset: 0x00002368
		public void Dispose()
		{
			foreach (int arrays in ModelManager.VaOs)
			{
				GL.DeleteVertexArray(arrays);
			}
			foreach (int buffers in ModelManager.VbOs)
			{
				GL.DeleteBuffer(buffers);
			}
			foreach (Model model in this._models.Values)
			{
				model.Dispose();
			}
		}

		// Token: 0x0400003F RID: 63
		private static readonly List<int> VaOs = new List<int>();

		// Token: 0x04000040 RID: 64
		private static readonly List<int> VbOs = new List<int>();

		// Token: 0x04000041 RID: 65
		private readonly Dictionary<string, Model> _models = new Dictionary<string, Model>();
	}
}
