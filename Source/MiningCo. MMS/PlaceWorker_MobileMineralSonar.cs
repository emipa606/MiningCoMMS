using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace MobileMineralSonar;

public class PlaceWorker_MobileMineralSonar : PlaceWorker
{
    public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
    {
        base.DrawGhost(def, center, rot, ghostCol, thing);
        IEnumerable<Building> enumerable =
            Find.CurrentMap.listerBuildings.AllBuildingsColonistOfDef(ThingDef.Named("MobileMineralSonar"));
        if (enumerable != null)
        {
            foreach (var building in enumerable)
            {
                (building as Building_MobileMineralSonar)?.DrawMaxScanRange();
            }
        }

        var isFinished = ResearchProjectDef.Named("ResearchMobileMineralSonarEnhancedScan").IsFinished;
        if (isFinished)
        {
            var material = MaterialPool.MatFrom("Effects/ScanRange50");
            var s = new Vector3(Settings.MaxEnhancedRange * 2, 1f, Settings.MaxEnhancedRange * 2);
            var matrix = default(Matrix4x4);
            matrix.SetTRS(
                center.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint) + new Vector3(0f, 15f, 0f) +
                Altitudes.AltIncVect, 0f.ToQuat(), s);
            Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
        }
        else
        {
            var material2 = MaterialPool.MatFrom("Effects/ScanRange30");
            var s2 = new Vector3(Settings.MaxRange * 2, 1f, Settings.MaxRange * 2);
            var matrix2 = default(Matrix4x4);
            matrix2.SetTRS(
                center.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint) + new Vector3(0f, 15f, 0f) +
                Altitudes.AltIncVect, 0f.ToQuat(), s2);
            Graphics.DrawMesh(MeshPool.plane10, matrix2, material2, 0);
        }
    }
}