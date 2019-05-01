using System.Collections.Generic;
using Harmony;
using Verse;
using RimWorld;

namespace IgniteEverything
{
    [HarmonyPatch(typeof(PawnAttackGizmoUtility))]
    [HarmonyPatch("GetAttackGizmos")]
    static class Harmony_PawnAttackGizmoUtility
    {
        [HarmonyPostfix]
        static void GetAttackGizmos_Postfix(ref IEnumerable<Gizmo> __result, Pawn pawn)
        {
            if (pawn.Drafted)
            {
                Gizmo igniteGizmo = IgniteEverything.GetIgniteGizmo();

                if (pawn.story.WorkTagIsDisabled(WorkTags.Firefighting) && !pawn.story.traits.HasTrait(TraitDefOf.Pyromaniac))
                {
                    igniteGizmo.Disable("Pawn is afraid of fire.");
                }

                __result = __result.Add(igniteGizmo);
            }
        }
    }
}
