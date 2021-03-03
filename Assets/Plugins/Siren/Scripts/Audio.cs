using System;
using System.Collections.Generic;

namespace Siren
{
    /// <summary>
    /// Entrypoint for all audio related things, this should be the only thing you'd have to talk to.
    /// </summary>
    public static class Audio
    {
        private static readonly Lazy<SFXManager> s_SFXManager = new Lazy<SFXManager>(false);
        private static readonly Lazy<SFXManager> s_MusicManager = new Lazy<SFXManager>(false);

		/// <summary>
		/// Used to send audio events inside of the audio 
		/// </summary>
		/// <param name="audioParams"></param>
		public static void SendSFXEvent(AudioEvent audioParams)
        {
			s_SFXManager.Value.ProcessEvent(audioParams);
        }
    }
}

