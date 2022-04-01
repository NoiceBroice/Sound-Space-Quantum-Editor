using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game.gui
{
	// Token: 0x02000045 RID: 69
	internal class GuiScreen : Gui
	{
		// Token: 0x0600011C RID: 284 RVA: 0x00007CF3 File Offset: 0x00005EF3
		public virtual void Init(GuiScreen last)
		{
			if (last != null)
			{
				this._position = last._position;
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00007D04 File Offset: 0x00005F04
		public override void Update()
		{
			foreach (Gui gui in this.Guis)
			{
				gui.Update();
			}
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00007D54 File Offset: 0x00005F54
		public override void Render(float delta)
		{
			foreach (Gui gui in this.Guis)
			{
				gui.Render(delta);
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00007DA8 File Offset: 0x00005FA8
		protected virtual void RenderBackground(float delta)
		{
			float num = 150f;
			int num2 = (int)Math.Ceiling((double)((float)BloxSaber.Instance.Width / num));
			int num3 = (int)Math.Ceiling((double)((float)BloxSaber.Instance.Height / num));
			bool flag = false;
			this._position = (this._position + delta * 50f) % num;
			for (int i = -1; i < num3; i++)
			{
				bool flag2 = flag;
				for (int j = -1; j < num2; j++)
				{
					if (flag2)
					{
						GL.Color3(0.075f, 0.075f, 0.125f);
					}
					else
					{
						GL.Color3(0.05f, 0.05f, 0.075f);
					}
					Glu.RenderQuad((double)((float)j * num + this._position), (double)((float)i * num + this._position), (double)num, (double)num);
					flag2 = !flag2;
				}
				flag = !flag;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00007E80 File Offset: 0x00006080
		public override void OnClick(int x, int y)
		{
			for (int i = this.Guis.Count - 1; i >= 0; i--)
			{
				GuiButton guiButton;
				if ((guiButton = (this.Guis[i] as GuiButton)) != null && guiButton.ClientRectangle.Contains((float)x, (float)y) && guiButton.Visible)
				{
					guiButton.OnClick(x, y);
					BloxSaber.Instance.GameManager.SoundPlayer.Play("click", 0.05f, 1f);
					this.OnButtonClicked(guiButton.Id);
					return;
				}
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00007F0B File Offset: 0x0000610B
		public virtual void OnButtonClicked(int id)
		{
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007F0D File Offset: 0x0000610D
		public virtual bool DoesGuiStopRender()
		{
			return true;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007F10 File Offset: 0x00006110
		public virtual void OnClose()
		{
		}

		// Token: 0x06000124 RID: 292 RVA: 0x00007F12 File Offset: 0x00006112
		public virtual void OnResize(int width, int height)
		{
		}

		// Token: 0x0400014E RID: 334
		public List<Gui> Guis = new List<Gui>();

		// Token: 0x0400014F RID: 335
		private float _position;
	}
}
