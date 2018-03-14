using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Data;

namespace MyProject
{
    /// <summary>
    /// Json operation class
    /// </summary>
    public static class JsonHelper
    {
        public static string ConvertDataTableToJson(this System.Data.DataTable dt)
        {
            return JsonConvert.SerializeObject(dt, new DataTableConverter());
        }

        public static string GetJsonFromDataTable(DataTable dt, int iTotalPage, int iPage, int iRecord)
        {
            var sbJsonRow = new StringBuilder();
            var sbJsonTotal = new StringBuilder();

            if (dt.Rows.Count > 0)
            {
                sbJsonTotal.AppendLine("{").AppendLine("\"page\":" + iPage + ",\"total\":" + iTotalPage + ",\"records\":" + iRecord + ",\"rows\":");
                sbJsonRow.AppendLine(dt.ConvertDataTableToJson());
                if (iTotalPage > 0)
                {
                    sbJsonTotal.AppendLine(sbJsonRow.ToString());
                    sbJsonTotal.Append("}");
                }
                else
                {
                    sbJsonTotal.Clear();
                    sbJsonTotal.Append(sbJsonRow.ToString());
                }
            }
            else
            {
                sbJsonTotal.AppendLine("{").AppendLine("\"page\":" + iPage + ",\"total\":0,\"records\":0,\"rows\":[]}");
            }
            return sbJsonTotal.ToString();

        }


