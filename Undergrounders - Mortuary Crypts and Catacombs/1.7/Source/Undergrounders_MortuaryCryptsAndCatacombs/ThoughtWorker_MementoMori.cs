using RimWorld;
using Verse;

namespace Undergrounders_MortuaryCryptsAndCatacombs
{
    class ThoughtWorker_MementoMori : ThoughtWorker
    {
        protected override ThoughtState CurrentStateInternal(Pawn pawn)
        {
            IntVec3 positionHeld = pawn.PositionHeld;
            int num2 = GenRadial.NumCellsInRadius(10f);
            for (int i = 0; i < num2; i++)
            {
                IntVec3 c = positionHeld + GenRadial.RadialPattern[i];
                if (!c.InBounds(pawn.Map) || c.Fogged(pawn.Map))
                {
                    continue;
                }
                Thing firstThing = c.GetFirstThing(pawn.Map, ThingDefOf.undrgdrs_grimVisage);
                if (firstThing == null || !GenSight.LineOfSight(pawn.PositionHeld, firstThing.PositionHeld, firstThing.Map, skipFirstCell: true))
                {
                    firstThing = c.GetFirstThing(pawn.Map, ThingDefOf.undrgdrs_deathMasque);
                    if (firstThing == null || !GenSight.LineOfSight(pawn.PositionHeld, firstThing.PositionHeld, firstThing.Map, skipFirstCell: true))
                    {
                        continue;
                    }
                }

                bool flag = false;
                for (int j = 0; j < pawn.surroundings.skullspikeSightings.Count; j++)
                {
                    if (pawn.surroundings.skullspikeSightings[j].skullspike == firstThing)
                    {
                        SkullspikeSighting value = pawn.surroundings.skullspikeSightings[j];
                        value.tickSighted = Find.TickManager.TicksGame;
                        pawn.surroundings.skullspikeSightings[j] = value;
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    pawn.surroundings.skullspikeSightings.Add(new SkullspikeSighting
                    {
                        skullspike = firstThing,
                        tickSighted = Find.TickManager.TicksGame
                    });
                }
            }
            return ThoughtState.Inactive;
        }
    }
}