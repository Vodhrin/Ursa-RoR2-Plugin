using System.IO;
using System.Reflection;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using R2API;

namespace Ursa.Core
{
    static class Assets
    {

        public static AssetBundle mainAssetBundle = null;

        public static Sprite icon1;
        public static Sprite icon2;
        public static Sprite icon3;
        public static Sprite icon4;
        public static Sprite icon5;
        public static Sprite portrait;
        public static Sprite defaultSkinIcon;
        public static Sprite furySwipesBuffIcon;

        public const string SwingSound = "Ursa_swing";
        public const string HitSound = "Ursa_hit";
        public const string OverpowerSound = "Ursa_overpower";
        public const string EarthshockSound = "Ursa_earthshock";
        public const string EnrageSound = "Ursa_enrage";

        public static GameObject earthshockEffect;

        public static void Initialize()
        {

            if (mainAssetBundle == null)
            {
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ursa.ursasurvivorbundle"))
                {
                    mainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }

            icon1 = mainAssetBundle.LoadAsset<Sprite>("Skill1Icon");
            icon2 = mainAssetBundle.LoadAsset<Sprite>("Skill2Icon");
            icon3 = mainAssetBundle.LoadAsset<Sprite>("Skill3Icon");
            icon4 = mainAssetBundle.LoadAsset<Sprite>("Skill4Icon");
            icon5 = mainAssetBundle.LoadAsset<Sprite>("Skill5Icon");
            portrait = mainAssetBundle.LoadAsset<Sprite>("Portrait");
            defaultSkinIcon = mainAssetBundle.LoadAsset<Sprite>("DefaultSkinIcon");
            furySwipesBuffIcon = mainAssetBundle.LoadAsset<Sprite>("FurySwipesBuffIcon.png");

            earthshockEffect = LoadCustomEffect("earthshockEffect", "");

            using (Stream manifestResourceStream2 = Assembly.GetExecutingAssembly().GetManifestResourceStream("Ursa.UrsaSoundbank.bnk"))
            {
                byte[] array = new byte[manifestResourceStream2.Length];
                manifestResourceStream2.Read(array, 0, array.Length);
                SoundAPI.SoundBanks.Add(array);
            }
        }

        //Yoinked from rob's PaladinMod :)
        private static GameObject LoadCustomEffect(string resourceName, string soundName)
        {
            GameObject newEffect = mainAssetBundle.LoadAsset<GameObject>(resourceName);

            newEffect.AddComponent<DestroyOnTimer>().duration = 12;
            newEffect.AddComponent<NetworkIdentity>();
            newEffect.AddComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            var effect = newEffect.AddComponent<EffectComponent>();
            effect.applyScale = false;
            effect.effectIndex = EffectIndex.Invalid;
            effect.parentToReferencedTransform = true;
            effect.positionAtReferencedTransform = true;
            effect.soundName = soundName;

            EffectAPI.AddEffect(newEffect);

            return newEffect;
        }
    }
}
