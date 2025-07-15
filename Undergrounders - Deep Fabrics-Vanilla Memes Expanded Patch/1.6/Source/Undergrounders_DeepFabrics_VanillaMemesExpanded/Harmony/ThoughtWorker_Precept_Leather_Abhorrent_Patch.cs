using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using VanillaMemesExpanded;

namespace Undergrounders_DeepFabrics_VanillaMemesExpanded
{
    [HarmonyPatch(typeof(ThoughtWorker_Precept_Leather_Abhorrent), "ShouldHaveThought")]
    public static class ThoughtWorker_Precept_Leather_Abhorrent_Patch
    {
        static bool Prefix(ref ThoughtState __result, ref Pawn p)
        {
            List<Apparel> wornApparel = p.apparel.WornApparel;
            for (int i = 0; i < wornApparel.Count; i++)
            {
                ThingDef stuff = wornApparel[i].Stuff;
                if (stuff != null && (stuff.stuffProps?.categories?.Contains(StuffCategoryDefOf.Leathery)).GetValueOrDefault())
                {
                    if (stuff.defName != "undrgdrs_Fungus_Leather")
                    {
                        __result = ThoughtState.ActiveAtStage(0);
                        return false;
                    }
                }
            }
            __result = ThoughtState.Inactive;
            return false;
        }
    }
}