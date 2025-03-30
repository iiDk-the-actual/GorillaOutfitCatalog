using System;
using GorillaNetworking;
using UnityEngine;

namespace GorillaOutfitCatalog.Behaviours.Models
{
	public class Outfit
	{
		public Outfit()
		{
			this.Name = GorillaComputer.instance.savedName;
			this.Colour = GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0].color;
			this.Set = new CosmeticsController.CosmeticSet
			{
				items = new CosmeticsController.CosmeticItem[]
				{
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem,
					CosmeticsController.instance.nullItem
				}
			};
		}

		public Outfit(CosmeticsController.CosmeticSet set, string name, Color colour)
		{
			this.Set = set;
			this.Name = name;
			this.Colour = colour;
		}

		public CosmeticsController.CosmeticSet Set;

		public string Name;
		public Color Colour;
	}
}
