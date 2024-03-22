using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace FinishIt
{
    [HarmonyPatch(typeof(UnfinishedThing), "GetGizmos")]
    public class UnfinishedThing_GetGizmos_Patch
    {
        static IEnumerable<Gizmo> Postfix(IEnumerable<Gizmo> gizmos, UnfinishedThing __instance)
        {
            foreach (var gizmo in gizmos) {
                yield return gizmo;
            }

            var action = new Command_Action();
            action.defaultLabel = "Finish it!";
            action.icon = ContentFinder<Texture2D>.Get("UI/Designators/Slaughter");
            action.action = () =>
            {
                if (__instance.BoundWorkTable is Building_WorkTable worktable)
                {
                    if (worktable.GetWorkgiver().Worker is WorkGiver_DoBill workGiver)
                    {
                        Pawn creator = __instance.Creator;
                        var jobMaker = Traverse.Create(workGiver).Method("FinishUftJob", creator, __instance, __instance.BoundBill);

                        __instance.Creator.jobs.TryTakeOrderedJobPrioritizedWork(jobMaker.GetValue<Job>(creator, __instance, __instance.BoundBill), workGiver, __instance.Position);
                    }
                }
            };

            yield return action;
        }
    }
}
