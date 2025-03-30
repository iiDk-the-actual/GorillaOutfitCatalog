using GorillaNetworking;
using System;
using UnityEngine;

namespace GorillaOutfitCatalog.Behaviours.Interactions
{
	public class OutfitButton : GorillaPressableButton
	{
		public void LateUpdate()
		{
			if (!(!unpressedMaterial || !pressedMaterial))
			{
				GetComponent<Renderer>().material = (touchTime + 0.25f > Time.time) ? pressedMaterial : unpressedMaterial;
			}
		}

		public void SetButtonMat(bool pressed)
		{
			GetComponent<Renderer>().material = pressed ? pressedMaterial : unpressedMaterial;
        }

        public override void ButtonActivation()
        {
            base.ButtonActivation();
			touchTime = Time.time;
        }
    }
}
