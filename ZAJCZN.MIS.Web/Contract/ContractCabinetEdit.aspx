<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractCabinetEdit.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractCabinetEdit" %>

<!DOCTYPE html>
<html>
<head id="Head1" runat="server">
    <title></title>
    <style>
        .f-grid-row-summary .f-grid-cell-inner {
            font-weight: bold;
            color: red;
        }
    </style>
    <meta name="sourcefiles" content="~/PublicWebForm/OrderGoodsSelectDialog.aspx" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server" />
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="Region" BoxConfigAlign="Stretch"
            BoxConfigPosition="Start" ShowHeader="false" Title="查看发货单">
            <Items>
                <f:Panel runat="server" ID="panelTopRegion" RegionPosition="Top" RegionSplit="true" EnableCollapse="true"
                    Title="顶部面板" ShowBorder="true" ShowHeader="false" BodyPadding="10px" IconFont="_PullUp">
                    <Items>
                        <f:Toolbar ID="Toolbar2" runat="server">
                            <Items>
                                <f:Label ID="lblOrderNo" runat="server" Label="定单号" LabelAlign="left" LabelWidth="70px"></f:Label>
                                <f:Label ID="lblProject" runat="server" Label="客户地址" LabelAlign="Right" LabelWidth="100px"></f:Label>
                                <f:Label ID="lblCustName" runat="server" Label="客户名称" LabelAlign="Right" LabelWidth="80px"></f:Label>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnReturn" OnClick="btnReturn_Click" runat="server" Icon="arrowleft"
                                    Text="返回定单列表">
                                </f:Button>
                                <f:HiddenField ID="hdNO" runat="server"></f:HiddenField>
                            </Items>
                        </f:Toolbar>
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="panelLeftRegion" RegionPosition="Right" RegionSplit="true" EnableCollapse="true"
                    Width="500px" Title="五金配件信息" TitleToolTip="五金配件信息" ShowBorder="true" ShowHeader="true"
                    BodyPadding="10px" IconFont="_PullLeft" Collapsed="true">
                    <Items>
                        <f:Grid ID="gdHandWare" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                            EnableCheckBoxSelect="true"
                            EnableAfterEditEvent="true" OnAfterEdit="gdHandWare_AfterEdit"
                            EnableSummary="true" SummaryPosition="Bottom">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar3" runat="server">
                                    <Items>
                                        <f:Button ID="btnNewHW" Text="选择五金" Icon="Add" EnablePostBack="false" runat="server">
                                        </f:Button>
                                        <f:Button ID="btnDeleteHW" Text="删除选中行" Icon="Delete"
                                            runat="server" OnClick="btnDeleteHW_Click">
                                        </f:Button>
                                        <f:ToolbarFill ID="ToolbarFill2" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnIsFree" Text="设置收费" Icon="Money" runat="server" OnClick="btnIsFree_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="GoodsName" Width="120px" HeaderText="商品名称" />
                                <f:RenderField Width="70px" ColumnID="GoodsNumber" DataField="GoodsNumber" FieldType="Double"
                                    HeaderText="数量">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox5" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="80px" ColumnID="GoodsUnitPrice" DataField="GoodsUnitPrice" FieldType="Double"
                                    HeaderText="单价">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox6" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="GoodAmount" ColumnID="GoodAmount" Width="100px" HeaderText="金额（元）" />
                                <f:TemplateField Width="60px" HeaderText="收费">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="60px" Text='<%#  Eval("IsFree").ToString().Equals ("1")?"是":"否" %>'
                                            ForeColor='<%# GetColor(Eval("IsFree").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="panelCenterRegion" RegionPosition="Center" AutoScroll="true"
                    Title="柜体定制明细" ShowBorder="true" ShowHeader="true" BodyPadding="10px" IconFont="_RoundPlus">
                    <Items>
                        <f:Grid ID="gdCostInfo" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                            EnableCheckBoxSelect="true"
                            EnableAfterEditEvent="true" OnAfterEdit="gdCostInfo_AfterEdit"
                            EnableSummary="true" SummaryPosition="Bottom">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnNew" Text="新增产品" Icon="Add" EnablePostBack="false" runat="server">
                                        </f:Button>
                                        <f:Button ID="btnDeleteSelected" Text="删除选中行" Icon="Delete"
                                            runat="server" OnClick="btnDeleteSelected_Click">
                                        </f:Button>
                                        <f:ToolbarFill ID="fill" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnExport" EnableAjax="false" DisableControlBeforePostBack="false"
                                            Icon="PageExcel" runat="server" Text="导出报价单" OnClick="btnExport_Click">
                                        </f:Button>
                                        <f:Button ID="btnPrint" EnableAjax="false" Icon="Printer" runat="server" Text="打印报价单"
                                            OnClientClick="onPrint();">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="GoodsType" SortField="GoodsType" Width="160px" HeaderText="房间位置及柜体名称" />
                                <f:BoundField DataField="GoodsName" SortField="GoodsName" Width="120px" HeaderText="商品名称" />
                                <f:RenderField Width="90px" ColumnID="GHeight" DataField="GHeight" FieldType="Double"
                                    HeaderText="高度(mm)">
                                    <Editor>
                                        <f:NumberBox ID="nbHeight" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="90px" ColumnID="GWide" DataField="GWide" FieldType="Double"
                                    HeaderText="宽度(mm)">
                                    <Editor>
                                        <f:NumberBox ID="nbWide" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="GArea" ColumnID="GArea" Width="120px" HeaderText="面积(㎡)" />
                                <f:RenderField Width="90px" ColumnID="OrderNumber" DataField="OrderNumber" FieldType="Double"
                                    HeaderText="数量(块)">
                                    <Editor>
                                        <f:NumberBox ID="nbNumber" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="GPrice" DataField="GPrice" FieldType="Double"
                                    HeaderText="单价(元)">
                                    <Editor>
                                        <f:NumberBox ID="nbPrice" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="OrderAmount" ColumnID="OrderAmount" Width="120px" HeaderText="金额(元)" />
                                <f:RenderField Width="100px" ColumnID="GColorOne" DataField="GColorOne" FieldType="string"
                                    HeaderText="柜体颜色">
                                    <Editor>
                                        <f:TextBox ID="tbGColorOne" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="GColorTwo" DataField="GColorTwo" FieldType="string"
                                    HeaderText="门板颜色">
                                    <Editor>
                                        <f:TextBox ID="tbGColorTwo" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:TemplateField Width="200px" HeaderText="生产厂家">
                                    <ItemTemplate>
                                        <asp:Label ID="Label2" runat="server" Width="200px" Text='<%#  GetSuplierName(Eval("SupplyID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:RenderField Width="300px" ColumnID="Remark" DataField="Remark" FieldType="string"
                                    HeaderText="备注">
                                    <Editor>
                                        <f:TextBox ID="tbRemark" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="panelRightRegion" RegionPosition="Right" RegionSplit="true" EnableCollapse="true"
                    Width="420px" Title="相关附件文件" TitleToolTip="相关附件文件" ShowBorder="true" ShowHeader="true"
                    BodyPadding="10px" IconFont="_PullLeft" Collapsed="true">
                    <Items>
                        <f:Grid ID="gdFiles" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" EnableCheckBoxSelect="true" OnRowCommand="gdFiles_RowCommand">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar4" runat="server">
                                    <Items>
                                        <f:FileUpload runat="server" LabelWidth="105px" ID="filePhoto" ShowRedStar="false" ShowEmptyLabel="true"
                                            ButtonText="上传文件" ButtonOnly="true" Required="false" ButtonIcon="ImageAdd" Label="上传文件" ShowLabel="false"
                                            AutoPostBack="true" OnFileSelected="filePhoto_FileSelected" LabelAlign="Right">
                                        </f:FileUpload>
                                        <f:ToolbarFill ID="ToolbarFill3" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnDeleteFiles" Text="删除选中行" Icon="Delete"
                                            runat="server" OnClick="btnDeleteFiles_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="FileName" Width="300px" HeaderText="文件名称" />
                                <f:LinkButtonField ColumnID="downLoadField" HeaderText="下载" TextAlign="Center" Icon="ArrowDown" ToolTip="下载"
                                    CommandName="DownLoad" Width="60px" EnableAjax="false" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="500px" Height="400px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="800px" Height="700px" OnClose="Window2_Close">
        </f:Window>
    </form>
    <script type="text/javascript">

        function onPrint() {
            var textbox1ID = '<%= hdNO.ClientID %>';
            var url = 'ContractCabinetPrint.aspx?printNO=' + F(textbox1ID).getValue();
            window.open(url);
        }

    </script>
</body>
</html>
