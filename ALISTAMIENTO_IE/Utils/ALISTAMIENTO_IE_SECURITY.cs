using Common.cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ALISTAMIENTO_IE.Utils
{
    public static class ALISTAMIENTO_IE_SECURITY
    {
        private static readonly string nombreApp = Assembly.GetEntryAssembly()?.GetName().Name;
        public static bool isAdmin = UserLoginCache.TienePermisoLike($"Administrador - [{nombreApp}]");
    }
}
