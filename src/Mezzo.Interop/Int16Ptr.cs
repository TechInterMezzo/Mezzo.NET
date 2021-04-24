using System;
using System.Runtime.InteropServices;

namespace Mezzo.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct Int16Ptr
    {
        private readonly short* _pointer;

        public Int16Ptr(short* pointer) => _pointer = pointer;
        public Int16Ptr(nint address) => _pointer = (short*)address;

        public nint Address => (nint)_pointer;
        public bool IsValid => Address != 0;

        public short Value
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

        public Span<short> AsSpan(int count)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return new Span<short>(_pointer, count);
        }

        public static implicit operator Int16Ptr(short* pointer) => new(pointer);
        public static implicit operator short*(Int16Ptr int16Ptr) => int16Ptr._pointer;
    }
}
