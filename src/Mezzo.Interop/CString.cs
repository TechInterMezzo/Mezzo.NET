using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mezzo.Interop
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe readonly struct CString
    {
        private readonly byte* _pointer;

        public CString(byte* pointer) => _pointer = pointer;
        public CString(nint address) => _pointer = (byte*)address;

        public CString(byte* pointer, int capacity, string value)
            : this(pointer)
        {
            SetValue(capacity, value);
        }

        public CString(byte* pointer, int capacity, ReadOnlySpan<char> chars)
            : this(pointer)
        {
            SetValue(capacity, chars);
        }

        public nint Address => (nint)_pointer;
        public bool IsValid => Address != 0;

        public static int GetCapacity(int length)
        {
            return Encoding.UTF8.GetMaxByteCount(length) + 1;
        }

        public int GetLength(int capacity = int.MaxValue)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            for (int i = 0; i < capacity; i++)
            {
                if (*(_pointer + i) == 0)
                {
                    return i;
                }
            }
            return 0;
        }

        public Span<char> GetValue(Span<char> buffer)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return buffer.Slice(0, Encoding.UTF8.GetChars(AsSpan(buffer.Length * sizeof(char)), buffer));
        }

        private void SetValue(int capacity, ReadOnlySpan<char> chars)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            var bytes = new Span<byte>(_pointer, capacity);
            int length = Encoding.UTF8.GetBytes(chars, bytes[0..^1]);
            bytes[length] = 0;
        }

        public Span<byte> AsSpan(int capacity = int.MaxValue)
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return new Span<byte>(_pointer, GetLength(capacity));
        }

        public override string ToString()
        {
            if (!IsValid)
            {
                throw new NullPointerException();
            }
            return Marshal.PtrToStringUTF8(Address) ?? "";
        }

        public static implicit operator CString(byte* pointer) => new(pointer);
        public static implicit operator byte*(CString cstring) => cstring._pointer;
    }
}
