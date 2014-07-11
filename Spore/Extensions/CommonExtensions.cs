using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spore.Extensions
{
    public static class CommonExtensions
    {
        public static int Int32Value(this Enum e)
        {
            return Convert.ToInt32(e);
        }
    }
}
