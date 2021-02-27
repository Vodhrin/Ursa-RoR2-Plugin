using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Networking;
using RoR2.Audio;
using R2API.Networking;
using R2API.Networking.Interfaces;
using Ursa.Core;
using EntityStates;

namespace Ursa.Core
{
    class Utils
    {
        public static void PlayAnimationOnOtherClients(GameObject gameObject, string layerName, string animationStateName, float duration, bool isSharpClaws = false)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.Animation(networkIdentity.netId, layerName, animationStateName, duration, isSharpClaws)
                    .Send(NetworkDestination.Clients);
            }
        }

        public static void PlayNetworkedSound(string soundName, GameObject gameObject, float rate = 1)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.Sound(networkIdentity.netId, soundName)
                    .Send(NetworkDestination.Clients);
            }
        }

        public static void ApplyNetworkedTimedBuff(GameObject gameObject, BuffIndex buffIndex, int stacks, float duration)
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
                        new NetMessages.TimedBuff(networkIdentity.netId, buffIndex, stacks, duration)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
        }

        public static void RemoveNetworkedTimedBuff(GameObject gameObject, BuffIndex buffIndex, int stacks)
        {
            CharacterBody characterBody = gameObject.GetComponent<CharacterBody>();

            if (characterBody)
            {
                if (NetworkServer.active)
                {
                    bool hasRemoved = false;
                    int count = 0;
                    foreach (CharacterBody.TimedBuff i in characterBody.timedBuffs)
                    {
                        if (hasRemoved) break;
                        if (i.buffIndex == buffIndex)
                        {
                            i.timer = 0;
                            count++;
                            if(count == stacks)hasRemoved = true;
                        }
                    }
                }
                else
                {
                    NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();

                    if (networkIdentity)
                    {
                        new NetMessages.RemoveTimedBuff(networkIdentity.netId, buffIndex, stacks)
                            .Send(NetworkDestination.Server);
                    }
                }
            }
        }

        public static void GrowOnOtherClients(GameObject gameObject, Vector3 targetScale)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.UrsaResize(networkIdentity.netId, targetScale)
                    .Send(NetworkDestination.Clients);
            }
        }

        public static void HandsGlowOnOtherClients(GameObject gameObject, bool glow)
        {
            NetworkIdentity networkIdentity = gameObject.GetComponent<NetworkIdentity>();
            if (networkIdentity)
            {
                new NetMessages.UrsaHandsGlow(networkIdentity.netId, glow)
                    .Send(NetworkDestination.Clients);
            }
        }
    }
}
