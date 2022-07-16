using System.Runtime.CompilerServices;
using BeatSaberMarkupLanguage.Attributes;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]

namespace ImagineBeingGood.Configuration
{
    internal class PluginConfig
    {
        public virtual bool Enabled { get; set; } = true;
        public virtual float TargetStarDifficulty { get; set; } = 10f;
        public virtual float TargetAccuracy { get; set; } = 0.95f;
        public virtual float Chance { get; set; } = 0.5f;

        [UIAction("percentage")]
        public string Percentage(float value)
        {
            return value.ToString("P0");
        }

        [UIAction("star")]
        public string Star(float value)
        {
            return value.ToString("F1");
        }
    }
}