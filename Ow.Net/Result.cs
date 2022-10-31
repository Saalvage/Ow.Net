using System.Diagnostics.CodeAnalysis;

namespace Ow.Net;

public static class ResultExtensions {
    public static Result<T, TErr> Ok<T, TErr>(this T t)
        where T : notnull where TErr : notnull
        => t;

    public static Result<T, TErr> Err<T, TErr>(this TErr err)
        where T : notnull where TErr : notnull
        => err;

    public static T Except<T, TErr>(this Result<T, TErr> res)
        where T : notnull where TErr : Exception
        => res.IsOk ? res.Ok : throw res.Err;

    public static Result<Optional<T>, TErr> Transpose<T, TErr>(this Optional<Result<T, TErr>> opt)
        where T : notnull where TErr : notnull
        => opt.HasValue
            ? opt.Value.IsOk
                ? opt.Value.Ok.ToOptional()
                : opt.Value.Err
            : Optional<T>.Empty;

    public static Optional<Result<T, TErr>> Transpose<T, TErr>(this Result<Optional<T>, TErr> res)
        where T : notnull where TErr : notnull
        => res.IsOk
            ? res.Ok.HasValue
                ? res.Ok.Value.Ok<T, TErr>()
                : Optional.Empty
            : res.Err.Err<T, TErr>();
}

public static class Result {
    #region Protect Async

