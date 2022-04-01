using System;
using OpenTK;
using OpenTK.Graphics;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000015 RID: 21
	internal class Particle
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00005A48 File Offset: 0x00003C48
		public Particle(Vector3 position, Vector3 motion, Color4 color, int maxAge, float modelScale, string model)
		{
			this._lastPosition = position;
			this._position = position + motion;
			this._motion = motion;
			this._color = new Vector4(color.R, color.G, color.B, color.A);
			this._maxAge = maxAge;
			this._model = BloxSaber.Instance.ModelManager.GetModel(model);
			this._modelScale = modelScale;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00005AC0 File Offset: 0x00003CC0
		public void Update()
		{
			int age = this._age;
			this._age = age + 1;
			if (age >= this._maxAge)
			{
				this.IsDead = true;
			}
			this._motion.Xz = this._motion.Xz * 0.94f;
			this._motion.Y = this._motion.Y - 0.14f;
			this._lastPosition = this._position;
			this._position += this._motion;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005B40 File Offset: 0x00003D40
		public void Render(double delta)
		{
			float num = Math.Max(0f, (float)this._age + BloxSaber.Instance.GetTickProgress()) / (float)this._maxAge;
			float num2 = num * 40f * Math.Abs(this._motion.X) / -this._motion.X / 180f * 3.1415927f;
			this._color.W = 1f - num;
			Vector3 vector = Vector3.Lerp(this._lastPosition, this._position, BloxSaber.Instance.GetTickProgress());
			Matrix4 right = Matrix4.CreateRotationZ(num2) * Matrix4.CreateRotationX(num2 * 0.75f);
			GameManager.CubeShader.SetMatrix4("transformationMatrix", Matrix4.CreateScale(this._modelScale) * right * Matrix4.CreateTranslation(vector));
			GameManager.CubeShader.SetVector4("colorIn", this._color);
			this._model.Bind();
			this._model.Render();
			this._model.Unbind();
		}

		// Token: 0x04000061 RID: 97
		private readonly Model _model;

		// Token: 0x04000062 RID: 98
		private Vector4 _color;

		// Token: 0x04000063 RID: 99
		private Vector3 _position;

		// Token: 0x04000064 RID: 100
		private Vector3 _lastPosition;

		// Token: 0x04000065 RID: 101
		private Vector3 _motion;

		// Token: 0x04000066 RID: 102
		private readonly float _modelScale;

		// Token: 0x04000067 RID: 103
		private int _age;

		// Token: 0x04000068 RID: 104
		private readonly int _maxAge;

		// Token: 0x04000069 RID: 105
		public bool IsDead;
	}
}
