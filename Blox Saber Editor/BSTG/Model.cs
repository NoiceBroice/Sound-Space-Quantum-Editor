using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200000E RID: 14
	internal class Model : IDisposable
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00003E97 File Offset: 0x00002097
		public Model(int buffer, float[] vertexes, int vao, bool centered = true)
		{
			this._buffer = buffer;
			this._vao = vao;
			this._bufferSize = vertexes.Length / 3;
			if (centered)
			{
				this.Init(vertexes);
			}
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003EC4 File Offset: 0x000020C4
		private void Init(float[] vertexes)
		{
			float num = float.MaxValue;
			float num2 = float.MaxValue;
			float num3 = float.MaxValue;
			float num4 = float.MinValue;
			float num5 = float.MinValue;
			float num6 = float.MinValue;
			for (int i = 0; i < vertexes.Length; i += 3)
			{
				float val = vertexes[i];
				float val2 = vertexes[i + 1];
				float val3 = vertexes[i + 2];
				num = Math.Min(num, val);
				num2 = Math.Min(num2, val2);
				num3 = Math.Min(num3, val3);
				num4 = Math.Max(num4, val);
				num5 = Math.Max(num5, val2);
				num6 = Math.Max(num6, val3);
			}
			this.Size.X = num4 - num;
			this.Size.Y = num5 - num2;
			this.Size.Z = num6 - num3;
			for (int j = 0; j < vertexes.Length; j += 3)
			{
				vertexes[j] -= num;
				vertexes[j + 1] -= num2;
				vertexes[j + 2] -= num3;
				vertexes[j] -= this.Size.X / 2f;
				vertexes[j + 1] -= this.Size.Y / 2f;
				vertexes[j + 2] -= this.Size.Z / 2f;
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004023 File Offset: 0x00002223
		public void Bind()
		{
			GL.BindVertexArray(this._vao);
			GL.EnableVertexAttribArray(0);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004036 File Offset: 0x00002236
		public void Unbind()
		{
			GL.BindVertexArray(0);
			GL.DisableVertexAttribArray(0);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00004044 File Offset: 0x00002244
		public void Render()
		{
			GL.DrawArrays(PrimitiveType.Triangles, 0, this._bufferSize);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00004053 File Offset: 0x00002253
		public void Dispose()
		{
			GL.DeleteBuffer(this._buffer);
			GL.DeleteVertexArray(this._vao);
		}

		// Token: 0x0400003B RID: 59
		private readonly int _bufferSize;

		// Token: 0x0400003C RID: 60
		private readonly int _buffer;

		// Token: 0x0400003D RID: 61
		private readonly int _vao;

		// Token: 0x0400003E RID: 62
		public Vector3 Size;
	}
}
