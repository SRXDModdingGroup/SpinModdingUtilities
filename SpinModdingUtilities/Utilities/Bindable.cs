using System;

namespace SMU.Utilities; 

/// <summary>
/// Class that stores a value and invokes an event when the value is changed
/// </summary>
/// <typeparam name="T">The type of the stored value</typeparam>
public sealed class Bindable<T> : IReadOnlyBindable<T> {
    /// <inheritdoc/>
    public T Value {
        get => value;
        set {
            this.value = value;
            onChanged?.Invoke(value);
        }
    }
        
    /// <summary>
    /// Constructor. Uses the default value for T
    /// </summary>
    public Bindable() => value = default;

    /// <summary>
    /// Constructor. Assigns an initial value
    /// </summary>
    /// <param name="value">The initial value</param>
    public Bindable(T value) => this.value = value;

    private T value;
    private Action<T> onChanged;

    /// <inheritdoc/>
    public void Bind(Action<T> action) => onChanged += action;
        
    /// <inheritdoc/>
    public void BindAndInvoke(Action<T> action) {
        onChanged += action;
        action?.Invoke(value);
    }

    /// <inheritdoc/>
    public void Unbind(Action<T> action) => onChanged -= action;

    /// <summary>
    /// Clears all actions subscribed to the bindable's onChanged event
    /// </summary>
    public void ClearBindings() => onChanged = null;
}