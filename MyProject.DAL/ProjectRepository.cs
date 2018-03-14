using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyProject.DAL.Models;
using Dapper;

namespace MyProject.DAL
{
    public class ProjectRepository : Repository<Models.ProjectModel>
    {

        public override void Add(ProjectModel model)
        {
            string sql = @"insert into uproject
                              (
                                ProjectId
                                ,ProjectName
                                ,Description
                                ,Priority
                                ,DifficultLevel
                                ,RequestBy
                                ,Users
                                ,status
                                ,EstimateFinishDate
                                ,ActualFinishDate
                                ,WorkTeam
                                ,Remark
                                ,CreationDate
                                ,LastUpdateBy
                                ,LCV
                              )
                             values
                              (
                                null
                                ,@ProjectName
                                ,@Description
                                ,@Priority
                                ,@DifficultLevel
                                ,@RequestBy
                                ,@Users
                                ,@status
                                ,@EstimateFinishDate
                                ,@ActualFinishDate
                                ,@WorkTeam
                                ,@Remark
                                ,@CreationDate
                                ,@LastUpdateBy
                                ,@LCV
                              )";
            try
            {
                var r = Connection.Execute(sql, model);
                int id = Connection.ExecuteScalar<int>("SELECT LAST_INSERT_ID() as id ");
                model.ProjectId = id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override void Add(IEnumerable<ProjectModel> models)
        {

        }

        public override void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public override void Delete(ProjectModel model)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<ProjectModel> Query(int Id)
        {
            string sql = "select * from uproject where projectid=@projectid";
            return Connection.Query<ProjectModel>(sql, new { projectid = Id });

        }


        public override IEnumerable<ProjectModel> Query(IDictionary<string, object> dicParams)
        {
            System.Text.StringBuilder sb = new StringBuilder();
            sb.Append(@"select 
                          p.ProjectId
                          ,p.ProjectName
                          ,p.Description
                          ,p.Priority
                          ,p.DifficultLevel
                          ,p.RequestBy
                          ,p.Users
                          ,p.Status
                          ,p.EstimateFinishDate
                          ,p.ActualFinishDate
                          ,p.WorkTeam
                          ,p.Remark
                          ,p.CreationDate
                          ,p.LastUpdateBy
                          ,p.LCV
                          ,c.CodeId
                          ,c.Code
                          ,c.CodeGroup
                        from
                          uproject p inner
                        join ucode c on p.Priority = c.CodeId
                        where 1 = 1 ");
            int kvCount = dicParams.Count;

            DynamicParameters dp = new DynamicParameters();
            foreach (KeyValuePair<string, object> kv in dicParams)
            {
                if (kv.Key.Equals("ProjectName"))
                {
                    sb.Append(string.Format(" and p.{0} like @{1}", kv.Key, kv.Key));
                    dp.Add(kv.Key, "%" + kv.Value + "%");
                }
                else
                {
                    sb.Append(string.Format(" and p.{0}=@{1}", kv.Key, kv.Key));
                    dp.Add(kv.Key, kv.Value);
                }
            }

            return Connection.Query<ProjectModel, CodeModel,ProjectModel>(sb.ToString(),(p,c)=> { p.CodePriority = c; return p; }, dp,splitOn: "ProjectId,CodeId");

           // return Connection.Query<ProjectModel>(sb.ToString(), dp);

        }

        public override void Update(ProjectModel model)
        {
            string sql = @"update uproject
                            set
                                ProjectName=@ProjectName
                                ,Description=@Description
                                ,Priority=@Priority
                                ,DifficultLevel=@DifficultLevel
                                ,RequestBy=@RequestBy
                                ,Users=@Users
                                ,status=@status
                                ,EstimateFinishDate=@EstimateFinishDate
                                ,ActualFinishDate=@ActualFinishDate
                                ,WorkTeam=@WorkTeam
                                ,Remark=@Remark
                                ,CreationDate=@CreationDate
                                ,LastUpdateBy=@LastUpdateBy
                                ,LCV=@LCV
                            where
                                ProjectId=@ProjectId";
            Connection.Execute(sql, model);
        }
    }
}
