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
    class Enrage : BaseSkillState
    {
        public static float buffDuration = 8f;
        public static float damageMult = 0.6f;
        public static float bonusArmor = 600f;
        public static float massMult = 3f;
        public static Vector3 bonusSize = new Vector3(0.5f, 0.5f, 0.5f); 

        public override void OnEnter()
        {
            base.OnEnter();

            PlaySound(Assets.ursaEnrageSound, base.gameObject);
            PlayAnimation("Gesture, HeadOnly, Override", "Roar", "Attack.playbackRate", 1f);
            if (ClientScene.readyConnection != null)
            {
                PlayAnimationOnOtherClients(base.gameObject, "Gesture, Headonly, Override", "Roar", 1f);
            }

            ApplyNetworkedTimedBuff(base.gameObject, UrsaPlugin.enrageBuff, 1, Enrage.buffDuration);

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

        private void PlayAnimationOnOtherClients(GameObject gameObject, string layerName, string animationStateName, float duration)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.AnimationMessage(networkIdentity.netId, layerName, animationStateName, duration)
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
