using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace Undergrounders_MortuaryCryptsAndCatacombs
{
    //Doesn't need harmony, called directly from xml
    public class Placeworker_AttachedToWall_Exclusive : PlaceWorker
    {
        //overrides the acceptance report function
        //to see if it can be placed there
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            // Get the list of things at the target location
            List<Thing> thingList = loc.GetThingList(map);

            // Iterate through the list of things
            for (int i = 0; i < thingList.Count; i++)
            {
                Thing thing2 = thingList[i];
                ThingDef thingDef = GenConstruct.BuiltDefOf(thing2.def) as ThingDef;

                // Check if the thing is a building and its fill category
                if (thingDef?.building != null)
                {
                    if (thingDef.Fillage == FillCategory.Full)
                    {
                        return false; // Cannot place if the space is fully occupied
                    }

                    // Check if the thing is an attachment and its rotation matches
                    if (thingDef.building.isAttachment && thing2.Rotation == rot)
                    {
                        return "SomethingPlacedOnThisWall".Translate();
                    }
                }
            }

            // Determine the support location based on rotation
            IntVec3 supportLoc = loc + GenAdj.CardinalDirections[rot.AsInt];

            // Check if the support location is within map bounds
            if (!supportLoc.InBounds(map))
            {
                return false; // Cannot place if the support location is out of bounds
            }

            // Get the list of things at the support location
            thingList = supportLoc.GetThingList(map);
            bool notAttachableflag = false;

            // Iterate through the list of things at the support location
            for (int j = 0; j < thingList.Count; j++)
            {
                if (GenConstruct.BuiltDefOf(thingList[j].def) is ThingDef thingDef2 && thingDef2.building != null)
                {
                    // Check if the support can hold wall attachments
                    if (!thingDef2.building.supportsWallAttachments)
                    {
                        notAttachableflag = true;
                    }
                    else if (thingDef2.Fillage == FillCategory.Full)
                    {
                        bool supportIsOccupiedFlag = false;
                        // Check adjacent locations around the support location
                        for (int k = 0; k < 4; k++)
                        {
                            IntVec3 adjLoc = supportLoc + GenAdj.CardinalDirections[k];

                            // Skip if the adjacent location is the target location or out of bounds
                            if (adjLoc == loc || !adjLoc.InBounds(map))
                            {
                                continue;
                            }

                            thingList = adjLoc.GetThingList(map);

                            // Iterate through the list of things at the adjacent location
                            for (int l = 0; l < thingList.Count; l++)
                            {
                                if (GenConstruct.BuiltDefOf(thingList[l].def) is ThingDef thingDef3 && thingDef3.building != null)
                                {
                                    // Check if the adjacent location is occupied by an attachment
                                    if (thingDef3.building.canPlaceOverWall && thingDef3.building.isAttachment)
                                    {
                                        if(thingList[l].Rotation.AsInt != k)
                                        {
                                            supportIsOccupiedFlag = true;
                                        }
                                    }
                                }
                            }
                        }

                        // Return message if the support is occupied
                        if (supportIsOccupiedFlag)
                        {
                            return "MessageWallOccupied".Translate();
                        }
                        return true; // Can place if all conditions are met
                    }
                }
            }

            // Return message if the support cannot hold wall attachments
            if (notAttachableflag)
            {
                return "CannotSupportAttachment".Translate();
            }

            // Default message if the placement fails
            return "MustPlaceOnWall".Translate();
        }
    }

}