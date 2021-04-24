using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

namespace Mezzo.Interop
{
    public sealed class UnmanagedPool : IDisposable
    {
        private readonly List<Memory> _pool;
        private bool _disposed;

        public UnmanagedPool()
        {
            _pool = new List<Memory>();
        }

        ~UnmanagedPool()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                foreach (Memory memory in _pool)
                {
                    Marshal.FreeHGlobal(memory.Pointer.Address);
                }
                if (disposing)
                {
                    _pool.Clear();
                }
                _disposed = true;
            }
        }

        public Memory Rent(int size)
        {
            int index = BinarySearch(size);
            if (index < 0)
            {
                index = ~index;
            }
            Memory memory;
            for (int i = index; i < _pool.Count; i++)
            {
                memory = _pool[i];
                if (memory.Rent())
                {
                    return memory;
                }
            }
            memory = new Memory(size);
            memory.Rent();
            _pool.Insert(index, memory);
            return memory;
        }

        private int BinarySearch(int size)
        {
            int lo = 0;
            int hi = _pool.Count - 1;
            while (lo <= hi)
            {
                int i = (int)(((uint)hi + (uint)lo) >> 1);
                int c = size - _pool[i].Size;
                if (c == 0)
                {
                    return i;
                }
                else if (c > 0)
                {
                    lo = i + 1;
                }
                else
                {
                    hi = i - 1;
                }
            }
            return ~lo;
        }

        public sealed class Memory : IDisposable
        {
            private int _rented;

            public Memory(int size)
            {
                Pointer = new BytePtr(Marshal.AllocHGlobal(size));
                Size = size;
            }

            internal BytePtr Pointer { get; }
            internal int Size { get; }

            public Span<byte> Span => Pointer.AsSpan(Size);

            public void Dispose()
            {
                Interlocked.Exchange(ref _rented, 0);
            }

            internal bool Rent()
            {
                return Interlocked.Exchange(ref _rented, 1) == 0;
            }
        }
    }
}
