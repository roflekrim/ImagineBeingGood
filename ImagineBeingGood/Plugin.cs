using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using ImagineBeingGood.Configuration;
using ImagineBeingGood.Installers;
using IPALogger = IPA.Logging.Logger;

namespace ImagineBeingGood
{
    [Plugin(RuntimeOptions.SingleStartInit), NoEnableDisable]
    public class Plugin
    {
        [Init]
        public void Init(Zenjector zenjector, IPALogger logger, Config config)
        {
            zenjector.UseLogger(logger);
            zenjector.UseMetadataBinder<Plugin>();
            zenjector.UseAutoBinder();

            zenjector.Install<AppInstaller>(Location.App, config.Generated<PluginConfig>());
        }
    }
}