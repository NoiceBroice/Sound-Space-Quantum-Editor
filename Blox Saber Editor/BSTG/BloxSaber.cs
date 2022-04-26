using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Blox_Saber_The_Game.gui;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Sound_Space_Editor;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000010 RID: 16
	internal class BloxSaber : GameWindow
	{
		// Token: 0x06000068 RID: 104 RVA: 0x00004268 File Offset: 0x00002468
		/*[STAThread]
		private static void BSTG(string[] args)
		{
			Environment.CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string file = null;
			try
			{
				if (File.Exists(args[0]))
				{
					file = args[0];
				}
			}
			catch
			{
			}
			using (BloxSaber bloxSaber = new BloxSaber(file))
			{
				bloxSaber.Run();
			}
		}*/

		// Token: 0x06000069 RID: 105 RVA: 0x000042D4 File Offset: 0x000024D4
		public BloxSaber() : base(EditorWindow.Instance.ClientSize.Width, EditorWindow.Instance.ClientSize.Height, new GraphicsMode(32, 24, 0, 2), "Sound Space Quantum Editor Tester" + Application.ProductVersion)
		{
			BloxSaber.Instance = this;
			this._mainThread = Thread.CurrentThread;
			GameSettings.Reload();
			this.Camera = new Camera();
			this.InputManager = new InputManager(this);
			this.ModelManager = new ModelManager();
			this.GameManager = new GameManager();
			this.GuiRenderer = new GuiRenderer();
			//this.FontRenderer = new FontRenderer("main");
			this._cursorTexture = TextureManager.GetOrRegister("cursor", null, true);
			base.TargetUpdatePeriod = 0.05;
		}
		// Token: 0x04000055 RID: 85
		private string _mapFile;
		// Token: 0x0600006A RID: 106 RVA: 0x000043CC File Offset: 0x000025CC
		protected override void OnLoad(EventArgs e)
		{
			//you might want to make a vsync toggle cus it tears to shit but then delay exists horrendously so its whatever
			base.VSync = VSyncMode.Off;
			this._mapFile = EditorWindow.Instance._file;
			GL.Enable(EnableCap.Multisample);
			GL.Enable(EnableCap.Texture2D);
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.CullFace(CullFaceMode.Back);
			base.CursorGrabbed = false;
			base.CursorVisible = false;
		//	this.GameManager.OnHitNote += this.OnHitNote;
			this.GameManager.OnGameEnded += this.OnGameEnded;
			this.LoadSounds();
			this.LoadModels();
			this.GameManager.Init();
			this.GameManager.SetCubeStyle("note");
			this.GameManager.SetCursorStyle("square");
			this.WindowState = WindowState.Fullscreen;
			this.OnFocusedChanged(EventArgs.Empty);
			base.TargetRenderFrequency = (double)GameSettings.FPSCap;
			if (File.Exists(this._mapFile))
			{

				//this.GuiRenderer.OpenScreen(new GuiScreenLoading());
				Task dothe = Task.Run(delegate()
				{	
					this.QueueMainThread(delegate
					{
						this.GameManager.LoadMapFile(this._mapFile, EditorWindow.Instance.currentTime.TotalMilliseconds);
						this.Camera.ResetCamera();
						if (camlock)
						{
							//this.Camera.CamView();
							//this.Camera.CalculateProjection();
							this.Camera.UploadProjection();
							this.RenderEnvironment(this.GetEnvironmentColor());
						}
					});
				
				});
				Task.WaitAll(dothe); //this is all kind of a joke i dont think it works but maybe
				Task dothe2 = Task.Run(delegate ()
				{
					this.QueueMainThread(delegate
					{
						this.GuiRenderer.OpenScreen(null);
						this.GameManager.Start();
					});
				});

				return;
			}
	
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000044F8 File Offset: 0x000026F8
		private void LoadSounds()
		{
			this.GameManager.SoundPlayer.Cache("click", "wav");
			this.GameManager.SoundPlayer.Cache("hit", "wav");
			this.GameManager.SoundPlayer.Cache("death", "wav");
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00004554 File Offset: 0x00002754
		private void LoadModels()
		{
			ObjModel objModel = ObjModel.FromFile("assets/models/note.obj");
			ObjModel objModel2 = ObjModel.FromFile("assets/models/note_left.obj");
			ObjModel objModel3 = ObjModel.FromFile("assets/models/note_right.obj");
			ObjModel objModel4 = ObjModel.FromFile("assets/models/square.obj");
			this.ModelManager.RegisterModel("note", objModel.GetVertexes(), null, null, true);
			this.ModelManager.RegisterModel("note_left", objModel2.GetVertexes(), null, null, false);
			this.ModelManager.RegisterModel("note_right", objModel3.GetVertexes(), null, null, false);
			this.ModelManager.RegisterModel("square", objModel4.GetVertexes(), null, null, true);
			this.ModelManager.RegisterModel("quad3d", new float[]
			{
				0f,
				0f,
				0f,
				0f,
				1f,
				0f,
				1f,
				0f,
				0f,
				1f,
				1f,
				0f,
				1f,
				0f,
				0f,
				0f,
				1f,
				0f
			}, null, null, false);
		}
		public void FuckIT()
        {
			base.Close();
        }
        protected override void OnClosed(EventArgs e)
        {
			Sound_Space_Editor.Gui.GuiScreenEditor.testin = false;
			this.GameManager.MusicPlayer.Stop();
            base.OnClosed(e);
        }

        // Token: 0x0600006D RID: 109 RVA: 0x000045F2 File Offset: 0x000027F2
        protected override void OnUpdateFrame(FrameEventArgs e)
		{
			this._tickTimer -= base.TargetUpdatePeriod;
			this.GameManager.Update();
			this.GuiRenderer.Update();
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004620 File Offset: 0x00002820
		protected override void OnRenderFrame(FrameEventArgs e)
		{
			GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
			GL.MatrixMode(MatrixMode.Projection);
			Queue<Action> mainThreadQueue = this._mainThreadQueue;
			lock (mainThreadQueue)
			{
				while (this._mainThreadQueue.Count > 0)
				{
					Action action = this._mainThreadQueue.Dequeue();
                    action?.Invoke();
                }
			}
			this._tickTimer += (double)((float)e.Time);
			this._colorTransition = Mathf.Clamp(this._colorTransition + (float)e.Time * 6f, 0f, 1f);
			Vector3 environmentColor = this.GetEnvironmentColor();
			float num = 1f - this._colorTransition;
			GL.ClearColor(environmentColor.X / (16f - num * 6f), environmentColor.Y / (16f - num * 6f), environmentColor.Z / (16f - num * 6f), 1f);
			this.UpdateCamera((float)e.Time);
			this._frameCounter++;
			if ((this._fpsTimer += e.Time) >= 1.0)
			{
				this._fps = this._frameCounter;
				this._frameCounter = 0;
				this._fpsTimer = 0.0;
			}
			GuiScreen guiScreen;
			if ((guiScreen = this.GuiRenderer.GuiScreen) == null || !guiScreen.DoesGuiStopRender())
			{
				this.GameManager.Render((float)e.Time);
				this.RenderEnvironment(environmentColor);
				if (!camlock)
				{
					this.Camera.UploadOrtho();
				}
				GL.Disable(EnableCap.DepthTest);
				GL.Color3(0f, 1f, 0f);
				//this.FontRenderer.Render(this._fps.ToString("##,###") + " FPS", 0, 0, 30);
				GL.Color3(0f, 1f, 0.65f);
				//this.FontRenderer.RenderCentered((this.GameManager.Score == 0L) ? "0" : this.GameManager.Score.ToString("##,###"), base.Width / 2, 0, 50);
				GL.Color3(1f, 0f, 0.65f);
				//this.FontRenderer.RenderCentered((this.GameManager.Misses == 0L) ? "0" : this.GameManager.Misses.ToString("##,###"), base.Width / 2, 50, 40);
				GL.Enable(EnableCap.DepthTest);
			}
			else
			{
				this.Camera.UploadOrtho();
			}
			this.GuiRenderer.Render((float)e.Time);
			//float height = this.FontRenderer.GetHeight(24);
			GL.Color3(1f, 0f, 0.65f);
			//this.FontRenderer.Render("Made by TominoCZ", 0, (int)((float)base.Height - height * 2f - 5f), 24);
			GL.Color3(0f, 1f, 0.65f);
			//this.FontRenderer.Render("www.twitch.tv/tominocz", 0, (int)((float)base.Height - height - 2f), 24);
			//int width = this.FontRenderer.GetWidth("ALPHA 0.4", 36);
			//height = this.FontRenderer.GetHeight(36);
			GL.Color4(1f, 1f, 1f, 0.25f);
			//this.FontRenderer.Render("ALPHA 0.4", base.Width - width, (int)((float)base.Height - height), 36);
			if (GuiRenderer.CursorVisible)
			{
				Point mousePos = this.InputManager.MousePos;
				double angle = DateTime.Now.TimeOfDay.TotalSeconds * 90.0;
				GL.Color4(Color4.FromHsv(new Vector4((float)(DateTime.Now.TimeOfDay.TotalSeconds % 3.0) / 3f, 1f, 1f, 1f)));
				GL.BindTexture(TextureTarget.Texture2D, this._cursorTexture);
				GL.Translate((float)mousePos.X, (float)mousePos.Y, 0f);
				GL.Rotate(angle, 0.0, 0.0, 1.0);
				GL.Scale(20f, 20f, 1f);
				GL.Begin(PrimitiveType.Quads);
				GL.TexCoord2(0, 0);
				GL.Vertex2(-0.5, -0.5);
				GL.TexCoord2(0, 1);
				GL.Vertex2(-0.5, 0.5);
				GL.TexCoord2(1, 1);
				GL.Vertex2(0.5, 0.5);
				GL.TexCoord2(1, 0);
				GL.Vertex2(0.5, -0.5);
				GL.End();
				GL.Scale(0.05f, 0.05f, 1f);
				GL.Rotate(angle, 0.0, 0.0, -1.0);
				GL.Translate((float)(-(float)mousePos.X), (float)(-(float)mousePos.Y), 0f);
				GL.BindTexture(TextureTarget.Texture2D, 0);
			}
			base.SwapBuffers();
		}

		// Token: 0x0600006F RID: 111 RVA: 0x00004B94 File Offset: 0x00002D94
		protected override void OnMouseDown(MouseButtonEventArgs e)
		{
			this.GuiRenderer.OnClick(e.X, e.Y);
		}

		// Token: 0x06000070 RID: 112 RVA: 0x00004BB0 File Offset: 0x00002DB0
		protected override void OnKeyDown(KeyboardKeyEventArgs e)
		{
			base.OnKeyDown(e);
			Key key = e.Key;
			int num = (int)key;
			if (num != 13)
			{
				if (num != 20)
				{
					return;
				}
				if (this.WindowState == WindowState.Fullscreen)
				{
					base.CursorGrabbed = true;
					base.CursorVisible = false;
					this.WindowState = WindowState.Normal;
					return;
				}
				if (this.WindowState != WindowState.Fullscreen)
				{
					this.WindowState = WindowState.Fullscreen;
				}
			}
			else if (e.Alt)
			{
				base.Close();
				return;
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004C16 File Offset: 0x00002E16
		protected override void OnFocusedChanged(EventArgs e)
		{
			base.CursorVisible = false;
			base.CursorGrabbed = (this.GuiRenderer.GuiScreen == null);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004C33 File Offset: 0x00002E33
		protected override void OnResize(EventArgs e)
		{
			GL.Viewport(base.ClientRectangle);
			this.Camera.CalculateProjection();
			this.GuiRenderer.OnResize(base.Width, base.Height);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004C64 File Offset: 0x00002E64
		private void UpdateCamera(float delta)
		{
			if (this.Camera.Fov > 70f)
			{
				this.Camera.Fov = Math.Max(70f, this.Camera.Fov - delta * 2f * 10f);
				this.Camera.CalculateProjection();
			}
			this.Camera.CalculateView();
			if (!camlock)
			{
				this.Camera.UploadView();
				this.Camera.UploadProjection();
			}
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004CE0 File Offset: 0x00002EE0
		private void RenderEnvironment(Vector3 c)
		{
			GL.LineWidth(2f);
			GL.Begin(PrimitiveType.LineLoop);
			GL.Color3(c);
			GL.Vertex3(-1.5, -1.5, 0.0);
			GL.Vertex3(-1.5, 1.5, 0.0);
			GL.Vertex3(1.5, 1.5, 0.0);
			GL.Vertex3(1.5, -1.5, 0.0);
			GL.End();
			if (GameSettings.RenderGrid)
			{
				GL.LineWidth(1f);
				GL.Begin(PrimitiveType.Lines);
				GL.Color4(1f, 1f, 1f, 0.25f);
				for (int i = 1; i < 3; i++)
				{
					GL.Vertex3(-1.5, -1.5 + (double)i, 0.0);
					GL.Vertex3(1.5, -1.5 + (double)i, 0.0);
				}
				for (int j = 1; j < 3; j++)
				{
					GL.Vertex3(-1.5 + (double)j, -1.5, 0.0);
					GL.Vertex3(-1.5 + (double)j, 1.5, 0.0);
				}
				GL.End();
			}
			GL.LineWidth(6f);
			GL.Begin(PrimitiveType.Lines);
			GL.Color3(c);
			GL.Vertex3(-2.5, -1.5, -10.0);
			GL.Color4(0, 0, 0, 0);
			GL.Vertex3(-2.5, -1.5, 75.0);
			GL.Vertex3(2.5, -1.5, 75.0);
			GL.Color3(c);
			GL.Vertex3(2.5, -1.5, -10.0);
			GL.End();
			GL.LineWidth(1f);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004F18 File Offset: 0x00003118
		private void OnHitNote(object sender, Cube c)
		{
			this._lastEnvironmentColor = this.GetEnvironmentColor();
			this._targetEnvironmentColor = new Vector3(c.Color.R, c.Color.G, c.Color.B);
			this._colorTransition = 0f;
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004F68 File Offset: 0x00003168
		private void OnGameEnded(object sender, bool won)
		{
			base.Close();
			/*double accuracy = (double)this.GameManager.Hits / (double)this.GameManager.Map.Notes.Count * 100.0;
			this.GuiRenderer.OpenScreen(new GuiScreenResults(this.GameManager.Score, this.GameManager.Misses, accuracy, won));*/
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004FCB File Offset: 0x000031CB
		public float GetTickProgress()
		{
			return (float)Math.Min(1.0, this._tickTimer / base.TargetUpdatePeriod);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00004FE9 File Offset: 0x000031E9
		public Vector3 GetEnvironmentColor()
		{
			return Vector3.Lerp(this._lastEnvironmentColor, this._targetEnvironmentColor, this._colorTransition);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00005002 File Offset: 0x00003202
		public void ResetEnvironmentColor()
		{
			this._colorTransition = 1f;
			this._targetEnvironmentColor = Vector3.One * 0.2f;
			this._lastEnvironmentColor = Vector3.One * 0.2f;
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0000503C File Offset: 0x0000323C
		public void QueueMainThread(Action a)
		{
			if (Thread.CurrentThread == this._mainThread)
			{
                a?.Invoke();
                return;
			}
			Queue<Action> mainThreadQueue = this._mainThreadQueue;
			lock (mainThreadQueue)
			{
				this._mainThreadQueue.Enqueue(a);
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x0000509C File Offset: 0x0000329C
		public override void Dispose()
		{
			Shader.DestroyAll();
			base.Dispose();
		}

		// Token: 0x04000042 RID: 66
		public static BloxSaber Instance;

		// Token: 0x04000043 RID: 67
		public const string Version = "0.4";

		// Token: 0x04000044 RID: 68
		public const string Stage = "ALPHA";

		// Token: 0x04000045 RID: 69
		public InputManager InputManager;

		// Token: 0x04000046 RID: 70
		public GuiRenderer GuiRenderer;

		// Token: 0x04000047 RID: 71
		//public FontRenderer FontRenderer;

		// Token: 0x04000048 RID: 72
		public ModelManager ModelManager;

		// Token: 0x04000049 RID: 73
		public GameManager GameManager;

		// Token: 0x0400004A RID: 74
		public Camera Camera;

		// Token: 0x0400004B RID: 75
		private readonly Queue<Action> _mainThreadQueue = new Queue<Action>();

		// Token: 0x0400004C RID: 76
		private readonly Thread _mainThread;

		// Token: 0x0400004D RID: 77
		private Vector3 _lastEnvironmentColor = Vector3.One * 0.2f;

		// Token: 0x0400004E RID: 78
		private Vector3 _targetEnvironmentColor = Vector3.One * 0.2f;

		// Token: 0x0400004F RID: 79
		private float _colorTransition = 1f;

		// Token: 0x04000050 RID: 80
		private double _tickTimer;

		// Token: 0x04000051 RID: 81
		private double _fpsTimer;

		// Token: 0x04000052 RID: 82
		private int _frameCounter;

		// Token: 0x04000053 RID: 83
		private int _fps;

		// Token: 0x04000054 RID: 84
		private readonly int _cursorTexture;

		public bool camlock = EditorSettings.Camlock;

		
	}
}
