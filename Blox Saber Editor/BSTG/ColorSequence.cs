using System;
using System.Drawing;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000003 RID: 3
	internal class ColorSequence
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00002319 File Offset: 0x00000519
		public ColorSequence(params Color[] colors)
		{
			this._colors = colors;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002328 File Offset: 0x00000528
		public Color Next()
		{
			Color result = this._colors[this._index];
			this._index = (this._index + 1) % this._colors.Length;
			return result;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002352 File Offset: 0x00000552
		public void Reset()
		{
			this._index = 0;
		}

		// Token: 0x0400000B RID: 11
		private readonly Color[] _colors;

		// Token: 0x0400000C RID: 12
		private int _index;
	}
}
