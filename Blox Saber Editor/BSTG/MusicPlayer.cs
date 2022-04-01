using System;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000011 RID: 17
	internal class MusicPlayer : IDisposable
	{
		// Token: 0x0600007E RID: 126 RVA: 0x000050F2 File Offset: 0x000032F2
		public MusicPlayer()
		{
			Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00005118 File Offset: 0x00003318
		public void Load(string file)
		{
			int channel = Bass.BASS_StreamCreateFile(file, 0L, 0L, BASSFlag.BASS_STREAM_PRESCAN | BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_OVER_VOL);
			float volume = this.Volume;
			float tempo = this.Tempo;
			Bass.BASS_StreamFree(this._streamId);
			Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 250);
			Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 5);
			this._streamId = BassFx.BASS_FX_TempoCreate(channel, BASSFlag.BASS_STREAM_PRESCAN);
			this.Volume = volume;
			this.Tempo = tempo;
			this.Reset();
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00005188 File Offset: 0x00003388
		public void Play()
		{
			Bass.BASS_ChannelPlay(this._streamId, false);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00005198 File Offset: 0x00003398
		public void Pause()
		{
			long pos = Bass.BASS_ChannelGetPosition(this._streamId, BASSMode.BASS_POS_BYTES);
			Bass.BASS_ChannelPause(this._streamId);
			Bass.BASS_ChannelSetPosition(this._streamId, pos, BASSMode.BASS_POS_BYTES);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x000051CC File Offset: 0x000033CC
		public void Stop()
		{
			long pos = Bass.BASS_ChannelGetPosition(this._streamId, BASSMode.BASS_POS_BYTES);
			Bass.BASS_ChannelStop(this._streamId);
			Bass.BASS_ChannelSetPosition(this._streamId, pos, BASSMode.BASS_POS_BYTES);
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000084 RID: 132 RVA: 0x00005220 File Offset: 0x00003420
		// (set) Token: 0x06000083 RID: 131 RVA: 0x00005200 File Offset: 0x00003400
		public float Tempo
		{
			get
			{
				float num = 0f;
				Bass.BASS_ChannelGetAttribute(this._streamId, BASSAttribute.BASS_ATTRIB_TEMPO, ref num);
				return -(num + 95f) / 100f;
			}
			set
			{
				Bass.BASS_ChannelSetAttribute(this._streamId, BASSAttribute.BASS_ATTRIB_TEMPO, value * 100f - 100f);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x06000086 RID: 134 RVA: 0x00005264 File Offset: 0x00003464
		// (set) Token: 0x06000085 RID: 133 RVA: 0x00005254 File Offset: 0x00003454
		public float Volume
		{
			get
			{
				float result = 1f;
				Bass.BASS_ChannelGetAttribute(this._streamId, BASSAttribute.BASS_ATTRIB_VOL, ref result);
				return result;
			}
			set
			{
				Bass.BASS_ChannelSetAttribute(this._streamId, BASSAttribute.BASS_ATTRIB_VOL, value);
			}
		}

		// Token: 0x06000087 RID: 135 RVA: 0x00005287 File Offset: 0x00003487
		public void Reset()
		{
			this.Stop();
			this.CurrentTime = TimeSpan.Zero;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000088 RID: 136 RVA: 0x0000529A File Offset: 0x0000349A
		public bool IsPlaying
		{
			get
			{
				return Bass.BASS_ChannelIsActive(this._streamId) == BASSActive.BASS_ACTIVE_PLAYING;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000052AA File Offset: 0x000034AA
		public bool IsPaused
		{
			get
			{
				return Bass.BASS_ChannelIsActive(this._streamId) == BASSActive.BASS_ACTIVE_PAUSED;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600008A RID: 138 RVA: 0x000052BC File Offset: 0x000034BC
		public TimeSpan TotalTime
		{
			get
			{
				long pos = Bass.BASS_ChannelGetLength(this._streamId, BASSMode.BASS_POS_BYTES);
				return TimeSpan.FromSeconds(Bass.BASS_ChannelBytes2Seconds(this._streamId, pos));
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600008B RID: 139 RVA: 0x000052E8 File Offset: 0x000034E8
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000533C File Offset: 0x0000353C
		public TimeSpan CurrentTime
		{
			get
			{
				long num = Bass.BASS_ChannelGetPosition(this._streamId, BASSMode.BASS_POS_BYTES);
				long value = Bass.BASS_ChannelGetLength(this._streamId, BASSMode.BASS_POS_BYTES);
				return TimeSpan.FromTicks((long)(this.TotalTime.Ticks * num / value));
			}
			set
			{
				long pos = Bass.BASS_ChannelSeconds2Bytes(this._streamId, value.TotalSeconds);
				Bass.BASS_ChannelSetPosition(this._streamId, pos, BASSMode.BASS_POS_BYTES);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x0600008D RID: 141 RVA: 0x0000536C File Offset: 0x0000356C
		public decimal Progress
		{
			get
			{
				long value = Bass.BASS_ChannelGetPosition(this._streamId, BASSMode.BASS_POS_BYTES);
				long value2 = Bass.BASS_ChannelGetLength(this._streamId, BASSMode.BASS_POS_BYTES);
				return value / value2;
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000053A2 File Offset: 0x000035A2
		public void Dispose()
		{
			Bass.BASS_Free();
		}

		// Token: 0x04000056 RID: 86
		private readonly object _locker = new object();

		// Token: 0x04000057 RID: 87
		private int _streamId;
	}
}
