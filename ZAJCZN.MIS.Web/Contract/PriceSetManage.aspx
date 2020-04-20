<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PriceSetManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.PriceSetManage" %>

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
            ShowHeader="false" Title="价格设置">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false"
                    ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:Label ID="lblTitle" runat="server"></f:Label>
                                <f:DropDownList ID="ddlWH" Label="价格套系" LabelAlign="Right" DataTextField="SetDate"
                                    DataValueField="ID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWH_SelectedIndexChanged">
                                </f:DropDownList>
                                <f:DropDownList ID="ddlCostType" Label="物品分类" DataTextField="Name" DataValueField="Id" EnableSimulateTree="true"
                                    DataSimulateTreeLevelField="Level" OnSelectedIndexChanged="ddlCostType_SelectedIndexChanged" AutoPostBack="true"
                                    runat="server" Hidden="true">
                                </f:DropDownList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" ShowBorder="true" ShowHeader="false" Title="物品列表" runat="server" MarginTop="10px"
                    DataKeyNames="ID" EnableCheckBoxSelect="false" AllowPaging="false" PageSize="10"
                    IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange"
                    AllowSorting="true" OnSort="Grid1_Sort" SortField="ID" SortDirection="DESC"
                    EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit" AllowCellEditing="true" ClicksToEdit="1">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server" Position="Top">
                            <Items>
                                <f:Button ID="btnNew" Text="新增套系" runat="server" Icon="SystemNew">
                                </f:Button>
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
                        <f:TemplateField Width="120px" HeaderText="分类名称">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Width="140px" Text='<%# GetType(Eval("GoodsTypeID").ToString()+","+Eval("EquipmentID").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:BoundField DataField="EquipmentCode" SortField="EquipmentCode" Width="80px" HeaderText="物品编码" />
                        <f:BoundField DataField="EquipmentName" SortField="EquipmentName" Width="160px" HeaderText="物品名称" />
                        <f:BoundField DataField="Standard" SortField="Standard" Width="100px" HeaderText="规格" />
                        <f:TemplateField Width="80px" HeaderText="单位">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="80px" Text='<%# GetUnitName(Eval("EquipmentUnit").ToString()) %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:RenderField Width="90px" ColumnID="DailyRents" DataField="DailyRents" FieldType="Double"
                            HeaderText="日租金(元)">
                            <Editor>
                                <f:NumberBox ID="tbxDailyRents" NoDecimal="false" NoNegative="true" MinValue="0.001" DecimalPrecision="3"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="90px" ColumnID="UnitPrice" DataField="UnitPrice" FieldType="Double"
                            HeaderText="单价(元)">
                            <Editor>
                                <f:NumberBox ID="nbxUnitPrice" NoDecimal="false" NoNegative="true" MinValue="0.001" DecimalPrecision="3"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="90px" ColumnID="FixPrice" DataField="FixPrice" FieldType="Double"
                            HeaderText="维修金(元)">
                            <Editor>
                                <f:NumberBox ID="nbxFixPrice" NoDecimal="false" NoNegative="true" MinValue="0.001" DecimalPrecision="3"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="90px" ColumnID="MinRentingDays" DataField="MinRentingDays" FieldType="int"
                            HeaderText="起租天数" Hidden="true">
                            <Editor>
                                <f:NumberBox ID="nbxMinRentingDays" NoDecimal="true" NoNegative="true" MinValue="1"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="120px" ColumnID="CustomerUnit" DataField="CustomerUnit" FieldType="Double"
                            HeaderText="吨位换算(客户)">
                            <Editor>
                                <f:NumberBox ID="nbxCustomerUnit" NoNegative="true" MinValue="1" NoDecimal="false" DecimalPrecision="2"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="120px" ColumnID="DriverUnit" DataField="DriverUnit" FieldType="Double"
                            HeaderText="吨位换算(司机)">
                            <Editor>
                                <f:NumberBox ID="nbxDriverUnit" NoNegative="true" MinValue="1" NoDecimal="false" DecimalPrecision="2"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="120px" ColumnID="StaffUnit" DataField="StaffUnit" FieldType="Double"
                            HeaderText="吨位换算(员工)">
                            <Editor>
                                <f:NumberBox ID="nbxStaffUnit" NoNegative="true" MinValue="1" NoDecimal="false" DecimalPrecision="2"
                                    MaxValue="5000" runat="server">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="400px" Height="200px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
