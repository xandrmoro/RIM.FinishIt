using HarmonyLib;
using RimWorld;
using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace FinishIt
{
    [HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.GetPriority), new Type[] { typeof(Pawn), typeof(TargetInfo) })]
    public class WorkGiver_Dobill_GetPriority_Patch
    {
        static bool Prefix(ref float __result, WorkGiver_Scanner __instance, Pawn pawn, TargetInfo t)
        {
            if (__instance is WorkGiver_DoBill && t.Thing is Building_WorkTable table)
            {
                var prio = 100f;

                var distance = pawn.Position.DistanceToSquared(t.CenterCell);
                var hasUft = table.BillStack.Bills.OfType<Bill_ProductionWithUft>().Any(b => !b.suspended && b.BoundWorker == pawn && pawn.CanReserveAndReach(b.BoundUft, PathEndMode.Touch, Danger.Deadly) && !b.BoundUft.IsForbidden(pawn) && (b.BoundUft.workLeft <= 100 || b.ShouldDoNow()));

                if (hasUft)
                {
                    var uftFactor = 10f;
                    var distanceFactor = 0.25f + Math.Min(Math.Max(0.75f - distance / 533f, 0f), 0.75f);

                    __result = prio * distanceFactor * uftFactor;
                }
                else
                {
                    __result = prio / distance;
                }

                return false;
            }

            return true;
        }
    }
}
