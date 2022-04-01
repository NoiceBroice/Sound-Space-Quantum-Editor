using System;
using System.Collections.Generic;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000019 RID: 25
	internal class SoundPlayer : IDisposable
	{
		// Token: 0x060000BF RID: 191 RVA: 0x0000623F File Offset: 0x0000443F
		public SoundPlayer()
		{
			Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006264 File Offset: 0x00004464
		public void Cache(string id, string ext = "wav")
		{
			this._sounds.Add(id, "assets/sounds/" + id + "." + ext);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006284 File Offset: 0x00004484
		public void Play(string id, float volume = 1f, float pitch = 1f)
		{
			string file;
			if (this._sounds.TryGetValue(id, out file))
			{
				int handle = BassFx.BASS_FX_TempoCreate(Bass.BASS_StreamCreateFile(file, 0L, 0L, BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_OVER_VOL), BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_AUTOFREE);
				Bass.BASS_ChannelSetAttribute(handle, BASSAttribute.BASS_ATTRIB_VOL, volume);
				Bass.BASS_ChannelSetAttribute(handle, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, (pitch - 1f) * 60f);
				Bass.BASS_ChannelPlay(handle, false);
			}
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x000062E3 File Offset: 0x000044E3
		public void Dispose()
		{
			Bass.BASS_Free();
		}

		// Token: 0x04000072 RID: 114
		private readonly Dictionary<string, string> _sounds = new Dictionary<string, string>();
	}
}
