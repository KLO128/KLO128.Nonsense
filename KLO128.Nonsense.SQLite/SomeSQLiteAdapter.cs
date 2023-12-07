using KLO128.Nonsense.Common;
using KLO128.Nonsense.Common.Abstract;
using KLO128.Nonsense.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KLO128.Nonsense.Common.Abstract.Models;
using Microsoft.Data.Sqlite;

namespace KLO128.Nonsense.SQLite
{
    public class SomeSQLiteAdapter : BaseSomeAdapter
    {
        public SomeSQLiteAdapter(ILexer lexer) : base(lexer)
        {

        }

        public override List<Token> LoadTokens(string code)
        {
            using (var connection = new SqliteConnection(@"Data Source=nonsense.sqlite"))
            {
                //connection.Open();
            }

            return base.LoadTokens(code);
        }
    }
}
