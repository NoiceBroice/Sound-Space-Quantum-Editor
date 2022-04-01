using System;
using System.IO;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000014 RID: 20
	internal class ObjMtl
	{
		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600009B RID: 155 RVA: 0x000059BC File Offset: 0x00003BBC
		// (set) Token: 0x0600009C RID: 156 RVA: 0x000059C4 File Offset: 0x00003BC4
		public string TextureFile { get; private set; }

		// Token: 0x0600009D RID: 157 RVA: 0x000059CD File Offset: 0x00003BCD
		private ObjMtl()
		{
		}

		// Token: 0x0600009E RID: 158 RVA: 0x000059D8 File Offset: 0x00003BD8
		public static ObjMtl FromFile(string file)
		{
			string[] array = File.ReadAllLines(file);
			ObjMtl objMtl = new ObjMtl();
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i];
				if (text.StartsWith("map_Kd "))
				{
					string text2 = text.Substring(7);
					if (File.Exists(text2))
					{
						objMtl.TextureFile = text2;
						break;
					}
					Console.WriteLine("MapName '" + text2 + "' was not found.");
					break;
				}
				else
				{
					i++;
				}
			}
			return objMtl;
		}
	}
}
