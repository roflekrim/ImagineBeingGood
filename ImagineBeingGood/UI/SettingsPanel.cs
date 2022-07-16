using System;
using System.ComponentModel;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.Settings;
using ImagineBeingGood.Configuration;
using SiraUtil.Attributes;
using SiraUtil.Zenject;
using Zenject;

namespace ImagineBeingGood.UI
{
    [Bind(Location.Menu)]
    internal class SettingsPanel : IInitializable, IDisposable
    {
        private readonly PluginConfig _config;

        public SettingsPanel(PluginConfig config)
        {
            _config = config;
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab("Imagine Being Good", "ImagineBeingGood.UI.BSML.settings.bsml", this, MenuType.Solo);
        }

        public void Dispose()
        {
            if (GameplaySetup.instance != null) 
                GameplaySetup.instance.RemoveTab("Imagine Being Good");
        }
    }
}