using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using Ursa.Core;
using EntityStates;

namespace Ursa.States
{
	public class UrsaMain : GenericCharacterMain
	{
		public const float baseFurySwipesDamageMult = 0.10f;
		public static float furySwipesDamageMult;
		public static float furySwipesDuration = 20f;
		public static float baseMass;

		private Transform model;

		public override void OnEnter()
		{
			base.OnEnter();
			this.model = base.GetModelTransform();
			UrsaMain.baseMass = base.rigidbody.mass;
			UrsaMain.furySwipesDamageMult = UrsaMain.baseFurySwipesDamageMult;
		}

		public override void OnExit()
		{
			base.OnExit();
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();

			#region Change run animation if low health.
			if(base.healthComponent.combinedHealth <= base.healthComponent.fullCombinedHealth / 2)
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
				HandsGlowOnOtherClients(base.gameObject, true);
			}
			else if (base.isAuthority && !base.characterBody.HasBuff(UrsaPlugin.overpowerBuff))
			{
				base.GetModelChildLocator().FindChild("R_Hand").GetComponent<Light>().enabled = false;
				base.GetModelChildLocator().FindChild("L_Hand").GetComponent<Light>().enabled = false;
				HandsGlowOnOtherClients(base.gameObject, false);
			}
			#endregion

			#region Multiplying Fury Swipes if Enraged.
			if (base.isAuthority && base.characterBody.HasBuff(UrsaPlugin.enrageBuff))
            {
				UrsaMain.furySwipesDamageMult = UrsaMain.baseFurySwipesDamageMult * 2f;
				base.rigidbody.mass *= Enrage.massMult;
            }
            else if(base.isAuthority && !base.characterBody.HasBuff(UrsaPlugin.enrageBuff))
            {
				UrsaMain.furySwipesDamageMult = UrsaMain.baseFurySwipesDamageMult;
				base.rigidbody.mass = UrsaMain.baseMass;
            }
            #endregion

            #region Ursa grows when Enraged.
            if (base.isAuthority && base.characterBody.HasBuff(UrsaPlugin.enrageBuff) && this.model.localScale.magnitude < (UrsaPlugin.ursaBaseSize + Enrage.bonusSize).magnitude)
            {
				float n = Time.deltaTime;
				model.localScale += new Vector3(n, n, n);
				GrowOnOtherClients(base.gameObject, (UrsaPlugin.ursaBaseSize + Enrage.bonusSize));
            }
			else if(base.isAuthority && !base.characterBody.HasBuff(UrsaPlugin.enrageBuff) && this.model.localScale.magnitude > UrsaPlugin.ursaBaseSize.magnitude)
            {
				float n = Time.deltaTime;
				model.localScale -= new Vector3(n, n, n);
				GrowOnOtherClients(base.gameObject, UrsaPlugin.ursaBaseSize);

			}
            #endregion

        }

		private void GrowOnOtherClients(GameObject gameObject, Vector3 targetScale)
		{
			NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
			if (networkIdentity)
			{
				new NetMessages.UrsaResizeMessage(networkIdentity.netId, targetScale)
					.Send(NetworkDestination.Clients);
			}
		}

		private void HandsGlowOnOtherClients(GameObject gameObject, bool glow)
        {
			NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
				new NetMessages.UrsaHandsGlowMessage(networkIdentity.netId, glow)
					.Send(NetworkDestination.Clients);
            }
        }
	}
}