        /// <summary>
        /// generate Json format
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetJson<T>(T obj)
        {
            var json = new DataContractJsonSerializer(obj.GetType());
            using (var stream = new MemoryStream())
            {
                json.WriteObject(stream, obj);
                string szJson = Encoding.UTF8.GetString(stream.ToArray());
                return szJson;
            }
        }
        /// <summary>
        /// Get Json to Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="szJson"></param>
        /// <returns></returns>
        public static T GetParseFromJson<T>(string szJson)
        {
            var obj = Activator.CreateInstance<T>();
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
            {
                var serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// generate Json for jqGrid
        /// ps:have't paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listModels"></param>
        /// <returns></returns>
        public static string GetJsonData<T>(IList<T> listModels)
        {
            var sbJsonRow = new StringBuilder();
            var sbJsonTotal = new StringBuilder();
            if (listModels.Count > 0)
            {
                sbJsonTotal.AppendLine("{").AppendLine("\"rows\":[");

                foreach (T model in listModels)
                {
                    sbJsonRow.AppendLine(GetJsonFromModel(model)).Append(",");
                }
                if (sbJsonRow.Length > 0)
                {
                    sbJsonRow.Remove(sbJsonRow.Length - 1, 1);
                    sbJsonTotal.AppendLine(sbJsonRow.ToString());
                    sbJsonTotal.Append("]}");
                }
            }
            else
            {
                sbJsonTotal.AppendLine("{").AppendLine("\"rows\":[]}");
            }
            return sbJsonTotal.ToString();
        }

        /// <summary>
        /// generate Json for jqGrid
        /// ps:have paging
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listModels">list</param>
        /// <param name="iTotalPage">total count</param>
        /// <param name="iPage">current page</param>
        /// <param name="iRecord">records page</param>
        /// <returns></returns>
        public static string GetJsonData<T>(IList<T> listModels, int iTotalPage, int iPage, int iRecord)
        {
            var sbJsonRow = new StringBuilder();
            var sbJsonTotal = new StringBuilder();
            if (listModels.Count > 0)
            {
                sbJsonTotal.AppendLine("{").AppendLine("\"page\":" + iPage + ",\"total\":" + iTotalPage + ",\"records\":" + iRecord + ",\"rows\":[");

                foreach (T model in listModels)
                {
                    sbJsonRow.AppendLine(GetJsonFromModel(model)).Append(",");
                }
                if (sbJsonRow.Length > 0)
                    sbJsonRow.Remove(sbJsonRow.Length - 1, 1);

                if (iTotalPage > 0)
                {
                    sbJsonTotal.AppendLine(sbJsonRow.ToString());
                    sbJsonTotal.Append("]}");
                }
                else
                {
                    sbJsonTotal.Clear();
                    sbJsonTotal.Append("[" + sbJsonRow.ToString() + "]");
                }
            }
            else
            {
                sbJsonTotal.AppendLine("{").AppendLine("\"page\":" + iPage + ",\"total\":0,\"records\":0,\"rows\":[]}");
            }
            return sbJsonTotal.ToString();
        }

        /// <summary>
        /// after analyzed Json string,generate Entity Class List
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="jsonStr">Json string</param>
        /// <returns></returns>
        public static T GetModelByJsonArray<T>(string jsonStr) where T : new()
        {
            var model = new T();
            GetModelByJson(GetDicByJson(jsonStr), ref model);
            return model;
        }

        /// <summary>
        /// after analyzed Json string and get dictionary type, generate Entity Class List
        /// </summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="jsonDataObject"></param>
        /// <returns></returns>
        public static List<T> GetListModelByDicArray<T>(dynamic[] jsonDataObject) where T : new()
        {
            var listModel = new List<T>();
            if (jsonDataObject.Count() > 0)
            {
                foreach (var obj in jsonDataObject)
                {
                    var dic = obj as Dictionary<string, object>;
                    var model = new T();
                    GetModelByJson(dic, ref model);
                    listModel.Add(model);
                }
            }
            return listModel;
        }


        /// <summary>
        /// after analyzed dictionary type and generate Entity Class List
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="jsonDataDic"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public static T GetModelByJson<T>(Dictionary<string, object> jsonDataDic, ref T model)
        {
            foreach (var d in jsonDataDic)
            {
                var pi = model.GetType().GetProperties().FirstOrDefault(p => p.Name == d.Key);
                if (pi != null)
                    pi.SetValue(model, CommonCls.GetValueType(jsonDataDic[pi.Name], pi), null);
            }
            return model;
        }

        /// <summary>
        /// Get dictionary by Json
        /// Key = 属性名
        /// Value = 值
        /// </summary>
        /// <param name="sJson"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetDicByJson(string sJson)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var jsonData = serializer.DeserializeObject(sJson) as Dictionary<string, object>;
            return jsonData;
        }

        /// <summary>
        /// Get Json by Dictionary
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetJsonByDic(Dictionary<string, object> dic)
        {
            var js = new System.Web.Script.Serialization.JavaScriptSerializer();
            return js.Serialize(dic);

        }

        private static string GetJsonFromModel<T>(T model)
        {
            var sbJosnRow = new StringBuilder();
            sbJosnRow.Append("{");
            if (model.GetType().Name == "ExpandoObject")
            {
                var dic = ((IDictionary<string, object>)model);
                dic.Remove("peta_rn");
                foreach (var o in dic.Keys)
                {
                    sbJosnRow.Append("\"" + o + "\":" + "\"" + dic[o] + "\",");
                }

                //拼接最后一行josn
                sbJosnRow.Remove(sbJosnRow.Length - 1, 1);
            }
            else
            {
                PropertyInfo[] pis = model.GetType().GetProperties();
                int iPisCount = pis.Count();
                //拼接除最后一行的josn
                for (int j = 0; j < iPisCount - 1; j++)
                {
                    sbJosnRow.Append("\"" + pis[j].Name + "\":" + "\"" + pis[j].GetValue(model, null) + "\",");
                }
                //拼接最后一行josn
                PropertyInfo lastPi = pis[iPisCount - 1];
                sbJosnRow.Append("\"" + lastPi.Name + "\":" + "\"" + lastPi.GetValue(model, null) + "\"");

            }
            sbJosnRow.Append("}");
            return sbJosnRow.ToString();
        }

        /// <summary>
        /// convert json by ListModel
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="total"></param>
        /// <param name="records"></param>
        /// <param name="listModel"></param>
        /// <returns></returns>
        public static string GetStringByLinqToJson<T>(int page, int total, int records, IList<T> listModel)
        {
            var data = new
            {
                page = page,
                total = total,
                records = records,
                rows = (from v in listModel
                        select new
                        {
                            cell = GetJsonFromModelByLinq<T>(v)
                        })
            };
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(data);
        }

        private static string[] GetJsonFromModelByLinq<T>(T model)
        {
            List<string> listCell = new List<string>();

            if (model.GetType().Name == "ExpandoObject")
            {
                var dic = ((IDictionary<string, object>)model);
                dic.Remove("peta_rn");
                foreach (var o in dic.Keys)
                {
                    listCell.Add(Convert.ToString(dic[o]));
                }
            }
            else
            {
                PropertyInfo[] infos = model.GetType().GetProperties();
                foreach (var v in infos)
                {
                    listCell.Add(Convert.ToString(v.GetValue(model, null)));
                }
            }
            return listCell.ToArray();
        }

    }

    public static class CommonCls
    {
        /// <summary>
        /// 转换类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pi"></param>
        /// <returns></returns>
        internal static object GetValueType(object value, System.Reflection.PropertyInfo pi)
        {
            object returnValue;
            switch (pi.PropertyType.Name)
            {
                case "Int32":
                    returnValue = value.ToString() == string.Empty ? 0 : Convert.ToInt32(value);
                    break;
                case "Double":
                    returnValue = value.ToString() == string.Empty ? 0.0 : Convert.ToDouble(value);
                    break;
                case "Decimal":
                    returnValue = value.ToString() == string.Empty ? 0 : Convert.ToDecimal(value);
                    break;
                case "Boolean":
                    if (value.ToString() == "0" || value.ToString() == "False")
                        returnValue = false;
                    else
                        returnValue = true;
                    break;
                case "DateTime":
                    returnValue = value.ToString() == string.Empty ? DateTime.Now : Convert.ToDateTime(value);
                    break;
                default:
                    returnValue = value as string;
                    break;
            }
            return returnValue;
        }


    


    }

}