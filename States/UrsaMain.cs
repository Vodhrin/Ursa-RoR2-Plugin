using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;

namespace Ursa.States
{
	public class UrsaMain : GenericCharacterMain
	{
		public static float furySwipesDamageMult;
		public static float furySwipesDuration = 20f;
		public static float baseMass;

		private Transform model;

		public override void OnEnter()
		{
			base.OnEnter();
			this.model = base.GetModelTransform();
			UrsaMain.baseMass = base.rigidbody.mass;
			UrsaMain.furySwipesDamageMult = Core.Config.passiveBaseDamageMult.Value;
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			#region Change run animation if low health.
			if (base.healthComponent.combinedHealth <= base.healthComponent.fullCombinedHealth / 2)
			{
				this.GetModelAnimator().SetBool("isLowHealth", true);
			}
			else
			{
				this.GetModelAnimator().SetBool("isLowHealth", false);
			}
			#endregion

			#region Arms glow if have Overpower.
			if (base.isAuthority && base.characterBody.HasBuff(UrsaPlugin.overpowerBuff))
			{
				base.GetModelChildLocator().FindChild("R_Hand").GetComponent<Light>().enabled = true;
				base.GetModelChildLocator().FindChild("L_Hand").GetComponent<Light>().enabled = true;
				Core.Utils.HandsGlowOnOtherClients(base.gameObject, true);
			}
			else if (base.isAuthority && !base.characterBody.HasBuff(UrsaPlugin.overpowerBuff))
			{
				base.GetModelChildLocator().FindChild("R_Hand").GetComponent<Light>().enabled = false;
				base.GetModelChildLocator().FindChild("L_Hand").GetComponent<Light>().enabled = false;
				Core.Utils.HandsGlowOnOtherClients(base.gameObject, false);
			}
			#endregion

			#region Multiplying Fury Swipes if Enraged.
			if (base.isAuthority && base.characterBody.HasBuff(UrsaPlugin.enrageBuff))
			{
				UrsaMain.furySwipesDamageMult = Core.Config.passiveBaseDamageMult.Value * 2f;
				base.rigidbody.mass *= Core.Config.specialMassMult.Value;
			}
			else if (base.isAuthority && !base.characterBody.HasBuff(UrsaPlugin.enrageBuff))
			{
				UrsaMain.furySwipesDamageMult = Core.Config.passiveBaseDamageMult.Value;
				base.rigidbody.mass = UrsaMain.baseMass;
			}
			#endregion

			#region Ursa grows when Enraged.
			if (base.isAuthority && base.characterBody.HasBuff(UrsaPlugin.enrageBuff) && this.model.localScale.magnitude < (UrsaPlugin.ursaBaseSize + Enrage.bonusSize).magnitude)
			{
				float n = Time.deltaTime;
				model.localScale += new Vector3(n, n, n);
				Core.Utils.GrowOnOtherClients(base.gameObject, (UrsaPlugin.ursaBaseSize + Enrage.bonusSize));
			}
			else if (base.isAuthority && !base.characterBody.HasBuff(UrsaPlugin.enrageBuff) && this.model.localScale.magnitude > UrsaPlugin.ursaBaseSize.magnitude)
			{
				float n = Time.deltaTime;
				model.localScale -= new Vector3(n, n, n);
				Core.Utils.GrowOnOtherClients(base.gameObject, UrsaPlugin.ursaBaseSize);

			}
            #endregion

            #region Resize Ursa to normal size
			// basically i cannot resize ursa's model anymore in the createcharacter function because the unity project shit itself
			// i spent >10 hours reimporting the ursa model in different configurations to try to get back to how it was before to no avail
			// if ursa's model's scale in any axis is >1 then the animator multiplies his scale by 3 whenever the body statemachine is entered
			// the only solution i have found is to scale him after the animator is initilialized and i still dont know if this will fix the problem completely
			if(base.isAuthority && model.localScale.magnitude < UrsaPlugin.ursaBaseSize.magnitude)
            {
				float n = Time.deltaTime;
				model.localScale += new Vector3(n, n, n);
			}
            #endregion
        }
    }
}


