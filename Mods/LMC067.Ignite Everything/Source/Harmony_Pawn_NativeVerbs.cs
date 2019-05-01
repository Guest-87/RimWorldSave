using RimWorld;
using Verse;
using Harmony;

namespace IgniteEverything
{
    [HarmonyPatch(typeof(Pawn_NativeVerbs))]
    [HarmonyPatch("TryBeatFire")]
    class Harmony_Pawn_NativeVerbs
    {
        [HarmonyPrefix]
        static bool TryBeatFire_Prefix(Pawn_NativeVerbs __instance, Fire targetFire, Pawn ___pawn)
        {
            if (IgniteEverything.Igniters.Contains(___pawn))
            {
                if (___pawn.Drafted)
                {
                    // Add some self-preservation will to pawns so they don't burn themselves while standing next to a fire
                    if (___pawn.IsBurning() || ___pawn.AmbientTemperature > 90.0f)
                    {
                        ___pawn.drafter.Drafted = false;
                        IgniteEverything.Igniters.Remove(___pawn);
                        return true;
                    }

                    return false;
                }

                IgniteEverything.Igniters.Remove(___pawn);
            }
            
            return true;
        }
    }
}
