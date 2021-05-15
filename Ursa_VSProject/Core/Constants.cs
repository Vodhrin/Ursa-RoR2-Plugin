using UnityEngine;

namespace Ursa.Core
{
    internal class Constants
    {
        public static readonly Vector3 BaseSize = new Vector3(1.3f, 1.3f, 1.3f);

        public const float BaseMaxHealth = 135f;
        public const float LevelMaxHealth = 75f;
        public const float BaseRegen = 2.5f;
        public const float LevelRegen = 1.1f;
        public const float BaseMaxShield = 0f;
        public const float LevelMaxShield = 0f;
        public const float BaseMoveSpeed = 7f;
        public const float LevelMoveSpeed = 0f;
        public const float BaseAcceleration = 80f;
        public const float BaseJumpPower = 15f;
        public const float LevelJumpPower = 0f;
        public const int BaseJumpCount = 1;
        public const float BaseDamage = 12f;
        public const float LevelDamage = 2f;
        public const float BaseAttackSpeed = 1f;
        public const float LevelAttackSpeed = 0f;
        public const float BaseCrit = 1f;
        public const float LevelCrit = 0f;
        public const float BaseArmor = 35f;
        public const float LevelArmor = 0f;
        public const float SprintingSpeedMultiplier = 1.5f;

        public const float PassiveBaseDamageMult = 0.30f;
        public const float PassiveDebuffDuration = 20f;

        public const float PrimaryBaseAttackDuration = 1f;
        public const float PrimaryDamageCoefficient = 1.5f;
        public const float PrimaryProcCoefficient = 1f;
        public const float PrimaryHopVelocity = 7.5f;

        public const float SecondaryAttackSpeedMult = 4f;
        public const float SecondaryBuffDuration = 10f;
        public const int SecondaryBaseCharges = 5;
        public const float SecondaryBaseCooldown = 2f;

        public const bool UtilityRocksEffect = true;
        public const float UtilityDamageCoefficient = 1.5f;
        public const float UtilityProcCoefficient = 0f;
        public const float UtilityForce = 500f;
        public const float UtilityRadius = 20f;
        public const float UtilityBonusMoveSpeed = 0.3f;
        public const float UtilityBonusJumpPower = 0.25f;
        public const float UtilityBuffDuration = 3f;
        public const float UtilityDebuffSlow = 0.5f;
        public const float UtilityDebuffDuration = 5f;
        public const float UtilityBaseCooldown = 11f;

        public const float SpecialDamageMult = 2f;
        public const float SpecialBonusArmor = 400f;
        public const float SpecialMassMult = 3f;
        public const float SpecialBuffDuration = 8f;
        public const float SpecialBaseCooldown = 18f;
    }
}
