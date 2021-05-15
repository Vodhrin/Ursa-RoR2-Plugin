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

    class Overpower : BaseSkillState
    {
        public static float attackSpeedMult = Core.Config.secondaryAttackSpeedMult.Value;
        public static float buffDuration = Core.Config.secondaryBuffDuration.Value;

        private int stacks;

        public override void OnEnter()
        {
            base.OnEnter();

            Core.Utils.PlayNetworkedSound(Core.Assets.OverpowerSound, base.gameObject);

            this.stacks = base.skillLocator.secondary.stock + 1;
            base.skillLocator.secondary.stock = 0;

            Core.Utils.ApplyNetworkedTimedBuff(base.gameObject, Survivor.Buffs.overpowerBuff, this.stacks, Overpower.buffDuration);

            this.outer.SetNextStateToMain();
        }
    }
}
