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

namespace Ursa
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin("com.Vodhr.UrsaSurvivor", "Ursa Survivor", "0.3.0")]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "SurvivorAPI",
        "LoadoutAPI",
        "BuffAPI",
        "LanguageAPI",
        "SoundAPI",
        "EffectAPI",
        "UnlockablesAPI",
        "ResourcesAPI",
        "EntityAPI",
        "NetworkingAPI"
    })]

    public class UrsaPlugin : BaseUnityPlugin
    {
        public static List<GameObject> characterBodies;
        public static List<GameObject> characterMasters;
        public static List<BuffDef> buffDefs;
        public static List<EffectDef> effectDefs;
        public static List<Type> entityStateTypes;
        public static List<NetworkSoundEventDef> networkSoundEventDefs;
        public static List<GameObject> projectilePrefabs;
        public static List<SkillDef> skillDefs;
        public static List<SkillFamily> skillFamilies;
        public static List<SurvivorDef> survivorDefs;
        public static List<UnlockableDef> unlockableDefs;

        public static BepInEx.Logging.ManualLogSource logger;
        public static UrsaPlugin instance;

        public void Awake()
        {
            instance = this;
            logger = this.Logger;

            CreateHooks();
            InitializeLists();

            Core.Assets.Initialize();
            Core.Config.Read();
            Core.Language.Initialize();
            Survivor.Character.Create();
            Survivor.Character.Initialize();
            Survivor.Buffs.Initialize();
            Survivor.Skills.Initialize();
            Survivor.Sounds.Initialize();
            Survivor.Effects.Initialize();
            Core.NetMessages.Initialize();

            new Core.UrsaContentPack().Initialize();
        }

        private void CreateHooks()
        {
            On.RoR2.CharacterBody.RecalculateStats += Core.Hooks.CharacterBody_RecalculateStats;
            On.RoR2.CharacterBody.ClearTimedBuffs_BuffIndex += Core.Hooks.CharacterBody_ClearTimedBuffs_BuffIndex;
            On.RoR2.HealthComponent.TakeDamage += Core.Hooks.HealthComponent_TakeDamage;
            On.RoR2.BuffCatalog.Init += Core.Hooks.BuffCatalog_Init;
        }

        private static void InitializeLists()
        {
            characterBodies = new List<GameObject>();
            characterMasters = new List<GameObject>();
            buffDefs = new List<BuffDef>();
            effectDefs = new List<EffectDef>();
            entityStateTypes = new List<Type>();
            networkSoundEventDefs = new List<NetworkSoundEventDef>();
            projectilePrefabs = new List<GameObject>();
            skillDefs = new List<SkillDef>();
            skillFamilies = new List<SkillFamily>();
            survivorDefs = new List<SurvivorDef>();
            unlockableDefs = new List<UnlockableDef>();
        }
    }
}
