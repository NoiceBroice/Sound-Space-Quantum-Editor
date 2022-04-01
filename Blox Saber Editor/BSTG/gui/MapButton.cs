using System;

namespace Blox_Saber_The_Game.gui
{
	// Token: 0x02000046 RID: 70
	internal class MapButton : GuiButton
	{
		// Token: 0x06000125 RID: 293 RVA: 0x00007F14 File Offset: 0x00006114
		public MapButton(int id, string mapName) : base(id, 100f, 480f, 500f, 40f, mapName)
		{
			this.MapName = mapName;
		}

		// Token: 0x04000150 RID: 336
		public string MapName;
	}
}
