using FineUIPro;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZAJCZN.MIS.Domain;
using ZAJCZN.MIS.Service;
using System.Text;
using System.Data;
using ZAJCZN.MIS.Helpers;

namespace ZAJCZN.MIS.Web
{
    public partial class ContractDoorEdit : PageBase
    {
        #region ViewPower

        /// <summary>
        /// 本页面的浏览权限，空字符串表示本页面不受权限控制
        /// </summary>
        public override string ViewPower
        {
            get
            {
                return "CoreContractDoor";
            }
        }

        #endregion

        #region Page_Load

        private int DoorID
        {
            get { return GetQueryIntValue("id"); }
        }
        private int ContractID
        {
            get { return ViewState["ContractID"] != null ? int.Parse(ViewState["ContractID"].ToString()) : 0; }
            set { ViewState["ContractID"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnClose.OnClientClick = ActiveWindow.GetHideReference();
                CheckPowerWithButton("CoreContractDoor", btnSaveClose);

                //绑定物品类别
                BindCostType();
                //绑定厂家
                BindSupplier();
                //绑定物品
                BindGoods();
                //绑定锁
                BindLock();
                //绑定门明细
                BindDoorInfo();
                txtAddress.Focus();
            }
        }

        #region 绑定商品类型

        /// <summary>
        /// 绑定物品类别
        /// </summary>
        private void BindCostType()
        {
            List<EquipmentTypeInfo> myList = new List<EquipmentTypeInfo>();
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentTypeInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentTypeInfo>().GetAllByKeys(qryList, orderList);

            //foreach (EquipmentTypeInfo obj in list)
            //{
            //    myList.Add(new EquipmentTypeInfo { ID = obj.ID, TypeName = string.Format("{0}-{1}", obj.TypeClass, obj.TypeName) });
            //}

            ddlGoodType.DataSource = list;
            ddlGoodType.DataBind();
            ddlGoodType.SelectedIndex = 0;
        }

        private void BindGoods()
        {
            nbWide.Text = "0";
            nbHeight.Text = "0";
            nbCount.Text = "0";

            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("EquipmentTypeID", int.Parse(ddlGoodType.SelectedValue)));
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<EquipmentInfo> list = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetAllByKeys(qryList, orderList);

            ddlGood.DataSource = list;
            ddlGood.DataBind();
            ddlGood.SelectedIndex = 0;

            if (!string.IsNullOrEmpty(ddlGood.SelectedValue))
            {
                EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(int.Parse(ddlGood.SelectedValue));
                if (equipmentInfo != null)
                {
                    //nbWide.Text = equipmentInfo.EWide.ToString();
                    //nbHeight.Text = equipmentInfo.EHeight.ToString();
                    //nbCount.Text = equipmentInfo.EThickness.ToString();
                }
                nbWide.Focus();
            }
        }

        /// <summary>
        /// 绑定锁
        /// </summary>
        private void BindLock()
        {
            string sql = @"select ID,PartsName from PartsInfo where PartsTypeID in (select ID from PartsTypeInfo where IsDoorDefault=1)";
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                ddlParts.DataSource = ds.Tables[0];
                ddlParts.DataBind();

                FineUIPro.ListItem item = new FineUIPro.ListItem();
                item.Text = "--请选择--";
                item.Value = "0";
                ddlParts.Items.Insert(0, item);

                ddlParts.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 绑定厂家
        /// </summary>
        private void BindSupplier()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("IsUsed", "1"));
            Order[] orderList = new Order[1];
            Order orderli = new Order("ID", true);
            orderList[0] = orderli;
            IList<SupplierInfo> list = Core.Container.Instance.Resolve<IServiceSupplierInfo>().GetAllByKeys(qryList, orderList);

            ddlSupply.DataSource = list;
            ddlSupply.DataBind();
            ddlSupply.SelectedIndex = 0;
        }

        #endregion

