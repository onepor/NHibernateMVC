using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Manager;
using ZAJCZN.MIS.Service;

namespace ZAJCZN.MIS.Component
{
    public class BaseComponent<T, M> : BaseService<T>
        where T : BaseEntity<T>, new()
        where M : BaseManager<T>, new()
    {
        protected M manager = (M)typeof(M).GetConstructor(Type.EmptyTypes).Invoke(null);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        public void Create(T entity)
        {
            manager.Create(entity);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(T entity)
        {
            manager.Delelte(entity);
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="sqlWhere">条件</param>
        public void DelelteAll(string sqlWhere)
        {
            manager.DelelteAll(sqlWhere);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id)
        {
            manager.Delelte(id);
        } 

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        public void Update(T entity)
        {
            manager.Update(entity);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="entity"></param>
        public T GetEntity(int id)
        {
            return manager.GetEntity(id);
        } 

        public IList<T> Query(IList<ICriterion> queryConditions)
        {
            return manager.Query(queryConditions);
        }

        /// <summary>
        /// 得到全部实体
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAll()
        {
            return manager.GetAll();
        }

        /// <summary>
        /// 查询排序分页获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param>
        /// <param name="orderList">排序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pagesize">每页行数</param>
        /// <param name="count">总记录数</param>
        /// <returns></returns>
        public IList<T> GetPaged(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pagesize, out int count)
        {
            return manager.GetPaged(queryConditions, orderList, pageIndex, pagesize, out count);
        }

        /// <summary>
        /// 查询排序获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param>
        /// <param name="orderList">排序</param> 
        /// <returns></returns>
        public IList<T> GetAllByKeys(IList<ICriterion> queryConditions, IList<Order> orderList)
        {
            return manager.GetAllByKeys(queryConditions, orderList);
        }

        /// <summary>
        /// 查询排序获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param> 
        /// <returns></returns>
        public IList<T> GetAllByKeys(IList<ICriterion> queryConditions)
        {
            return manager.GetAllByKeys(queryConditions);
        }

        /// <summary>
        /// 根据非ID字段查询实体
        /// </summary>
        /// <param name="entity"></param>
        public T GetEntityByFields(IList<ICriterion> queryConditions)
        {
            return manager.GetEntityByFields(queryConditions);
        }
        
        /// <summary>
        /// 查询获取数据记录数
        /// </summary>
        /// <param name="queryConditions">查询条件</param> 
        /// <returns></returns>
        public int GetRecordCountByFields(IList<ICriterion> queryConditions)
        {
            return manager.GetRecordCountByFields(queryConditions);
        }

        /// <summary>
        /// 根据非ID字段查询实体
        /// </summary>
        /// <param name="entity"></param>
        public T GetFirstEntityByFields(IList<ICriterion> queryConditions, IList<Order> orderList)
        {
            return manager.GetFirstEntityByFields(queryConditions, orderList);
        }
    }
}
