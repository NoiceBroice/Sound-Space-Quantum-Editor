using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000009 RID: 9
	internal class GameMap
	{
		// Token: 0x06000040 RID: 64 RVA: 0x00003809 File Offset: 0x00001A09
		private GameMap(long songId, List<Note> notes)
		{
			this.SongId = songId;
			this.Notes = notes;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003820 File Offset: 0x00001A20
		public static GameMap LoadFile(string file)
		{
			string[] array = File.ReadAllText(file).Split(new char[]
			{
				','
			});
			long songId = long.Parse(array[0]);
			List<Note> list = new List<Note>();
			string numberDecimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
			for (int i = 1; i < array.Length; i++)
			{
				string[] array2 = array[i].Split(new char[]
				{
					'|'
				});
				Note item = new Note
				{
					X = float.Parse(array2[0].Replace(".", numberDecimalSeparator).Replace(",", numberDecimalSeparator)),
					Y = float.Parse(array2[1].Replace(".", numberDecimalSeparator).Replace(",", numberDecimalSeparator)),
					Time = (double)long.Parse(array2[2]) / 1000.0
				};
				list.Add(item);
			}
			return new GameMap(songId, list)
			{
				MapName = Path.GetFileNameWithoutExtension(file)
			};
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003917 File Offset: 0x00001B17
		public static GameMap Load(string name)
		{
			return GameMap.LoadFile(name); //full dir + extension
		}

		// Token: 0x04000034 RID: 52
		public List<Note> Notes;

		// Token: 0x04000035 RID: 53
		public string MapName;

		// Token: 0x04000036 RID: 54
		public long SongId;
	}
}
