using R2API;
using RoR2;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Ursa.Core
{

    class ItemDisplays
    {
        public static void Create()
        {
            #region Defining and referencing needed info.
            GameObject body = UrsaPlugin.ursaBody;
            ModelLocator modelLocator = body.GetComponentInChildren<ModelLocator>();
            CharacterModel characterModel = modelLocator.modelTransform.GetComponent<CharacterModel>();

            ItemDisplayRuleSet itemDisplayRuleSet = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            List<ItemDisplayRuleSet.NamedRuleGroup> itemRules = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            List<ItemDisplayRuleSet.NamedRuleGroup> equipmentRules = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            #endregion

            #region Grabbing all of the default item display info.
            //This method of collecting all default item prefabs is taken from EnforcerGang's EnforcerMod.
            ItemDisplayRuleSet defaultItemDisplayRuleset = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            ItemDisplayRuleSet.NamedRuleGroup[] defaultItemNamedGroups = defaultItemDisplayRuleset.namedItemRuleGroups;
            ItemDisplayRuleSet.NamedRuleGroup[] defaultEquipmentNamedGroups = defaultItemDisplayRuleset.namedEquipmentRuleGroups;

            Dictionary<string, GameObject> itemDisplayPrefabs = new Dictionary<string, GameObject>();

            for (int i = 0; i < defaultItemNamedGroups.Length; i++)
            {
                ItemDisplayRule[] rules = defaultItemNamedGroups[i].displayRuleGroup.rules;

                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;
                    if (followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = (name != null) ? name.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }

            for (int i = 0; i < defaultEquipmentNamedGroups.Length; i++)
            {
                ItemDisplayRule[] rules = defaultEquipmentNamedGroups[i].displayRuleGroup.rules;
                for (int j = 0; j < rules.Length; j++)
                {
                    GameObject followerPrefab = rules[j].followerPrefab;

                    if (followerPrefab)
                    {
                        string name = followerPrefab.name;
                        string key = (name != null) ? name.ToLower() : null;
                        if (!itemDisplayPrefabs.ContainsKey(key))
                        {
                            itemDisplayPrefabs[key] = followerPrefab;
                        }
                    }
                }
            }
            #endregion

            #region Miscellaneous initialization.
            GameObject capacitorPrefab = PrefabAPI.InstantiateClone(defaultItemDisplayRuleset.FindEquipmentDisplayRuleGroup("Lightning").rules[0].followerPrefab, "DisplayUrsaLightning", true);
            capacitorPrefab.AddComponent<UnityEngine.Networking.NetworkIdentity>();

            var limbMatcher = capacitorPrefab.GetComponent<LimbMatcher>();

            limbMatcher.limbPairs[0].targetChildLimb = "L_Shoulder";
            limbMatcher.limbPairs[1].targetChildLimb = "L_Elbow";
            limbMatcher.limbPairs[2].targetChildLimb = "L_Hand";
            #endregion

            #region Creating all of the item displays for the base game.
            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CritGlasses",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGlasses"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.017f, 0.01f),
                            localAngles = new Vector3(-25, 0, 0),
                            localScale = new Vector3(0.027f, 0.028f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Syringe",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySyringeCluster"),
                            childName = "L_Shoulder",
                            localPos = new Vector3(-0.2266F, -0.1361F, -0.0754F),
                            localAngles = new Vector3(357.2155F, 320.744F, 144.3718F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Bear",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBear"),
                            childName = "L_Shoulder",
                            localPos = new Vector3(-0.2266F, -0.1361F, -0.0754F),
                            localAngles = new Vector3(357.2155F, 320.744F, 144.3718F),
                            localScale = new Vector3(0F, 0f, 0F),
                            limbMask = LimbFlags.None
                        }
        }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Behemoth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBehemoth"),
                            childName = "R_Elbow",
                            localPos = new Vector3(0.369F, 0.3354F, -0.0976F),
                            localAngles = new Vector3(292.6143F, 174.7119F, 89.8416F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBehemoth"),
                            childName = "L_Elbow",
                            localPos = new Vector3(-0.4709F, -0.3097F, 0.1209F),
                            localAngles = new Vector3(53.9598F, 340.9976F, 65.7309F),
                            localScale = new Vector3(0.1F, 0.1F, 0.1F),
                            limbMask = LimbFlags.None
                        }

                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Missile",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMissileLauncher"),
                            childName = "L_Shoulder",
                            localPos = new Vector3(-0.1609F, -0.3265F, -0.1299F),
                            localAngles = new Vector3(0.641F, 176.5264F, 183.7002F),
                            localScale = new Vector3(0.07F, 0.07F, 0.07F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Dagger",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayDagger"),
                            childName = "R_Shoulder",
                            localPos = new Vector3(0.1254F, 0.1409F, 0.1578F),
                            localAngles = new Vector3(330.7945F, 264.5016F, 99.3162F),
                            localScale = new Vector3(0.2F, 0.2F, 0.2F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Hoof",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayHoof"),
                            childName = "",
                            localPos = new Vector3(-0.0025f, 0.012f, -0.0125f),
                            localAngles = new Vector3(60, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ChainLightning",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayUkulele"),
                            childName = "Root",
                            localPos = new Vector3(-0.0057f, -0.0011f, -0.023f),
                            localAngles = new Vector3(0, 180, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GhostOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMask"),
                            childName = "Head",
                            localPos = new Vector3(-0.3766F, -0.0006F, -0.0882F),
                            localAngles = new Vector3(6.3771F, 254.629F, 268.8696F),
                            localScale = new Vector3(0.3F, 0.3F, 0.3F),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Mushroom",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMushroom"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(64, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AttackSpeedOnCrit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWolfPelt"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.016f, -0.005f),
                            localAngles = new Vector3(-25, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BleedOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTriTip"),
                            childName = "Spine3",
                            localPos = new Vector3(0.01261f, 0.04054f, 0.02003f),
                            localAngles = new Vector3(124, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTriTip"),
                            childName = "Spine3",
                            localPos = new Vector3(0.01261f, 0.04054f, 0.02003f),
                            localAngles = new Vector3(124, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "WardOnLevel",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWarbanner"),
                            childName = "Spine3",
                            localPos = new Vector3(0, -0.01383f, -0.02629f),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "HealOnCrit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayScythe"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.0064f, 0.0177f, -0.023f),
                            localAngles = new Vector3(-145, 92, -94),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "HealWhileSafe",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySnail"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 45, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Clover",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayClover"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, 0.01f),
                            localAngles = new Vector3(45, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BarrierOnOverHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAegis"),
                            childName = "Root",
                            localPos = new Vector3(-0.0068f, 0.0204f, 0),
                            localAngles = new Vector3(-101, -90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GoldOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBoneCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "WarCryOnMultiKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayPauldron"),
                            childName = "Root",
                            localPos = new Vector3(-0.015f, 0.01f, 0),
                            localAngles = new Vector3(60, 270, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintArmor",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBuckler"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.028f, -0.008f),
                            localAngles = new Vector3(0, 180, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IceRing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayIceRing"),
                            childName = "Root",
                            localPos = new Vector3(0.0004f, 0.0267f, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FireRing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFireRing"),
                            childName = "Root",
                            localPos = new Vector3(0.00042f, 0.0313f, 0),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "UtilitySkillMagazine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAfterburnerShoulderRing"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.015f, 0),
                            localAngles = new Vector3(0, 22, -90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Root",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWaxBird"),
                            childName = "Head",
                            localPos = new Vector3(0, -0.028f, -0.0037f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ArmorReductionOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                        {
                            new ItemDisplayRule
                            {
                                ruleType = ItemDisplayRuleType.ParentedPrefab,
                                followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWarhammer"),
                                childName = "Spine3",
                                localPos = new Vector3(0, 0.03f, -0.023f),
                                localAngles = new Vector3(-90, 0, 0),
                                localScale = new Vector3(0f, 0f, 0f),
                                limbMask = LimbFlags.None
                            }
                        }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NearbyDamageBonus",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayDiamond"),
                            childName = "Root",
                            localPos = new Vector3(-0.0031f, 0.0297f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ArmorPlate",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayRepulsionArmorPlate"),
                            childName = "Root",
                            localPos = new Vector3(2.5f, 0.5f, -2),
                            localAngles = new Vector3(0, 0, 180),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CommandMissile",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMissileRack"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.0173f, -0.0261f),
                            localAngles = new Vector3(90, 180, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Feather",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFeather"),
                            childName = "Root",
                            localPos = new Vector3(-0.0047f, 0.0185f, -0.0034f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Crowbar",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayCrowbar"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.0022f, 0.0168f, -0.023f),
                            localAngles = new Vector3(45, 90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FallBoots",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGravBoots"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.02246f, -0.00456f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ExecuteLowHealthElite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGuillotine"),
                            childName = "Root",
                            localPos = new Vector3(-0.0152f, 0.0308f, 0),
                            localAngles = new Vector3(90, -90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "EquipmentMagazine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBattery"),
                            childName = "Spine3",
                            localPos = new Vector3(0.0118f, 0.0129f, -0.0246f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NovaOnHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayDevilHorns"),
                            childName = "Head",
                            localPos = new Vector3(0.0094f, 0.01f, 0),
                            localAngles = new Vector3(0, 0, 20),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Infusion",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayInfusion"),
                            childName = "Root",
                            localPos = new Vector3(-0.01452f, 0.02237f, 0.01542f),
                            localAngles = new Vector3(0, -20, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Medkit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMedkit"),
                            childName = "Root",
                            localPos = new Vector3(-0.0148f, 0.0061f, 0),
                            localAngles = new Vector3(80, 90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Bandolier",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBandolier"),
                            childName = "Spine1",
                            localPos = new Vector3(0, 0.0452f, 0.005f),
                            localAngles = new Vector3(-134.304f, -90, 100.864f),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BounceNearby",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayHook"),
                            childName = "Shield",
                            localPos = new Vector3(0, 0, 7.5f),
                            localAngles = new Vector3(0, 0, 25),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IgniteOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGasoline"),
                            childName = "Root",
                            localPos = new Vector3(0.0142f, 0.0123f, 0),
                            localAngles = new Vector3(96, 90, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "StunChanceOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayStunGrenade"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.04f, -0.02f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Firework",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFirework"),
                            childName = "Spine1",
                            localPos = new Vector3(-0.02187928f, 0.02602776f, 0.01359699f),
                            localAngles = new Vector3(-108, 102, -99),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarDagger",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayLunarDagger"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0, -0.0219f),
                            localAngles = new Vector3(-54, 90, -90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Knurl",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayKnurl"),
                            childName = "Spine3",
                            localPos = new Vector3(0.01961f, 0.0076f, 0.02533f),
                            localAngles = new Vector3(0, 116, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BeetleGland",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBeetleGland"),
                            childName = "Root",
                            localPos = new Vector3(-0.02f, 0.02f, 0),
                            localAngles = new Vector3(0, 206, 64),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintBonus",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySoda"),
                            childName = "Spine1",
                            localPos = new Vector3(-0.015f, 0.01f, 0.015f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SecondarySkillMagazine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayDoubleMag"),
                            childName = "Root",
                            localPos = new Vector3(-0.0244f, 0, -0.01927f),
                            localAngles = new Vector3(0, 2, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "StickyBomb",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayStickyBomb"),
                            childName = "Root",
                            localPos = new Vector3(-0.018f, 0.02f, -0.01f),
                            localAngles = new Vector3(0, 45, 45),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TreasureCache",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayKey"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.00693f, -0.0298f, -0.02f),
                            localAngles = new Vector3(90, 0, -14),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BossDamageBonus",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAPRound"),
                            childName = "Spine1",
                            localPos = new Vector3(0.01988f, 0.02043f, -0.00202f),
                            localAngles = new Vector3(-90, 0, -84),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SlowOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBauble"),
                            childName = "Spine3",
                            localPos = new Vector3(0.0272f, -0.0164f, -0.028f),
                            localAngles = new Vector3(0, 0, 64),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ExtraLife",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayHippo"),
                            childName = "Root",
                            localPos = new Vector3(0, 1.15f, -3.65f),
                            localAngles = new Vector3(-80, 180, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "KillEliteFrenzy",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBrainstalk"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.015f, 0.0033f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "RepeatHeal",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayCorpseFlower"),
                            childName = "Root",
                            localPos = new Vector3(0.00647f, 0.01332f, 0),
                            localAngles = new Vector3(0, -38, -90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AutoCastEquipment",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFossil"),
                            childName = "Root",
                            localPos = new Vector3(0.0063f, -0.0223f, -0.022f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "IncreaseHealing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(0.0083f, 0.015f, -0.0054f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAntler"),
                            childName = "Head",
                            localPos = new Vector3(-0.0083f, 0.015f, -0.0054f),
                            localAngles = new Vector3(0, -90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TitanGoldDuringTP",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGoldHeart"),
                            childName = "Spine3",
                            localPos = new Vector3(0.00011f, 0.00371f, 0.02213f),
                            localAngles = new Vector3(0, 0, -75),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintWisp",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBrokenMask"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.02f, 0.01f),
                            localAngles = new Vector3(328, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BarrierOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBrooch"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.01043f, 0.01134f, 0.025f),
                            localAngles = new Vector3(45, 90, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TPHealingNova",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGlowFlower"),
                            childName = "Root",
                            localPos = new Vector3(0, -0.03373f, 0.00664f),
                            localAngles = new Vector3(64, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarUtilityReplacement",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBirdFoot"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.0268f, -0.0169f),
                            localAngles = new Vector3(0, -90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Thorns",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayRazorwireLeft"),
                            childName = "Root",
                            localPos = new Vector3(-0.006f, 0, 0),
                            localAngles = new Vector3(270, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarPrimaryReplacement",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBirdEye"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.0075f, 0.01f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "NovaOnLowHealth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayJellyGuts"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.0135f, -0.0077f),
                            localAngles = new Vector3(-41, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LunarTrinket",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBeads"),
                            childName = "Root",
                            localPos = new Vector3(-0.0031f, 0.0147f, 0.0033f),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Plant",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayInterstellarDeskPlant"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(342, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Bear",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBear"),
                            childName = "Root",
                            localPos = new Vector3(0, 1.28f, 0.97f),
                            localAngles = new Vector3(-77, 180, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "DeathMark",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayDeathMark"),
                            childName = "Root",
                            localPos = new Vector3(0.0053f, 0.0128f, 0.0011f),
                            localAngles = new Vector3(0, 0, 180),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ExplodeOnDeath",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWilloWisp"),
                            childName = "Root",
                            localPos = new Vector3(0.1094F, -0.386F, 0.0087F),
                            localAngles = new Vector3(14.493F, 72.2113F, 60.9474F),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Seed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySeed"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.035f, -0.01f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SprintOutOfCombat",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWhip"),
                            childName = "Spine1",
                            localPos = new Vector3(0.02f, 0, -0.015f),
                            localAngles = new Vector3(0, 45, 15),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CooldownOnCrit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySkull"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 90, 180),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Phasing",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayStealthkit"),
                            childName = "Root",
                            localPos = new Vector3(-0.0025f, 0, -0.01f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "PersonalShield",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayShieldGenerator"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.0075f, 0.01f, 0.025f),
                            localAngles = new Vector3(45, 90, -90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ShockNearby",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTeslaCoil"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, 0),
                            localAngles = new Vector3(0, 0, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ShieldOnly",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(0.0083f, 0.0207f, -0.0054f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayShieldBug"),
                            childName = "Head",
                            localPos = new Vector3(-0.0083f, 0.0207f, -0.0054f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AlienHead",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAlienHead"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.02f, 0.02f, -0.015f),
                            localAngles = new Vector3(180, 45, 180),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "HeadHunter",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySkullCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.02f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "EnergizedOnEquipmentUse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWarHorn"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.04f, -0.01f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "RegenOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySteakCurved"),
                            childName = "Root",
                            localPos = new Vector3(2, 0, 7.8f),
                            localAngles = new Vector3(-25, 0, 180),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Tooth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayToothMeshLarge"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.03f, 0.015f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Pearl",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayPearl"),
                            childName = "Root",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "ShinyPearl",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayShinyPearl"),
                            childName = "Root",
                            localPos = new Vector3(0, 0, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BonusGoldPackOnKill",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTome"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.035f, -0.016f),
                            localAngles = new Vector3(10, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Squid",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySquidTurret"),
                            childName = "Root",
                            localPos = new Vector3(0.015f, 0, -0.025f),
                            localAngles = new Vector3(0, -90, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Icicle",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFrostRelic"),
                            childName = "Spine3",
                            localPos = new Vector3(0.05f, 0, -0.1f),
                            localAngles = new Vector3(90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Talisman",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTalisman"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.05f, 0, -0.1f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LaserTurbine",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayLaserTurbine"),
                            childName = "Spine3",
                            localPos = new Vector3(0.015f, -0.01f, -0.023f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FocusConvergence",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFocusedConvergence"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0, -0.15f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Incubator",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayAncestralIncubator"),
                            childName = "Spine3",
                            localPos = new Vector3(-0.01f, 0.01f, 0),
                            localAngles = new Vector3(-25, 25, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FireballsOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFireballsOnHit"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.02f, -0.015f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "SiphonOnLowHealth",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySiphonOnLowHealth"),
                            childName = "Spine1",
                            localPos = new Vector3(0.01f, 0.0075f, 0.02f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BleedOnHitAndExplode",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBleedOnHitAndExplode"),
                            childName = "Root",
                            localPos = new Vector3(-0.002f, 0.01f, 0.02f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "MonstersOnShrineUse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMonstersOnShrineUse"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.01f, -0.0125f),
                            localAngles = new Vector3(0, -90, 0),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            itemRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "RandomDamageZone",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayRandomDamageZone"),
                            childName = "Root",
                            localPos = new Vector3(-0.01f, 0.005f, 0.005f),
                            localAngles = new Vector3(0, 90, 90),
                            localScale = new Vector3(0f, 0f, 0f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            #endregion

            #region Creating all of the equipment displays for the base game.
            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Jetpack",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBugWings"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.0214f, -0.0208f),
                            localAngles = new Vector3(45, 0, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
        }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GoldGat",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGoldGat"),
                            childName = "Pelvis",
                            localPos = new Vector3(0.08f, 0.15f, 0.05f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BFG",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs ,"DisplayBFG"),
                            childName = "Spine3",
                            localPos = new Vector3(0.0118f, 0.0301f, -0.0082f),
                            localAngles = new Vector3(0, 0, -30f),
                            localScale = new Vector3(0.03f, 0.03f, 0.03f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixRed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
        {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(0.0072f, 0.015f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.009661668f, 0.009661668f, 0.009661668f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3(-0.0072f, 0.015f, 0),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(-0.009661668f, 0.009661668f, 0.009661668f),
                            limbMask = LimbFlags.None
                        }
        }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixBlue",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.015f, 0.0168f),
                            localAngles = new Vector3(-45, 0, 0),
                            localScale = new Vector3(0.032f, 0.032f, 0.032f),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.02f, 0.01012f),
                            localAngles = new Vector3(-69, 0, 0),
                            localScale = new Vector3(0.016f, 0.016f, 0.016f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixWhite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.0254f, -0.0012f),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.002664f, 0.002664f, 0.002664f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixPoison",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.015f, 0),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixHaunted",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.025f, 0),
                            localAngles = new Vector3(-90, 0, 0),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CritOnUse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayNeuralImplant"),
                            childName = "Head",
                            localPos = new Vector3(0, 0.005f, 0.025f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.025f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "DroneBackup",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayRadio"),
                            childName = "Spine1",
                            localPos = new Vector3(0.02f, 0.02f, 0),
                            localAngles = new Vector3(-20, 90, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Lightning",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = capacitorPrefab,
                            childName = "Root",
                            localPos = new Vector3(-0.0128f, -0.0046f, 0.0311f),
                            localAngles = new Vector3(64, 12, 120),
                            localScale = new Vector3(0.06833912f, 0.06833912f, 0.06833912f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "BurnNearby",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayPotion"),
                            childName = "Spine3",
                            localPos = new Vector3(0.02f, 0.04f, 0),
                            localAngles = new Vector3(0, 0, -45),
                            localScale = new Vector3(0.005f, 0.005f, 0.005f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "CrippleWard",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEffigy"),
                            childName = "Spine3",
                            localPos = new Vector3(0.0134f, 0.02949f, -0.00808f),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.04f, 0.04f, 0.04f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "QuestVolatileBattery",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayBatteryArray"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.0189f, -0.0274f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GainArmor",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayElephantFigure"),
                            childName = "Root",
                            localPos = new Vector3(0, 0.02f, 0.008f),
                            localAngles = new Vector3(115, 0, 0),
                            localScale = new Vector3(0.05f, 0.05f, 0.05f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Recycle",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayRecycler"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.015f, -0.03f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "FireBallDash",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayEgg"),
                            childName = "Spine3",
                            localPos = new Vector3(0.015f, 0.035f, -0.01f),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Cleanse",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayWaterPack"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0, -0.02f),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Tonic",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTonic"),
                            childName = "Spine3",
                            localPos = new Vector3(0.015f, 0.045f, 0),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Gateway",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayVase"),
                            childName = "Spine3",
                            localPos = new Vector3(0.01f, 0.04f, -0.015f),
                            localAngles = new Vector3(-45, 0, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Meteor",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayMeteor"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.05f, -0.15f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(1, 1, 1),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Saw",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplaySawmerang"),
                            childName = "Root",
                            localPos = new Vector3(-0.05f, 0.05f, 0),
                            localAngles = new Vector3(0, 90, 0),
                            localScale = new Vector3(0.2f, 0.2f, 0.2f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Blackhole",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayGravCube"),
                            childName = "Spine3",
                            localPos = new Vector3(0, 0.05f, -0.15f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(1, 1, 1),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Scanner",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayScanner"),
                            childName = "Spine1",
                            localPos = new Vector3(0.025f, 0.01f, 0),
                            localAngles = new Vector3(-90, 90, 0),
                            localScale = new Vector3(0.02f, 0.02f, 0.02f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "DeathProjectile",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayDeathProjectile"),
                            childName = "Spine1",
                            localPos = new Vector3(-0.01f, 0.005f, -0.025f),
                            localAngles = new Vector3(0, 180, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "LifestealOnHit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayLifestealOnHit"),
                            childName = "Head",
                            localPos = new Vector3(0.02f, 0.04f, -0.01f),
                            localAngles = new Vector3(45, 270, 0),
                            localScale = new Vector3(0.015f, 0.015f, 0.015f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "TeamWarCry",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayTeamWarCry"),
                            childName = "Spine1",
                            localPos = new Vector3(0, 0.015f, 0.03f),
                            localAngles = new Vector3(0, 0, 0),
                            localScale = new Vector3(0.01f, 0.01f, 0.01f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });

            equipmentRules.Add(new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Fruit",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay(itemDisplayPrefabs, "DisplayFruit"),
                            childName = "Spine3",
                            localPos = new Vector3(0.0193f, -0.0069f, 0.0025f),
                            localAngles = new Vector3(0, -70, 30),
                            localScale = new Vector3(0.02759527f, 0.02759527f, 0.02759527f),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            });
            #endregion

            #region Actually assigning all of the displays to Ursa.
            itemDisplayRuleSet.namedItemRuleGroups = itemRules.ToArray();
            itemDisplayRuleSet.namedEquipmentRuleGroups = equipmentRules.ToArray();

            characterModel.itemDisplayRuleSet = itemDisplayRuleSet;
            #endregion
        }

        private static GameObject LoadDisplay(Dictionary<string, GameObject> dict, string name)
        {
            if (dict.ContainsKey(name.ToLower()))
            {
                if (dict[name.ToLower()]) return dict[name.ToLower()];
            }
            return null;
        }
    }
}
