using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DizuAutomation.Repositorio.Context
{
    public class SqlServerStrategy
    {
        public IDbConnection GetConnection(string ConnectionString)
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