        public void BindDoorInfo()
        {
            if (DoorID > 0)
            {
                ContractDoorInfo doorInfo = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetEntity(DoorID);

                if (doorInfo == null)
                {
                    // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                    Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                    return;
                }
                //合同基本信息
                txtAddress.Text = doorInfo.GoodsLocation;
                taRemark.Text = doorInfo.Remark;
                nbOtherAmount.Text = doorInfo.OtherAmount.ToString();
                ddlGoodType.SelectedValue = doorInfo.TypeClass.ToString();
                //绑定物品
                BindGoods();
                ddlGood.SelectedValue = doorInfo.GoodsID.ToString();
                tbColor.Text = doorInfo.DoorColor;
                ddlSupply.SelectedValue = doorInfo.SupplyID.ToString();
                ddlParts.SelectedValue = doorInfo.LockID.ToString();
                nbHeight.Text = doorInfo.GHeight.ToString();
                nbWide.Text = doorInfo.GWide.ToString();
                nbCount.Text = doorInfo.GThickness.ToString();
                tbLine.Text = doorInfo.LineName;
                tbGlass.Text = doorInfo.GlassRemark;
                tbModel.Text = doorInfo.ModelName;
                ddlDirection.SelectedValue = doorInfo.DoorDirection;
                ContractID = doorInfo.ContractInfo.ID;
            }
        }

        #endregion

        #region 录入信息保存

