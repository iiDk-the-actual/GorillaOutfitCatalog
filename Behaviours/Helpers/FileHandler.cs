using System;
using System.IO;
using GorillaOutfitCatalog.Behaviours.Models;
using UnityEngine;

namespace GorillaOutfitCatalog.Behaviours.Helpers
{
	public class FileHandler
	{
		public FileHandler()
		{
			_filePath = Application.persistentDataPath;
		}

		public Outfit Load(string fileName)
		{
			string text = Path.Combine(_filePath, fileName + ".txt");
			bool flag = !File.Exists(text);
			Outfit outfit2;
			if (flag)
			{
				Outfit outfit = new Outfit();
				Save(fileName, outfit);
				outfit2 = outfit;
			}
			else
			{
				outfit2 = JsonUtility.FromJson<Outfit>(File.ReadAllText(text));
			}
			return outfit2;
		}

		public void Save(string fileName, Outfit outfit)
		{
			string text = Path.Combine(_filePath, fileName + ".txt");
			File.WriteAllText(text, JsonUtility.ToJson(outfit));
		}

		private string _filePath;
	}
}
