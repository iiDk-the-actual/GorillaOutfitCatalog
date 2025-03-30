using System;
using BepInEx;
using Bepinject;
using HarmonyLib;
using Unity.Burst.CompilerServices;
namespace GorillaOutfitCatalog
{
	[BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
	public class Plugin : BaseUnityPlugin
	{
		public Plugin()
		{
			new Harmony(Constants.Guid).PatchAll(typeof(Plugin).Assembly);
			Zenjector.Install<MainInstaller>().OnProject().WithLog(Logger);
		}
	}
}
