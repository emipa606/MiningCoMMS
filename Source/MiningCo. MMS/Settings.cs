using UnityEngine;
using Verse;

namespace MobileMineralSonar;

public class Settings : ModSettings
{
    public static int MaxEnhancedRange = 50;
    public static int MaxRange = 30;
    public static int ScanInterval = 60;
    public static bool PeriodicLightIsEnabled;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref PeriodicLightIsEnabled, "periodicLightIsEnabled");
        Scribe_Values.Look(ref ScanInterval, "scanInterval", 60);
        Scribe_Values.Look(ref MaxRange, "maxRange", 30);
        Scribe_Values.Look(ref MaxEnhancedRange, "maxEnhancedRange", 50);
    }

    public static void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = inRect.width / 1.3f
        };
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("MMS.PeriodicLight".Translate(), ref PeriodicLightIsEnabled,
            "MMS.PeriodicLightTT".Translate());
        listingStandard.GapLine();
        listingStandard.Label("MMS.ReloadNeeded".Translate());
        listingStandard.Label("MMS.ScanInterval".Translate(ScanInterval));
        listingStandard.IntAdjuster(ref ScanInterval, 5, 10);
        listingStandard.Label("MMS.MaxRange".Translate(MaxRange));
        listingStandard.IntAdjuster(ref MaxRange, 5, 10);
        if (MaxRange > MaxEnhancedRange)
        {
            MaxEnhancedRange = MaxRange;
        }

        listingStandard.Label("MMS.MaxEnhancedRange".Translate(MaxEnhancedRange));
        listingStandard.IntAdjuster(ref MaxEnhancedRange, 5, 15);
        if (MaxEnhancedRange < MaxRange)
        {
            MaxRange = MaxEnhancedRange;
        }

        if (Controller.CurrentVersion != null)
        {
            listingStandard.Gap();
            GUI.contentColor = Color.gray;
            listingStandard.Label("MMS.CurrentModVersion".Translate(Controller.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listingStandard.End();
    }
}