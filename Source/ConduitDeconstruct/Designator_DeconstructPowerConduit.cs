using System;
using RimWorld;
using UnityEngine;
using Verse;

/* Mod: Conduit Deconstruct.
 * Game: RimWorld.
 * Author: TheWireLord.
 * Special Thanks: Mlie (No this is not a typo). 
 * Special Thanks: Ganjaman [NL] (For 1.5 update).
 */

namespace ConduitDeconstruct
{
    // This class extends the Designator_Deconstruct class and is used to deconstruct power conduits.
    public class Designator_DeconstructPowerConduit : Designator_Deconstruct
    {
        // This method checks if the ThingDef is a power conduit.
        private bool CheckDef(ThingDef conduit)
        {
            // If the conduit is null, return false.
            if (conduit == null) return false;
            // Check if the conduit is a Building.
            if (conduit.thingClass != typeof(Building)) return false;
            // Check if the conduit is a Transmitter.
            if (conduit.graphicData.linkType != LinkDrawerType.Transmitter) return false;
            // Check if the conduit is a PowerConduit.
            if ((conduit.graphicData.linkFlags & LinkFlags.PowerConduit) == LinkFlags.None) return false;
            // Check if the conduit has a PlaceWorker_Conduit.
            if (!conduit.placeWorkers.Contains(typeof(PlaceWorker_Conduit))) return false;
            // Check if the conduit has draggable dimensions of 1.
            if (conduit.placingDraggableDimensions != 1) return false;
            // Check if the conduit ever transmits power.
            if (!conduit.EverTransmitsPower) return false;
            // Check if the conduit has a CompPowerTransmitter.
            if (!conduit.HasComp(typeof(CompPowerTransmitter))) return false;
            // Check if the conduit is on the Conduits altitude layer.
            if (conduit.altitudeLayer != AltitudeLayer.Conduits) return false;
            // If all checks pass, return true.
            return true;
        }

        // This method overrides the CanDesignateThing method from the base class.
        public override AcceptanceReport CanDesignateThing(Thing conduit)
        {
            // Check if the Thing has already been designated for deconstruction.
            if (conduit.Map.designationManager.DesignationOn(conduit, DesignationDefOf.Deconstruct) != null)
            {
                // If so do nothing.
                return false;
            }

            // Check if the base method accepts the Thing and if the Thing is a power conduit.
            return (base.CanDesignateThing(conduit).Accepted && this.CheckDef(conduit.def)) || (conduit is Blueprint_Build && this.CheckDef(conduit.def.entityDefToBuild as ThingDef));
        }

        // This method is called every frame when this designator is selected.
        public override void SelectedUpdate() // Shows the power grid.
        {
            // It calls the base method and then draws the power grid overlay.
            base.SelectedUpdate();
            OverlayDrawHandler.DrawPowerGridOverlayThisFrame();
        }

        // This is the constructor for the Designator_DeconstructPowerConduit class.
        public Designator_DeconstructPowerConduit() // Shows toolbar icon.
        {
            // It sets the label, icon, and hotkey for this designator.
            this.defaultLabel = "DesignatorDeconstructPowerConduit".Translate();
            this.defaultDesc = "DesignatorDeconstructPowerConduitDesc".Translate();
            this.icon = ContentFinder<Texture2D>.Get("ToolbarIcon/ConduitDeconstructIcon", true);
            this.hotKey = null;
        }
    }
}
