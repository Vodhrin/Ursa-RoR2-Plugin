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
    internal static class Skills
    {
        public static void Initialize()
        {
            SkillDef sharpClawsSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            sharpClawsSkillDef.activationState = new SerializableEntityStateType(typeof(States.SharpClaws));
            sharpClawsSkillDef.activationStateMachineName = "Weapon";
            sharpClawsSkillDef.baseMaxStock = 1;
            sharpClawsSkillDef.baseRechargeInterval = 0f;
            sharpClawsSkillDef.beginSkillCooldownOnSkillEnd = false;
            sharpClawsSkillDef.canceledFromSprinting = false;
            sharpClawsSkillDef.fullRestockOnAssign = true;
            sharpClawsSkillDef.interruptPriority = InterruptPriority.Any;
            sharpClawsSkillDef.isCombatSkill = true;
            sharpClawsSkillDef.mustKeyPress = false;
            sharpClawsSkillDef.cancelSprintingOnActivation = true;
            sharpClawsSkillDef.rechargeStock = 1;
            sharpClawsSkillDef.requiredStock = 1;
            sharpClawsSkillDef.stockToConsume = 1;
            sharpClawsSkillDef.icon = Core.Assets.icon1;
            sharpClawsSkillDef.skillDescriptionToken = "URSA_PRIMARY_DESCRIPTION";
            sharpClawsSkillDef.skillName = "URSA_PRIMARY_NAME";
            sharpClawsSkillDef.skillNameToken = "URSA_PRIMARY_NAME";

            SkillDef overpowerSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            overpowerSkillDef.activationState = new SerializableEntityStateType(typeof(States.Overpower));
            overpowerSkillDef.activationStateMachineName = "Body";
            overpowerSkillDef.baseMaxStock = Core.Config.secondaryBaseCharges.Value;
            overpowerSkillDef.baseRechargeInterval = Core.Config.secondaryBaseCooldown.Value;
            overpowerSkillDef.beginSkillCooldownOnSkillEnd = false;
            overpowerSkillDef.canceledFromSprinting = false;
            overpowerSkillDef.fullRestockOnAssign = true;
            overpowerSkillDef.interruptPriority = InterruptPriority.Skill;
            overpowerSkillDef.isCombatSkill = true;
            overpowerSkillDef.mustKeyPress = true;
            overpowerSkillDef.cancelSprintingOnActivation = false;
            overpowerSkillDef.rechargeStock = 1;
            overpowerSkillDef.requiredStock = 1;
            overpowerSkillDef.stockToConsume = 1;
            overpowerSkillDef.icon = Core.Assets.icon2;
            overpowerSkillDef.skillDescriptionToken = "URSA_SECONDARY_DESCRIPTION";
            overpowerSkillDef.skillName = "URSA_SECONDARY_NAME";
            overpowerSkillDef.skillNameToken = "URSA_SECONDARY_NAME";

            SkillDef earthshockSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            earthshockSkillDef.activationState = new SerializableEntityStateType(typeof(States.Earthshock));
            earthshockSkillDef.activationStateMachineName = "Weapon";
            earthshockSkillDef.baseMaxStock = 1;
            earthshockSkillDef.baseRechargeInterval = Core.Config.utilityBaseCooldown.Value;
            earthshockSkillDef.beginSkillCooldownOnSkillEnd = true;
            earthshockSkillDef.canceledFromSprinting = false;
            earthshockSkillDef.fullRestockOnAssign = true;
            earthshockSkillDef.interruptPriority = InterruptPriority.Skill;
            earthshockSkillDef.isCombatSkill = false;
            earthshockSkillDef.mustKeyPress = true;
            earthshockSkillDef.cancelSprintingOnActivation = false;
            earthshockSkillDef.rechargeStock = 1;
            earthshockSkillDef.requiredStock = 1;
            earthshockSkillDef.stockToConsume = 1;
            earthshockSkillDef.icon = Core.Assets.icon3;
            earthshockSkillDef.skillDescriptionToken = "URSA_UTILITY_DESCRIPTION";
            earthshockSkillDef.skillName = "URSA_UTILITY_NAME";
            earthshockSkillDef.skillNameToken = "URSA_UTILITY_NAME";

            SkillDef enrageSkillDef = ScriptableObject.CreateInstance<SkillDef>();
            enrageSkillDef.activationState = new SerializableEntityStateType(typeof(States.Enrage));
            enrageSkillDef.activationStateMachineName = "Body";
            enrageSkillDef.baseMaxStock = 1;
            enrageSkillDef.baseRechargeInterval = Core.Config.specialBaseCooldown.Value;
            enrageSkillDef.beginSkillCooldownOnSkillEnd = true;
            enrageSkillDef.canceledFromSprinting = false;
            enrageSkillDef.fullRestockOnAssign = true;
            enrageSkillDef.interruptPriority = InterruptPriority.Frozen;
            enrageSkillDef.isCombatSkill = true;
            enrageSkillDef.mustKeyPress = true;
            enrageSkillDef.cancelSprintingOnActivation = true;
            enrageSkillDef.rechargeStock = 1;
            enrageSkillDef.requiredStock = 1;
            enrageSkillDef.stockToConsume = 1;
            enrageSkillDef.icon = Core.Assets.icon4;
            enrageSkillDef.skillDescriptionToken = "URSA_SPECIAL_DESCRIPTION";
            enrageSkillDef.skillName = "URSA_SPECIAL_NAME";
            enrageSkillDef.skillNameToken = "URSA_SPECIAL_NAME";

            LoadoutAPI.AddSkillDef(sharpClawsSkillDef);
            LoadoutAPI.AddSkillDef(overpowerSkillDef);
            LoadoutAPI.AddSkillDef(earthshockSkillDef);
            LoadoutAPI.AddSkillDef(enrageSkillDef);

            foreach (GenericSkill i in Survivor.Character.body.GetComponentsInChildren<GenericSkill>())
            {
                UnityEngine.Object.DestroyImmediate(i);
            }

            SkillLocator skillLocator = Survivor.Character.body.GetComponent<SkillLocator>();

            //Passive
            skillLocator.passiveSkill.enabled = true;
            skillLocator.passiveSkill.skillNameToken = "URSA_PASSIVE_NAME";
            skillLocator.passiveSkill.skillDescriptionToken = "URSA_PASSIVE_DESCRIPTION";
            skillLocator.passiveSkill.icon = Core.Assets.icon5;

            //Primary
            skillLocator.primary = Survivor.Character.body.AddComponent<GenericSkill>();
            SkillFamily newFamilyPrimary = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilyPrimary.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilyPrimary);
            skillLocator.primary.SetFieldValue("_skillFamily", newFamilyPrimary);
            SkillFamily skillFamilyPrimary = skillLocator.primary.skillFamily;
            skillFamilyPrimary.variants[0] = new SkillFamily.Variant
            {
                skillDef = sharpClawsSkillDef,
                viewableNode = new ViewablesCatalog.Node(sharpClawsSkillDef.skillNameToken, false, null)
            };

            //Secondary
            skillLocator.secondary = Survivor.Character.body.AddComponent<GenericSkill>();
            SkillFamily newFamilySecondary = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilySecondary.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilySecondary);
            skillLocator.secondary.SetFieldValue("_skillFamily", newFamilySecondary);
            SkillFamily skillFamilySecondary = skillLocator.secondary.skillFamily;
            skillFamilySecondary.variants[0] = new SkillFamily.Variant
            {
                skillDef = overpowerSkillDef,
                viewableNode = new ViewablesCatalog.Node(overpowerSkillDef.skillNameToken, false, null)
            };

            //Utility
            skillLocator.utility = Survivor.Character.body.AddComponent<GenericSkill>();
            SkillFamily newFamilyUtility = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilyUtility.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilyUtility);
            skillLocator.utility.SetFieldValue("_skillFamily", newFamilyUtility);
            SkillFamily skillFamilyUtility = skillLocator.utility.skillFamily;
            skillFamilyUtility.variants[0] = new SkillFamily.Variant
            {
                skillDef = earthshockSkillDef,
                viewableNode = new ViewablesCatalog.Node(earthshockSkillDef.skillNameToken, false, null)
            };

            //Special
            skillLocator.special = Survivor.Character.body.AddComponent<GenericSkill>();
            SkillFamily newFamilySpecial = ScriptableObject.CreateInstance<SkillFamily>();
            newFamilySpecial.variants = new SkillFamily.Variant[1];
            LoadoutAPI.AddSkillFamily(newFamilySpecial);
            skillLocator.special.SetFieldValue("_skillFamily", newFamilySpecial);
            SkillFamily skillFamilySpecial = skillLocator.special.skillFamily;
            skillFamilySpecial.variants[0] = new SkillFamily.Variant
            {
                skillDef = enrageSkillDef,
                viewableNode = new ViewablesCatalog.Node(enrageSkillDef.skillNameToken, false, null)
            };

        }
    }
}
