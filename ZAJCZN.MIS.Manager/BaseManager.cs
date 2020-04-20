using Castle.ActiveRecord;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ZAJCZN.MIS.Manager
{
    public class BaseManager<T> : ActiveRecordBase<T>
         where T : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体对象</param>
        public new void Create(T entity)
        {
            ActiveRecordBase.Create(entity);
        }
        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <param name="id"></param>
        public void Delelte(int id)
        {
            T entity = GetEntity(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity"></param>
        public void Delelte(T entity)
        {
            ActiveRecordBase.Delete(entity);
        }

        /// <summary>
        /// 根据实体删除
        /// </summary>
        /// <param name="entity"></param>
        public void DelelteAll(string sqlWhere)
        {
            ActiveRecordBase.DeleteAll(typeof(T), sqlWhere);
        }

        /// <summary>
        /// 修改提交
        /// </summary>
        /// <param name="entity"></param>
        public new void Update(T entity)
        {
            ActiveRecordBase.Update(entity);
        }

        /// <summary>
        /// 得到实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity(int id)
        {
            return ActiveRecordBase.FindByPrimaryKey(typeof(T), id, false) as T;
        }
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        public IList<T> Query(IList<ICriterion> queryConditions)
        {
            {
                Array arry = ActiveRecordBase.FindAll(typeof(T), queryConditions.ToArray());
                return arry as IList<T>;
            }
        }

        /// <summary>
        /// 得到全部实体
        /// </summary>
        /// <returns></returns>
        public IList<T> GetAll()
        {
            return ActiveRecordBase.FindAll(typeof(T)) as IList<T>;
        }

        /// <summary>
        /// 查询排序
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <param name="orderList"></param>
        /// <returns></returns>
        public IList<T> GetPaged(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pageSize, out int count)
        {
            //1.符合条件的总记录数
            count = ActiveRecordBase.Count(typeof(T), queryConditions.ToArray());
            //2.符合条件的分页获取对象集
            return ActiveRecordBase.SlicedFindAll(typeof(T), pageIndex * pageSize, pageSize, orderList.ToArray(), queryConditions.ToArray()) as IList<T>;
        }

        /// <summary>
        /// 查询排序获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param>
        /// <param name="orderList">排序</param> 
        /// <returns></returns>
        public IList<T> GetAllByKeys(IList<ICriterion> queryConditions, IList<Order> orderList)
        {
            return ActiveRecordBase.FindAll(typeof(T), orderList.ToArray(), queryConditions.ToArray()) as IList<T>;
        }

        /// <summary>
        /// 查询排序获取数据
        /// </summary>
        /// <param name="queryConditions">查询条件</param> 
        /// <returns></returns>
        public IList<T> GetAllByKeys(IList<ICriterion> queryConditions)
        {
            return ActiveRecordBase.FindAll(typeof(T), queryConditions.ToArray()) as IList<T>;
        }

        /// <summary>
        /// 根据非ID字段查询实体
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        public T GetEntityByFields(IList<ICriterion> queryConditions)
        {
            return ActiveRecordBase.FindOne(typeof(T), queryConditions.ToArray()) as T;
        }

        /// <summary>
        /// 根据非ID字段查询实体
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        public T GetFirstEntityByFields(IList<ICriterion> queryConditions, IList<Order> orderList)
        {
            return ActiveRecordBase.FindFirst(typeof(T), orderList.ToArray(), queryConditions.ToArray()) as T;
        }

        /// <summary>
        /// 查询获取数据记录数
        /// </summary>
        /// <param name="queryConditions">查询条件</param> 
        /// <returns></returns>
        public int GetRecordCountByFields(IList<ICriterion> queryConditions)
        {
            //1.符合条件的总记录数
            int count = ActiveRecordBase.Count(typeof(T), queryConditions.ToArray());
            //2.符合条件的分页获取对象集
            return count;
        }
    }
}
