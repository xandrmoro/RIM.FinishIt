using HarmonyLib;
using RimWorld;

namespace FinishIt
{
    [HarmonyPatch(typeof(WorkGiver_Scanner), nameof(WorkGiver_Scanner.Prioritized), MethodType.Getter)]
    public class WorkGiver_DoBill_Prioritize_Patch
    {
        static bool Prefix(ref bool __result, WorkGiver_Scanner __instance)
        {
            if (!(__instance is WorkGiver_DoBill))
            {
                return true;
            }

            __result = true;

            return false;
        }
    }
}
