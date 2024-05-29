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
        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = inRect.width / 2f
        };
        listing_Standard.Begin(inRect);
        listing_Standard.CheckboxLabeled("MMS.PeriodicLight".Translate(), ref periodicLightIsEnabled,
            "MMS.PeriodicLightTT".Translate());
        if (Controller.CurrentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("MMS.CurrentModVersion".Translate(Controller.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}