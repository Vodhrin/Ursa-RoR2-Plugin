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

        public static void Read()
        {
            baseMaxHealth = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Max Health"), Core.Constants.BaseMaxHealth, new ConfigDescription("", null, Array.Empty<object>()));
            levelMaxHealth = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Max Health"), Core.Constants.LevelMaxHealth, new ConfigDescription("", null, Array.Empty<object>()));
            baseRegen = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Regen"), Core.Constants.BaseRegen, new ConfigDescription("", null, Array.Empty<object>()));
            levelRegen = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Regen"), Core.Constants.LevelRegen, new ConfigDescription("", null, Array.Empty<object>()));
            baseMaxShield = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "LBase Max Shield"), Core.Constants.BaseMaxShield, new ConfigDescription("", null, Array.Empty<object>()));
            levelMaxShield = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Max Shield"), Core.Constants.LevelMaxShield, new ConfigDescription("", null, Array.Empty<object>()));
            baseMoveSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Move Speed"), Core.Constants.BaseMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            levelMoveSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Move Speed"), Core.Constants.LevelMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            baseAcceleration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Acceleration"), Core.Constants.BaseAcceleration, new ConfigDescription("", null, Array.Empty<object>()));
            baseJumpPower = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Jump Power"), Core.Constants.BaseJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            levelJumpPower = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Jump Power"), Core.Constants.LevelJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            baseJumpCount = UrsaPlugin.instance.Config.Bind<int>(new ConfigDefinition("01 - Character Stats", "Base Jump Count"), Core.Constants.BaseJumpCount, new ConfigDescription("", null, Array.Empty<object>()));
            baseDamage = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Damage"), Core.Constants.BaseDamage, new ConfigDescription("", null, Array.Empty<object>()));
            levelDamage = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Damage"), Core.Constants.LevelDamage, new ConfigDescription("", null, Array.Empty<object>()));
            baseAttackSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Attack Speed"), Core.Constants.BaseAttackSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            levelAttackSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Attack Speed"), Core.Constants.LevelAttackSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            baseCrit = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Crit Chance"), Core.Constants.BaseCrit, new ConfigDescription("", null, Array.Empty<object>()));
            levelCrit = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Crit Chance"), Core.Constants.LevelCrit, new ConfigDescription("", null, Array.Empty<object>()));
            baseArmor = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Base Armor"), Core.Constants.BaseArmor, new ConfigDescription("", null, Array.Empty<object>()));
            levelArmor = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Level Armor"), Core.Constants.LevelArmor, new ConfigDescription("", null, Array.Empty<object>()));
            sprintingSpeedMultiplier = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("01 - Character Stats", "Sprinting Speed Multiplier"), Core.Constants.SprintingSpeedMultiplier, new ConfigDescription("", null, Array.Empty<object>()));

            passiveBaseDamageMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Passive - Fury Swipes", "Damage Multiplier Per Stack"), Core.Constants.PassiveBaseDamageMult, new ConfigDescription("", null, Array.Empty<object>()));
            passiveDebuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("02 - Passive - Fury Swipes", "Timer Duration"), Core.Constants.PassiveDebuffDuration, new ConfigDescription("", null, Array.Empty<object>()));

            primaryDamageCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Damage Coefficient"), Core.Constants.PrimaryDamageCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            primaryBaseAttackDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Base Attack Duration"), Core.Constants.PrimaryBaseAttackDuration, new ConfigDescription("", null, Array.Empty<object>()));
            primaryProcCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "Proc Coefficient"), Core.Constants.PrimaryProcCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            primaryHopVelocity = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("03 - Primary - Sharp Claws", "On-Hit Upward Velocity"), Core.Constants.PrimaryHopVelocity, new ConfigDescription("", null, Array.Empty<object>()));

            secondaryAttackSpeedMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Secondary - Overpower", "Attack Speed Multiplier"), Core.Constants.SecondaryAttackSpeedMult, new ConfigDescription("", null, Array.Empty<object>()));
            secondaryBuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Secondary - Overpower", "Buff Duration"), Core.Constants.SecondaryBuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            secondaryBaseCharges = UrsaPlugin.instance.Config.Bind<int>(new ConfigDefinition("04 - Secondary - Overpower", "Base Charges"), Core.Constants.SecondaryBaseCharges, new ConfigDescription("", null, Array.Empty<object>()));
            secondaryBaseCooldown = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("04 - Secondary - Overpower", "Base Cooldown"), Core.Constants.SecondaryBaseCooldown, new ConfigDescription("", null, Array.Empty<object>()));

            utilityRocksEffect = UrsaPlugin.instance.Config.Bind<bool>(new ConfigDefinition("05 - Utility - Earthshock", "Rock Kickup Effect"), Core.Constants.UtilityRocksEffect, new ConfigDescription("", null, Array.Empty<object>()));
            utilityDamageCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Damage Coefficient"), Core.Constants.UtilityDamageCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            utilityProcCoefficient = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Proc Coefficient"), Core.Constants.UtilityProcCoefficient, new ConfigDescription("", null, Array.Empty<object>()));
            utilityForce = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Force"), Core.Constants.UtilityForce, new ConfigDescription("", null, Array.Empty<object>()));
            utilityRadius = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Radius"), Core.Constants.UtilityRadius, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBonusMoveSpeed = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Bonus Move Speed"), Core.Constants.UtilityBonusMoveSpeed, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBonusJumpPower = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Bonus Jump Height"), Core.Constants.UtilityBonusJumpPower, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Buff Duration"), Core.Constants.UtilityBuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            utilityDebuffSlow = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Debuff Slow"), Core.Constants.UtilityDebuffSlow, new ConfigDescription("", null, Array.Empty<object>()));
            utilityDebuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Debuff Duration"), Core.Constants.UtilityDebuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            utilityBaseCooldown = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("05 - Utility - Earthshock", "Base Cooldown"), Core.Constants.UtilityBaseCooldown, new ConfigDescription("", null, Array.Empty<object>()));

            specialDamageMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Fury Swipes Damage Multiplier"), Core.Constants.SpecialDamageMult, new ConfigDescription("", null, Array.Empty<object>()));
            specialBonusArmor = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Bonus Armor"), Core.Constants.SpecialBonusArmor, new ConfigDescription("", null, Array.Empty<object>()));
            specialMassMult = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Mass Multiplier"), Core.Constants.SpecialMassMult, new ConfigDescription("", null, Array.Empty<object>()));
            specialBuffDuration = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Buff Duration"), Core.Constants.SpecialBuffDuration, new ConfigDescription("", null, Array.Empty<object>()));
            specialBaseCooldown = UrsaPlugin.instance.Config.Bind<float>(new ConfigDefinition("06 - Special - Enrage", "Base Cooldown"), Core.Constants.SpecialBaseCooldown, new ConfigDescription("", null, Array.Empty<object>()));
        }
    }
}
