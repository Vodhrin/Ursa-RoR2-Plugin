using System;
using BepInEx.Configuration;

namespace Ursa.Core
{
    static class Config
    {
        public static ConfigEntry<float> baseMaxHealth;
        public static ConfigEntry<float> levelMaxHealth;
        public static ConfigEntry<float> baseRegen;
        public static ConfigEntry<float> levelRegen;
        public static ConfigEntry<float> baseMaxShield;
        public static ConfigEntry<float> levelMaxShield;
        public static ConfigEntry<float> baseMoveSpeed;
        public static ConfigEntry<float> levelMoveSpeed;
        public static ConfigEntry<float> baseAcceleration;
        public static ConfigEntry<float> baseJumpPower;
        public static ConfigEntry<float> levelJumpPower;
        public static ConfigEntry<int> baseJumpCount;
        public static ConfigEntry<float> baseDamage;
        public static ConfigEntry<float> levelDamage;
        public static ConfigEntry<float> baseAttackSpeed;
        public static ConfigEntry<float> levelAttackSpeed;
        public static ConfigEntry<float> baseCrit;
        public static ConfigEntry<float> levelCrit;
        public static ConfigEntry<float> baseArmor;
        public static ConfigEntry<float> levelArmor;
        public static ConfigEntry<float> sprintingSpeedMultiplier;

        public static ConfigEntry<float> passiveBaseDamageMult;
        public static ConfigEntry<float> passiveDebuffDuration;

        public static ConfigEntry<float> primaryBaseAttackDuration;
        public static ConfigEntry<float> primaryDamageCoefficient;
        public static ConfigEntry<float> primaryProcCoefficient;
        public static ConfigEntry<float> primaryHopVelocity;

        public static ConfigEntry<float> secondaryAttackSpeedMult;
        public static ConfigEntry<float> secondaryBuffDuration;
        public static ConfigEntry<int> secondaryBaseCharges;
        public static ConfigEntry<float> secondaryBaseCooldown;

        public static ConfigEntry<bool> utilityRocksEffect;
        public static ConfigEntry<float> utilityDamageCoefficient;
        public static ConfigEntry<float> utilityProcCoefficient;
        public static ConfigEntry<float> utilityForce;
        public static ConfigEntry<float> utilityRadius;
        public static ConfigEntry<float> utilityBonusMoveSpeed;
        public static ConfigEntry<float> utilityBonusJumpPower;
        public static ConfigEntry<float> utilityBuffDuration;
        public static ConfigEntry<float> utilityDebuffSlow;
        public static ConfigEntry<float> utilityDebuffDuration;
        public static ConfigEntry<float> utilityBaseCooldown;

        public static ConfigEntry<float> specialDamageMult;
        public static ConfigEntry<float> specialBonusArmor;
        public static ConfigEntry<float> specialMassMult;
        public static ConfigEntry<float> specialBuffDuration;
        public static ConfigEntry<float> specialBaseCooldown;

