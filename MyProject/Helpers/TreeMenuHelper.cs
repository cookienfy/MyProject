using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyProject.DAL;
using MyProject.DAL.EF;
using Newtonsoft.Json;

namespace MyProject.Helpers
{
    public class TreeMenuHelper
    {

        public static string GetMenuJson()
        {
            IEnumerable<uFunction> querys = null;
            IEnumerable<uFunction> noneParentId = null;
            using (MyProject.DAL.UnitOfWork uw = new DAL.UnitOfWork())
            {
                querys = uw.FunctionRepository.DbSet.Where(p => p.FunTypeId == uw.CodeRepository.DbSet.FirstOrDefault(r => r.Code == "Menu").CodeId && p.LCV == false).ToList();
                noneParentId = querys.Where(p => p.FunParentId == null).ToList();
            }
            IList<TreeNode> list = new List<TreeNode>();

            foreach (uFunction f in noneParentId)
            {
                TreeNode node = new TreeNode();
                node.id = f.FunId;
                node.text = f.FunName;
                node.icon = f.FunPic;
                node.href = string.IsNullOrEmpty(f.FunLink) ? "#" : f.FunLink;
                list.Add(node);

                var sub = querys.Where(p => p.FunParentId == f.FunId);
                if (sub.Count() > 0)
                    RecursionGetNode(node, sub, querys);
            }
            var settings = new Newtonsoft.Json.JsonSerializerSettings();
            settings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

            Newtonsoft.Json.JsonConvert.SerializeObject(list, Formatting.Indented, settings);

            return JsonHelper.GetJson(list);
        }

        private static void RecursionGetNode(TreeNode parentNode, IEnumerable<uFunction> functions, IEnumerable<uFunction> querys)
        {
            foreach (uFunction f in functions)
            {
                TreeNode node = new TreeNode();
                node.id = f.FunId;
                node.text = f.FunName;
                node.icon = f.FunPic;

                node.href = string.IsNullOrEmpty(f.FunLink) ? "#" : f.FunLink;
                node.state = new state() { expanded = true };
                if (parentNode.nodes == null)
                    parentNode.nodes = new List<TreeNode>();
                parentNode.nodes.Add(node);

                var sub = querys.Where(p => p.FunParentId == f.FunId);
                if (sub.Count() > 0)
                    RecursionGetNode(node, sub, querys);
            }


        }

    }

    public class TreeNode
    {
        public int id { get; set; }

        public string text { get; set; }

        public string icon { get; set; }

        //public string selectedIcon { get; set; }

        //public string color { get; set; }

        public string href { get; set; }
        //public bool selectable { get; set; }

        //public int parentid { get; set; }

        public state state { get; set; }

        public IList<TreeNode> nodes
        {
            get; set;
        }

    }

    public class state
    {
        public bool Checked { get; set; }
        public bool disabled { get; set; }
        public bool expanded { get; set; }
        public bool selected { get; set; }
    }

}