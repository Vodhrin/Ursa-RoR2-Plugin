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
    class Enrage : BaseSkillState
    {
        public static float buffDuration = Core.Config.specialBuffDuration.Value;
        public static Vector3 bonusSize = new Vector3(0.5f, 0.5f, 0.5f);

        public override void OnEnter()
        {
            base.OnEnter();

            Core.Utils.PlayNetworkedSound(Core.Assets.EnrageSound, base.gameObject);
            PlayAnimation("Gesture, HeadOnly, Override", "Roar", "Attack.playbackRate", 1f);
            if (ClientScene.readyConnection != null)
            {
                Core.Utils.PlayAnimationOnOtherClients(base.gameObject, "Gesture, HeadOnly, Override", "Roar", 1f);
            }

            Core.Utils.ApplyNetworkedTimedBuff(base.gameObject, Survivor.Buffs.enrageBuff, 1, Enrage.buffDuration);

            this.outer.SetNextStateToMain();
        }  
    }
}
