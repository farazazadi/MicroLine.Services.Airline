namespace MicroLine.Services.Airline.Tests.Integration.Common;

public class SynchronizedList<T>
{
    private readonly ReaderWriterLockSlim _lockSlim = new();
    private readonly List<T> _list = new();

    public int Count
    {
        get
        {
            _lockSlim.EnterReadLock();
            try
            {
                return _list.Count;
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }
    }

    public T SingleOrDefault(Func<T, bool> predicate)
    {
        _lockSlim.EnterReadLock();
        try
        {
            return _list.SingleOrDefault(predicate);
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }


    public T FirstOrDefault(Func<T, bool> predicate)
    {
        _lockSlim.EnterReadLock();
        try
        {
            return _list.FirstOrDefault(predicate);
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }


    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        _lockSlim.EnterReadLock();
        try
        {
            return _list.Where(predicate);
        }
        finally
        {
            _lockSlim.ExitReadLock();
        }
    }

    public void Add(T item)
    {
        _lockSlim.EnterWriteLock();
        try
        {
            _list.Add(item);
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    public bool Remove(T item)
    {
        _lockSlim.EnterWriteLock();
        try
        {
            return _list.Remove(item);
        }
        finally
        {
            _lockSlim.ExitWriteLock();
        }
    }

    ~SynchronizedList() => _lockSlim?.Dispose();
}