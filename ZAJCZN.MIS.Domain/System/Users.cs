using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    [ActiveRecord]
    public class users : BaseEntity<users>
    {
        /// <summary>
        /// 用户名称
        /// </summary>
        [Property]
        public string Name { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Property]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Property]
        public string Password { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [Property]
        public bool Enabled { get; set; }

        /// <summary>
        /// 部门性别名称
        /// </summary>
        [Property]
        public string Gender { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        [Property]
        public string ChineseName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        [Property]
        public string EnglishName { get; set; }

        /// <summary>
        /// 照片
        /// </summary>
        [Property]
        public string Photo { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [Property]
        public string QQ { get; set; }

        /// <summary>
        /// 部门邮箱
        /// </summary>
        [Property]
        public string CompanyEmail { get; set; }

        /// <summary>
        /// 公司电话
        /// </summary>
        [Property]
        public string OfficePhone { get; set; }

        /// <summary>
        /// 传真
        /// </summary>
        [Property]
        public string OfficePhoneExt { get; set; }

        /// <summary>
        /// 家庭电话
        /// </summary>
        [Property]
        public string HomePhone { get; set; }

        /// <summary>
        /// 移动电话
        /// </summary>
        [Property]
        public string CellPhone { get; set; }

        /// <summary>
        /// 联系地址
        /// </summary>
        [Property]
        public string Address { get; set; }

        /// <summary>
        /// 部门名备注称
        /// </summary>
        [Property]
        public string Remark { get; set; }

        /// <summary>
        /// 证件号
        /// </summary>
        [Property]
        public string IdentityCard { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        [Property]  
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 离职时间
        /// </summary>
        [Property]  
        public DateTime? TakeOfficeTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Property]  
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Property]  
        public DateTime? CreateTime { get; set; } 
              
        /// <summary>
        /// 部门ID
        /// </summary>
        [Property]
        public int DeptID { get; set; }

        public virtual IList<roles> Roles { get; set; } 

    }
}