        private void SaveItem()
        {
            ContractDoorInfo contractDoorInfo = Core.Container.Instance.Resolve<IServiceContractDoorInfo>().GetEntity(DoorID);
            contractDoorInfo.GoodsLocation = txtAddress.Text;
            contractDoorInfo.Remark = taRemark.Text;
            contractDoorInfo.OtherAmount = decimal.Parse(nbOtherAmount.Text);
            //商品类型及名称
            contractDoorInfo.TypeClass = int.Parse(ddlGoodType.SelectedValue);
            contractDoorInfo.TypeName = ddlGoodType.SelectedText;
            contractDoorInfo.GoodsID = int.Parse(ddlGood.SelectedValue);
            contractDoorInfo.GoodsName = ddlGood.SelectedText;
            contractDoorInfo.GStandardPrice = 0;
            contractDoorInfo.OrderNumber = 1;
            contractDoorInfo.DoorColor = tbColor.Text;
            contractDoorInfo.LineName = tbLine.Text;
            contractDoorInfo.GlassRemark = tbGlass.Text;
            contractDoorInfo.SupplyID = int.Parse(ddlSupply.SelectedValue);
            contractDoorInfo.ModelName = tbModel.Text;
            contractDoorInfo.DoorDirection = ddlDirection.SelectedValue;
            //锁具
            contractDoorInfo.HardWareAmount = 0;
            contractDoorInfo.LockID = 0;
            if (!ddlParts.SelectedValue.Equals("0"))
            {
                contractDoorInfo.LockID = int.Parse(ddlParts.SelectedValue);
                PartsInfo partsInfo = Core.Container.Instance.Resolve<IServicePartsInfo>().GetEntity(contractDoorInfo.LockID);
                //获取门收费五金总金额
                decimal hwNumber = 0;
                string sqlHW = string.Format(@"select isnull(sum(GoodAmount),0) as GoodAmount from ContractHandWareDetail where ContractID ={0} and IsFree=1 and OrderType=1 ", ContractID);
                DataSet dsHW = DbHelperSQL.Query(sqlHW);
                if (dsHW.Tables[0] != null)
                {
                    hwNumber = decimal.Parse(dsHW.Tables[0].Rows[0]["GoodAmount"].ToString());
                }
                //更新门五金总价
                contractDoorInfo.HardWareAmount = hwNumber + (partsInfo != null ? partsInfo.UnitPrice : 0);
            }
            //尺寸
            contractDoorInfo.GHeight = int.Parse(nbHeight.Text);
            contractDoorInfo.GWide = int.Parse(nbWide.Text);
            contractDoorInfo.GThickness = int.Parse(nbCount.Text);
            /*-------------------根据基本录入尺寸信息计算超标和费用---------------------*/
            //计算面积  
            contractDoorInfo.GArea = ((decimal)contractDoorInfo.GHeight / 1000) * ((decimal)contractDoorInfo.GWide / 1000);
            //计算尺寸超标和金额          
            EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(contractDoorInfo.GoodsID);
            if (equipmentInfo != null)
            {
                contractDoorInfo.InstallCost = equipmentInfo.InstallCost;
                contractDoorInfo.GStandardPrice = equipmentInfo.UnitPrice;
                contractDoorInfo.PassPriceAmount = 0;
                contractDoorInfo.GPrice = equipmentInfo.UnitPrice;
                contractDoorInfo.GPassHeightAmount = 0;
                contractDoorInfo.GPassWideAmount = 0;
                contractDoorInfo.GPassThicknessAmount = 0;
                contractDoorInfo.GPassAreaAmount = 0;
                // 商品单位 1:樘  2：米  3：平方米
                contractDoorInfo.GoodUnit = equipmentInfo.EquipmentUnit == "1" ? "樘" : equipmentInfo.EquipmentUnit == "2" ? "米" : "平方米";
                //计算尺寸面积超标
                contractDoorInfo.GPassHeight = contractDoorInfo.GHeight > equipmentInfo.EHeight ? contractDoorInfo.GHeight - equipmentInfo.EHeight : 0;
                contractDoorInfo.GPassWide = contractDoorInfo.GWide > equipmentInfo.EWide ? contractDoorInfo.GWide - equipmentInfo.EWide : 0;
                contractDoorInfo.GPassThickness = contractDoorInfo.GThickness > equipmentInfo.EThickness ? contractDoorInfo.GThickness - equipmentInfo.EThickness : 0;
                decimal standerArea = ((decimal)equipmentInfo.EHeight / 1000) * ((decimal)equipmentInfo.EWide / 1000);
                contractDoorInfo.GPassArea = contractDoorInfo.GArea > standerArea ? contractDoorInfo.GArea - standerArea : 0;
            }
            //计算各项超标金额 【超标计算 1：按公分计算 2:单价加价  3：面积加价】
            switch (equipmentInfo.PassCalcType)
            {
                case 1:     //按公分计算
                    contractDoorInfo.GPassHeightAmount = contractDoorInfo.GPassHeight / 10 * equipmentInfo.PassHeight;
                    contractDoorInfo.GPassWideAmount = contractDoorInfo.GPassWide / 10 * equipmentInfo.PassWide;
                    contractDoorInfo.GPassThicknessAmount = contractDoorInfo.GPassThickness / 10 * equipmentInfo.PassThckness;
                    break;
                case 2:     //单价加价
                    contractDoorInfo.GPassHeightAmount = contractDoorInfo.GPassHeight > 0 ? equipmentInfo.PassHeight : 0;
                    contractDoorInfo.GPassWideAmount = contractDoorInfo.GPassWide > 0 ? equipmentInfo.PassWide : 0;
                    contractDoorInfo.GPassThicknessAmount = contractDoorInfo.GPassThickness > 0 ? equipmentInfo.PassThckness : 0;
                    //计算单价加价
                    contractDoorInfo.PassPriceAmount = contractDoorInfo.GPassHeightAmount + contractDoorInfo.GPassWideAmount + contractDoorInfo.GPassThicknessAmount;
                    contractDoorInfo.GPrice = contractDoorInfo.GStandardPrice + contractDoorInfo.PassPriceAmount;
                    break;
                case 3:     //面积加价
                    contractDoorInfo.GPassAreaAmount = contractDoorInfo.GPassArea * equipmentInfo.PassArea;
                    break;
            }
            //计算商品数量 1：单位数量 2:三方周长  3：四方周长 4：面积 
            switch (equipmentInfo.CalcUnitType)
            {
                case 1:     //单位数量
                    contractDoorInfo.OrderNumber = 1;
                    break;
                case 2:     //三方周长
                    contractDoorInfo.OrderNumber = ((decimal)contractDoorInfo.GHeight * 2 + (decimal)contractDoorInfo.GWide) / 1000;
                    break;
                case 3:     //四方周长
                    contractDoorInfo.OrderNumber = ((decimal)contractDoorInfo.GHeight * 2 + (decimal)contractDoorInfo.GWide * 2) / 1000;
                    break;
                case 4:     //面积
                    contractDoorInfo.OrderNumber = contractDoorInfo.GArea;
                    break;
            }
            //计算商品总价
            contractDoorInfo.PassAmount = contractDoorInfo.GPassHeightAmount
                                           + contractDoorInfo.GPassWideAmount
                                           + contractDoorInfo.GPassThicknessAmount
                                           + contractDoorInfo.GPassAreaAmount;
            contractDoorInfo.GoodsAmount = contractDoorInfo.GPrice * contractDoorInfo.OrderNumber;
            contractDoorInfo.OrderAmount = contractDoorInfo.GoodsAmount
                                            + contractDoorInfo.PassAmount
                                            + contractDoorInfo.OtherAmount
                                            + contractDoorInfo.InstallCost
                                            + contractDoorInfo.HardWareAmount;
            //保存商品信息
            Core.Container.Instance.Resolve<IServiceContractDoorInfo>().Update(contractDoorInfo);
            //更新合同门总金额及合同总金额信息
            UpdateTotalAmount(contractDoorInfo.ContractInfo.ID, contractDoorInfo.HardWareAmount);
            //保存厂家成本信息
            CreateCostInfo();
            //清除多余厂家信息
            CheckCostInfo();
        }

