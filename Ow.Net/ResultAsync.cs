namespace Ow.Net; 

public static class ResultAsync {
    public static async Task<Result<T, TErr>> OkAsync<T, TErr>(this Task<T> t)
        where T : notnull where TErr : notnull
        => await t.ConfigureAwait(false);

    public static async Task<Result<T, TErr>> ErrAsync<T, TErr>(this Task<TErr> err)
        where T : notnull where TErr : notnull
        => await err.ConfigureAwait(false);

    public static async Task<T> Except<T, TErr>(this Task<Result<T, TErr>> res)
        where T : notnull where TErr : Exception
        => (await res.ConfigureAwait(false)).Except();

    public static async Task<T> Ok<T, TErr>(this Task<Result<T, TErr>> res)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Ok;

    public static async Task<TErr> Err<T, TErr>(this Task<Result<T, TErr>> res)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Err;

    public static async Task<Result<S, TErr>> Map<T, TErr, S>(this Task<Result<T, TErr>> res, Func<T, S> mapper)
        where T : notnull where TErr : notnull where S : notnull
        => (await res.ConfigureAwait(false)).Map(mapper);

    public static async Task<Result<T, SErr>> Map<T, TErr, SErr>(this Task<Result<T, TErr>> res, Func<TErr, SErr> mapper)
        where T : notnull where TErr : notnull where SErr : notnull
        => (await res.ConfigureAwait(false)).MapErr(mapper);

    public static async Task<Result<S, SErr>> Map<T, TErr, S, SErr>(this Task<Result<T, TErr>> res,
        Func<T, S> okMapper, Func<TErr, SErr> errMapper)
        where T : notnull where TErr : notnull where S : notnull where SErr : notnull
        => (await res.ConfigureAwait(false)).BiMap(okMapper, errMapper);

    public static async Task<Result<S, SErr>> Bind<T, TErr, S, SErr>(this Task<Result<T, TErr>> res,
        Func<T, Result<S, SErr>> okMapper, Func<TErr, Result<S, SErr>> errMapper)
        where T : notnull where TErr : notnull where S : notnull where SErr : notnull
        => (await res.ConfigureAwait(false)).Bind(okMapper, errMapper);

    public static async Task<bool> Match<T, TErr>(this Task<Result<T, TErr>> res, Action<T> ok, Action<TErr> err)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Match(ok, err);

    public static async Task<bool> Contains<T, TErr>(this Task<Result<T, TErr>> res, Func<T, bool> predicate)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Contains(predicate);

    public static async Task<bool> ContainsErr<T, TErr>(this Task<Result<T, TErr>> res, Func<TErr, bool> predicate)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).ContainsErr(predicate);

    public static async Task<T?> OkOrDefault<T, TErr>(this Task<Result<T, TErr>> res)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).OkOrDefault();

    public static async Task<T> OkOr<T, TErr>(this Task<Result<T, TErr>> res, T t)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).OkOr(t);

    public static async Task<T> OkOr<T, TErr>(this Task<Result<T, TErr>> res, Func<T> factoryT)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).OkOr(factoryT);

    public static async Task<T> OkOr<T, TErr>(this Task<Result<T, TErr>> res, Lazy<T> lazyT)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).OkOr(lazyT);

    public static async Task<T> Except<T, TErr>(this Task<Result<T, TErr>> res, Exception e)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Except(e);

    public static async Task<T> Except<T, TErr>(this Task<Result<T, TErr>> res, string str)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Except(str);

    internal static async Task<Result<Optional<T>, TErr>> Transpose<T, TErr>(this Task<Optional<Result<T, TErr>>> opt)
        where T : notnull where TErr : notnull
        => (await opt.ConfigureAwait(false)).Transpose();

    internal static async Task<Optional<Result<T, TErr>>> Transpose<T, TErr>(this Task<Result<Optional<T>, TErr>> res)
        where T : notnull where TErr : notnull
        => (await res.ConfigureAwait(false)).Transpose();
}
