using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK.Input;

namespace Blox_Saber_The_Game
{
	// Token: 0x0200000C RID: 12
	internal class InputManager
	{
		// Token: 0x06000053 RID: 83 RVA: 0x00003D14 File Offset: 0x00001F14
		public InputManager(BloxSaber window)
		{
			window.KeyDown += this.KeyDown;
			window.KeyUp += this.KeyUp;
			window.MouseMove += new EventHandler<MouseMoveEventArgs>(this.MouseMove);
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003D68 File Offset: 0x00001F68
		public bool IsKeyDown(Key key)
		{
			Dictionary<Key, bool> keysDown = this._keysDown;
			bool result;
			lock (keysDown)
			{
				bool flag2;
				result = this._keysDown.TryGetValue(key, out flag2);
			}
			return result;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003DB4 File Offset: 0x00001FB4
		private void MouseMove(object send, MouseEventArgs e)
		{
			this.MousePos.X = e.X;
			this.MousePos.Y = e.Y;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003DD8 File Offset: 0x00001FD8
		private void KeyDown(object sender, KeyboardKeyEventArgs e)
		{
			Dictionary<Key, bool> keysDown = this._keysDown;
			lock (keysDown)
			{
				bool flag2;
				if (!this._keysDown.TryGetValue(e.Key, out flag2))
				{
					this._keysDown.Add(e.Key, true);
				}
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003E3C File Offset: 0x0000203C
		private void KeyUp(object sender, KeyboardKeyEventArgs e)
		{
			Dictionary<Key, bool> keysDown = this._keysDown;
			lock (keysDown)
			{
				this._keysDown.Remove(e.Key);
			}
		}

		// Token: 0x04000039 RID: 57
		private readonly Dictionary<Key, bool> _keysDown = new Dictionary<Key, bool>();

		// Token: 0x0400003A RID: 58
		public Point MousePos;
	}
}
