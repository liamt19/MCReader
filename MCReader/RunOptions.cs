#define INLINE
//#define NOINLINING

using System.Runtime.CompilerServices;

namespace MCReader
{
    public static class RunOptions
    {

        //  PreserveSig shouldn't have any meaningful impact on performance... I hope.

#if (INLINE && !NOINLINING)
        public const MethodImplOptions Inline = MethodImplOptions.AggressiveInlining;
#else
        public const MethodImplOptions Inline = MethodImplOptions.PreserveSig;
#endif
    }
}
