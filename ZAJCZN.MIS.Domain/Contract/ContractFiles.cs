using Castle.ActiveRecord;
using System;
using System.Collections.Generic;

namespace ZAJCZN.MIS.Domain
{
    /// <summary>
    /// 项目信息
    /// </summary>
    [ActiveRecord]
    public partial class ContractFiles : BaseEntity<ContractFiles>
    {
        /// <summary>
        /// 合同信息
        /// </summary>
        //多对一，对应ContractInfo的ID属性
        [BelongsTo(Lazy = FetchWhen.OnInvoke, Column = "ContractID")]
        public ContractInfo ContractInfo { get; set; }
         
        /// <summary>
        /// 文件类型  1：门附件  2：柜子附件  3：财务附件  4：其他附件
        /// </summary>
        [Property]
        public int FileType { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Property]
        public string FileName { get; set; }

        /// <summary> 
        /// 显示路径
        /// </summary>
        [Property]
        public string FileShowPath { get; set; }

        /// <summary>
        /// 保存物理路径
        /// </summary>
        [Property]
        public string FileSavePath { get; set; }
         

    }
}

