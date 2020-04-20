using System;
using System.Collections.Generic;
using System.Text;

namespace ZAJCZN.MIS.Helpers
{
    /// <summary>
    /// 缴费单，单据缴费状态
    /// </summary>
    public enum enum_PayBillSuccessState
    {
        未确认 = 0,
        已确认 = 1
    }

    /// <summary>
    /// 从国通返回记录中更新的统计类型
    /// </summary>
    public enum enum_consump_Type
    {
        充值 = 1,
        返现 = 2,
        消费 = 3,
        其他消费 = 4
    }

    #region - 二期新增 -

    /// <summary>
    /// 大用户的状态信息
    /// </summary>
    public enum enum_BigUserState
    {
        未验证 = 0,
        在用 = 1,
        废弃 = -1
    }

    /// <summary>
    /// 请求验证码类型
    /// </summary>
    public enum enum_VerCodeType
    {
        用户注册 = 1,
        用户忘记密码 = 2,
        用户设置交易密码 = 3,
        用户忘记交易密码 = 4
    }

    #endregion

}
