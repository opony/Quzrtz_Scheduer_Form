using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Interfaces.Rule
{
    interface IBinCodeChangeRule
    {
        void ChangeBinCode(DataTable mapDataTb);
        Dictionary<string, string> GetBinCodeChangeMap();
    }
}