        /// <summary>
        /// 更新合同门总金额及合同总金额信息
        /// </summary>
        public void UpdateTotalAmount(int contractID, decimal hwNumber)
        {
            ContractInfo contractInfo = Core.Container.Instance.Resolve<IServiceContractInfo>().GetEntity(contractID);
            //更新订单总金额
            decimal payNumber = 0;
            string sql = string.Format(@"select isnull(sum(OrderAmount),0) as OrderAmount from ContractDoorInfo where ContractID ={0}  ", ContractID);
            DataSet ds = DbHelperSQL.Query(sql);
            if (ds.Tables[0] != null)
            {
                payNumber = decimal.Parse(ds.Tables[0].Rows[0]["OrderAmount"].ToString());
            }
            ////获取柜子收费五金总金额
            //decimal hwNumber = 0;
            //string sqlHW = string.Format(@"select isnull(sum(GoodAmount),0) as GoodAmount from ContractHandWareDetail where ContractID ={0} and IsFree=1 and OrderType=1 ", DoorID);
            //DataSet dsHW = DbHelperSQL.Query(sqlHW);
            //if (dsHW.Tables[0] != null)
            //{
            //    hwNumber = decimal.Parse(dsHW.Tables[0].Rows[0]["GoodAmount"].ToString());
            //}
            contractInfo.DoorAmount = payNumber + hwNumber;
            contractInfo.TotalAmount = contractInfo.CabinetAmount + contractInfo.DoorAmount;
            Core.Container.Instance.Resolve<IServiceContractInfo>().Update(contractInfo);
        }

        /// <summary>
        /// 保存厂家成本信息
        /// </summary>
        public void CreateCostInfo()
        {
            IList<ICriterion> qryList = new List<ICriterion>();
            qryList.Add(Expression.Eq("ContractID", ContractID));
            qryList.Add(Expression.Eq("SuppilerID", int.Parse(ddlSupply.SelectedValue)));
            qryList.Add(Expression.Eq("CostType", 1));
            ContractCostInfo objInfo = Core.Container.Instance.Resolve<IServiceContractCostInfo>().GetEntityByFields(qryList);
            if (objInfo == null)
            {
                objInfo = new ContractCostInfo();
                objInfo.ContractID = ContractID;
                objInfo.CostAmount = 0;
                objInfo.CostType = 1;
                objInfo.SuppilerID = int.Parse(ddlSupply.SelectedValue);
                objInfo.SuppilerName = ddlSupply.SelectedText;
                objInfo.PayAmount = 0;
                objInfo.SuppilerState = 1;
                objInfo.ProduceState = 1;
                objInfo.SendingState = 0;
                objInfo.ProduceRemark = "下单";
                Core.Container.Instance.Resolve<IServiceContractCostInfo>().Create(objInfo);
            }
        }

        /// <summary>
        /// 清除多余厂家信息
        /// </summary>
        public void CheckCostInfo()
        {
            string sql = string.Format(@"delete from ContractCostInfo where ContractID ={0} and CostType=1 and SuppilerID not in
(select SupplyID from ContractDoorInfo where ContractID ={0})", ContractID);
            DbHelperSQL.ExecuteSql(sql);
        }

        #endregion 录入信息保存

        #region Events

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void ddlGoodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //绑定物品
            BindGoods();
        }

        protected void ddlGood_SelectedIndexChanged(object sender, EventArgs e)
        {
            EquipmentInfo equipmentInfo = Core.Container.Instance.Resolve<IServiceEquipmentInfo>().GetEntity(int.Parse(ddlGood.SelectedValue));
            if (equipmentInfo != null)
            {
                //nbWide.Text = equipmentInfo.EWide.ToString();
                //nbHeight.Text = equipmentInfo.EHeight.ToString();
                //nbCount.Text = equipmentInfo.EThickness.ToString();
            }
            nbWide.Focus();
        }

        #endregion

    }
}