using Mlie;
using UnityEngine;
using Verse;

namespace MobileMineralSonar;

public class Controller : Mod
{
    public static Settings Settings;
    public static string CurrentVersion;

    public Controller(ModContentPack content) : base(content)
    {
        GetSettings<Settings>();
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public static ResearchProjectDef ResearchMobileMineralSonarEnhancedScan
    {
        get
        {
            field ??=
                ResearchProjectDef.Named("ResearchMobileMineralSonarEnhancedScan");
            return field;
        }
    }

    public override string SettingsCategory()
    {
        return "MiningCo. MMS";
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoSettingsWindowContents(inRect);
    }
}