using System.Reflection.Emit;
using HarmonyLib;

namespace SMU.Extensions; 

/// <summary>
/// Extension class for CodeInstructions
/// </summary>
public static class CodeInstructionExtensions {
    /// <summary>
    /// Checks if an instruction loads a local variable at a given index
    /// </summary>
    /// <param name="instruction">The instruction to check</param>
    /// <param name="index">The local index to check for</param>
    /// <returns>True if the instruction loads a local at that index</returns>
    public static bool LoadsLocalAtIndex(this CodeInstruction instruction, int index)
        => (instruction.opcode == OpCodes.Ldloc || instruction.opcode == OpCodes.Ldloc_S) && OperandIsAtIndex(instruction.operand, index);
        
    /// <summary>
    /// Checks if an instruction loads the address of a local variable at a given index
    /// </summary>
    /// <param name="instruction">The instruction to check</param>
    /// <param name="index">The local index to check for</param>
    /// <returns>True if the instruction loads the address of a local at that index</returns>
    public static bool LoadsLocalAddressAtIndex(this CodeInstruction instruction, int index)
        => (instruction.opcode == OpCodes.Ldloca || instruction.opcode == OpCodes.Ldloca_S) && OperandIsAtIndex(instruction.operand, index);
        
    /// <summary>
    /// Checks if an instruction stores a local variable at a given index
    /// </summary>
    /// <param name="instruction">The instruction to check</param>
    /// <param name="index">The local index to check for</param>
    /// <returns>True if the instruction stores a local at that index</returns>
    public static bool StoresLocalAtIndex(this CodeInstruction instruction, int index)
        => (instruction.opcode == OpCodes.Stloc || instruction.opcode == OpCodes.Stloc_S) && OperandIsAtIndex(instruction.operand, index);

    private static bool OperandIsAtIndex(object operand, int index) {
        if (operand is LocalBuilder builder)
            return builder.LocalIndex == index;

        return false;
    }
}