    public static async Task<Result<T, Exception>> Protect<T>
        (this Func<Task<T>> act) where T : notnull {
        try { return await act().ConfigureAwait(false); } catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect<T, T1>
        (this Func<T1, Task<T>> act, T1 t1) where T : notnull {
        try { return await act(t1).ConfigureAwait(false); } catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect<T, T1, T2>
        (this Func<T1, T2, Task<T>> act, T1 t1, T2 t2) where T : notnull {
        try { return await act(t1, t2).ConfigureAwait(false); } catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect<T, T1, T2, T3>
        (this Func<T1, T2, T3, Task<T>> act, T1 t1, T2 t2, T3 t3) where T : notnull {
        try { return await act(t1, t2, t3).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect<T, T1, T2, T3, T4>
        (this Func<T1, T2, T3, T4, Task<T>> act, T1 t1, T2 t2, T3 t3, T4 t4) where T : notnull {
        try { return await act(t1, t2, t3, t4).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect<T, T1, T2, T3, T4, T5>
        (this Func<T1, T2, T3, T4, T5, Task<T>> act, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect<T, T1, T2, T3, T4, T5, T6>
        (this Func<T1, T2, T3, T4, T5, T6, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7>
        (this Func<T1, T2, T3, T4, T5, T6, T7, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9
            , T10 t10) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    public static async Task<Result<T, Exception>> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<T>> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) where T : notnull {
        try { return await act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16).ConfigureAwait(false); }
        catch (Exception e) { return e; }
    }

    #endregion

    #region Protect

    public static Result<T, Exception> Protect<T>(this Func<T> act) where T : notnull {
        try { return act(); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect<T, T1>(this Func<T1, T> act, T1 t1) where T : notnull {
        try { return act(t1); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect<T, T1, T2>
        (this Func<T1, T2, T> act, T1 t1, T2 t2) where T : notnull {
        try { return act(t1, t2); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect<T, T1, T2, T3>
        (this Func<T1, T2, T3, T> act, T1 t1, T2 t2, T3 t3) where T : notnull {
        try { return act(t1, t2, t3); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect<T, T1, T2, T3, T4>
        (this Func<T1, T2, T3, T4, T> act, T1 t1, T2 t2, T3 t3, T4 t4) where T : notnull {
        try { return act(t1, t2, t3, t4); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect<T, T1, T2, T3, T4, T5>
        (this Func<T1, T2, T3, T4, T5, T> act, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) where T : notnull {
        try { return act(t1, t2, t3, t4, t5); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect<T, T1, T2, T3, T4, T5, T6>
        (this Func<T1, T2, T3, T4, T5, T6, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9); } catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9
            , T10 t10) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10); }
        catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11); }
        catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12); }
        catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13); }
        catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14); }
        catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15); }
        catch (Exception e) { return e; }
    }

    public static Result<T, Exception> Protect
        <T, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
        (this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T> act,
            T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9,
            T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) where T : notnull {
        try { return act(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10, t11, t12, t13, t14, t15, t16); }
        catch (Exception e) { return e; }
    }

    #endregion
}

public sealed class Result<T, TErr>
    where T : notnull where TErr : notnull {
    private readonly T _ok;
    private readonly TErr _err;

    public bool IsOk { get; }
    public bool IsErr => !IsOk;

    public T Ok => IsOk ? _ok : throw new InvalidOperationException("Result was in error state");
    public TErr Err => IsErr ? _err : throw new InvalidOperationException("Result was in success state");

    private object Value => IsOk ? _ok : _err;

    private static Result<T, TErr> MakeOk(T t)
        => new(t);

    private static Result<T, TErr> MakeErr(TErr err)
        => new(err);

    private Result(T ok) {
        IsOk = true;
        _ok = ok;
        _err = default!;
    }

    private Result(TErr err) {
        IsOk = false;
        _ok = default!;
        _err = err;
    }

    public static explicit operator T(Result<T, TErr> res)
        => res.Ok;

    public static explicit operator TErr(Result<T, TErr> res)
        => res.Err;

    public static implicit operator Result<T, TErr>(T t)
        => MakeOk(t);

    public static implicit operator Result<T, TErr>(TErr err)
        => MakeErr(err);

    public Result<S, TErr> Map<S>(Func<T, S> mapper)
        where S : notnull
        => IsOk ? mapper(_ok) : _err;

    public Result<T, SErr> MapErr<SErr>(Func<TErr, SErr> mapper)
        where SErr : notnull
        => IsOk ? _ok : mapper(_err);

    public Result<S, SErr> BiMap<S, SErr>(Func<T, S> okMapper, Func<TErr, SErr> errMapper)
        where S : notnull where SErr : notnull
        => IsOk ? okMapper(_ok) : errMapper(_err);

    public Result<S, SErr> Bind<S, SErr>(Func<T, Result<S, SErr>> okMapper, Func<TErr, Result<S, SErr>> errMapper)
        where S : notnull where SErr : notnull
        => IsOk ? okMapper(_ok) : errMapper(_err);

    public bool Match(Action<T> ok, Action<TErr> err) {
        if (IsOk) {
            ok(_ok);
        } else {
            err(_err);
        }

        return IsOk;
    }

    public bool Contains(Func<T, bool> predicate)
        => IsOk && predicate(_ok);

    public bool ContainsErr(Func<TErr, bool> predicate)
        => IsErr && predicate(_err);

    public T? OkOrDefault()
        => IsOk ? _ok : default;

    public T OkOr(T t)
        => IsOk ? _ok : t;

    public T OkOr(Func<T> factoryT)
        => IsOk ? _ok : factoryT();

    public T OkOr(Lazy<T> lazyT)
        => IsOk ? _ok : lazyT.Value;

    public T Except(Exception e)
        => IsOk ? _ok : throw e;

    public T Except(string str)
        => IsOk ? _ok : throw new InvalidOperationException(str);

    public bool TryOk([MaybeNullWhen(false)] out T t) {
        t = _ok;
        return IsOk;
    }

    public bool TryErr([MaybeNullWhen(false)] out TErr err) {
        err = _err;
        return IsErr;
    }

    public bool Extract([MaybeNullWhen(false)] out T t, [MaybeNullWhen(true)] out TErr err) {
        t = _ok;
        err = _err;
        return IsErr;
    }

    public override bool Equals(object? obj)
        => obj switch {
            Result<T, TErr> other => IsOk == other.IsOk && Value.Equals(other.Value),
            T t => IsOk && _ok.Equals(t),
            TErr err => IsErr && _err.Equals(err),
            _ => Value.Equals(obj),
        };

    public override int GetHashCode()
        => Value.GetHashCode();

    public static bool operator ==(Result<T, TErr> lhs, Result<T, TErr> rhs)
        => lhs.Equals(rhs);

    public static bool operator !=(Result<T, TErr> lhs, Result<T, TErr> rhs)
        => !lhs.Equals(rhs);

    public override string ToString()
        => IsOk ? $"MakeOk({_ok})" : $"MakeErr({_err})";
}
