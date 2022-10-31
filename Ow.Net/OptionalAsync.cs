namespace Ow.Net;

public static class OptionalAsync {
    public static async Task<bool> HasValue<T>(this Task<Optional<T>> opt)
        where T : notnull
        => (await opt.ConfigureAwait(false)).HasValue;

    public static async Task<bool> IsEmpty<T>(this Task<Optional<T>> opt)
        where T : notnull
        => (await opt.ConfigureAwait(false)).IsEmpty;

    public static async Task<T> Value<T>(this Task<Optional<T>> opt)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Value;

    public static async Task<Optional<TOut>> Map<T, TOut>(this Task<Optional<T>> opt, Func<T, TOut> mapper)
        where T : notnull where TOut : notnull
        => (await opt.ConfigureAwait(false)).Map(mapper);

    public static async Task<Optional<TOut>> Bind<T, TOut>(this Task<Optional<T>> opt, Func<T, Optional<TOut>> binder)
        where T : notnull where TOut : notnull
        => (await opt.ConfigureAwait(false)).Bind(binder);

    public static async Task<TOut> Match<T, TOut>(this Task<Optional<T>> opt, Func<T, TOut> onValue, Func<TOut> onEmpty)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Match(onValue, onEmpty);

    public static async Task Match<T>(this Task<Optional<T>> opt, Action<T> onValue, Action onEmpty)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Match(onValue, onEmpty);

    public static async Task Do<T>(this Task<Optional<T>> opt, Action<T> action)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Do(action);

    public static async Task<bool> HasValueAnd<T>(this Task<Optional<T>> opt, Func<T, bool> predicate)
        where T : notnull
        => (await opt.ConfigureAwait(false)).HasValueAnd(predicate);

    public static async Task<Optional<T>> Filter<T>(this Task<Optional<T>> opt, Func<T, bool> predicate)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Filter(predicate);

    public static async Task<Optional<T>> Or<T>(this Task<Optional<T>> opt, Optional<T> other)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Or(other);

    public static async Task<Optional<T>> Or<T>(this Task<Optional<T>> opt, Func<Optional<T>> otherFactory)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Or(otherFactory);

    public static async Task<Optional<T>> Or<T>(this Task<Optional<T>> opt, Lazy<Optional<T>> otherLazy)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Or(otherLazy);

    public static async Task<Optional<T>> And<T>(this Task<Optional<T>> opt, Optional<T> other)
        where T : notnull
        => (await opt.ConfigureAwait(false)).And(other);

    public static async Task<Optional<T>> And<T>(this Task<Optional<T>> opt, Func<Optional<T>> otherFactory)
        where T : notnull
        => (await opt.ConfigureAwait(false)).And(otherFactory);

    public static async Task<Optional<T>> And<T>(this Task<Optional<T>> opt, Lazy<Optional<T>> otherLazy)
        where T : notnull
        => (await opt.ConfigureAwait(false)).And(otherLazy);

    public static async Task<Optional<T>> Xor<T>(this Task<Optional<T>> opt, Optional<T> other)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Xor(other);

    public static async Task<T?> ValueOrDefault<T>(this Task<Optional<T>> opt)
        where T : notnull
        => (await opt.ConfigureAwait(false)).ValueOrDefault();

    public static async Task<T> ValueOr<T>(this Task<Optional<T>> opt, T alternative)
        where T : notnull
        => (await opt.ConfigureAwait(false)).ValueOr(alternative);

    public static async Task<T> ValueOr<T>(this Task<Optional<T>> opt, Func<T> alternativeFactory)
        where T : notnull
        => (await opt.ConfigureAwait(false)).ValueOr(alternativeFactory);

    public static async Task<T> ValueOr<T>(this Task<Optional<T>> opt, Lazy<T> alternativeLazy)
        where T : notnull
        => (await opt.ConfigureAwait(false)).ValueOr(alternativeLazy);

    public static async Task<T> Except<T>(this Task<Optional<T>> opt, Exception e)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Except(e);

    public static async Task<T> Except<T>(this Task<Optional<T>> opt, string str)
        where T : notnull
        => (await opt.ConfigureAwait(false)).Except(str);
}
