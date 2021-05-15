using System;
using R2API;

namespace Ursa.Core
{
    internal static class Language
    {
        public static void Initialize()
        {
            LanguageAPI.Add("URSA_NAME", "Ursa");
            LanguageAPI.Add("URSA_DESCRIPTION", "Angry bear." + Environment.NewLine);
            LanguageAPI.Add("URSA_SUBTITLE", "Den Father" + Environment.NewLine);
            LanguageAPI.Add("URSA_OUTRO_FLAVOR", "..and so he left, licking his wounds.");

            LanguageAPI.Add("URSA_SKIN_DEFAULT_NAME", "Default");

            LanguageAPI.Add("URSA_PASSIVE_NAME", "Fury Swipes");
            LanguageAPI.Add("URSA_PASSIVE_DESCRIPTION", "Consecutive attacks on the same target deal <style=cIsDamage>" + Core.Config.passiveBaseDamageMult.Value * 100 + "% more base damage</style>. If the same target is not attacked after 20 seconds, the bonus damage is lost.");
            LanguageAPI.Add("URSA_PRIMARY_NAME", "Sharp Claws");
            LanguageAPI.Add("URSA_PRIMARY_DESCRIPTION", "Swipe at enemies in front of you, dealing <style=cIsDamage>" + Core.Config.primaryDamageCoefficient.Value * 100 + "% damage</style>.");
            LanguageAPI.Add("URSA_SECONDARY_NAME", "Overpower");
            LanguageAPI.Add("URSA_SECONDARY_DESCRIPTION", "For each charge consumed, you gain<style=cIsUtility>" + Core.Config.secondaryAttackSpeedMult.Value + "x attack speed</style> on your next use of Sharp Claws. Has <style=cIsUtility>" + Core.Config.secondaryBaseCharges.Value + " charges</style> at base.");
            LanguageAPI.Add("URSA_UTILITY_NAME", "Earthshock");
            LanguageAPI.Add("URSA_UTILITY_DESCRIPTION", "<style=cIsUtility>Slowing</style>. <style=cIsUtility>Grounding</style>. Slams the ground, dealing <style=cIsDamage>" + Core.Config.utilityDamageCoefficient.Value * 100 + "% damage</style> " +
                                                        "and granting you <style=cIsUtility> bonus move speed and jump height </style>.");
            LanguageAPI.Add("URSA_SPECIAL_NAME", "Enrage");
            LanguageAPI.Add("URSA_SPECIAL_DESCRIPTION", "<style=cIsUtility>Grants " + Core.Config.specialBonusArmor.Value + " armor</style> and <style=cIsDamage>" + Core.Config.specialDamageMult.Value + "x Fury Swipes damage</style> for 8 seconds.");
        }
    }
}
