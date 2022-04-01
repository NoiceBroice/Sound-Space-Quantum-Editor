using System;
using System.Net;

namespace Blox_Saber_The_Game
{
	// Token: 0x02000017 RID: 23
	internal class SecureWebClient : WebClient
	{
		// Token: 0x060000A6 RID: 166 RVA: 0x00005D21 File Offset: 0x00003F21
		protected override WebRequest GetWebRequest(Uri address)
		{
			HttpWebRequest httpWebRequest = base.GetWebRequest(address) as HttpWebRequest;
			httpWebRequest.UserAgent = "RobloxProxy";
			httpWebRequest.AutomaticDecompression = (DecompressionMethods.GZip | DecompressionMethods.Deflate);
			return httpWebRequest;
		}
	}
}
