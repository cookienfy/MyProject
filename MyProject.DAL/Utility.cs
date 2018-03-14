using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.DAL
{
    public static class Utility
    {
        /// <summary>
        /// Convert a IDictinoary instance to object
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static object ConvertDictionaryToObject(this IDictionary<string, object> dictionary)
        {
            var eo = new System.Dynamic.ExpandoObject();
            var eoColl = (ICollection<KeyValuePair<string, object>>)eo;

            foreach (var kvp in dictionary)
            {
                eoColl.Add(kvp);
            }
            return eo;
        }
    }
}
