<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractTrackManage.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractTrackManage" %>

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
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server"
                            Text="关闭">
                        </f:Button>
                    </Items>
                </f:Panel>
                <f:Grid ID="gdHandWare" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                    runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                    EnableCheckBoxSelect="false"
                    EnableAfterEditEvent="true" OnAfterEdit="gdHandWare_AfterEdit">
                    <Columns>
                        <f:RowNumberField></f:RowNumberField>
                        <f:BoundField DataField="SuppilerName" Width="160px" HeaderText="厂商名称" />
                        <f:TemplateField Width="100px" HeaderText="厂商类型">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Width="100px" Text='<%#  Eval("CostType").ToString().Equals ("1")?"门厂商":"柜子厂商" %>'></asp:Label>
                            </ItemTemplate>
                        </f:TemplateField>
                        <f:RenderField Width="160px" ColumnID="CostAmount" DataField="CostAmount" FieldType="Double"
                            HeaderText="成本(元)" Hidden="true">
                            <Editor>
                                <f:NumberBox ID="NumberBox5" NoDecimal="false" NoNegative="true" MinValue="1"
                                    runat="server" DecimalPrecision="2">
                                </f:NumberBox>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="100px" ColumnID="ProduceState" DataField="ProduceState" FieldType="Int"
                            HeaderText="生产状态" RendererFunction="renderState">
                            <Editor>
                                <f:DropDownList ID="ddlState" runat="server">
                                    <f:ListItem Text="生产中" Value="1" />
                                    <f:ListItem Text="已完成" Value="2" />
                                </f:DropDownList>
                            </Editor>
                        </f:RenderField>
                        <f:RenderField Width="200px" ColumnID="ProduceRemark" DataField="ProduceRemark" FieldType="string"
                            HeaderText="生产备注">
                            <Editor>
                                <f:TextBox ID="taRemark" LabelAlign="Right" runat="server" Width="200px">
                                </f:TextBox>
                            </Editor>
                        </f:RenderField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
    </form>
    <script>
        function renderState(value) {
            return value == 1 ? '生产中' : '已完成';
        }
    </script>
</body>
</html>
