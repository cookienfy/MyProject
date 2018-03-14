using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace MyProject
{
    public static class EntityHelper
    {

        internal static string GetWebKeyValue(this string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetString(this object obj)
        {
            string str = Convert.ToString(obj);
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            else
            {
                return str;
            }
        }

        public static T GetEntity<T>(DataTable table) where T : new()
        {
            T entity = new T();
            foreach (DataRow row in table.Rows)
            {
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (row.Table.Columns.Contains(item.Name))
                    {
                        if (DBNull.Value != row[item.Name])
                        {
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                        }

                    }
                }
            }

            return entity;
        }

        public static IList<T> GetEntities<T>(DataTable table) where T : new()
        {
            IList<T> entities = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T entity = new T();
                foreach (var item in entity.GetType().GetProperties())
                {
                    if (table.Columns.Contains(item.Name))
                    {
                        if (row[item.Name] != DBNull.Value)
                            item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                    }
                }
                entities.Add(entity);
            }
            return entities;
        }


        public static string GetProjectListHtml()
        {
            string strVar = @"<div class='hpanel hgreen'>
                                < div class='panel-body'>
                                    <div class='row'>
                                        <div class='col-sm-8'>
                                            <h4>
                                                <a href = '' >ProjectName </ a >
                                            </ h4 >
                                            < p > It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout.The point of using Lorem Ipsum is that it has..</p>
                                            <div class='row'>
                                                <div class='col-sm-3'>
                                                    <div class='project-label'>CLIENT</div>
                                                    <small>Hendrix Corp</small>
                                                </div>
                                                <div class='col-sm-3'>
                                                    <div class='project-label'>VERSION</div>
                                                    <small>1.5.2</small>
                                                </div>
                                                <div class='col-sm-3'>
                                                    <div class='project-label'>DEDLINE</div>
                                                    <small>12.06.2015</small>
                                                </div>
                                                <div class='col-sm-3'>
                                                    <div class='project-label'>PROGRESS</div>
                                                    <div class='progress m-t-xs full progress-small'>
                                                        <div style = 'width:80%' aria-valuemax='100' aria-valuemin='0' aria-valuenow='12' role='progressbar' class='progress-bar progress-bar-success'></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class='col-sm-4 project-info'>
                                            <div class='project-action m-t-md'>
                                                <div class='btn-group'>
                                                    <button class='btn btn-xs btn-default'>View</button><button class='btn btn-xs btn-default'>Edit</button>
                                                    <button class='btn btn-xs btn-default'>Delete</button>
                                                </div>
                                            </div><div class='project-value'><h2 class='text-success'>$1 206,40</h2></div><div class='project-people'>
                                                <img alt = 'logo' class='img-circle' src='~/Images/a1.jpg'><img alt = 'logo' class='img-circle' src='~/Images/a2.jpg'>
                                                <img alt = 'logo' class='img-circle' src='~/Images/a3.jpg'><img alt = 'logo' class='img-circle' src='~/Images/a4.jpg'>
                                                <img alt = 'logo' class='img-circle' src='~/Images/a5.jpg'><img alt = 'logo' class='img-circle' src='~/Images/a6.jpg'>
                                                <img alt = 'logo' class='img-circle' src='~/Images/a7.jpg'>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class='panel-footer'>Additional information about project in footer</div>
                            </div>";

            return strVar;

        }

    }
}