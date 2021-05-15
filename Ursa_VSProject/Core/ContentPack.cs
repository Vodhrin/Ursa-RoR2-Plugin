using RoR2.ContentManagement;

namespace Ursa.Core
{
    internal class UrsaContentPack : IContentPackProvider
    {
        public ContentPack contentPack = new ContentPack();
        public string identifier => "com.Vodhr.UrsaSurvivor";

        public void Initialize()
        {
            ContentManager.collectContentPackProviders += ContentManager_collectContentPackProviders;
        }

        private void ContentManager_collectContentPackProviders(ContentManager.AddContentPackProviderDelegate addContentPackProvider)
        {
            addContentPackProvider(this);
        }

        public System.Collections.IEnumerator LoadStaticContentAsync(LoadStaticContentAsyncArgs args)
        {
            this.contentPack.identifier = this.identifier;
            contentPack.bodyPrefabs.Add(UrsaPlugin.characterBodies.ToArray());
            contentPack.buffDefs.Add(UrsaPlugin.buffDefs.ToArray());
            contentPack.effectDefs.Add(UrsaPlugin.effectDefs.ToArray());
            contentPack.entityStateTypes.Add(UrsaPlugin.entityStateTypes.ToArray());
            contentPack.masterPrefabs.Add(UrsaPlugin.characterMasters.ToArray());
            contentPack.networkSoundEventDefs.Add(UrsaPlugin.networkSoundEventDefs.ToArray());
            contentPack.projectilePrefabs.Add(UrsaPlugin.projectilePrefabs.ToArray());
            contentPack.skillDefs.Add(UrsaPlugin.skillDefs.ToArray());
            contentPack.skillFamilies.Add(UrsaPlugin.skillFamilies.ToArray());
            contentPack.survivorDefs.Add(UrsaPlugin.survivorDefs.ToArray());
            contentPack.unlockableDefs.Add(UrsaPlugin.unlockableDefs.ToArray());

            args.ReportProgress(1f);
            yield break;
        }

        public System.Collections.IEnumerator GenerateContentPackAsync(GetContentPackAsyncArgs args)
        {
            ContentPack.Copy(this.contentPack, args.output);
            args.ReportProgress(1f);
            yield break;
        }

        public System.Collections.IEnumerator FinalizeAsync(FinalizeAsyncArgs args)
        {
            args.ReportProgress(1f);
            yield break;
        }
    }
}

