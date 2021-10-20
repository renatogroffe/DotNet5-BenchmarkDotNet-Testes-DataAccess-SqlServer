using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using BenchmarkDotNet.Attributes;
using Dapper;
using Slapper;
using TesteBenchmarkDataAccessSqlServer.EFCore;

namespace TesteBenchmarkDataAccessSqlServer.Tests
{
    public class TestesDataAccess
    {
        private const string _COD_REGIAO = "NE";
        private RegioesContext _context;
        private SqlConnection _connection;

        [IterationSetup(Target = nameof(QueryWithEntityFrameworkCore))]
        public void SetupEntityFrameworkCore()
        {
            _context = new RegioesContext();
        }

        [Benchmark]
        public EFCore.Regiao QueryWithEntityFrameworkCore()
        {
            return _context.Regioes
                .Where(r => r.CodRegiao == _COD_REGIAO)
                .Include(r => r.Estados)
                .Single();
        }

        [IterationCleanup(Target = nameof(QueryWithEntityFrameworkCore))]
        public void CleanupEntityFrameworkCore()
        {
            _context = null;
        }

        [IterationSetup(Target = nameof(QueryWithDapper))]
        public void SetupDapper()
        {
            _connection = new SqlConnection(Configurations.BaseDapper);
        }

        [Benchmark]
        public Dapper.Regiao QueryWithDapper()
        {
            _connection.Open();
            var dados = _connection.Query<dynamic>(
                "SELECT R.IdRegiao, " +
                        "R.CodRegiao, " +
                        "R.NomeRegiao, " +
                        "E.SiglaEstado AS Estados_SiglaEstado, " +
                        "E.NomeEstado AS Estados_NomeEstado, " +
                        "E.NomeCapital AS Estados_NomeCapital " +
                "FROM dbo.Regioes R " +
                "INNER JOIN dbo.Estados E " +
                    "ON E.IdRegiao = R.IdRegiao " +
                "WHERE (R.CodRegiao = @CodigoRegiao) " +
                "ORDER BY R.NomeRegiao, E.NomeEstado",
                new { CodigoRegiao = _COD_REGIAO });
            _connection.Close();

            AutoMapper.Configuration.AddIdentifier(
                typeof(Dapper.Regiao), "IdRegiao");
            AutoMapper.Configuration.AddIdentifier(
                typeof(Dapper.Estado), "SiglaEstado");

            return (AutoMapper.MapDynamic<Dapper.Regiao>(dados)
                as IEnumerable<Dapper.Regiao>).ToArray().Single();
        }

        [IterationCleanup(Target = nameof(QueryWithDapper))]
        public void CleanupDapper()
        {
            _connection = null;
        }
    }
}