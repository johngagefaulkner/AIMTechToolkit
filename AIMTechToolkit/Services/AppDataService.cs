using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMTechToolkit.Services
{
	internal static class AppDataService
	{
		public static string GetLocalAppDataPath() => Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		public static string GetRoamingAppDataPath() => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
	}
}
