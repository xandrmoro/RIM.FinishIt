using HarmonyLib;
using RimWorld;
using System.Linq;
using Verse;
using Verse.AI;

namespace FinishIt
{
    [HarmonyPatch(typeof(WorkGiver_DoBill), nameof(WorkGiver_DoBill.StartOrResumeBillJob))]
    public class WorkGiver_DoBill_StartOrResumeBillJob_Patch
    {
        static bool Prefix(ref Job __result, Pawn pawn, IBillGiver giver)
        {
            if (giver is Building_WorkTable table)
            {
                var ufts = table.BillStack.Bills.OfType<Bill_ProductionWithUft>().Where(b => !b.suspended && b.BoundWorker == pawn && pawn.CanReserveAndReach(b.BoundUft, PathEndMode.Touch, Danger.Deadly) && !b.BoundUft.IsForbidden(pawn) && (b.BoundUft.workLeft <= 100 || b.ShouldDoNow()));

                if (!ufts.Any())
                {
                    return true;
                }

                var bill = ufts.OrderBy(u => u.BoundUft.workLeft).First();

                __result = WorkGiver_DoBill.FinishUftJob(pawn, bill.BoundUft, bill);

                return false;
            }

            return true;
        }
    }
}
