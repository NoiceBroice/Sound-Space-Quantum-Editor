using System;
using OpenTK.Graphics;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000005 RID: 5
	internal class Cube
	{
		// Token: 0x06000017 RID: 23 RVA: 0x0000263E File Offset: 0x0000083E
		public Cube(float z, float indexX, float indexY, Color4 color)
		{
			this.Z = z;
			this.IndexX = indexX;
			this.IndexY = indexY;
			this.Color = color;
		}

		// Token: 0x04000013 RID: 19
		public float Z;

		// Token: 0x04000014 RID: 20
		public float IndexX;

		// Token: 0x04000015 RID: 21
		public float IndexY;

		// Token: 0x04000016 RID: 22
		public Color4 Color;
	}
}
