using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlDBManager.DBClasses
{
    internal class TestCatalog : BaseDBConnector
    {
        public TestCatalog(string source, string catalog, string login, string password) : base(source, catalog, login, password) { }


        public Tuple<int, List<Dictionary<string, string>>> TestSelectAdapter(string request)
        {
            return SelectAdapter(request, ReturnConnection(), ReturnTransaction());
        }

        public int TestSelectCountAdapter(string request)
        {
            return SelectCountAdapter(request, ReturnConnection(), ReturnTransaction());
        }
    }
}
