using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AIMTechToolkit.Services
{
	internal static class SettingsService
	{
		private static ApplicationDataContainer _localSettings = ApplicationData.Current.LocalSettings;

		internal static int GetInt(string key)
		{
			if (!_localSettings.Values.ContainsKey(key))
			{
				return -1;
			}

			object valueFromSettings = _localSettings.Values[key];

			if (valueFromSettings == null)
			{
				_localSettings.Values[key] = -1;
				valueFromSettings = -1;
			}

			return (int)valueFromSettings;
		}


		internal static bool GetBool(string key)
		{
			object valueFromSettings = _localSettings.Values[key];

			if (valueFromSettings == null)
			{
				_localSettings.Values[key] = true;
				valueFromSettings = true;
			}

			return (bool)valueFromSettings;
		}

		internal static string GetString(string key, string optionalDefaultValue = "")
		{
			object valueFromSettings = _localSettings.Values[key];

			if (valueFromSettings == null)
			{
				if (!string.IsNullOrEmpty(optionalDefaultValue))
				{
					_localSettings.Values[key] = optionalDefaultValue;
					valueFromSettings = optionalDefaultValue;
				}

				else
				{
					valueFromSettings = string.Empty;
				}
			}

			return (string)valueFromSettings;
		}

		internal static void SetSetting(string key, object value)
		{
			if (string.IsNullOrEmpty(key))
			{
				throw new ArgumentNullException(nameof(key));
			}

			else if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			else
			{
				_localSettings.Values[key] = value;
			}
		}

		internal static void RemoveSetting(string key)
		{
			Windows.Storage.ApplicationData.Current.LocalSettings.Values.Remove(key);
		}
	}
}
