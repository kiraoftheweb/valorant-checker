using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Colorful;

namespace Valorant
{
	// Token: 0x02000002 RID: 2
	internal class Program
	{
		// Token: 0x06000001 RID: 1 RVA: 0x0000207C File Offset: 0x0000027C
		[STAThread]
		private static void Main(string[] args)
		{
			Colorful.Console.SetWindowSize(15, 15);
			Colorful.Console.Title = (Colorful.Console.Title = "Valorant Checker | Cracked By SteezeyBreezey");
			Colorful.Console.Write("\n");
			Colorful.Console.Write("                 ██▒   █▓ ▄▄▄       ██▓     ▒█████   ██▀███   ▄▄▄       ███▄    █ ▄▄▄█████▓    \n", Color.Red);
			Colorful.Console.Write("                ▓██░   █▒▒████▄    ▓██▒    ▒██▒  ██▒▓██ ▒ ██▒▒████▄     ██ ▀█   █ ▓  ██▒ ▓▒    \n", Color.Red);
			Colorful.Console.Write("                ▓██  █▒░▒██  ▀█▄  ▒██░    ▒██░  ██▒▓██ ░▄█ ▒▒██  ▀█▄  ▓██  ▀█ ██▒▒ ▓██░ ▒░    \n", Color.Red);
			Colorful.Console.Write("                 ▒██ █░░░██▄▄▄▄██ ▒██░    ▒██   ██░▒██▀▀█▄  ░██▄▄▄▄██ ▓██▒  ▐▌██▒░ ▓██▓ ░     \n", Color.Red);
			Colorful.Console.Write("                 ▒▀█░   ▓█   ▓██▒░██████▒░ ████▓▒░░██▓ ▒██▒ ▓█   ▓██▒▒██░   ▓██░  ▒██▒ ░     \n", Color.Red);
			Colorful.Console.Write("                ░ ▐░   ▒▒   ▓▒█░░ ▒░▓  ░░ ▒░▒░▒░ ░ ▒▓ ░▒▓░ ▒▒   ▓▒█░░ ▒░   ▒ ▒   ▒ ░░       \n", Color.Red);
			Colorful.Console.Write("                ░ ░░    ▒   ▒▒ ░░ ░ ▒  ░  ░ ▒ ▒░   ░▒ ░ ▒░  ▒   ▒▒ ░░ ░░   ░ ▒░    ░    \n", Color.Red);
			Colorful.Console.Write("                 ░░    ░   ▒     ░ ░   ░ ░ ░ ▒    ░░   ░   ░   ▒      ░   ░ ░   ░          \n", Color.Red);
			Colorful.Console.Write("                   ░        ░  ░    ░  ░    ░ ░     ░           ░  ░         ░              \n", Color.Red);
			Colorful.Console.Write("                   ░	Cracked By SteezeyBreezey on Nulled.to								\n", Color.Red);
			Colorful.Console.Write(" [Threads] ", Color.Red);
			try
			{
				CheckerHelper.threads = int.Parse(Colorful.Console.ReadLine());
			}
			catch
			{
				CheckerHelper.threads = 100;
			}
			for (;;)
			{
				Colorful.Console.Write(" [Proxy] {NONE - SOCKS4 - SOCKS5 - HTTP} ", Color.Red);
				CheckerHelper.proxytype = Colorful.Console.ReadLine();
				CheckerHelper.proxytype = CheckerHelper.proxytype.ToUpper();
				if (CheckerHelper.proxytype == "HTTP" || CheckerHelper.proxytype == "SOCKS4" || CheckerHelper.proxytype == "SOCKS5" || CheckerHelper.proxytype == "NONE")
				{
					break;
				}
				Thread.Sleep(2000);
			}
			Task.Factory.StartNew(delegate()
			{
				CheckerHelper.UpdateTitle();
			});
			OpenFileDialog openFileDialog = new OpenFileDialog();
			string fileName;
			do
			{
				Colorful.Console.WriteLine(" [Load Combolist] ", Color.Red);
				Thread.Sleep(500);
				openFileDialog.Title = "Combolist";
				openFileDialog.DefaultExt = "txt";
				openFileDialog.Filter = "Text files|*.txt";
				openFileDialog.RestoreDirectory = true;
				openFileDialog.ShowDialog();
				fileName = openFileDialog.FileName;
			}
			while (!File.Exists(fileName));
			CheckerHelper.accounts = new List<string>(File.ReadAllLines(fileName));
			CheckerHelper.LoadCombos(fileName);
			if (CheckerHelper.proxytype != "NONE")
			{
				string fileName2;
				do
				{
					Colorful.Console.WriteLine(" [Load Proxy] ", Color.Red);
					Thread.Sleep(500);
					openFileDialog.Title = "Proxy";
					openFileDialog.DefaultExt = "txt";
					openFileDialog.Filter = "Text files|*.txt";
					openFileDialog.RestoreDirectory = true;
					openFileDialog.ShowDialog();
					fileName2 = openFileDialog.FileName;
				}
				while (!File.Exists(fileName2));
				CheckerHelper.proxies = new List<string>(File.ReadAllLines(fileName2));
				CheckerHelper.LoadProxies(fileName2);
				Colorful.Console.Write("[ ", Color.SlateGray);
				Colorful.Console.Write(CheckerHelper.total.ToString() + " ", Color.Red);
				Colorful.Console.Write(" / ", Color.SlateGray);
				Colorful.Console.Write(CheckerHelper.proxytotal, Color.Red);
				Colorful.Console.Write(" ]\n ", Color.SlateGray);
			}
			for (int i = 1; i <= CheckerHelper.threads; i++)
			{
				new Thread(new ThreadStart(CheckerHelper.Check)).Start();
			}
			Colorful.Console.ReadLine();
			Environment.Exit(0);
		}
	}
}
