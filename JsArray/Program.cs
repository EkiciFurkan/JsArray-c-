using System.Collections;

var arr = new JsArray<string>();

arr.Push("Hilal");
arr.Push("Hilal");
arr.Push("Hilal - Bıdı");
arr.Push("Hilal");
arr.Push("MEHMUT");
arr.Push("Furkan");

arr.Push("asdksadasl");

arr.Print.Show();

internal class JsArray<T> : IEnumerable<T>
{
    private Dictionary<int, T> _items = new();

    public PrintExtensions Print => new(this);

    private bool IsEmpty() => _items.Count == 0;

    public T? this[T item]
    {
        get
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");
            
            return _items.Values.FirstOrDefault(x => x!.Equals(item));
        }
        set
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item), "Item cannot be null.");

            foreach (var kv in _items.Where(kv => kv.Value!.Equals(item)))
            {
                if (value != null)
                    _items[kv.Key] = value;
            }
        }
    }

    public T this[int index]
    {
        get
        {
            if (!_items.TryGetValue(index, out var item))
                throw new KeyNotFoundException($"The key '{index}' was not found in the dictionary.");

            return item;
        }
        set
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), "Index cannot be negative.");

            _items[index] = value;
        }
    }

    public int Length
    {
        get => _items.Count;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Length cannot be negative.");

            if (_items.Count > value)
                _items = new Dictionary<int, T>(_items.Take(value));
            else
                _items.EnsureCapacity(value);
        }
    }

    public string? FindOne(string value)
    {
        if (value == null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null.");

        var item = _items.FirstOrDefault(kv => kv.Value != null && kv.Value.Equals(value));

        return item.Equals(default(KeyValuePair<int, T>)) ? null : $"{item.Key}: {item.Value}";
    }

    public List<string?> FindAll(string values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values), "Values cannot be null.");

        var listItem = _items.Values
            .Select(v => v?.ToString())
            .Where(x => string.Equals(x, values, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return listItem;
    }

    public List<string?> FoundAll(string values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values), "Values cannot be null.");

        var listItem = _items.Values
            .Select(v => v?.ToString())
            .Where(x => x != null && x.Contains(values))
            .ToList();

        return listItem;
    }

    public void Push(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        Push(NextIndex, item);
    }

    public IEnumerable<T> Values => _items.Values;

    public IEnumerable<int> Keys => _items.Keys;

    private void Push(int index, T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item), "Item cannot be null.");

        if (!_items.TryAdd(index, item))
            throw new ArgumentException("An item with the same index already exists.");
    }

    public void Clear() => _items.Clear();

    private int NextIndex => IsEmpty() ? 0 : _items.Count;

    public class PrintExtensions(JsArray<T> parent)
    {
        public void Show()
        {
            Console.WriteLine("\nTotal Count: {0}", parent._items.Count);

            foreach (var (key, value) in parent._items)
                Console.WriteLine($"{key} : {value}");
        }

        public IEnumerable<int> GetKeys() => parent._items.Keys.ToList();

        public IEnumerable<T> GetValues() => parent._items.Values.ToList();
    }

    private int GetFirstKey => _items.First().Key;

    private T GetFirstValue => _items.First().Value;

    private int GetLastKey => _items.Last().Key;

    private T GetLastValue => _items.Last().Value;

    public TResult GetFirstElement<TResult>()
    {
        if (IsEmpty())
            throw new InvalidOperationException("The collection is empty. Cannot retrieve the first element.");

        var firstKey = GetFirstKey;
        var firstValue = GetFirstValue;

        return typeof(TResult) switch
        {
            var type when type == typeof(string) => (TResult)(object)$"{firstKey} : {firstValue}",
            var type when type == typeof(Dictionary<int, T>) => (TResult)(object)new Dictionary<int, T> { { firstKey, firstValue } },
            _ => throw new ArgumentException("Unexpected return type. Please provide a valid return type.")
        };
    }

    public TResult GetLastElement<TResult>()
    {
        if (IsEmpty())
            throw new InvalidOperationException("The collection is empty. Cannot retrieve the last element.");

        var lastKey = GetLastKey;
        var lastValue = GetLastValue;

        return typeof(TResult) switch
        {
            var type when type == typeof(string) => (TResult)(object)$"{lastKey} : {lastValue}",
            var type when type == typeof(Dictionary<int, T>) => (TResult)(object)new Dictionary<int, T> { { lastKey, lastValue } },
            _ => throw new ArgumentException("Unexpected return type. Please provide a valid return type.")
        };
    }

    public IEnumerator<T> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}