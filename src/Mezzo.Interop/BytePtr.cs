using System;
using System.Runtime.InteropServices;

namespace Mezzo.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct BytePtr
    {
        private readonly byte* _pointer;

        public BytePtr(byte* pointer) => _pointer = pointer;
        public BytePtr(nint address) => _pointer = (byte*)address;

        public nint Address => (nint)_pointer;
        public bool IsValid => Address != 0;

        public byte Value
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

        public Span<byte> AsSpan(int count)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return new Span<byte>(_pointer, count);
        }

        public static implicit operator BytePtr(byte* pointer) => new(pointer);
        public static implicit operator byte*(BytePtr uint32Ptr) => uint32Ptr._pointer;
    }
}
