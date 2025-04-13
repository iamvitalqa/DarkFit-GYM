using System;
using System.Collections.Generic;
using System.Text;

namespace DarkFit_app
{
    public static class DarkFitDatabase
    {
        public static string ConnectionString =>
            "Host=192.168.0.106;Port=5432;Database=postgres;Username=postgres;Password=admin;Timeout=5;";
    }
}
