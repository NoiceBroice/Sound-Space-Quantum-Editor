using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200001A RID: 26
	internal static class TextureManager
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x000062EC File Offset: 0x000044EC
		public static int GetOrRegister(string textureName, Bitmap bmp = null, bool filter = false)
		{
			int result;
			if (TextureManager.Textures.TryGetValue(textureName, out result))
			{
				return result;
			}
			Bitmap bitmap = bmp;
			if (bitmap == null)
			{
				string text = "assets/textures/" + textureName + ".png";
				if (!File.Exists(text))
				{
					Console.WriteLine("Could not find file " + text);
					return -1;
				}
				using (FileStream fileStream = File.OpenRead(text))
				{
					bitmap = (Bitmap)Image.FromStream(fileStream);
				}
			}
			int num = TextureManager.LoadTexture(bitmap, filter);
			TextureManager.Textures.Add(textureName, num);
			return num;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006384 File Offset: 0x00004584
		private static int LoadTexture(Bitmap img, bool filter)
		{
			int num = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, num);
			BitmapData bitmapData = img.LockBits(new Rectangle(0, 0, img.Width, img.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
			img.UnlockBits(bitmapData);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, filter ? 9729 : 9728);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, filter ? 9729 : 9728);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, 33071);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, 33071);
			return num;
		}

		// Token: 0x04000073 RID: 115
		private static readonly Dictionary<string, int> Textures = new Dictionary<string, int>();
	}
}
