using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GorillaNetworking;
using GorillaOutfitCatalog.Behaviours.Helpers;
using GorillaOutfitCatalog.Behaviours.Interactions;
using GorillaOutfitCatalog.Behaviours.Models;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace GorillaOutfitCatalog.Behaviours
{
	public class Main : MonoBehaviour, IInitializable
	{
		[Inject]
		public void Construct(FileHandler fileHandler, OutfitHandler outfitHandler)
		{
			_fileHandler = fileHandler;
			_outfitHandler = outfitHandler;
		}

		public void Initialize()
		{
			CosmeticWardrobe wardrobe = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/SatelliteWardrobe").GetComponent<CosmeticWardrobe>();
			OutfitButton outfitButton;
			CreateItem(wardrobe, "CurrentValue_B", new Vector3(0f, 0.28f, 0f), Vector3.up * 0.28f, out outfitButton);
			outfitButton.name = "Load1";
			outfitButton.myText.text = "LOAD [1]";
			OutfitButton outfitButton2;
			CreateItem(wardrobe, "CurrentValue_B", new Vector3(0f, 0.2f, 0f), Vector3.up * 0.2f, out outfitButton2);
			outfitButton2.name = "Load2";
			outfitButton2.myText.text = "LOAD [2]";
			OutfitButton outfitButton3;
			CreateItem(wardrobe, "CurrentValue_B", new Vector3(0f, 0.12f, 0f), Vector3.up * 0.12f, out outfitButton3);
			outfitButton3.name = "Load3";
			outfitButton3.myText.text = "LOAD [3]";
			OutfitButton outfitButton4;
			CreateItem(wardrobe, "CurrentValue_R", new Vector3(0f, 0.28f, 0f), Vector3.up * 0.28f, out outfitButton4);
			outfitButton4.name = "Save1";
			outfitButton4.myText.text = "SAVE [1]";
			OutfitButton outfitButton5;
			CreateItem(wardrobe, "CurrentValue_R", new Vector3(0f, 0.2f, 0f), Vector3.up * 0.2f, out outfitButton5);
			outfitButton5.name = "Save2";
			outfitButton5.myText.text = "SAVE [2]";
			OutfitButton outfitButton6;
			CreateItem(wardrobe, "CurrentValue_R", new Vector3(0f, 0.12f, 0f), Vector3.up * 0.12f, out outfitButton6);
			outfitButton6.name = "Save3";
			outfitButton6.myText.text = "SAVE [3]";
			OutfitButton[] array = new OutfitButton[] { outfitButton, outfitButton2, outfitButton3, outfitButton4, outfitButton5, outfitButton6 };
			for (int i = 0; i < array.Length; i++)
			{
				int stored = i;
				UnityEvent unityEvent = new UnityEvent();
				unityEvent.AddListener(delegate
                {
                    switch (stored)
					{
					case 0:
						_outfitHandler.EquipOutfit(_outfit1);
						break;
					case 1:
						_outfitHandler.EquipOutfit(_outfit2);
						break;
					case 2:
						_outfitHandler.EquipOutfit(_outfit3);
						break;
					case 3:
						_outfit1 = _outfitHandler.GenerateOutfit();
						_fileHandler.Save(string.Join("_", new string[] { "GorillaOutftCatalog", "Outfit1" }), _outfit1);
						break;
					case 4:
						_outfit2 = _outfitHandler.GenerateOutfit();
						_fileHandler.Save(string.Join("_", new string[] { "GorillaOutftCatalog", "Outfit2" }), _outfit2);
						break;
					case 5:
						_outfit3 = _outfitHandler.GenerateOutfit();
						_fileHandler.Save(string.Join("_", new string[] { "GorillaOutftCatalog", "Outfit3" }), _outfit3);
						break;
					}
				});
				array[i].onPressButton = unityEvent;
			}
			_outfit1 = _fileHandler.Load(string.Join("_", new string[] { "GorillaOutftCatalog", "Outfit1" }));
			_outfit2 = _fileHandler.Load(string.Join("_", new string[] { "GorillaOutftCatalog", "Outfit2" }));
			_outfit3 = _fileHandler.Load(string.Join("_", new string[] { "GorillaOutftCatalog", "Outfit3" }));
		}

		private void CreateItem(CosmeticWardrobe wardrobe, string name, Vector3 offset, Vector3 textOffset, out OutfitButton outfitButton)
		{
			GameObject gameObject = wardrobe.gameObject.transform.Find(name).gameObject;

			GameObject button = LoadAsset("button").transform.Find("Cube").gameObject;
			GameObject textObject = button.transform.Find("Canvas/Text").gameObject;
			Text text = textObject.GetComponent<Text>();

			button.layer = LayerMask.NameToLayer("GorillaInteractable");

			button.transform.parent = gameObject.transform.parent;
			button.transform.localPosition = wardrobe.gameObject.transform.Find(name).gameObject.transform.localPosition + offset;// - new Vector3(0f, 0f, 0.020525f);
            button.transform.localRotation = wardrobe.gameObject.transform.Find(name).gameObject.transform.localRotation;

			outfitButton = button.AddComponent<OutfitButton>();
			outfitButton.pressedMaterial = LoadMaterialFromResource("pressedMaterial");
			outfitButton.unpressedMaterial = LoadMaterialFromResource("unpressedMaterial");
			outfitButton.buttonRenderer = button.GetComponent<MeshRenderer>();
			outfitButton.myText = text;
			outfitButton.debounceTime = 2f;
		}

        private AssetBundle assetBundle;
        public Dictionary<string, Material> matPool = new Dictionary<string, Material> { };
        public Material LoadMaterialFromResource(string resourcePath)
        {
            Material mat = null;

            if (!matPool.ContainsKey(resourcePath))
            {
                Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaOutfitCatalog.Resources.outfit");
                if (stream != null)
                {
                    if (assetBundle == null)
                    {
                        assetBundle = AssetBundle.LoadFromStream(stream);
                    }
                    mat = assetBundle.LoadAsset(resourcePath) as Material;
                    matPool.Add(resourcePath, mat);
                }
                else
                {
                    Debug.LogError("Failed to load material from resource: " + resourcePath);
                }
            }
            else
            {
                mat = matPool[resourcePath];
            }

            return mat;
        }

        public GameObject LoadAsset(string assetName)
        {
            GameObject gameObject = null;

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaOutfitCatalog.Resources.outfit");
            if (stream != null)
            {
                if (assetBundle == null)
                    assetBundle = AssetBundle.LoadFromStream(stream);

                gameObject = Instantiate<GameObject>(assetBundle.LoadAsset<GameObject>(assetName));
            }
            else
            {
                Debug.LogError("Failed to load asset from resource: " + assetName);
            }

            return gameObject;
        }

        private FileHandler _fileHandler;
		private OutfitHandler _outfitHandler;

		private Outfit _outfit1;
		private Outfit _outfit2;
		private Outfit _outfit3;
	}
}
