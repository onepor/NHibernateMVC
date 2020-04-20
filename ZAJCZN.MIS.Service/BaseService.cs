using NHibernate.Criterion;
using System.Collections.Generic;
using ZAJCZN.MIS.Domain;

namespace ZAJCZN.MIS.Service
{
    public interface BaseService<T> where T : BaseEntity<T>, new()
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="enttity"></param>
        void Create(T enttity);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="sqlWhere">条件</param>
        void DelelteAll(string sqlWhere);
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="entity"></param>
        T GetEntity(int id);
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="entity"></param>
        IList<T> Query(IList<ICriterion> queryConditions);
        /// <summary>
        /// 得到全部实体
        /// </summary>
        /// <returns></returns>
        IList<T> GetAll();

        /// <summary>
        /// 查询排序分页获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param>
        /// <param name="orderList">排序</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pagesize">每页行数</param>
        /// <param name="count">总记录数</param>
        /// <returns></returns>
        IList<T> GetPaged(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pagesize, out int count);

        /// <summary>
        /// 查询排序获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param>
        /// <param name="orderList">排序</param> 
        /// <returns></returns>
        IList<T> GetAllByKeys(IList<ICriterion> queryConditions, IList<Order> orderList);

        /// <summary>
        /// 查询排序获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param> 
        /// <returns></returns>
        IList<T> GetAllByKeys(IList<ICriterion> queryConditions);

        /// <summary>
        /// 根据非ID字段查询实体
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        T GetEntityByFields(IList<ICriterion> queryConditions);

        /// <summary>
        /// 根据非ID字段查询实体，获取第一条记录
        /// </summary>
        /// <param name="entity"></param>
        T GetFirstEntityByFields(IList<ICriterion> queryConditions, IList<Order> orderList);
        /// <summary>
        /// 查询获取数据记录数
        /// </summary>
        /// <param name="queryConditions">查询条件</param> 
        /// <returns></returns>
        int GetRecordCountByFields(IList<ICriterion> queryConditions);

    }
}
