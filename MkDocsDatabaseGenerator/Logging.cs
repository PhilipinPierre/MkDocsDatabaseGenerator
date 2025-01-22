using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MkDocsDatabaseGenerator
{
    internal class Logging
    {
        public static Fay.Logger.Logging Instance { get => Fay.Logger.Logging.Instance; }
    }
}