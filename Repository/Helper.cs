using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MITT_HIMAWAN_SUTANTO.Repository
{
    public class Helper
    {
        public string getNullstring(string obj)
        {
            if (obj == null)
            {
                return null;
            }
            return obj;
        }
    }
}