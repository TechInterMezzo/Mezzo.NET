using System;
using System.Runtime.InteropServices;

namespace Mezzo.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct Int32Ptr
    {
        private readonly int* _pointer;

        public Int32Ptr(int* pointer) => _pointer = pointer;
        public Int32Ptr(nint address) => _pointer = (int*)address;

        public nint Address => (nint)_pointer;
        public bool IsValid => Address != 0;

        public int Value
        {
            get
            {
                if (!IsValid)
                {
                    throw new NullPointerException();
                }
                return *_pointer;
            }
            set
            {
                if (!IsValid)
                {
                    throw new NullPointerException();
                }
                *_pointer = value;
            }
        }

        public Span<int> AsSpan(int count)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return new Span<int>(_pointer, count);
        }

        public static implicit operator Int32Ptr(int* pointer) => new(pointer);
        public static implicit operator int*(Int32Ptr int32Ptr) => int32Ptr._pointer;
    }
}
