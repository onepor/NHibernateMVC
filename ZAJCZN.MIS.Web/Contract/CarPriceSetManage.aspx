<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CarPriceSetManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.CarPriceSetManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="车辆运费设置">
            <Items>
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" Title="车辆列表" runat="server"
                    DataKeyNames="ID" EnableCheckBoxSelect="false" AllowPaging="false" PageSize="10" 
                    IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" Height="350px"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit" AllowCellEditing="true" ClicksToEdit="1">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                            <Items>
                                <f:Label ID="lblTitle" runat="server"></f:Label>
                                <f:ToolbarFill ID="fill" runat="server"></f:ToolbarFill>
                                <f:Button ID="btnClose" Text="关闭" runat="server" Icon="SystemClose"
                                    OnClick="btnClose_Click">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" Selected="true" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                    <Columns>
                        <f:BoundField DataField="CarInfo.CarNO" SortField="CarInfo.CarNO" Width="100px" HeaderText="车牌号" ExpandUnusedSpace="true" />
                        <f:RenderField Width="120px" ColumnID="TonPayPrice" DataField="TonPayPrice" FieldType="Double"
                            HeaderText="每吨费用(元)">
                            <Editor>
                                <f:NumberBox ID="tbxDailyRents" NoDecimal="false" NoNegative="true" MinValue="0.001" DecimalPrecision="3"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="120px" ColumnID="CarPayPrice" DataField="CarPayPrice" FieldType="Double"
                            HeaderText="整车费用(元)">
                            <Editor>
                                <f:NumberBox ID="nbxUnitPrice" NoDecimal="false" NoNegative="true" MinValue="0.001" DecimalPrecision="3"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="120px" ColumnID="MinTon" DataField="MinTon" FieldType="Double"
                            HeaderText="保底数量(吨)">
                            <Editor>
                                <f:NumberBox ID="nbxFixPrice" NoDecimal="false" NoNegative="true" MinValue="0.01" DecimalPrecision="3"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
