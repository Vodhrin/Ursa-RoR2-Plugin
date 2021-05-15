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

namespace Ursa.Core
{
    internal static class Hooks
    {
        public static void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            if (self)
            {
                if (self.HasBuff(Survivor.Buffs.enrageBuff))
                {
                    self.armor += Core.Config.specialBonusArmor.Value;
                }
                if (self.HasBuff(Survivor.Buffs.earthshockBuff))
                {
                    float bonusSpeed = self.moveSpeed * Core.Config.utilityBonusMoveSpeed.Value;
                    float bonusJumpHeight = self.jumpPower * Core.Config.utilityBonusJumpPower.Value;
                    self.moveSpeed += bonusSpeed;
                    self.jumpPower += bonusJumpHeight;
                }
                if (self.HasBuff(Survivor.Buffs.earthshockDebuff))
                {
                    self.moveSpeed *= States.Earthshock.debuffSlow;
                    if (!self.gameObject.GetComponent<Miscellaneous.AntiFlyingComponent>())
                    {
                        self.gameObject.AddComponent<Miscellaneous.AntiFlyingComponent>();
                    }
                }
            }
        }

        public static void CharacterBody_ClearTimedBuffs_BuffIndex(On.RoR2.CharacterBody.orig_ClearTimedBuffs_BuffIndex orig, CharacterBody self, BuffIndex buffType)
        {
            if (buffType == Survivor.Buffs.furySwipesDebuff) return;

            orig(self, buffType);
        }

        public static void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {

            if (damageInfo == null) { orig(self, damageInfo); return; }
            else if (!self || !self.gameObject.GetComponent<CharacterBody>()) { orig(self, damageInfo); return; }
            else if (!damageInfo.attacker || !damageInfo.attacker.GetComponent<CharacterBody>()) { orig(self, damageInfo); return; }

            CharacterBody characterBody = self.gameObject.GetComponent<CharacterBody>();
            CharacterBody attackerCharacterBody = damageInfo.attacker.GetComponent<CharacterBody>();
            Miscellaneous.FurySwipesController furySwipesController = attackerCharacterBody.GetComponent<Miscellaneous.FurySwipesController>();

            if (attackerCharacterBody.baseNameToken == "URSA_NAME" && characterBody.HasBuff(Survivor.Buffs.furySwipesDebuff) && damageInfo.procCoefficient >= 1 && furySwipesController)
            {
                int count = characterBody.GetBuffCount(Survivor.Buffs.furySwipesDebuff);
                float bonusDamage = count * (attackerCharacterBody.damage * furySwipesController.furySwipesMult);
                damageInfo.damage += bonusDamage;
            }

            orig(self, damageInfo);

            if (attackerCharacterBody.baseNameToken == "URSA_NAME" && damageInfo.procCoefficient >= 1)
            {
                characterBody.AddTimedBuff(Survivor.Buffs.furySwipesDebuff, Core.Config.passiveDebuffDuration.Value);
                foreach (CharacterBody.TimedBuff i in characterBody.timedBuffs)
                {
                    i.timer = Core.Config.passiveDebuffDuration.Value;
                }
            }
        }

        public static void BuffCatalog_Init(On.RoR2.BuffCatalog.orig_Init orig)
        {
            orig();
            Survivor.Buffs.GetBuffIndices();
        }
    }
}
