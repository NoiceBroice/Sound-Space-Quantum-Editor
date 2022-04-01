using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000002 RID: 2
	internal class Camera
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public float RotX { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		public float RotY { get; private set; } = 3.1415927f;

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		// (set) Token: 0x06000006 RID: 6 RVA: 0x0000207A File Offset: 0x0000027A
		public Vector3 LookVec { get; private set; }

		// Token: 0x06000007 RID: 7 RVA: 0x00002084 File Offset: 0x00000284
		public void CalculateView()
		{
			MouseState state = Mouse.GetState();
			if (BloxSaber.Instance.Focused)
			{
				int num = state.X - this._lastX;
				int num2 = state.Y - this._lastY;
				this.RotY += (float)num / 150f * GameSettings.Sensitivity;
				this.RotX = MathHelper.Clamp(this.RotX + (float)num2 / 150f * GameSettings.Sensitivity, -MathHelper.DegreesToRadians(88f), MathHelper.DegreesToRadians(88f));
			}
			this._lastX = state.X;
			this._lastY = state.Y;
			Matrix4 matrix = Matrix4.CreateRotationY(this.RotY) * Matrix4.CreateRotationX(this.RotX);
			this.LookVec = (matrix * -Vector4.UnitZ).Xyz;
			this.Pos = -Vector3.UnitZ * 3.5f + this.LookVec * new Vector3(1.25f, 1.25f, 0f);
			this._view = Matrix4.CreateTranslation(-this.Pos) * matrix;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021C0 File Offset: 0x000003C0
		public void CalculateProjection()
		{
			this._projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(this.Fov), (float)BloxSaber.Instance.Size.Width / (float)BloxSaber.Instance.Size.Height, 0.1f, 1000f);
			this._ortho = Matrix4.CreateOrthographicOffCenter(0f, (float)BloxSaber.Instance.Width, (float)BloxSaber.Instance.Height, 0f, 0f, 1f);
		}

		// Token: 0x06000009 RID: 9 RVA: 0x0000224C File Offset: 0x0000044C
		public void UploadProjection()
		{
			Matrix4 matrix = this._view * this._projection;
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref matrix);
			Shader.SetProjectionMatrix(this._projection);
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002287 File Offset: 0x00000487
		public void UploadOrtho()
		{
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadMatrix(ref this._ortho);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x0000229E File Offset: 0x0000049E
		public void UploadView()
		{
			Shader.SetViewMatrix(this._view);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000022AC File Offset: 0x000004AC
		public void ResetCamera()
		{
			MouseState state = Mouse.GetState();
			this._lastX = state.X;
			this._lastY = state.Y;
			this.RotX = 0f;
			this.RotY = 3.1415927f;
			this.CalculateView();
			this.UploadView();
		}

		// Token: 0x04000001 RID: 1
		private Matrix4 _ortho;

		// Token: 0x04000002 RID: 2
		private Matrix4 _projection;

		// Token: 0x04000003 RID: 3
		private Matrix4 _view;

		// Token: 0x04000004 RID: 4
		private int _lastX;

		// Token: 0x04000005 RID: 5
		private int _lastY;

		// Token: 0x04000006 RID: 6
		public Vector3 Pos;

		// Token: 0x0400000A RID: 10
		public float Fov = 70f;
	}
}
