namespace AIMTechToolkit.Services
{
	public static class SettingsManagerService
	{
		private static readonly ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

		public static void Set<T>(string key, T value)
		{
			var setting = new Setting
			{
				Type = typeof(T).AssemblyQualifiedName,
				Value = JsonSerializer.Serialize(value)
			};

			localSettings.Values[key] = JsonSerializer.Serialize(setting);
		}

		public static T Get<T>(string key)
		{
			if (localSettings.Values.ContainsKey(key))
			{
				var jsonString = localSettings.Values[key] as string;

				if (!string.IsNullOrEmpty(jsonString))
				{
					var setting = JsonSerializer.Deserialize<Setting>(jsonString);

					if (setting is not null && setting.Type == typeof(T).AssemblyQualifiedName)
					{
						return JsonSerializer.Deserialize<T>(setting.Value);
					}
				}
			}

			return default(T);
		}

		public static (bool, T) GetSafely<T>(string key)
		{
			if (localSettings.Values.ContainsKey(key))
			{
				string jsonString = localSettings.Values[key] as string;
				if (jsonString != null)
				{
					var setting = JsonSerializer.Deserialize<Setting>(jsonString);
					if (setting != null && setting.Type == typeof(T).AssemblyQualifiedName)
					{
						T value = JsonSerializer.Deserialize<T>(setting.Value);
						return (true, value);
					}
				}
			}
			return (false, default(T));
		}

		public static bool Remove(string key) => localSettings.Values.Remove(key);

		public static bool ContainsKey(string key) => localSettings.Values.ContainsKey(key);

		private class Setting
		{
			public string Type { get; set; }
			public string Value { get; set; }
		}
	}
}