        public static void ReadConfig()
        {
            baseMaxHealth = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Max Health"), Core.Constants.ursaBaseMaxHealth, new ConfigDescription("", null, Array.Empty<object>()));
            levelMaxHealth = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Max Health"), Core.Constants.ursaLevelMaxHealth, new ConfigDescription("", null, Array.Empty<object>()));
            baseRegen = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Regen"), Core.Constants.ursaBaseRegen, new ConfigDescription("", null, Array.Empty<object>()));
            levelRegen = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Regen"), Core.Constants.ursaLevelRegen, new ConfigDescription("", null, Array.Empty<object>()));
            baseMaxShield = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "LBase Max Shield"), Core.Constants.ursaBaseMaxShield, new ConfigDescription("", null, Array.Empty<object>()));
            levelMaxShield = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Max Shield"), Core.Constants.ursaLevelMaxShield, new ConfigDescription("", null, Array.Empty<object>()));
            baseMoveSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Move Speed"), Core.Constants.ursaBaseMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            levelMoveSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Move Speed"), Core.Constants.ursaLevelMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            baseAcceleration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Acceleration"), Core.Constants.ursaBaseAcceleration, new ConfigDescription("", null, Array.Empty<object>()));
            baseJumpPower = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Jump Power"), Core.Constants.ursaBaseJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            levelJumpPower = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Jump Power"), Core.Constants.ursaLevelJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            baseJumpCount = UrsaPlugin.instance.Config.Bind<int>(new ConfigDefinition("01 - Character Stats", "Base Jump Count"), Core.Constants.ursaBaseJumpCount, new ConfigDescription("", null, Array.Empty<object>()));
            baseDamage = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Damage"), Core.Constants.ursaBaseDamage, new ConfigDescription("", null, Array.Empty<object>()));
            levelDamage = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Damage"), Core.Constants.ursaLevelDamage, new ConfigDescription("", null, Array.Empty<object>()));
            baseAttackSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Attack Speed"), Core.Constants.ursaBaseAttackSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            levelAttackSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Attack Speed"), Core.Constants.ursaLevelAttackSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            baseCrit = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Crit Chance"), Core.Constants.ursaBaseCrit, new ConfigDescription("", null, Array.Empty<object>()));
            levelCrit = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Crit Chance"), Core.Constants.ursaLevelCrit, new ConfigDescription("", null, Array.Empty<object>()));
            baseArmor = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Armor"), Core.Constants.ursaBaseArmor, new ConfigDescription("", null, Array.Empty<object>()));
            levelArmor = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Armor"), Core.Constants.ursaLevelArmor, new ConfigDescription("", null, Array.Empty<object>()));
            sprintingSpeedMultiplier = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Sprinting Speed Multiplier"), Core.Constants.ursaSprintingSpeedMultiplier, new ConfigDescription("", null, Array.Empty<object>()));

            passiveBaseDamageMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Passive - Fury Swipes", "Damage Multiplier Per Stack"), Core.Constants.ursaPassiveBaseDamageMult, new ConfigDescription("", null, Array.Empty<object>()));
            passiveDebuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Passive - Fury Swipes", "Timer Duration"), Core.Constants.ursaPassiveDebuffDuration, new ConfigDescription("", null, Array.Empty<object>()));

            primaryDamageCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Damage Coefficient"), Core.Constants.ursaPrimaryDamageCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            primaryBaseAttackDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Base Attack Duration"), Core.Constants.ursaPrimaryBaseAttackDuration, new ConfigDescription("", null, Array.Empty<object>()));
            primaryProcCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Proc Coefficient"), Core.Constants.ursaPrimaryProcCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            primaryHopVelocity = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "On-Hit Upward Velocity"), Core.Constants.ursaPrimaryHopVelocity, new ConfigDescription("", null, Array.Empty<object>()));

            secondaryAttackSpeedMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Secondary - Overpower", "Attack Speed Multiplier"), Core.Constants.ursaSecondaryAttackSpeedMult, new ConfigDescription("", null, Array.Empty<object>()));
            secondaryBuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Secondary - Overpower", "Buff Duration"), Core.Constants.ursaSecondaryBuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            secondaryBaseCharges = UrsaPlugin.instance.Config.Bind<int>(new ConfigDefinition("04 - Secondary - Overpower", "Base Charges"), Core.Constants.ursaSecondaryBaseCharges, new ConfigDescription("", null, Array.Empty<object>()));
            secondaryBaseCooldown = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Secondary - Overpower", "Base Cooldown"), Core.Constants.ursaSecondaryBaseCooldown, new ConfigDescription("", null, Array.Empty<object>()));

            utilityRocksEffect = UrsaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("05 - Utility - Earthshock", "Rock Kickup Effect"), Core.Constants.ursaUtilityRocksEffect, new ConfigDescription("", null, Array.Empty<object>()));
            utilityDamageCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Damage Coefficient"), Core.Constants.ursaUtilityDamageCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            utilityProcCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Proc Coefficient"), Core.Constants.ursaUtilityProcCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            utilityForce = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Force"), Core.Constants.ursaUtilityForce, new ConfigDescription("", null, Array.Empty<object>()));
            utilityRadius = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Radius"), Core.Constants.ursaUtilityRadius, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBonusMoveSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Bonus Move Speed"), Core.Constants.ursaUtilityBonusMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBonusJumpPower = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Bonus Jump Height"), Core.Constants.ursaUtilityBonusJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Buff Duration"), Core.Constants.ursaUtilityBuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            utilityDebuffSlow = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Debuff Slow"), Core.Constants.ursaUtilityDebuffSlow, new ConfigDescription("", null, Array.Empty<object>()));
            utilityDebuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Debuff Duration"), Core.Constants.ursaUtilityDebuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBaseCooldown = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Base Cooldown"), Core.Constants.ursaUtilityBaseCooldown, new ConfigDescription("", null, Array.Empty<object>()));

            specialDamageMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Fury Swipes Damage Multiplier"), Core.Constants.ursaSpecialDamageMult, new ConfigDescription("", null, Array.Empty<object>()));
            specialBonusArmor = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Bonus Armor"), Core.Constants.ursaSpecialBonusArmor, new ConfigDescription("", null, Array.Empty<object>()));
            specialMassMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Mass Multiplier"), Core.Constants.ursaSpecialMassMult, new ConfigDescription("", null, Array.Empty<object>()));
            specialBuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Buff Duration"), Core.Constants.ursaSpecialBuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            specialBaseCooldown = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Base Cooldown"), Core.Constants.ursaSpecialBaseCooldown, new ConfigDescription("", null, Array.Empty<object>()));
        }
    }
}
