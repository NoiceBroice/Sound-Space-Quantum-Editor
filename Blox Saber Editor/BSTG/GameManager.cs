using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Sound_Space_Editor;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000008 RID: 8
	internal class GameManager
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000027DD File Offset: 0x000009DD
		// (set) Token: 0x0600001F RID: 31 RVA: 0x000027E4 File Offset: 0x000009E4
		public static Shader CubeShader { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000027EC File Offset: 0x000009EC
		// (set) Token: 0x06000021 RID: 33 RVA: 0x000027F3 File Offset: 0x000009F3
		public static Model CubeModel { get; private set; }
		public static Model CursorModel { get; private set; }
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000022 RID: 34 RVA: 0x000027FB File Offset: 0x000009FB
		public MusicPlayer MusicPlayer { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000023 RID: 35 RVA: 0x00002803 File Offset: 0x00000A03
		public SoundPlayer SoundPlayer { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000024 RID: 36 RVA: 0x0000280B File Offset: 0x00000A0B
		public ParticleManager ParticleManager { get; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000025 RID: 37 RVA: 0x00002814 File Offset: 0x00000A14
		// (remove) Token: 0x06000026 RID: 38 RVA: 0x0000284C File Offset: 0x00000A4C
		public event EventHandler<bool> OnGameEnded;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000027 RID: 39 RVA: 0x00002884 File Offset: 0x00000A84
		// (remove) Token: 0x06000028 RID: 40 RVA: 0x000028BC File Offset: 0x00000ABC
		public event EventHandler<Cube> OnHitNote;

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000029 RID: 41 RVA: 0x000028F1 File Offset: 0x00000AF1
		// (set) Token: 0x0600002A RID: 42 RVA: 0x000028F9 File Offset: 0x00000AF9
		public long Score { get; private set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002902 File Offset: 0x00000B02
		// (set) Token: 0x0600002C RID: 44 RVA: 0x0000290A File Offset: 0x00000B0A
		public long Hits { get; private set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00002913 File Offset: 0x00000B13
		// (set) Token: 0x0600002E RID: 46 RVA: 0x0000291B File Offset: 0x00000B1B
		public long Misses { get; private set; }

		// Token: 0x0600002F RID: 47 RVA: 0x00002924 File Offset: 0x00000B24
		public GameManager()
		{
			this.MusicPlayer = new MusicPlayer();
			this.SoundPlayer = new SoundPlayer();
			this.ParticleManager = new ParticleManager();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x000029D0 File Offset: 0x00000BD0
		public void Init()
		{
			GameManager.CubeShader = new Shader("note", new string[]
			{
				"colorIn"
			});
			
			Color NoteColor1 = EditorWindow.Instance.NoteColor1;
			Color NoteColor2 = EditorWindow.Instance.NoteColor2;

			Color []_colors = new Color[] { NoteColor1, NoteColor2 };
			this.Colors = new ColorSequence(_colors);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002AF0 File Offset: 0x00000CF0
		private void Cleanup()
		{
			this.MusicPlayer.Reset();
			this._cubes.Clear();
			this._noteIndex = 0;
			this._startTimer = 1f;
			//this.Colors.Reset();
			this.Score = 0L;
			this.Hits = 0L;
			this.Misses = 0L;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002B48 File Offset: 0x00000D48
		public void LoadMap(string map, double ms)
		{
			this.Cleanup();
			this.Map = GameMap.Load(map);
			this.MusicPlayer.Load(this.GetAudio(this.Map.SongId), ms);
			this.MusicPlayer.Volume = 0.04f;
		}
		public void SetMusic(double ms)
		{
			this.MusicPlayer.Set(ms);

		}

		// Token: 0x06000033 RID: 51 RVA: 0x00002B88 File Offset: 0x00000D88
		public void LoadMapFile(string file, double ms)
		{
			this.Cleanup();
			this.Map = GameMap.LoadFile(file);
			this.MusicPlayer.Load(this.GetAudio(this.Map.SongId),ms);
			this.MusicPlayer.Volume = 0.04f;
			this._songLoaded = true;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002BC8 File Offset: 0x00000DC8
		public void Start()
		{
			this._gameRunning = true;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002BD4 File Offset: 0x00000DD4
		private string GetAudio(long id)
		{
			try
			{
				if (!Directory.Exists("cached"))
				{
					Directory.CreateDirectory("cached");
				}
				string text = string.Format("cached/{0}.asset", id);
				if (!File.Exists(text))
				{
					using (SecureWebClient secureWebClient = new SecureWebClient())
					{
						secureWebClient.DownloadFile(string.Format("https://assetgame.roblox.com/asset/?id={0}", id), text);
					}
				}
				return text;
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Failed to download asset with id '{0}':\n\n{1}", id, ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return null;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002C84 File Offset: 0x00000E84
		public void SetCubeStyle(string name)
		{
			GameManager.CubeModel = BloxSaber.Instance.ModelManager.GetModel(name);
			float num = 1f;
			num = Math.Min(num, this._cubeSize.X / GameManager.CubeModel.Size.X);
			num = Math.Min(num, this._cubeSize.Y / GameManager.CubeModel.Size.Y);
			num = Math.Min(num, this._cubeSize.Z / GameManager.CubeModel.Size.Z);
			this._modelScale = Matrix4.CreateScale(num);
		}
		public void SetCursorStyle(string name)
		{
			GameManager.CursorModel = BloxSaber.Instance.ModelManager.GetModel(name);
			float num = 1f;
			num = Math.Min(num, this._cubeSize.X / GameManager.CursorModel.Size.X);
			num = Math.Min(num, this._cubeSize.Y / GameManager.CursorModel.Size.Y);
			num = Math.Min(num, this._cubeSize.Z / GameManager.CursorModel.Size.Z);
			this._modelScale = Matrix4.CreateScale(num);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002D1E File Offset: 0x00000F1E
		public void Update()
		{
			this.ParticleManager.Update();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002D2C File Offset: 0x00000F2C
		public void Render(float delta)
		{
			this.UpdateSaber();
			if (this._gameRunning && BloxSaber.Instance.InputManager.IsKeyDown(Key.R))
			{
				this._stopTimer = Math.Min(1f, this._stopTimer + delta * 2f);
				if (this._stopTimer == 1f)
				{
					this._stopTimer = 0f;
					this.OnEnded(false);
				}
				BloxSaber.Instance.Camera.UploadOrtho();

				GL.Disable(EnableCap.DepthTest);
				GL.Color4(1f, 0f, 0.65f, this._stopTimer);
				//BloxSaber.Instance.FontRenderer.RenderCentered("GIVING UP", BloxSaber.Instance.Width / 2, BloxSaber.Instance.Height / 2 - 75, 150);
				GL.LineWidth(8f);
				GL.Color4(0f, 0.65f, 1f, this._stopTimer);
				GL.Begin(PrimitiveType.Lines);
				GL.Vertex2(BloxSaber.Instance.Width / 2 - 300, BloxSaber.Instance.Height / 2 + 75);
				GL.Vertex2((float)(BloxSaber.Instance.Width / 2) - 300.1f + 600f * this._stopTimer, (float)(BloxSaber.Instance.Height / 2 + 75));
				GL.End();
				GL.LineWidth(1f);
				GL.Enable(EnableCap.DepthTest);
				if (!BloxSaber.Instance.camlock)
				{
					BloxSaber.Instance.Camera.UploadProjection();
				}
                
			}
			else
			{
				this._stopTimer = 0f;
			}
			if (this._gameRunning)
			{
			//	Task.Wait(200f)
				if (this._songLoaded)
                {
					this.MoveCubes(delta);
                }	
				/*if (!this.MusicPlayer.IsPlaying && this._songStarted && this._noteIndex == this.Map.Notes.Count)					this literally is just to trigger it to stop at the end but breaks when starting in a certain position
				{																													just hold r to exit			(or hit the x or alt+f4 ig)
					this.OnEnded(true);
				}*/
			}
			GameManager.CubeShader.Bind();
			GameManager.CubeModel.Bind();
			Vector4 zero = Vector4.Zero;
			float num = (new Vector4(GameManager.CubeModel.Size) * this._modelScale).Z / 2f;
			for (int i = 0; i < this._cubes.Count; i++)
			{
				Cube cube = this._cubes[i];
				float x = cube.IndexX - 1f;
				float y = cube.IndexY - 1f;
				float num2 = cube.Z - num;
				zero.X = cube.Color.R;
				zero.Y = cube.Color.G;
				zero.Z = cube.Color.B;
				zero.W = Math.Min(1f, (this._spawnZ - num2) / 10f);
				GameManager.CubeShader.SetMatrix4("transformationMatrix", this._modelScale * Matrix4.CreateTranslation(x, y, num2));
				GameManager.CubeShader.SetVector4("colorIn", zero);
				GameManager.CubeModel.Render();
			}
			GameManager.CubeModel.Unbind();
			GameManager.CubeShader.Unbind();
			this.ParticleManager.Render((double)delta);
		}
		// Token: 0x06000039 RID: 57 RVA: 0x00003048 File Offset: 0x00001248
		private void MoveCubes(float delta)
		{
			float num = this._spawnZ / this._noteSpeed;
			while (this._noteIndex < this.Map.Notes.Count && this.Map.Notes[this._noteIndex].Time < this.MusicPlayer.CurrentTime.TotalSeconds + (double)num - (double)this._startTimer)
			{
				Note note = this.Map.Notes[this._noteIndex];
				double num2 = note.Time - this.MusicPlayer.CurrentTime.TotalSeconds + (double)this._startTimer;
				float z = this._noteSpeed * ((float)num2 + delta);
				this._cubes.Add(new Cube(z, note.X, note.Y, this.Colors.Next()));
				this._noteIndex++;
			}
			float num3 = delta * this._noteSpeed;
			for (int i = this._cubes.Count - 1; i >= 0; i--)
			{
				Cube cube = this._cubes[i];
				cube.Z -= num3;
				if (cube.Z <= 0f)
				{
					float x = cube.IndexX - 1f;
					float y = cube.IndexY - 1f;
					float z2 = cube.Z;
					if (this.IsPointInside(new Vector3(x, y, z2), (double)num3))
					{
						this._cubes.RemoveAt(i);
						this.OnHit(cube);
					}
					else if (cube.Z < -this._cubeSize.Z)
					{
						this._cubes.RemoveAt(i);
						this.OnMiss();
					}
				}
			}
			this._startTimer = Math.Max(0f, this._startTimer - delta);
			if (this._startTimer == 0f && !this._songStarted)
			{
				this._songStarted = true;
				this.MusicPlayer.Play();
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003254 File Offset: 0x00001454
		private void UpdateSaber()
		{
			Camera camera = BloxSaber.Instance.Camera;
			Vector3 vector = camera.Pos + new Vector3(camera.LookVec.X, camera.LookVec.Y, -Math.Abs(camera.LookVec.Z)) * (float)Math.Abs(Math.Abs((double)(camera.Pos.Z - 0f) + 0.15) / (double)camera.LookVec.Z);
			float x = Mathf.Clamp(vector.X, -1.5f + this._saberSize.X / 2f, 1.5f - this._saberSize.X / 2f);
			float y = Mathf.Clamp(vector.Y, -1.5f + this._saberSize.Y / 2f, 1.5f - this._saberSize.Y / 2f);
			this._saberPos = new Vector3(x, y, 0f);
			Vector3 vector2 = this._saberPos - Vector3.UnitZ * 0.01f;
			Vector4 vec = this.Hue((float)(DateTime.Now.TimeOfDay.TotalSeconds % 3.0) / 3f);
			Matrix4 left = Matrix4.CreateScale(Math.Min(Math.Min(Math.Min(1f, this._saberSize.X / GameManager.CursorModel.Size.X), this._saberSize.Y / GameManager.CursorModel.Size.Y), (this._saberSize.Z + 0.05f) / GameManager.CursorModel.Size.Z));
			GameManager.CubeShader.Bind();
			GameManager.CursorModel.Bind();
			GameManager.CubeShader.SetMatrix4("transformationMatrix", left * Matrix4.CreateTranslation(vector2));
			GameManager.CubeShader.SetVector4("colorIn", vec);
			GameManager.CursorModel.Render();
			GameManager.CursorModel.Unbind();
			GameManager.CubeShader.Unbind();
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003480 File Offset: 0x00001680
		private void OnHit(Cube cube)
		{
			long hits = this.Hits;
			this.Hits = hits + 1L;
			this.Score += 50L;
			if (GameSettings.HitFov)
			{
				BloxSaber.Instance.Camera.Fov = 73f;
			}
			this.SoundPlayer.Play("hit", 0.085f, 1f);
			EventHandler<Cube> onHitNote = this.OnHitNote;
			if (onHitNote != null)
			{
				onHitNote(this, cube);
			}
			if (!GameSettings.HitParticles)
			{
				return;
			}
			float num = 1f;
			num = Math.Min(num, this._cubeSize.X / GameManager.CubeModel.Size.X);
			num = Math.Min(num, this._cubeSize.Y / GameManager.CubeModel.Size.Y);
			num = Math.Min(num, this._cubeSize.Z / GameManager.CubeModel.Size.Z);
			float x = cube.IndexX - 1f;
			float y = cube.IndexY - 1f;
			float z = cube.Z + this._cubeSize.Z / 2f;
			Vector3 position = new Vector3(x, y, z);
			Color4 color = new Color4(cube.Color.R, cube.Color.G, cube.Color.B, 1f);
			Particle p = new Particle(position, new Vector3(-0.25f, 0.25f, -0.25f), color, 5, num, "note_left");
			Particle p2 = new Particle(position, new Vector3(0.25f, 0.25f, -0.25f), color, 5, num, "note_right");
			this.ParticleManager.Spawn(p);
			this.ParticleManager.Spawn(p2);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003638 File Offset: 0x00001838
		private void OnEnded(bool won)
		{
			this.MusicPlayer.Stop();
			this._gameRunning = false;
			this._songStarted = false;
			if (!won)
			{
				this.SoundPlayer.Play("death", 0.15f, 1f);
			}
			EventHandler<bool> onGameEnded = this.OnGameEnded;
			if (onGameEnded == null)
			{
				return;
			}
			onGameEnded(this, won);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003690 File Offset: 0x00001890
		private void OnMiss()
		{
			long misses = this.Misses;
			this.Misses = misses + 1L;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x000036B0 File Offset: 0x000018B0
		private bool IsPointInside(Vector3 cubePos, double step)
		{
			Vector3 saberPos = this._saberPos;
			Vector3 vector = (this._cubeSize + this._saberSize) / 2f;
			Vector3 right = vector * new Vector3(1f, 1f, 0f);
			Vector3 vector2 = cubePos - right;
			Vector3 vector3 = cubePos + vector;
			return saberPos.Z >= vector2.Z && saberPos.Y >= vector2.Y && saberPos.X >= vector2.X && ((double)saberPos.Z <= (double)vector3.Z + step & saberPos.Y <= vector3.Y) && saberPos.X <= vector3.X;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003778 File Offset: 0x00001978
		private Vector4 Hue(float v)
		{
			float num = 6.2831855f * v;
			Vector4 one = Vector4.One;
			one.X = (float)(Math.Sin((double)num) * 0.5 + 0.5);
			one.Y = (float)(Math.Sin((double)(num + 2.0943952f)) * 0.5 + 0.5);
			one.Z = (float)(Math.Sin((double)(num + 4.1887903f)) * 0.5 + 0.5);
			return one;
		}

		// Token: 0x04000021 RID: 33
		public ColorSequence Colors;

		// Token: 0x04000022 RID: 34
		public GameMap Map;

		// Token: 0x04000028 RID: 40
		private readonly List<Cube> _cubes = new List<Cube>();

		// Token: 0x04000029 RID: 41
		private Matrix4 _modelScale = Matrix4.CreateScale(1f);

		// Token: 0x0400002A RID: 42
		private Vector3 _saberPos = Vector3.Zero;

		// Token: 0x0400002B RID: 43
		private readonly Vector3 _cubeSize = Vector3.One * 0.875f;

		// Token: 0x0400002C RID: 44
		private readonly Vector3 _saberSize = new Vector3(0.275f, 0.275f, 0f);

		// Token: 0x0400002D RID: 45
		private bool _gameRunning;

		private bool _songLoaded;

		// Token: 0x0400002E RID: 46
		private bool _songStarted;

		// Token: 0x0400002F RID: 47
		private readonly float _noteSpeed = 25f;

		// Token: 0x04000030 RID: 48
		private float _startTimer = 1f;

		// Token: 0x04000031 RID: 49
		private float _stopTimer;

		// Token: 0x04000032 RID: 50
		private readonly float _spawnZ = 37f;

		// Token: 0x04000033 RID: 51
		private int _noteIndex;
	}
}
