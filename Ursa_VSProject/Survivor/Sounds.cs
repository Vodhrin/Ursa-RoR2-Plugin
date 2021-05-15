using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;
using RoR2.Audio;
using RoR2.Skills;
using RoR2.Projectile;
using RoR2.CharacterAI;
using BepInEx;
using BepInEx.Configuration;
using R2API;
using R2API.Utils;
using R2API.Networking;
using R2API.Networking.Interfaces;
using EntityStates;
using KinematicCharacterController;

namespace Ursa.Survivor
{
    internal static class Sounds
    {
        public static NetworkSoundEventDef hitNetworkSoundEventDef;

        public static void Initialize()
        {

            hitNetworkSoundEventDef = CreateNetworkSoundEventDef(Core.Assets.HitSound);

            UrsaPlugin.networkSoundEventDefs.Add(hitNetworkSoundEventDef);

        }

        private static NetworkSoundEventDef CreateNetworkSoundEventDef(string eventName)
        {
            NetworkSoundEventDef networkSoundEventDef = ScriptableObject.CreateInstance<NetworkSoundEventDef>();
            networkSoundEventDef.akId = AkSoundEngine.GetIDFromString(eventName);
            networkSoundEventDef.eventName = eventName;
            networkSoundEventDef.index = new NetworkSoundEventIndex();

            return networkSoundEventDef;
        }
    }
}
