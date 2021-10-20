using System.Collections.Generic;

namespace TesteBenchmarkDataAccessSqlServer.Dapper
{
    public class Regiao
    {
        public int IdRegiao { get; set; }
        public string NomeRegiao { get; set; }
        public List<Estado> Estados { get; set; }
    }

    public class Estado
    {
        public string SiglaEstado { get; set; }
        public string NomeEstado { get; set; }
        public string NomeCapital { get; set; }
    }
}