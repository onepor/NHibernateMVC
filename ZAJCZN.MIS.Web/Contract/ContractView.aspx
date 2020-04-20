<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractView.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractView" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
    <style>
        .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowBorder="false" ShowHeader="false" Layout="VBox">
            <Items>
                <f:Panel ID="Panel2" ShowHeader="false" ShowBorder="false" Layout="Column" runat="server">
                    <Items>
                        <f:TextBox ID="txtSearch" runat="server" LabelWidth="80px" Label="关键字" LabelAlign="Right"
                            EmptyText="在客户名称、电话号码、订单编号、客户地址中搜索" Width="400px">
                        </f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="订单签订时间" Width="260px"
                            runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:DropDownList ID="ddlState" Label="订单状态" runat="server" LabelWidth="80px" LabelAlign="Right" Width="200px">
                            <f:ListItem Text="全部" Selected="true" Value="" />
                            <f:ListItem Text="登记中" Value="1" />
                            <f:ListItem Text="待测量" Value="2" />
                            <f:ListItem Text="测量完成" Value="3" />
                            <f:ListItem Text="生产中" Value="4" />
                            <f:ListItem Text="生产完成" Value="5" />
                            <f:ListItem Text="送货中" Value="6" />
                            <f:ListItem Text="送货完成" Value="7" />
                            <f:ListItem Text="待安装" Value="8" />
                            <f:ListItem Text="安装完成" Value="9" />
                            <f:ListItem Text="质保中" Value="10" />
                            <f:ListItem Text="售后中" Value="11" />
                        </f:DropDownList>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" OnRowDoubleClick="Grid1_RowDoubleClick" EnableRowDoubleClickEvent="true"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                    EnableSummary="true" SummaryPosition="Bottom" AllowCellEditing="true" ClicksToEdit="1"
                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit">
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:LinkButtonField Hidden="true" HeaderText="详情" TextAlign="Center" Icon="Zoom" ToolTip="查看订单信息" CommandName="editField" Width="50px" ColumnID="lbtnEditField" />
                        <f:TemplateField Width="90px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="80px" Text='<%# GetOrderState(Eval("ContractState").ToString()) %>'
                                    ForeColor='<%# GetColor(Eval("ContractState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractNO" HeaderText="订单号" Width="160px" SortField="ContractNO" />
                        <f:GroupField HeaderText="客户信息" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="ProjectName" HeaderText="客户地址" Width="200px" />
                                <f:BoundField DataField="CustomerName" HeaderText="客户名称" Width="200px" />
                                <f:BoundField DataField="ContactPhone" HeaderText="联系电话" Width="180px" />
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="订单信息" TextAlign="Center">
                            <Columns>
                                <f:TemplateField Width="80px" HeaderText="销售人员">
                                    <ItemTemplate>
                                        <asp:Label ID="Label4" runat="server" Width="80px" Text='<%# GetPerson(Eval("SalePerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="ContractDate" HeaderText="登记时间" Width="100px" />
                                <f:BoundField DataField="MeasureDate" HeaderText="测量时间" Width="100px" />
                                <f:TemplateField Width="80px" HeaderText="测量人员">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="80px" Text='<%# GetPerson(Eval("MeasurePerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="SendDate" HeaderText="送货时间" Width="100px" />
                                <f:TemplateField Width="80px" HeaderText="送货人员">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetPerson(Eval("SendPerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="InstalDate" HeaderText="安装时间" Width="100px" />
                                <f:TemplateField Width="80px" HeaderText="安装人员">
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Width="80px" Text='<%# GetPerson(Eval("InstallPerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="订单金额(元)" TextAlign="Center">
                            <Columns>
                                <f:RenderField Width="100px" ColumnID="DoorAmount" DataField="DoorAmount" FieldType="Double"
                                    HeaderText="门总价">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox5" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="CabinetAmount" DataField="CabinetAmount" FieldType="Double"
                                    HeaderText="柜子总价">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox1" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="DoorAmount" HeaderText="门总价" Width="100px" Hidden="true" />
                                <f:BoundField DataField="CabinetAmount" HeaderText="柜子总价" Width="100px" Hidden="true" />
                                <f:BoundField DataField="TotalAmount" ColumnID="TotalAmount" HeaderText="总金额" Width="100px" />
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="利润分析(元)" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="TotalAmount" HeaderText="总金额" Width="100px" />
                                <f:BoundField DataField="CabinetCost" HeaderText="柜子成本" Width="100px" Hidden="true" />
                                <f:BoundField DataField="DoorCost" HeaderText="门成本" Width="100px" Hidden="true" />
                                <f:BoundField DataField="SendCost" HeaderText="安装费用" Width="100px" Hidden="true" />
                                <f:BoundField DataField="HandWareCost" HeaderText="五金锁具" Width="100px" Hidden="true" />
                                <f:BoundField DataField="AfterSaleCost" HeaderText="售后成本" Width="100px" Hidden="true" />

                                <f:RenderField Width="100px" ColumnID="CabinetCost" DataField="CabinetCost" FieldType="Double"
                                    HeaderText="柜子成本">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox2" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="DoorCost" DataField="DoorCost" FieldType="Double"
                                    HeaderText="门成本">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox3" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="SendCost" DataField="SendCost" FieldType="Double"
                                    HeaderText="安装费用">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox4" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="HandWareCost" DataField="HandWareCost" FieldType="Double"
                                    HeaderText="五金锁具">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox6" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="AfterSaleCost" DataField="AfterSaleCost" FieldType="Double"
                                    HeaderText="售后成本">
                                    <Editor>
                                        <f:NumberBox ID="AfterSaleCost" NoDecimal="false" NoNegative="false" MinValue="0"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:TemplateField Width="100px" HeaderText="利润" ColumnID="ProfitMoney">
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Width="100px" Text='<%# GetProfitMoney(Eval("ID").ToString()) %>' ForeColor="Red" Font-Bold="true"> </asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="ProfitMoney" HeaderText="利润" Width="100px" Hidden="true" />
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="收\付款" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="ReturnMoney" ColumnID="ReturnMoney" HeaderText="已收金额" Width="100px" />
                                <f:BoundField DataField="PayCostMoney" ColumnID="PayCostMoney" HeaderText="已付金额" Width="100px" />
                            </Columns>
                        </f:GroupField>
                        <f:BoundField DataField="Remark" HeaderText="备注" Width="300px" />
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="600px" Height="400px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
