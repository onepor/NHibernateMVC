<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SetMealInfoEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.SetMealInfoEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px"
            ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start"
            ShowHeader="false" Title="餐区管理">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="Button1" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click"
                            runat="server" Text="保存后关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:GroupPanel runat="server" Title="套餐信息">
                    <Items>
                        <f:Form ShowBorder="false" ShowHeader="false" runat="server">
                            <Items>
                                <f:FormRow>
                                    <Items>
                                        <f:TextBox ShowRedStar="true" Required="true" ID="txbSetMealName" Label="套餐名称" runat="server"></f:TextBox>
                                        <f:Label ID="labPrice" Label="菜品原价" runat="server"></f:Label>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow>
                                    <Items>
                                        <f:NumberBox ShowRedStar="true" Required="true" ID="numPreferentialPrice" Label="享套餐价" runat="server"></f:NumberBox>
                                        <f:RadioButtonList ID="rblEnabled" Label="是否启用" runat="server">
                                            <f:RadioItem Text="是" Value="1" Selected="true" />
                                            <f:RadioItem Text="否" Value="2" />
                                        </f:RadioButtonList>
                                    </Items>
                                </f:FormRow>
                                <f:FormRow Hidden="true">
                                    <Items>
                                        <f:DatePicker ShowRedStar="true" Required="true" ID="dateStart" Label="开始时间" runat="server"></f:DatePicker>
                                        <f:DatePicker ShowRedStar="true" Required="true" ID="dateFinish" Label="结束时间" runat="server"></f:DatePicker>
                                    </Items>
                                </f:FormRow>
                            </Items>
                        </f:Form>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel runat="server" Title="套餐所含菜品">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false"
                            EnableCheckBoxSelect="false" EnableCollapse="true" AllowCellEditing="true" ClicksToEdit="2"
                            DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortField="ID"
                            SortDirection="ASC" AllowPaging="false" IsDatabasePaging="false" Height="360px"
                            OnPageIndexChange="Grid1_PageIndexChange" EnableAfterEditEvent="true" OnAfterEdit="Grid1_AfterEdit"
                            OnRowCommand="Grid1_RowCommand">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="添加菜品">
                                        </f:Button>
                                        <f:ToolbarFill runat="server"></f:ToolbarFill>
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
                                    <f:ListItem Text="10" Value="10" />
                                    <f:ListItem Text="20" Value="20" />
                                    <f:ListItem Text="50" Value="50" />
                                    <f:ListItem Text="100" Value="100" />
                                </f:DropDownList>
                            </PageItems>
                            <Columns>
                                <f:RowNumberField TextAlign="Center" Width="35px" EnablePagingNumber="true" />
                                <f:TemplateField Width="200px" HeaderText="菜品名称" ExpandUnusedSpace="true">
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text='<%# GetDishesNmae(Eval("DishID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:RenderField Width="100px" ColumnID="DishCount" DataField="DishCount" FieldType="Int"
                                    HeaderText="菜品数量">
                                    <Editor>
                                        <f:NumberBox ID="numDishesCount" NoDecimal="true" NoNegative="true" MinValue="1"
                                            MaxValue="5000" runat="server">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="Price" SortField="Price" Width="100px" HeaderText="菜品单价" />
                                <f:BoundField DataField="TotalPrice" SortField="TotalPrice" Width="100px" HeaderText="小计" />
                                <f:LinkButtonField ColumnID="deleteField" HeaderText="删除" TextAlign="Center" Icon="Delete" ToolTip="删除"
                                    ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="80px" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:GroupPanel>
                <f:HiddenField ID="txbhidden" runat="server"></f:HiddenField>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true"
            EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="800px"
            Height="495px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
