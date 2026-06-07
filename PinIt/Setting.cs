using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using PinIt.Systems;
using System.Collections.Generic;

namespace PinIt
{
    [FileLocation(nameof(PinIt))]
    [SettingsUIGroupOrder(kTestGroup)]
    [SettingsUIShowGroupName(kTestGroup)]
    public class Setting : ModSetting
    {
        public const string kTestGroup = "Testing";

        public Setting(IMod mod) : base(mod) { }

        public override void SetDefaults() { }

        [SettingsUISection(kTestGroup)]
        [SettingsUIButton]
        public bool TestButton
        {
            set
            {
                Mod.log.Info("[PinIt] TEST BUTTON PRESSED");
                var data = FavouritesService.Load();
                Mod.log.Info($"[PinIt] JSON store test — {data.Collections.Count} collections loaded");
            }
        }
    }

    public class LocaleEN : IDictionarySource
    {
        private readonly Setting m_Setting;

        public LocaleEN(Setting setting) { m_Setting = setting; }

        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { m_Setting.GetSettingsLocaleID(), "PinIt" },
                { m_Setting.GetOptionGroupLocaleID(Setting.kTestGroup), "Testing" },
                { m_Setting.GetOptionLabelLocaleID(nameof(Setting.TestButton)), "Test Button" },
                { m_Setting.GetOptionDescLocaleID(nameof(Setting.TestButton)), "Writes a test line to the PinIt log file" },
            };
        }

        public void Unload() { }
    }
}
