using System.Diagnostics.CodeAnalysis;

namespace Ow.Net;

public sealed class Empty {
    private Empty() { }
}

public static class Optional {
    public static Optional<T> From<T>(T? t) where T : notnull {
        return t;
    }

    public static readonly Empty Empty = null!;

    public static Optional<T> ToOptional<T>(this T? t) where T : notnull => t;
}

public readonly struct Optional<T> where T : notnull {
    public static readonly Optional<T> Empty = default;

    private readonly T _value;

    public bool HasValue { get; }

    public bool IsEmpty => !HasValue;

    public T Value => HasValue ? _value : throw new InvalidOperationException("Optional is empty");

    private Optional(T t) {
        _value = t;
        HasValue = true;
    }

    public static implicit operator Optional<T>(T? t)
        => t is null ? Empty : new Optional<T>(t);

    public static explicit operator T(Optional<T> opt) => opt.Value;

    public static implicit operator Optional<T>(Empty _) => Empty;

    public Optional<TOut> Map<TOut>(Func<T, TOut> mapper) where TOut : notnull
        => HasValue ? mapper(_value) : Optional.Empty;

    public Optional<TOut> Bind<TOut>(Func<T, Optional<TOut>> binder) where TOut : notnull
        => HasValue ? binder(_value) : Optional<TOut>.Empty;

    public TOut Match<TOut>(Func<T, TOut> onValue, Func<TOut> onEmpty)
        => HasValue ? onValue(_value) : onEmpty();

    public void Match(Action<T> onValue, Action onEmpty) {
        if (HasValue) {
            onValue(_value);
        } else {
            onEmpty();
        }
    }

    public void Do(Action<T> action) {
        if (HasValue) { action(_value); }
    }

    public bool HasValueAnd(Func<T, bool> predicate)
        => HasValue && predicate(_value);

    public Optional<T> Filter(Func<T, bool> predicate)
        => HasValue && predicate(_value) ? this : Empty;

    public Optional<T> Or(Optional<T> other)
        => HasValue ? this : other;

    public Optional<T> Or(Func<Optional<T>> otherFactory)
        => HasValue ? this : otherFactory();

    public Optional<T> Or(Lazy<Optional<T>> otherLazy)
        => HasValue ? this : otherLazy.Value;

    public Result<T, TErr> OrErr<TErr>(TErr err)
        where TErr : notnull
        => HasValue ? _value : err;

    public Result<T, TErr> OrErr<TErr>(Func<TErr> errFactory)
        where TErr : notnull
        => HasValue ? _value : errFactory();

    public Result<T, TErr> OrErr<TErr>(Lazy<TErr> errLazy)
        where TErr : notnull
        => HasValue ? _value : errLazy.Value;

    public Optional<T> And(Optional<T> other)
        => HasValue ? other : Empty;

    public Optional<T> And(Func<Optional<T>> otherFactory)
        => HasValue ? otherFactory() : Empty;

    public Optional<T> And(Lazy<Optional<T>> otherLazy)
        => HasValue ? otherLazy.Value : Empty;

    public Optional<T> Xor(Optional<T> other)
        => HasValue
            ? other.HasValue
                ? Empty
                : this
            : other;

    public T? ValueOrDefault()
        => HasValue ? _value : default;

    public T ValueOr(T alternative)
        => HasValue ? _value : alternative;

    public T ValueOr(Func<T> alternativeFactory)
        => HasValue ? _value : alternativeFactory();

    public T ValueOr(Lazy<T> alternativeLazy)
        => HasValue ? _value : alternativeLazy.Value;

    public T Except(Exception e)
        => HasValue ? _value : throw e;

    public T Except(string str)
        => Except(new InvalidOperationException(str));

    public bool TryValue([MaybeNullWhen(false)] out T t) {
        t = _value;
        return HasValue;
    }

    public override bool Equals(object? obj) =>
        obj switch {
            Optional<T> opt => HasValue == opt.HasValue && (!HasValue || _value.Equals(opt._value)),
            _ => HasValue && _value.Equals(obj),
        };

    public override int GetHashCode()
        => HasValue ? _value.GetHashCode() : 0;

    public static bool operator ==(Optional<T> lhs, Optional<T> rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(Optional<T> lhs, Optional<T> rhs)
        => !lhs.Equals(rhs);

    public override string ToString()
        => HasValue ? $"Optional({_value})" : "Empty";
}
