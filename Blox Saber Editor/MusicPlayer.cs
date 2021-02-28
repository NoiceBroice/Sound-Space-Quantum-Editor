using System;
using ManagedBass;
using ManagedBass.Fx;

namespace Sound_Space_Editor
{
	class MusicPlayer : IDisposable
	{
		private object locker = new object();

		private int streamID;

		public MusicPlayer()
		{
			Bass.Init(-1, 44100, DeviceInitFlags.Default, IntPtr.Zero);
		}

		public void Load(string file)
		{
			var stream = Bass.CreateStream(file, 0, 0, BassFlags.Decode | BassFlags.Prescan | BassFlags.FxFreeSource);
			var volume = Volume;
			var tempo = Tempo;

			Bass.StreamFree(streamID);

			Bass.Configure(Configuration.PlaybackBufferLength, 250);
			Bass.Configure(Configuration.UpdatePeriod, 5);

			streamID = BassFx.TempoCreate(stream, BassFlags.Prescan);

			Volume = volume;
			Tempo = tempo;

			Reset();
		}

		public void Play()
		{
			Bass.ChannelPlay(streamID, false);
		}

		public void Pause()
		{
			var pos = Bass.ChannelGetPosition(streamID, PositionFlags.Bytes);

			Bass.ChannelPause(streamID);

			Bass.ChannelSetPosition(streamID, pos, PositionFlags.Bytes);
		}

		public void Stop()
		{
			Bass.ChannelStop(streamID);

			Bass.ChannelSetPosition(streamID, 0, PositionFlags.Bytes);
		}

		public float Tempo
		{
			set => Bass.ChannelSetAttribute(streamID, ChannelAttribute.Tempo, value * 100 - 100);
			get
			{
				float val = 0;

				Bass.ChannelGetAttribute(streamID, ChannelAttribute.Tempo, out val);

				return -(val + 95) / 100;
			}
		}

		public float Volume
		{
			set => Bass.ChannelSetAttribute(streamID, ChannelAttribute.Volume, value);
			get
			{
				float val = 1;

				Bass.ChannelGetAttribute(streamID, ChannelAttribute.Volume, out val);

				return val;
			}
		}

		public void Reset()
		{
			Stop();

			CurrentTime = TimeSpan.Zero;
		}
		
		public bool IsPlaying => Bass.ChannelIsActive(streamID) == PlaybackState.Playing;
		public bool IsPaused => Bass.ChannelIsActive(streamID) == PlaybackState.Paused;

		public TimeSpan TotalTime
		{
			get
			{
				long len = Bass.ChannelGetLength(streamID, PositionFlags.Bytes);
				var time = TimeSpan.FromSeconds(Bass.ChannelBytes2Seconds(streamID, len));

				return time;
			}
		}

		public TimeSpan CurrentTime
		{
			get
			{
				var pos = Bass.ChannelGetPosition(streamID, PositionFlags.Bytes);
				var length = Bass.ChannelGetLength(streamID, PositionFlags.Bytes);

				return TimeSpan.FromTicks((long)(TotalTime.Ticks * pos / (decimal)length));
			}
			set
			{
				//lock (locker)
				{
					var pos = Bass.ChannelSeconds2Bytes(streamID, value.TotalSeconds);

					Bass.ChannelSetPosition(streamID, pos, PositionFlags.Bytes);
				}
			}
		}

		public decimal Progress
		{
			get
			{
				var pos = Bass.ChannelGetPosition(streamID, PositionFlags.Bytes);
				var length = Bass.ChannelGetLength(streamID, PositionFlags.Bytes);

				return pos / (decimal)length;
			}
		}

		public void Dispose()
		{
			Bass.Free();
		}
	}
}