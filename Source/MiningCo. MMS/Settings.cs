using UnityEngine;
using Verse;

namespace MobileMineralSonar;

public class Settings : ModSettings
{
    public static bool periodicLightIsEnabled;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref periodicLightIsEnabled, "periodicLightIsEnabled");
    }

    public static void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard
        {
            ColumnWidth = inRect.width / 2f
        };
        listingStandard.Begin(inRect);
        listingStandard.CheckboxLabeled("MMS.PeriodicLight".Translate(), ref periodicLightIsEnabled,
            "MMS.PeriodicLightTT".Translate());
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