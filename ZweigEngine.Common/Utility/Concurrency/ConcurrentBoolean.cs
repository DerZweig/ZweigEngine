﻿namespace ZweigEngine.Common.Utility.Concurrency;

public struct ConcurrentBoolean
{
    private const long FALSE_VALUE = 0L;
    private const long TRUE_VALUE  = 1L;

    private long m_location;

    public ConcurrentBoolean(bool value)
    {
        m_location = BoolToLong(value);
    }

    public bool Read()
    {
        var oval = Interlocked.Read(ref m_location);
        return LongToBool(oval);
    }

    public bool Exchange(bool value)
    {
        var lval = BoolToLong(value);
        var oval = Interlocked.Exchange(ref m_location, lval);
        return LongToBool(oval);
    }

    public bool CompareExchange(bool value, bool comparand)
    {
        var lval = BoolToLong(value);
        var cval = BoolToLong(comparand);
        var oval = Interlocked.CompareExchange(ref m_location, lval, cval);
        return LongToBool(oval);
    }

    private static bool LongToBool(long value)
    {
        return value != FALSE_VALUE;
    }

    private static long BoolToLong(bool value)
    {
        return value ? TRUE_VALUE : FALSE_VALUE;
    }
}