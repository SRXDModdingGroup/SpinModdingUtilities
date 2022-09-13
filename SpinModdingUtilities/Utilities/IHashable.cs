namespace SMU.Utilities; 

/// <summary>
/// Interface for classes that can be hashed by HashUtility
/// </summary>
public interface IHashable {
    /// <summary>
    /// Generates a consistent hash value for use by HashUtility
    /// </summary>
    /// <remarks>Use HashUtility.Combine to combine the hash values for all immutable fields and properties that you want to contribute to the hash</remarks>
    /// <returns></returns>
    public int GetStableHash();
}