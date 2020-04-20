<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SortingGoodsManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.SortingGoodsManage" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowBorder="false" ShowHeader="false" Layout="VBox">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items> 
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="建单时间" Width="260px"
                            runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>                        
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="true" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnRowDoubleClick="Grid1_RowDoubleClick" EnableRowDoubleClickEvent="true"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>
                                <f:Button ID="btnNew" runat="server" Icon="BookAdd" EnablePostBack="false" Text="新增维修分拣单">
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>                               
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns>
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑维修分拣单信息"
                            WindowID="Window1" Title="编辑维修分拣单信息" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractEdit.aspx?id={0}"
                            Width="50px" />
                        <f:LinkButtonField HeaderText="订单" TextAlign="Center" Icon="Pictures" ToolTip="查看发货\收货单"
                            CommandName="Basket" Width="50px" />
                        <f:WindowField ColumnID="priceField" HeaderText="报价" TextAlign="Center" Icon="MoneyYen" ToolTip="编辑报价"
                            WindowID="Window3" Title="编辑报价信息" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/PriceSetManage.aspx?id={0}"
                            Width="50px" />
                        <f:WindowField ColumnID="carField" HeaderText="运费" TextAlign="Center" Icon="Car" ToolTip="编辑车辆运费"
                            WindowID="Window2" Title="编辑车辆运费" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/CarPriceSetManage.aspx?id={0}"
                            Width="50px" />
                        <f:TemplateField Width="60px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetIsUsed(Eval("ContractState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractNO" HeaderText="合同号" Width="160px" SortField="ContractNO" />
                        <f:BoundField DataField="CustomerName" HeaderText="客户名称" SortField="CustomerName" Width="200px" />
                        <f:BoundField DataField="CustContractNO" HeaderText="客户合同号" Width="180px" />
                        <f:TemplateField Width="120px" HeaderText="签订公司" SortField="SignCompany">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="120px" Text='<%# GetUnitName(Eval("SignCompany").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="100px" HeaderText="价格表">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# GetPriceSet(Eval("PriceSetID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="70px" HeaderText="含税价">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Width="70px" Text='<%#  Eval("PriceTaxType").ToString().Equals("0")?"否":"是" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ProjectName" HeaderText="项目名称" Width="200px" />
                        <f:BoundField DataField="ContractDate" HeaderText="签订时间" Width="100px" SortField="ContractDate" DataFormatString="{0:yyyy-MM-dd}" />
                        <f:BoundField DataField="FinishDate" HeaderText="结项时间" Width="100px" SortField="FinishDate" />
                        <f:BoundField DataField="TotalAmount" HeaderText="结算金额(元)" Width="120px" />
                        <f:BoundField DataField="ReturnMoney" HeaderText="已回款(元)" Width="100px" />
                        <f:BoundField DataField="WaitingPaymentMoney" HeaderText="待收(元)" Width="90px" />
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="900px" Height="700px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window3" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="1200px" Height="600px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="500px" Height="400px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
