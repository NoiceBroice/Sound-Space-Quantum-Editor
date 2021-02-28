using System;
using System.Collections.Generic;
using ManagedBass;
using ManagedBass.Fx;

namespace Sound_Space_Editor
{
	class SoundPlayer : IDisposable
	{
		private readonly Dictionary<string, string> _sounds = new Dictionary<string, string>();

		public SoundPlayer()
		{
			Bass.Init(-1, 44100, DeviceInitFlags.Default, IntPtr.Zero);
		}

		public void Cache(string id, string ext = "wav")
		{
			_sounds.Add(id, $"assets/sounds/{id}.{ext}");
		}

		public void Play(string id, float volume = 1, float pitch = 1)
		{
			if (_sounds.TryGetValue(id, out var sound))
            {
                var s = Bass.CreateStream(sound, 0, 0, BassFlags.Decode | BassFlags.Prescan | BassFlags.FxFreeSource);//sound, 0, 0, BASSFlag.BASS_STREAM_AUTOFREE);
				
				s = BassFx.TempoCreate(s, BassFlags.Prescan | BassFlags.AutoFree);

				Bass.ChannelSetAttribute(s, ChannelAttribute.Volume, volume);
				Bass.ChannelSetAttribute(s, ChannelAttribute.Pitch, (pitch - 1) * 60);

				//Bass.BASS_ChannelPlay(sound, false);
				Bass.ChannelPlay(s, false);
			}
		}

		public void Dispose()
		{
			Bass.Free();
		}
	}
}