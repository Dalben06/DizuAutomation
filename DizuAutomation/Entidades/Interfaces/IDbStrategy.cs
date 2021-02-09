using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace DizuAutomation.Entidades.Interfaces
{
    public interface IDbStrategy
    {
        IDbConnection GetConnection(string ConnectionString);
    }
}
