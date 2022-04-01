using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Blox_Saber_The_Game.gui
{
	// Token: 0x02000044 RID: 68
	internal class GuiButton : Gui
	{
		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00007A75 File Offset: 0x00005C75
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00007A7D File Offset: 0x00005C7D
		public bool Visible
		{
			get
			{
				return this._visible;
			}
			set
			{
				this._visible = value;
				if (!this.Visible)
				{
					this.IsMouseOver = false;
				}
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00007A95 File Offset: 0x00005C95
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00007A9D File Offset: 0x00005C9D
		public bool IsMouseOver { get; protected set; }

		// Token: 0x06000117 RID: 279 RVA: 0x00007AA6 File Offset: 0x00005CA6
		public GuiButton(int id, float x, float y, float sx, float sy)
		{
			this.Id = id;
			this.ClientRectangle = new RectangleF(x, y, sx, sy);
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007AD8 File Offset: 0x00005CD8
		public GuiButton(int id, float x, float y, float sx, float sy, int texture) : this(id, x, y, sx, sy)
		{
			this.Texture = texture;
		}

		// Token: 0x06000119 RID: 281 RVA: 0x00007AEF File Offset: 0x00005CEF
		public GuiButton(int id, float x, float y, float sx, float sy, string text) : this(id, x, y, sx, sy)
		{
			this.Text = text;
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007B08 File Offset: 0x00005D08
		public override void Render(float delta)
		{
			if (!this.Visible)
			{
				this.IsMouseOver = false;
				return;
			}
			Point mousePos = BloxSaber.Instance.InputManager.MousePos;
			this.IsMouseOver = this.ClientRectangle.Contains((float)mousePos.X, (float)mousePos.Y);
			this._alpha = MathHelper.Clamp(this._alpha + (float)(this.IsMouseOver ? 10 : -10) * delta, 0f, 1f);
			if (this.Texture > 0)
			{
				if (this.IsMouseOver)
				{
					GL.Color3(0.75f, 0.75f, 0.75f);
				}
				else
				{
					GL.Color3(1f, 1f, 1f);
				}
				Glu.RenderTexturedQuad(this.ClientRectangle, 0f, 0f, 1f, 1f, this.Texture);
			}
			else
			{
				float num = 0.075f * this._alpha;
				GL.Color3(0.1f + num, 0.1f + num, 0.1f + num);
				Glu.RenderQuad(this.ClientRectangle);
				GL.LineWidth(2f);
				GL.Color3(0.2f + num, 0.2f + num, 0.2f + num);
				Glu.RenderOutline(this.ClientRectangle);
				GL.LineWidth(1f);
			}
			/*FontRenderer fontRenderer = BloxSaber.Instance.FontRenderer;
			int width = fontRenderer.GetWidth(this.Text, 28);
			float height = fontRenderer.GetHeight(28);*/
			GL.Color3(1f, 1f, 1f);
			//fontRenderer.Render(this.Text, (int)(this.ClientRectangle.X + this.ClientRectangle.Width / 2f - (float)width / 2f), (int)(this.ClientRectangle.Y + this.ClientRectangle.Height / 2f - height / 2f), 28);
		}

		// Token: 0x04000147 RID: 327
		public RectangleF ClientRectangle;

		// Token: 0x04000149 RID: 329
		public int Id;

		// Token: 0x0400014A RID: 330
		public string Text = "";

		// Token: 0x0400014B RID: 331
		protected int Texture;

		// Token: 0x0400014C RID: 332
		private bool _visible = true;

		// Token: 0x0400014D RID: 333
		private float _alpha;
	}
}
