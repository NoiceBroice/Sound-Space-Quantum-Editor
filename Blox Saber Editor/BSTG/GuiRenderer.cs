using System;
using Blox_Saber_The_Game.gui;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200000B RID: 11
	internal class GuiRenderer
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600004A RID: 74 RVA: 0x00003C3C File Offset: 0x00001E3C
		// (set) Token: 0x0600004B RID: 75 RVA: 0x00003C44 File Offset: 0x00001E44
		public GuiScreen GuiScreen { get; private set; }

		// Token: 0x0600004C RID: 76 RVA: 0x00003C4D File Offset: 0x00001E4D
		public void Update()
		{
			GuiScreen guiScreen = this.GuiScreen;
			if (guiScreen == null)
			{
				return;
			}
			guiScreen.Update();
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003C5F File Offset: 0x00001E5F
		public void Render(float delta)
		{
			if (this.GuiScreen != null)
			{
				GL.Disable(EnableCap.DepthTest);
				this.GuiScreen.Render(delta);
				GL.Enable(EnableCap.DepthTest);
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003C8C File Offset: 0x00001E8C
		public void OpenScreen(GuiScreen s)
		{
			GuiScreen guiScreen = this.GuiScreen;
			if (guiScreen != null)
			{
				guiScreen.OnClose();
			}
			this.GuiScreen = s;
			GuiScreen guiScreen2 = this.GuiScreen;
			if (guiScreen2 != null)
			{
				guiScreen2.Init(guiScreen);
			}
			GuiRenderer.CursorVisible = (s != null);
			BloxSaber.Instance.CursorGrabbed = (s == null);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00003CD9 File Offset: 0x00001ED9
		public void OnClick(int x, int y)
		{
			GuiScreen guiScreen = this.GuiScreen;
			if (guiScreen == null)
			{
				return;
			}
			guiScreen.OnClick(x, y);
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003CED File Offset: 0x00001EED
		public void OnResize(int width, int height)
		{
			GuiScreen guiScreen = this.GuiScreen;
			if (guiScreen == null)
			{
				return;
			}
			guiScreen.OnResize(width, height);
		}

		// Token: 0x04000038 RID: 56
		public static bool CursorVisible = true;
	}
}
