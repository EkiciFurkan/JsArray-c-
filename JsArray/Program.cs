using System.Collections;

var arr = new JsArray<string>();
arr.Push("Banana");
arr.Push("Apple");
arr.Push(100_000, "osman");

Console.WriteLine(arr[100]);

arr.Print.Show();

internal class JsArray<T> : IEnumerable<T>
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Dictionary<int, T> _items = [];

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

    private int NextIndex => _items.Count == 0 ? 0 : _items.Count;

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
    
    public IEnumerator<T> GetEnumerator()
    {
        return _items.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}