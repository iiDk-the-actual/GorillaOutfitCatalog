using System;
using GorillaGameModes;
using GorillaNetworking;
using GorillaOutfitCatalog.Behaviours.Models;
using Photon.Pun;
using UnityEngine;

namespace GorillaOutfitCatalog.Behaviours.Helpers
{
	public class OutfitHandler
	{
		public void EquipOutfit(Outfit outfit)
		{
			for (int i = 0; i < outfit.Set.items.Length; i++)
			{
				CosmeticsController.instance.currentWornSet.items[i] = outfit.Set.items[i];
				if (!outfit.Set.items[i].isNullItem)
				{
					CosmeticsController.CosmeticItem cosmeticItem = outfit.Set.items[i];
					if (cosmeticItem.itemCategory == CosmeticsController.CosmeticCategory.Chest && cosmeticItem.itemName == "Slingshot")
					{
						if (!PhotonNetwork.InRoom || GameMode.ActiveGameMode.GetType() != typeof(GorillaPaintbrawlManager))
						{
							CosmeticsController.instance.currentWornSet.items[i] = CosmeticsController.instance.nullItem;
							PlayerPrefs.SetString(CosmeticsController.CosmeticSet.SlotPlayerPreferenceName((CosmeticsController.CosmeticSlots)i), CosmeticsController.instance.nullItem.itemName);
						}
					}
					else
					{
						PlayerPrefs.SetString(CosmeticsController.CosmeticSet.SlotPlayerPreferenceName((CosmeticsController.CosmeticSlots)i), cosmeticItem.itemName);
					}
				}
				else
				{
					PlayerPrefs.SetString(CosmeticsController.CosmeticSet.SlotPlayerPreferenceName((CosmeticsController.CosmeticSlots)i), CosmeticsController.instance.nullItem.itemName);
				}
			}
			CosmeticsController.instance.UpdateShoppingCart();
			CosmeticsController.instance.UpdateWornCosmetics(true);
			float r = outfit.Colour.r;
			float g = outfit.Colour.g;
			float b = outfit.Colour.b;
			string name = outfit.Name;
			PhotonNetwork.LocalPlayer.NickName = name;
			GorillaComputer.instance.offlineVRRigNametagText.text = name;
			GorillaComputer.instance.currentName = name;
			GorillaComputer.instance.savedName = name;
			PlayerPrefs.SetString("playerName", name);
			PlayerPrefs.SetFloat("redValue", r);
			PlayerPrefs.SetFloat("greenValue", g);
			PlayerPrefs.SetFloat("blueValue", b);
			GorillaTagger.Instance.UpdateColor(r, g, b);

			if (PhotonNetwork.InRoom && GorillaTagger.Instance.myVRRig != null)
				GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[] { r, g, b });
			
			PlayerPrefs.Save();

			CosmeticsController.instance.OnCosmeticsUpdated.Invoke();
        }

		public Outfit GenerateOutfit()
		{
			CosmeticsController.CosmeticSet cosmeticSet = new CosmeticsController.CosmeticSet();
			for (int i = 0; i < CosmeticsController.instance.currentWornSet.items.Length; i++)
				cosmeticSet.items[i] = CosmeticsController.instance.currentWornSet.items[i];
			
			string savedName = GorillaComputer.instance.savedName;
			Color color = GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0].color;
			return new Outfit(cosmeticSet, savedName, color);
		}
	}
}
