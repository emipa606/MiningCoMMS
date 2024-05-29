using Verse;

namespace MobileMineralSonar;

public class ResearchMod_MobileMineralSonar : ResearchMod
{
    public override void Apply()
    {
        Building_MobileMineralSonar.Notify_EnhancedScanResearchCompleted();
    }
}