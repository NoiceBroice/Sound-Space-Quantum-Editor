using System;
using System.Drawing;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200000A RID: 10
	internal static class Glu
	{
		// Token: 0x06000043 RID: 67 RVA: 0x00003930 File Offset: 0x00001B30
		public static void RenderQuad(double x, double y, double sx, double sy)
		{
			GL.Translate(x, y, 0.0);
			GL.Scale(sx, sy, 1.0);
			GL.Begin(PrimitiveType.Quads);
			GL.Vertex2(0, 0);
			GL.Vertex2(0, 1);
			GL.Vertex2(1, 1);
			GL.Vertex2(1, 0);
			GL.End();
			GL.Scale(1.0 / sx, 1.0 / sy, 1.0);
			GL.Translate(-x, -y, 0.0);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x000039BA File Offset: 0x00001BBA
		public static void RenderQuad(RectangleF rect)
		{
			Glu.RenderQuad((double)rect.X, (double)rect.Y, (double)rect.Width, (double)rect.Height);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000039E4 File Offset: 0x00001BE4
		public static void RenderTexturedQuad(float x, float y, float sx, float sy, float us, float vs, float ue, float ve, int texture)
		{
			GL.BindTexture(TextureTarget.Texture2D, texture);
			GL.Translate(x, y, 0f);
			GL.Scale(sx, sy, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(us, vs);
			GL.Vertex2(0, 0);
			GL.TexCoord2(us, ve);
			GL.Vertex2(0, 1);
			GL.TexCoord2(ue, ve);
			GL.Vertex2(1, 1);
			GL.TexCoord2(ue, vs);
			GL.Vertex2(1, 0);
			GL.End();
			GL.Scale(1f / sx, 1f / sy, 1f);
			GL.Translate(-x, -y, 0f);
			GL.BindTexture(TextureTarget.Texture2D, 0);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003A94 File Offset: 0x00001C94
		public static void RenderTexturedQuad(float x, float y, float sx, float sy, float us, float vs, float ue, float ve)
		{
			GL.Translate(x, y, 0f);
			GL.Scale(sx, sy, 1f);
			GL.Begin(PrimitiveType.Quads);
			GL.TexCoord2(us, vs);
			GL.Vertex2(0, 0);
			GL.TexCoord2(us, ve);
			GL.Vertex2(0, 1);
			GL.TexCoord2(ue, ve);
			GL.Vertex2(1, 1);
			GL.TexCoord2(ue, vs);
			GL.Vertex2(1, 0);
			GL.End();
			GL.Scale(1f / sx, 1f / sy, 1f);
			GL.Translate(-x, -y, 0f);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003B2C File Offset: 0x00001D2C
		public static void RenderTexturedQuad(RectangleF rect, float us, float vs, float ue, float ve, int texture)
		{
			Glu.RenderTexturedQuad(rect.X, rect.Y, rect.Width, rect.Height, us, vs, ue, ve, texture);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003B64 File Offset: 0x00001D64
		public static void RenderOutline(float x, float y, float sx, float sy)
		{
			GL.PolygonMode(MaterialFace.Front, PolygonMode.Line);
			x += 0.5f;
			y += 0.5f;
			sx -= 1f;
			sy -= 1f;
			GL.Translate(x, y, 0f);
			GL.Scale(sx, sy, 1f);
			GL.Begin(PrimitiveType.Polygon);
			GL.Vertex2(0, 0);
			GL.Vertex2(0, 1);
			GL.Vertex2(1, 1);
			GL.Vertex2(1, 0);
			GL.End();
			GL.Scale(1f / sx, 1f / sy, 1f);
			GL.Translate(-x, -y, 0f);
			GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003C19 File Offset: 0x00001E19
		public static void RenderOutline(RectangleF rect)
		{
			Glu.RenderOutline(rect.X, rect.Y, rect.Width, rect.Height);
		}
	}
}
