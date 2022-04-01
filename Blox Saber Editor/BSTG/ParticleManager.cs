using System;
using System.Collections.Generic;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000016 RID: 22
	internal class ParticleManager
	{
		// Token: 0x060000A2 RID: 162 RVA: 0x00005C4C File Offset: 0x00003E4C
		public void Update()
		{
			for (int i = this._particles.Count - 1; i >= 0; i--)
			{
				Particle particle = this._particles[i];
				particle.Update();
				if (particle.IsDead)
				{
					this._particles.RemoveAt(i);
				}
			}
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00005C98 File Offset: 0x00003E98
		public void Render(double delta)
		{
			GameManager.CubeShader.Bind();
			foreach (Particle particle in this._particles)
			{
				particle.Render(delta);
			}
			GameManager.CubeShader.Unbind();
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00005D00 File Offset: 0x00003F00
		public void Spawn(Particle p)
		{
			this._particles.Add(p);
		}

		// Token: 0x0400006A RID: 106
		private readonly List<Particle> _particles = new List<Particle>();
	}
}
