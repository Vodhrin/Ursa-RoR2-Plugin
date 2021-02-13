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

    class Overpower : BaseSkillState
    {
        public static float attackSpeedMult = 4f;
        public static float buffDuration = 10f;

        private int stacks;

        public override void OnEnter()
        {
            base.OnEnter();

            PlaySound(Assets.ursaOverpowerSound, base.gameObject);

            this.stacks = base.skillLocator.secondary.stock + 1;
            base.skillLocator.secondary.stock = 0;

            ApplyNetworkedTimedBuff(base.gameObject, UrsaPlugin.overpowerBuff, this.stacks, Overpower.buffDuration);

            this.outer.SetNextStateToMain();
        }

        private void PlaySound(string soundName, GameObject gameObject, float rate = 1) 
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.SoundMessage(networkIdentity.netId, soundName)
                    .Send(NetworkDestination.Clients);
            }
        }

        private void ApplyNetworkedTimedBuff(GameObject gameObject, BuffIndex buffIndex, int stacks, float duration)
        {
            CharacterBody characterBody = gameObject.GetComponent<CharacterBody>();

            if (characterBody)
            {
                if (NetworkServer.active)
                {
                    for (int i = 0; i < stacks; i++)
                    {
                        characterBody.AddTimedBuff(buffIndex, duration);
                    }
                }
                else
                {
                    NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
                    if (networkIdentity)
                    {
                        new NetMessages.TimedBuffMessage(networkIdentity.netId, buffIndex, stacks, duration)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
        }
    }
}
