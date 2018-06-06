using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MesAutoLoaderLib.Config
{
    class JobConfig
    {
        public static Dictionary<string, string> ParserString(string param, char splitCh)
        {
            Dictionary<string, string> paramMap = new Dictionary<string, string>();
            StringReader strReader = new StringReader(param);
            string line = null;
            string[] tokens;
            while ((line = strReader.ReadLine()) != null)
            {
                if (line.Contains(splitCh))
                {
                    tokens = line.Split(splitCh);
                    paramMap.Add(tokens[0].Trim(), tokens[1].Trim());
                }
            }

            return paramMap;
        }
    }
}
