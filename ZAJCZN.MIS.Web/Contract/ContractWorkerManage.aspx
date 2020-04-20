<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractWorkerManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractWorkerManage" %>

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
                        <f:TextBox ID="txtSearch" runat="server" LabelWidth="80px" Label="关键字" LabelAlign="Right"
                            EmptyText="在客户名称、电话号码、订单编号、项目地址中搜索" Width="500px">
                        </f:TextBox>
                        <f:DatePicker ID="dpStartDate" LabelWidth="120px" Label="订单登记时间" Width="260px"
                            runat="server" LabelAlign="Right">
                        </f:DatePicker>
                        <f:DatePicker ID="dpEndDate" LabelWidth="10px" Label="~" Width="150px" runat="server"
                            CompareControl="dpStartDate" CompareOperator="GreaterThan" CompareMessage="结束日期应该大于开始日期！">
                        </f:DatePicker>
                        <f:Button ID="btnSearch" MarginLeft="20px" runat="server" OnClick="btnSearch_Click" Icon="Find" Text="查询"></f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false"
                    EnableCheckBoxSelect="false" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound"
                    OnRowCommand="Grid1_RowCommand" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange">
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
                        <f:WindowField ColumnID="editField" HeaderText="编辑" TextAlign="Center" Icon="Pencil" ToolTip="编辑订单信息"
                            WindowID="Window2" Title="编辑订单信息" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractEdit.aspx?id={0}&type=2"
                            Width="50px" />
                        <f:LinkButtonField HeaderText="门设计" TextAlign="Center" Icon="BuildingEdit" ToolTip="编辑门设计报价信息" CommandName="editFieldDoor" Width="70px" ColumnID="lbtnEditDoorField" />
                        <f:LinkButtonField HeaderText="柜子设计" TextAlign="Center" Icon="BookmarkEdit" ToolTip="编辑柜体设计报价信息" CommandName="editFieldCabinet" Width="80px" ColumnID="lbtnEditCabinetField" />
                        <f:WindowField ColumnID="wfSupplierField" HeaderText="生产跟踪" TextAlign="Center" Icon="UserEdit" ToolTip="生产厂商信息"
                            WindowID="Window3" Title="厂商跟踪" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/Contract/ContractTrackManage.aspx?id={0}"
                            Width="80px" />
                        <f:LinkButtonField HeaderText="收\付款" TextAlign="Center" Icon="MoneyYen" ToolTip="订单收款付款信息" CommandName="editFieldMoney" Width="70px" ColumnID="lbtnEditMoneyField" />
                        <f:TemplateField Width="70px" HeaderText="状态">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Width="60px" Text='<%# GetOrderState(Eval("ContractState").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:TemplateField Width="60px" HeaderText="加急">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="60px" Text='<%# Eval("IsUrgent").ToString() =="1"?"加急":"" %>' ForeColor="Red"></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="ContractNO" HeaderText="订单号" Width="160px" SortField="ContractNO" />
                        <f:GroupField HeaderText="客户信息" TextAlign="Center">
                            <Columns>
                                <f:BoundField DataField="ProjectName" HeaderText="项目地址" Width="200px" />
                                <f:BoundField DataField="CustomerName" HeaderText="客户名称" Width="160px" />
                                <f:BoundField DataField="ContactPhone" HeaderText="联系电话" Width="160px" />
                            </Columns>
                        </f:GroupField>
                        <f:GroupField HeaderText="订单内容" TextAlign="Center">
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
                                        <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetPerson(Eval("MeasurePerson").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:BoundField DataField="PerSendDate" HeaderText="预约送货日期" Width="130px" />
                                <f:BoundField DataField="PerInstalDate" HeaderText="预约安装日期" Width="130px" />
                                <f:BoundField DataField="DoorAmount" ColumnID="DoorAmount" HeaderText="门总金额(元)" Width="130px" />
                                <f:BoundField DataField="CabinetAmount" ColumnID="CabinetAmount" HeaderText="柜子总金额(元)" Width="130px" />
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
        <f:Window ID="Window2" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="600px" Height="480px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window3" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="800px" Height="680px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
