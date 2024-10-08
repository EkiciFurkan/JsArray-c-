using System.Collections;

var arr = new JsArray<string>();

arr.Push("Elma");
arr.Push("Armut");
arr.Push("Muz");

arr.Print.Show();

internal class JsArray<T> : IEnumerable<T>
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Dictionary<int, T> _items = [];
    
    private bool IsEmpty()
    {
        return _items.Count != 0;
    }

    public PrintExtensions Print => new(this);

    public T? this[T item]
    {
        get => _items.Values.FirstOrDefault(x => x!.Equals(item));

        set
        {
            foreach (var kv in _items.Where(kv => kv.Value!.Equals(item)))
            {
                if (value != null) _items[kv.Key] = value;
            }
        }
    }

    public T this[int index]
    {
        get
        {
            if (!_items.TryGetValue(index, out var item))
            {
                throw new KeyNotFoundException($"The key '{index}' was not found in the dictionary.");
            }

            return item;
        }

        set => _items[index] = value;
    }

    public int Lenght
    {
        get => _items.Count;

        set
        {
            if (_items.Count > value)
            {
                _items = new Dictionary<int, T>(_items.Take(value));
            }
            else
            {
                _items.EnsureCapacity(value);
            }
        }
    }

    public void Push(T item)
    {
        Push(NextIndex, item);
    }

    public IEnumerable<T> Values => _items.Values;

    public IEnumerable<int> Keys => _items.Keys;

    // ReSharper disable once MemberCanBePrivate.Global
    public void Push(int index, T item) => _items.Add(index, item);

    public void Clear() => _items.Clear();

    private int NextIndex => !IsEmpty() ? 0 : _items.Count;

    public class PrintExtensions(JsArray<T> parent)
    {
        public void Show()
        {
            Console.WriteLine("\n Total Count: {0}", parent._items.Count);

            foreach (var (key, value) in parent._items)
            {
                Console.WriteLine($"{key} : {value}");
            }
        }

        public IEnumerable<int> GetKeys()
        {
            var keys = parent._items.Keys;
            return keys.ToList();
        }

        public IEnumerable<T> GetValues()
        {
            var keys = parent._items.Values;
            return keys.ToList();
        }
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public int GetFirstKey => _items.First().Key;

    // ReSharper disable once MemberCanBePrivate.Global
    public T GetFirstValue => _items.First().Value;

    public int GetLastKey => _items.Last().Key;

    public T GetLastValue => _items.Last().Value;

    public TResult GetFirstElement<TResult>()
    {
        if (!IsEmpty())
        {
            throw new InvalidOperationException("The collection is empty. Cannot retrieve the first element.");
        }

        var firstKey = GetFirstKey;
        var firstValue = GetFirstValue;

        if (typeof(TResult) == typeof(string))
        {
            return (TResult)(object)$"{firstKey} : {firstValue}";
        }

        if (typeof(TResult) == typeof(Dictionary<int, T>))
        {
            return (TResult)(object)new Dictionary<int, T> { { firstKey, firstValue } };
        }

        throw new ArgumentException("Unexpected return type. Please provide a valid return type.");
    }
    
    public TResult GetLastElement<TResult>()
    {
        if (!IsEmpty())
        {
            throw new InvalidOperationException("The collection is empty. Cannot retrieve the first element.");
        }

        var lastKey = GetLastKey;
        var lastValue = GetLastValue;

        if (typeof(TResult) == typeof(string))
        {
            return (TResult)(object)$"{lastKey} : {lastValue}";
        }

        if (typeof(TResult) == typeof(Dictionary<int, T>))
        {
            return (TResult)(object)new Dictionary<int, T> { { lastKey, lastValue } };
        }

        throw new ArgumentException("Unexpected return type. Please provide a valid return type.");
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        return _items.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}