using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000004 RID: 4
	internal static class GameSettings
	{
		// Token: 0x06000011 RID: 17 RVA: 0x0000235C File Offset: 0x0000055C
		public static void Reload()
		{
			GameSettings.Reset();
			try
			{
				foreach (string text in File.ReadAllLines(GameSettings._file))
				{
					try
					{
						string[] array2 = text.Trim().Replace(" ", "").Split(new char[]
						{
							'='
						});
						string text2 = array2[0].ToLower();
						string s = array2[1];
						string text3 = text2;
						if (text3 != null)
						{
							float sensitivity;
							if (!(text3 == "sensitivity"))
							{
								float num;
								if (!(text3 == "fpscap"))
								{
									bool renderGrid;
									if (!(text3 == "rendergrid"))
									{
										bool hitParticles;
										if (!(text3 == "hitparticles"))
										{
											if (text3 == "hitfov")
											{
												bool hitFov;
												if (GameSettings.TryParse(s, out hitFov))
												{
													GameSettings.HitFov = hitFov;
												}
											}
										}
										else if (GameSettings.TryParse(s, out hitParticles))
										{
											GameSettings.HitParticles = hitParticles;
										}
									}
									else if (GameSettings.TryParse(s, out renderGrid))
									{
										GameSettings.RenderGrid = renderGrid;
									}
								}
								else if (GameSettings.TryParse(s, out num))
								{
									GameSettings.FPSCap = ((num > 0f) ? Math.Max(30f, num) : 0f);
								}
							}
							else if (GameSettings.TryParse(s, out sensitivity))
							{
								GameSettings.Sensitivity = sensitivity;
							}
						}
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			GameSettings.Save();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000024D4 File Offset: 0x000006D4
		public static void Save()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("Sensitivity={0}", GameSettings.Sensitivity));
			stringBuilder.AppendLine(string.Format("FPSCap={0}", GameSettings.FPSCap));
			stringBuilder.AppendLine(string.Format("RenderGrid={0}", GameSettings.RenderGrid));
			stringBuilder.AppendLine(string.Format("HitParticles={0}", GameSettings.HitParticles));
			stringBuilder.AppendLine(string.Format("HitFov={0}", GameSettings.HitFov));
			try
			{
				File.WriteAllText(GameSettings._file, stringBuilder.ToString());
			}
			catch
			{
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002594 File Offset: 0x00000794
		public static void Reset()
		{
			GameSettings.Sensitivity = 0.085f;
			GameSettings.FPSCap = 0f;
			GameSettings.RenderGrid = false;
			GameSettings.HitParticles = false;
			GameSettings.HitFov = false;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025BC File Offset: 0x000007BC
		private static bool TryParse(string s, out float f)
		{
			string currencyDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			return float.TryParse(s.Replace(".", currencyDecimalSeparator).Replace(",", currencyDecimalSeparator), out f);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025F8 File Offset: 0x000007F8
		private static bool TryParse(string s, out bool b)
		{
			string currencyDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator;
			return bool.TryParse(s.Replace(".", currencyDecimalSeparator).Replace(",", currencyDecimalSeparator), out b);
		}

		// Token: 0x0400000D RID: 13
		public static float Sensitivity;

		// Token: 0x0400000E RID: 14
		public static float FPSCap;

		// Token: 0x0400000F RID: 15
		public static bool RenderGrid;

		// Token: 0x04000010 RID: 16
		public static bool HitParticles;

		// Token: 0x04000011 RID: 17
		public static bool HitFov;

		// Token: 0x04000012 RID: 18
		private static readonly string _file = "game.cfg";
	}
}
