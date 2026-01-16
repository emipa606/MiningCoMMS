using System.Collections.Generic;
using RimWorld;
using Verse;

namespace MobileMineralSonar;

public class ThingDef_MobileMineralSonar : ThingDef
{
    private bool loaded;
    private List<ThingDef> scannedThingDefs;

    public List<ThingDef> ScannedThingDefs
    {
        get
        {
            if (loaded)
            {
                return scannedThingDefs;
            }

            foreach (var thingDef in DefDatabase<ThingDef>.AllDefs)
            {
                if (thingDef.building != null &&
                    (thingDef.building.isResourceRock || thingDef == ThingDefOf.AncientCryptosleepCasket) &&
                    !scannedThingDefs.Contains(thingDef))
                {
                    scannedThingDefs.Add(thingDef);
                }
            }

            loaded = true;

            return scannedThingDefs;
        }
    }
}