using System;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000006 RID: 6
	internal struct Face
	{
		// Token: 0x06000018 RID: 24 RVA: 0x00002663 File Offset: 0x00000863
		public Face(long[] indices, long[] uvs, long[] normals)
		{
			this.Indices = indices;
			this.UVs = uvs;
			this.Normals = normals;
		}

		// Token: 0x04000017 RID: 23
		public readonly long[] Indices;

		// Token: 0x04000018 RID: 24
		public readonly long[] UVs;

		// Token: 0x04000019 RID: 25
		public readonly long[] Normals;
	}
}
