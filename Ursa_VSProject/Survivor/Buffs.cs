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
    internal static class Buffs
    {
        public static BuffIndex overpowerBuff;
        public static BuffIndex earthshockBuff;
        public static BuffIndex earthshockDebuff;
        public static BuffIndex enrageBuff;
        public static BuffIndex furySwipesDebuff;

        public static void Initialize()
        {
            BuffDef overpowerBuffDef = ScriptableObject.CreateInstance<BuffDef>();
            overpowerBuffDef.name = "Overpower";
            overpowerBuffDef.iconSprite = Resources.Load<Sprite>("textures/bufficons/texWarcryBuffIcon");
            overpowerBuffDef.buffColor = Color.magenta;
            overpowerBuffDef.canStack = true;
            CustomBuff overpowerCustomBuff = new CustomBuff(overpowerBuffDef);
            BuffAPI.Add(overpowerCustomBuff);

            BuffDef enrageBuffDef = ScriptableObject.CreateInstance<BuffDef>();
            enrageBuffDef.name = "Enrage";
            enrageBuffDef.iconSprite = Resources.Load<Sprite>("textures/bufficons/texBuffGenericShield");
            enrageBuffDef.buffColor = Color.magenta;
            enrageBuffDef.canStack = false;
            CustomBuff enrageCustomBuff = new CustomBuff(enrageBuffDef);
            BuffAPI.Add(enrageCustomBuff);

            BuffDef earthshockBuffDef = ScriptableObject.CreateInstance<BuffDef>();

            earthshockBuffDef.name = "Earthshock";
            earthshockBuffDef.iconSprite = Resources.Load<Sprite>("textures/bufficons/texMovespeedBuffIcon");
            earthshockBuffDef.buffColor = Color.magenta;
            earthshockBuffDef.canStack = false;
            CustomBuff earthshockCustomBuff = new CustomBuff(earthshockBuffDef);
            BuffAPI.Add(earthshockCustomBuff);

            BuffDef earthshockDebuffDef = ScriptableObject.CreateInstance<BuffDef>();
            earthshockDebuffDef.name = "Earthshock Debuff";
            earthshockDebuffDef.iconSprite = Resources.Load<Sprite>("textures/bufficons/texBuffSlow50Icon");
            earthshockDebuffDef.buffColor = Color.magenta;
            earthshockDebuffDef.canStack = false;
            earthshockDebuffDef.isDebuff = true;
            CustomBuff earthshockCustomDebuff = new CustomBuff(earthshockDebuffDef);
            BuffAPI.Add(earthshockCustomDebuff);

            BuffDef furySwipesDebuffDef = ScriptableObject.CreateInstance<BuffDef>();
            furySwipesDebuffDef.name = "Fury Swipes";
            furySwipesDebuffDef.iconSprite = Core.Assets.furySwipesBuffIcon;
            furySwipesDebuffDef.buffColor = Color.magenta;
            furySwipesDebuffDef.canStack = true;
            furySwipesDebuffDef.isDebuff = true;
            CustomBuff furySwipesCustomDebuff = new CustomBuff(furySwipesDebuffDef);
            BuffAPI.Add(furySwipesCustomDebuff);
        }

        public static void GetBuffIndices()
        {
            overpowerBuff = BuffCatalog.FindBuffIndex("Overpower");
            enrageBuff = BuffCatalog.FindBuffIndex("Enrage");
            earthshockBuff = BuffCatalog.FindBuffIndex("Earthshock");
            earthshockDebuff = BuffCatalog.FindBuffIndex("Earthshock Debuff");
            furySwipesDebuff = BuffCatalog.FindBuffIndex("Fury Swipes");
        }
    }
}
