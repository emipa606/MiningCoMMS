using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace MobileMineralSonar;

[StaticConstructorOnStartup]
internal class Building_MobileMineralSonar : Building
{
    private const int baseMaxScanRange = 30;

    private const int enhancedMaxScanRange = 50;

    private const float baseDetectionChance = 0.2f;

    private const float enhancedDetectionChance = 0.4f;

    private const int scanProgressThresholdPerCell = 1000;

    private const int flashPeriodInSeconds = 5;

    private static int maxScanRange = baseMaxScanRange;

    private static float detectionChance = baseDetectionChance;

    private static readonly Material scanRange10 = MaterialPool.MatFrom("Effects/ScanRange10");

    private static readonly Material scanRange20 = MaterialPool.MatFrom("Effects/ScanRange20");

    private static readonly Material scanRange30 = MaterialPool.MatFrom("Effects/ScanRange30");

    private static readonly Material scanRange40 = MaterialPool.MatFrom("Effects/ScanRange40");

    private static readonly Material scanRange50 = MaterialPool.MatFrom("Effects/ScanRange50");

    private static readonly Material satelliteDish = MaterialPool.MatFrom("Things/Building/SatelliteDish");

    private static readonly Material scanRayDynamic =
        MaterialPool.MatFrom("Effects/ScanRay50x50", ShaderDatabase.MetaOverlay);

    private static readonly Material scanSpot = MaterialPool.MatFrom("Effects/ScanSpot", ShaderDatabase.Transparent);

    private readonly Vector3 satelliteDishScale = new(2f, 1f, 2f);

    private readonly Vector3 scanRangeScale30 = new(60f, 1f, 60f);

    private readonly Vector3 scanRangeScale50 = new(100f, 1f, 100f);

    private readonly Vector3 scanSpotScale = new(1f, 1f, 1f);

    private List<ThingDef> detectedDefList;

    private int nextFlashTick;

    private CompPowerTrader powerComp;

    private Matrix4x4 satelliteDishMatrix;

    private float satelliteDishRotation;

    private int scanProgress;

    private int scanRange = 1;

    private Material scanRangeDynamic;

    private Matrix4x4 scanRangeDynamicMatrix;

    private Vector3 scanRangeDynamicScale = new(1f, 1f, 1f);

    public Matrix4x4 scanRangeMatrix10 = default;

    public Matrix4x4 scanRangeMatrix20 = default;

    private Matrix4x4 scanRangeMatrix30;

    public Matrix4x4 scanRangeMatrix40 = default;

    private Matrix4x4 scanRangeMatrix50;

    public Vector3 scanRangeScale10 = new(20f, 1f, 20f);

    public Vector3 scanRangeScale20 = new(40f, 1f, 40f);

    public Vector3 scanRangeScale40 = new(80f, 1f, 80f);

    private Matrix4x4 scanRayDynamicMatrix;

    private Vector3 scanRayDynamicScale = new(1f, 1f, 1f);

    private Matrix4x4 scanSpotMatrix;

