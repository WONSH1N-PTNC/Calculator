using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model
{
    // "admin", "user", "guest"
    public static class SessionManager
    {
        public static string CurrentUserId { get; set; } = "guest";
        public static string Authority { get; set; } = "guest";
    }
}
