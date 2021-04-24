using System;
using System.Runtime.InteropServices;

namespace Mezzo.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct UInt16Ptr
    {
        private readonly ushort* _pointer;

        public UInt16Ptr(ushort* pointer) => _pointer = pointer;
        public UInt16Ptr(nint address) => _pointer = (ushort*)address;

        public nint Address => (nint)_pointer;
        public bool IsValid => Address != 0;

        public ushort Value
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

        public Span<ushort> AsSpan(int count)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return new Span<ushort>(_pointer, count);
        }

        public static implicit operator UInt16Ptr(ushort* pointer) => new(pointer);
        public static implicit operator ushort*(UInt16Ptr uint16Ptr) => uint16Ptr._pointer;
    }
}
