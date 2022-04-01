using System;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game.gui
{
	// Token: 0x0200003F RID: 63
	internal class GuiScreenLoading : GuiScreen
	{
		// Token: 0x060000FD RID: 253 RVA: 0x000071C8 File Offset: 0x000053C8
		public override void Render(float delta)
		{
			GL.Color3(1f, 0.8f, 0f);
			//BloxSaber.Instance.FontRenderer.RenderCentered("LOADING", BloxSaber.Instance.Width / 2, BloxSaber.Instance.Height / 2 - 75, 150);
		}
	}
}