    public static void Notify_EnhancedScanResearchCompleted()
    {
        maxScanRange = enhancedMaxScanRange;
        detectionChance = enhancedDetectionChance;
    }

    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, respawningAfterLoad);
        detectedDefList = [];
        foreach (var item in ((ThingDef_MobileMineralSonar)def).scannedThingDefs)
        {
            detectedDefList.Add(item);
        }

        powerComp = GetComp<CompPowerTrader>();
        powerComp.powerOutputInt = 0f;
        scanRangeMatrix30.SetTRS(Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FogOfWar) + Altitudes.AltIncVect,
            0f.ToQuat(), scanRangeScale30);
        scanRangeMatrix50.SetTRS(Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FogOfWar) + Altitudes.AltIncVect,
            0f.ToQuat(), scanRangeScale50);
        satelliteDishMatrix.SetTRS(Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Building) + Altitudes.AltIncVect,
            satelliteDishRotation.ToQuat(), satelliteDishScale);
    }

    public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
    {
        base.DeSpawn(mode);
        scanRange = 1;
        scanProgress = 0;
        satelliteDishRotation = 0f;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref scanRange, "scanRange", 1);
        Scribe_Values.Look(ref scanProgress, "scanProgress", 1);
        Scribe_Values.Look(ref satelliteDishRotation, "satelliteDishRotation");
        Scribe_Values.Look(ref nextFlashTick, "nextFlashTick");
    }

    protected override void Tick()
    {
        base.Tick();
        performScanUpdate();
        var periodicLightIsEnabled = Settings.periodicLightIsEnabled;
        if (!periodicLightIsEnabled)
        {
            return;
        }

        if (Find.TickManager.TicksGame < nextFlashTick)
        {
            return;
        }

        nextFlashTick = Find.TickManager.TicksGame + (300 * (int)Find.TickManager.CurTimeSpeed);
        throwFlash();
    }

    private void performScanUpdate()
    {
        var powerOn = powerComp.PowerOn;
        if (powerOn)
        {
            satelliteDishRotation = (satelliteDishRotation + 1f) % 360f;
        }
        else
        {
            satelliteDishRotation = (satelliteDishRotation + 0.2f) % 360f;
        }

        if (scanRange == maxScanRange)
        {
            powerComp.powerOutputInt = 0f;
            return;
        }

        if (Find.TickManager.TicksGame % 60 != 0)
        {
            return;
        }

        if (powerOn)
        {
            scanProgress += 300;
        }
        else
        {
            scanProgress += 60;
        }

        if (scanProgress < scanRange * scanProgressThresholdPerCell)
        {
            return;
        }

        foreach (var thingDefParameter in detectedDefList)
        {
            unfogSomeRandomThingAtScanRange(thingDefParameter);
        }

        scanRange++;
        scanProgress = 0;
    }

    private void unfogSomeRandomThingAtScanRange(ThingDef thingDefParameter)
    {
        IEnumerable<Thing> enumerable = Map.listerThings.ThingsOfDef(thingDefParameter);
        if (enumerable == null)
        {
            return;
        }

        var enumerable2 = from thing in enumerable
            where thing.Position.InHorDistOf(Position, scanRange) &&
                  !thing.Position.InHorDistOf(Position, scanRange - 1)
            select thing;
        foreach (var thing2 in enumerable2)
        {
            var num = detectionChance + (detectionChance * (1f - (scanRange / 50f)));
            if (Rand.Range(0f, 1f) <= num)
            {
                Map.fogGrid.Unfog(thing2.Position);
            }
        }
    }

    private void throwFlash()
    {
        if (!(!Position.ShouldSpawnMotesAt(Map) || Map.moteCounter.SaturatedLowPriority))
        {
            FleckMaker.Static(Position.ToVector3Shifted(), Map, FleckDefOf.ExplosionFlash, flashPeriodInSeconds);
        }
    }

    protected override void DrawAt(Vector3 drawLoc, bool flip = false)
    {
        base.DrawAt(drawLoc, flip);
        drawSatelliteDish();
        if (!Find.Selector.IsSelected(this))
        {
            return;
        }

        DrawMaxScanRange();
        drawDynamicScanRangeAndScanRay();
        foreach (var thingDefParameter in detectedDefList)
        {
            DrawScanSpotOnThingsWithinScanRange(thingDefParameter);
        }
    }

    private void drawSatelliteDish()
    {
        satelliteDishMatrix.SetTRS(base.DrawPos + Altitudes.AltIncVect, satelliteDishRotation.ToQuat(),
            satelliteDishScale);
        Graphics.DrawMesh(MeshPool.plane10, satelliteDishMatrix, satelliteDish, 0);
    }

    public void DrawMaxScanRange()
    {
        if (maxScanRange == baseMaxScanRange)
        {
            Graphics.DrawMesh(MeshPool.plane10, scanRangeMatrix30, scanRange30, 0);
        }
        else
        {
            Graphics.DrawMesh(MeshPool.plane10, scanRangeMatrix50, scanRange50, 0);
        }
    }

    private void drawDynamicScanRangeAndScanRay()
    {
        switch (scanRange)
        {
            case <= 10:
                scanRangeDynamic = scanRange10;
                break;
            case <= 20:
                scanRangeDynamic = scanRange20;
                break;
            case <= 30:
                scanRangeDynamic = scanRange30;
                break;
            case <= 40:
                scanRangeDynamic = scanRange40;
                break;
            default:
                scanRangeDynamic = scanRange50;
                break;
        }

        scanRangeDynamicScale = new Vector3(2f * scanRange, 1f, 2f * scanRange);
        scanRangeDynamicMatrix.SetTRS(
            Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FogOfWar) + Altitudes.AltIncVect, 0f.ToQuat(),
            scanRangeDynamicScale);
        Graphics.DrawMesh(MeshPool.plane10, scanRangeDynamicMatrix, scanRangeDynamic, 0);
        scanRayDynamicScale = new Vector3(2f * scanRange, 1f, 2f * scanRange);
        scanRayDynamicMatrix.SetTRS(
            Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FogOfWar) + Altitudes.AltIncVect,
            satelliteDishRotation.ToQuat(), scanRayDynamicScale);
        Graphics.DrawMesh(MeshPool.plane10, scanRayDynamicMatrix, scanRayDynamic, 0);
    }

    private void DrawScanSpotOnThingsWithinScanRange(ThingDef thingDefParameter)
    {
        IEnumerable<Thing> enumerable = Map.listerThings.ThingsOfDef(thingDefParameter);
        if (enumerable == null)
        {
            return;
        }

        enumerable = from thing in enumerable
            where thing.Position.InHorDistOf(Position, scanRange)
            select thing;
        foreach (var thing2 in enumerable)
        {
            if (Map.fogGrid.IsFogged(thing2.Position))
            {
                continue;
            }

            var v = thing2.Position.ToVector3Shifted() - Position.ToVector3Shifted();
            var num = v.AngleFlat();
            var alpha = 1f - ((satelliteDishRotation - num + 360f) % 360f / 360f);
            scanSpotMatrix.SetTRS(
                thing2.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.FogOfWar) + Altitudes.AltIncVect,
                0f.ToQuat(), scanSpotScale);
            Graphics.DrawMesh(MeshPool.plane10, scanSpotMatrix,
                FadedMaterialPool.FadedVersionOf(scanSpot, alpha), 0);
        }
    }

    public override string GetInspectString()
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.Append(base.GetInspectString());
        stringBuilder.AppendLine();
        stringBuilder.Append("MMS.ScanRange".Translate(scanRange, maxScanRange));
        if (scanRange >= maxScanRange)
        {
            return stringBuilder.ToString();
        }

        var num = scanProgress / (float)(scanRange * scanProgressThresholdPerCell) * 100f;
        stringBuilder.AppendLine();
        stringBuilder.Append("MMS.ScanProgress".Translate(num));

        return stringBuilder.ToString();
    }
}