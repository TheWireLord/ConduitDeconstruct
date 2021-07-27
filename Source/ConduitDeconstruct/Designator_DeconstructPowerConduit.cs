using System;
using RimWorld;
using UnityEngine;
using Verse;

/* Mod: Conduit Deconstruct.
 * Game: RimWorld.
 * Game Version: 1.1.
 * Author: TheWireLord.
 * Special Thanks: Mlie (No this is not a typo). */

namespace ConduitDeconstruct
{
    public class Designator_DeconstructPowerConduit : Designator_Deconstruct
    {
        private bool CheckDef(ThingDef conduit)
        {
            Log.Warning("is conduict " + (conduit == ThingDefOf.PowerConduit)+" "+conduit.defName);
            if (conduit == null) return false;
            bool flag = conduit != null;
            string defName = conduit.defName;
            string text = defName;

            if (text != null)
            {
                if (text == "MUR_SubsurfaceConduit")
                {
                    return true;
                }
                if (text == "M13PowerConduit")
                {
                    return true;
                }
                if (text == "M13WaterproofConduit")
                {
                    return true;
                }
                if (text == "PowerConduitInvisible")
                {
                    return true;
                }
            }
            if (conduit.thingClass != typeof(Building)) return false;
            if (conduit.graphicData.linkType != LinkDrawerType.Transmitter) return false;
            if ((conduit.graphicData.linkFlags & LinkFlags.PowerConduit) == LinkFlags.None) return false;
            if (!conduit.placeWorkers.Contains(typeof(PlaceWorker_Conduit))) return false;
            if (conduit.placingDraggableDimensions != 1) return false;
            if (!conduit.EverTransmitsPower) return false;
            if (!conduit.HasComp(typeof(CompPowerTransmitter))) return false;
            if (conduit.altitudeLayer != AltitudeLayer.Conduits) return false;
            return true;
        }

        public override AcceptanceReport CanDesignateThing(Thing conduit)
        {
            return (base.CanDesignateThing(conduit).Accepted && this.CheckDef(conduit.def)) || (conduit is Blueprint_Build && this.CheckDef(conduit.def.entityDefToBuild as ThingDef));
        }

        public override void SelectedUpdate() // Shows the power grid.
        {
            base.SelectedUpdate();
            OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
        }

        public Designator_DeconstructPowerConduit() // Shows toolbar icon.
        {
            this.defaultLabel = "Deconstruct Conduits";
            this.icon = ContentFinder<Texture2D>.Get("ToolbarIcon/ConduitDeconstructIcon", true);
            this.hotKey = null;
        }
    }
}
