using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Sound_Space_Editor
{
	class Program
	{

		[STAThread]
		static void Main(string[] args)
		{
			Application.SetCompatibleTextRenderingDefault(false);

			EditorWindow w;

			try
			{
				long offset = 0;

				var launcherDir = Environment.CurrentDirectory;
				var testDir = "C:\\Users\\Jayden J\\AppData\\Roaming\\SSQE\\";
				//launcherDir = testDir;
				if (args.Length > 0)
                {
					launcherDir = "";
					var i = 0;
					foreach (string argument in args.ToList<string>())
                    {
						i++;
						if (i == 1)
                        {
							launcherDir = argument;
                        } else
                        {
							launcherDir += " " + argument;
                        }
                    }
                }
				MessageBox.Show(launcherDir);

				w = new EditorWindow(offset, launcherDir);

				using (w)
				{
					w.Run();
				}
			}
			catch(Exception e)
			{
				MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}
	}
}