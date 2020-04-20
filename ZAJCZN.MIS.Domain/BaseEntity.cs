using Castle.ActiveRecord;
using System;

namespace ZAJCZN.MIS.Domain
{ 
    public class BaseEntity<T> : ActiveRecordBase
         where T : class
    {
        [PrimaryKey]
        public int ID { get; set; }
    }
}
