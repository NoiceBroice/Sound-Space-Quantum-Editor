using System;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200000D RID: 13
	internal static class Mathf
	{
		// Token: 0x06000058 RID: 88 RVA: 0x00003E88 File Offset: 0x00002088
		public static float Clamp(float f, float min, float max)
		{
			if (f < min)
			{
				return min;
			}
			if (f <= max)
			{
				return f;
			}
			return max;
		}
	}
}
