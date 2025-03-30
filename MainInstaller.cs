using System;
using GorillaNetworking;
using GorillaOutfitCatalog.Behaviours;
using GorillaOutfitCatalog.Behaviours.Helpers;
using UnityEngine;
using Zenject;

namespace GorillaOutfitCatalog
{
	public class MainInstaller : Installer
	{
		public GameObject CosmeticController(InjectContext ctx)
		{
			return UnityEngine.Object.FindObjectOfType<CosmeticsController>().gameObject;
		}

		public override void InstallBindings()
		{
			Container.BindInterfacesAndSelfTo<Main>().FromNewComponentOn(new Func<InjectContext, GameObject>(this.CosmeticController)).AsSingle();
			Container.Bind<FileHandler>().AsSingle();
			Container.Bind<OutfitHandler>().AsSingle();
		}
	}
}
