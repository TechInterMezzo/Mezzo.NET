using System;

namespace Mezzo.Interop
{
    public unsafe readonly struct UInt32Ptr
    {
        private readonly uint* _pointer;

        public UInt32Ptr(uint* pointer) => _pointer = pointer;
        public UInt32Ptr(nint address) => _pointer = (uint*)address;

        public nint Address => (nint)_pointer;
        public bool IsValid => Address != 0;

        public uint Value
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

        public Span<uint> AsSpan(int count)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return new Span<uint>(_pointer, count);
        }

        public static implicit operator UInt32Ptr(uint* pointer) => new(pointer);
        public static implicit operator uint*(UInt32Ptr uint32Ptr) => uint32Ptr._pointer;
    }
}
