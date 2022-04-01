using System;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200001B RID: 27
	internal struct Vertex
	{
		// Token: 0x060000C6 RID: 198 RVA: 0x00006467 File Offset: 0x00004667
		public Vertex(params float[] position)
		{
			this.Positions = position;
		}

		// Token: 0x04000074 RID: 116
		public float[] Positions;
	}
}
