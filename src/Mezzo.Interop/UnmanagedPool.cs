using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
                if (disposing)
                {
                    // TODO: Verwalteten Zustand (verwaltete Objekte) bereinigen
                }

                
                _disposed = true;
            }
        }

        public sealed class Memory : IDisposable
        {
            private readonly BytePtr _pointer;
            private readonly int _size;

            public Memory(int size)
            {
                _pointer = new BytePtr(Marshal.AllocHGlobal(size));
                _size = size;
            }

            internal bool Rented { get; set; }
            public Span<byte> Span => _pointer.AsSpan(_size);

            public void Dispose()
            {
                Rented = false;
            }
        }
    }
}
