<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContractDoorDetail.aspx.cs" Inherits="ZAJCZN.MIS.Web.ContractDoorDetail" %>

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
            BoxConfigPosition="Start" ShowHeader="false" Title="查看门设计详细">
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
                <f:Panel runat="server" ID="panel2" RegionPosition="Right" RegionSplit="true" EnableCollapse="true"
                    Width="250px" Title="锁具信息" TitleToolTip="锁具信息" ShowBorder="true" ShowHeader="true"
                    BodyPadding="10px" IconFont="_PullLeft" Collapsed="true">
                    <Items>
                        <f:Grid ID="gdLock" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="GoodsName">
                            <Columns>
                                <f:BoundField DataField="GoodsName" Width="160px" HeaderText="锁型号" />
                                <f:BoundField DataField="GoodsNumber" Width="60px" HeaderText="数量" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="panelLeftRegion" RegionPosition="Right" RegionSplit="true" EnableCollapse="true"
                    Width="450px" Title="五金配件信息" TitleToolTip="五金配件信息" ShowBorder="true" ShowHeader="true"
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
                                        <f:Button ID="btnIsFree" Text="设置收费" Hidden="true" Icon="Money" runat="server" OnClick="btnIsFree_Click">
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
                                <f:TemplateField Width="60px" HeaderText="收费" Hidden="true">
                                    <ItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Width="60px" Text='<%#  Eval("IsFree").ToString().Equals ("1")?"是":"否" %>'
                                            ForeColor='<%# GetColor(Eval("IsFree").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Panel>
                <f:Panel runat="server" ID="panelCenterRegion" RegionPosition="Center" AutoScroll="true"
                    Title="门定制明细" ShowBorder="true" ShowHeader="true" BodyPadding="10px" IconFont="_RoundPlus">
                    <Items>
                        <f:Grid ID="gdCostInfo" ShowBorder="true" ShowHeader="false" EnableCollapse="true"
                            runat="server" DataKeyNames="ID" AllowCellEditing="true" ClicksToEdit="1"
                            EnableCheckBoxSelect="true"
                            EnableAfterEditEvent="true" OnAfterEdit="gdCostInfo_AfterEdit"
                            EnableSummary="true" SummaryPosition="Bottom"
                            EnableRowDoubleClickEvent="true" OnRowDoubleClick="gdCostInfo_RowDoubleClick">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="btnNew" Text="新增产品" Icon="Add" EnablePostBack="false" runat="server">
                                        </f:Button>
                                        <f:Button ID="btnDeleteSelected" Text="删除选中行" Icon="Delete"
                                            runat="server" OnClick="btnDeleteSelected_Click">
                                        </f:Button>
                                        <f:Label ID="lblNotice" runat="server" ShowLabel="false" Text="双击记录修改录入信息" CssStyle="color:red;"></f:Label>
                                        <f:ToolbarFill ID="fill" runat="server">
                                        </f:ToolbarFill>
                                        <f:Button ID="btnExport" EnableAjax="false" DisableControlBeforePostBack="false"
                                            Icon="PageExcel" runat="server" Text="导出报价单" OnClick="btnExport_Click">
                                        </f:Button>
                                        <f:Button ID="btnExportCS" EnableAjax="false" DisableControlBeforePostBack="false"
                                            Icon="PageExcel" runat="server" Text="导出厂商单" OnClick="btnExportCS_Click">
                                        </f:Button>
                                        <f:Button ID="btnPrint" EnableAjax="false" Icon="Printer" runat="server" Text="打印报价单"
                                            OnClientClick="onPrint();">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:BoundField DataField="GoodsLocation" SortField="GoodsLocation" Width="130px" HeaderText="门位置" />
                                <f:GroupField HeaderText="商品尺寸" TextAlign="Center">
                                    <Columns>
                                        <f:RenderField Width="100px" ColumnID="GHeight" DataField="GHeight" FieldType="Int"
                                            HeaderText="高度(mm)">
                                            <Editor>
                                                <f:NumberBox ID="nbHeight" NoDecimal="true" NoNegative="true" MinValue="0"
                                                    runat="server">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:RenderField Width="100px" ColumnID="GWide" DataField="GWide" FieldType="Int"
                                            HeaderText="宽度(mm)">
                                            <Editor>
                                                <f:NumberBox ID="nbWide" NoDecimal="true" NoNegative="true" MinValue="0"
                                                    runat="server">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:RenderField Width="100px" ColumnID="GThickness" DataField="GThickness" FieldType="Int"
                                            HeaderText="厚度(mm)">
                                            <Editor>
                                                <f:NumberBox ID="nbThickness" NoDecimal="true" NoNegative="true" MinValue="0"
                                                    runat="server">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:BoundField DataField="GArea" Width="100px" HeaderText="面积(㎡)" />
                                    </Columns>
                                </f:GroupField>
                                <f:RenderField Width="100px" ColumnID="LineName" DataField="LineName" FieldType="String"
                                    HeaderText="线条">
                                    <Editor>
                                        <f:TextBox ID="TextBox1" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="TypeName" SortField="TypeName" Width="150px" HeaderText="材质" />
                                <f:RenderField Width="70px" ColumnID="DoorColor" DataField="DoorColor" FieldType="String"
                                    HeaderText="颜色">
                                    <Editor>
                                        <f:TextBox ID="txtDoorColor" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:RenderField Width="100px" ColumnID="ModelName" DataField="ModelName" FieldType="String"
                                    HeaderText="款式型号">
                                    <Editor>
                                        <f:TextBox ID="txtModel" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="GoodsName" SortField="GoodsName" Width="120px" HeaderText="商品名称" />
                                <f:BoundField DataField="DoorDirection" Width="100px" HeaderText="开启方向" />
                                <f:RenderField Width="100px" ColumnID="GlassRemark" DataField="GlassRemark" FieldType="String"
                                    HeaderText="玻璃款式">
                                    <Editor>
                                        <f:TextBox ID="TextBox2" runat="server">
                                        </f:TextBox>
                                    </Editor>
                                </f:RenderField>
                                <f:BoundField DataField="GoodUnit" Width="70px" HeaderText="单位" />
                                <f:GroupField HeaderText="尺寸超标信息" TextAlign="Center" Hidden="true">
                                    <Columns>
                                        <f:BoundField DataField="GPassHeight" Width="90px" HeaderText="超高(mm)" />
                                        <f:BoundField DataField="GPassHeightAmount" Width="100px" HeaderText="加价(元)" />
                                        <f:BoundField DataField="GPassWide" Width="90px" HeaderText="超宽(mm)" />
                                        <f:BoundField DataField="GPassWideAmount" Width="100px" HeaderText="加价(元)" />
                                        <f:BoundField DataField="GPassThickness" Width="90px" HeaderText="超厚(mm)" />
                                        <f:BoundField DataField="GPassThicknessAmount" Width="100px" HeaderText="加价(元)" />
                                        <f:BoundField DataField="GPassArea" Width="100px" HeaderText="超面积(㎡)" />
                                        <f:BoundField DataField="GPassAreaAmount" Width="100px" HeaderText="加价(元)" />
                                    </Columns>
                                </f:GroupField>
                                <f:RenderField Width="90px" ColumnID="GPrice" DataField="GPrice" FieldType="Double"
                                    HeaderText="单价">
                                    <Editor>
                                        <f:NumberBox ID="NumberBox1" NoDecimal="false" NoNegative="true" MinValue="1"
                                            runat="server" DecimalPrecision="2">
                                        </f:NumberBox>
                                    </Editor>
                                </f:RenderField>
                                <f:GroupField HeaderText="单价信息(元)" TextAlign="Center" Hidden="true">
                                    <Columns>
                                        <f:RenderField Width="100px" ColumnID="GStandardPrice" DataField="GStandardPrice" FieldType="Double"
                                            HeaderText="原单价">
                                            <Editor>
                                                <f:NumberBox ID="nbPrice" NoDecimal="false" NoNegative="true" MinValue="1"
                                                    runat="server" DecimalPrecision="2">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:BoundField DataField="PassPriceAmount" Width="100px" HeaderText="超标加价" />
                                        <f:BoundField DataField="GPrice" Width="100px" HeaderText="最终单价" />
                                    </Columns>
                                </f:GroupField>
                                <f:BoundField DataField="OrderNumber" ColumnID="OrderNumber" Width="90px" HeaderText="商品数量" />
                                <f:GroupField HeaderText="报价信息(元)" TextAlign="Center">
                                    <Columns>
                                        <f:RenderField Width="100px" ColumnID="GoodsAmount" DataField="GoodsAmount" FieldType="Double"
                                            HeaderText="商品金额">
                                            <Editor>
                                                <f:NumberBox ID="nbGoodsAmount" NoDecimal="false" NoNegative="true" MinValue="1"
                                                    runat="server" DecimalPrecision="2">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:RenderField Width="100px" ColumnID="PassAmount" DataField="PassAmount" FieldType="Double"
                                            HeaderText="超标加价">
                                            <Editor>
                                                <f:NumberBox ID="NumberBox2" NoDecimal="false" NoNegative="true" MinValue="1"
                                                    runat="server" DecimalPrecision="2">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:RenderField Width="100px" ColumnID="InstallCost" DataField="InstallCost" FieldType="Double"
                                            HeaderText="运输安装">
                                            <Editor>
                                                <f:NumberBox ID="nbInstallCost" NoDecimal="false" NoNegative="true" MinValue="1"
                                                    runat="server" DecimalPrecision="2">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:RenderField Width="100px" ColumnID="HardWareAmount" DataField="HardWareAmount" FieldType="Double"
                                            HeaderText="五金费用">
                                            <Editor>
                                                <f:NumberBox ID="nbHardWareAmount" NoDecimal="false" NoNegative="true" MinValue="1"
                                                    runat="server" DecimalPrecision="2">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:RenderField Width="100px" ColumnID="OtherAmount" DataField="OtherAmount" FieldType="Double"
                                            HeaderText="其他金额">
                                            <Editor>
                                                <f:NumberBox ID="nbOtherAmount" NoDecimal="false" NoNegative="true" MinValue="0"
                                                    runat="server" DecimalPrecision="2">
                                                </f:NumberBox>
                                            </Editor>
                                        </f:RenderField>
                                        <f:BoundField DataField="OrderAmount" ColumnID="OrderAmount" Width="120px" HeaderText="总金额" />
                                    </Columns>
                                </f:GroupField>
                                <f:TemplateField Width="120px" HeaderText="锁具型号 ">
                                    <ItemTemplate>
                                        <asp:Label ID="Label1" runat="server" Width="120px" Text='<%# GetLock(Eval("LockID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
                                <f:RenderField Width="400px" ColumnID="Remark" DataField="Remark" FieldType="string"
                                    HeaderText="工艺备注">
                                    <Editor>
                                        <f:TextArea ID="taRemark" LabelAlign="Right" runat="server" AutoGrowHeight="true" AutoGrowHeightMin="100" AutoGrowHeightMax="600">
                                        </f:TextArea>
                                    </Editor>
                                </f:RenderField>
                                <f:TemplateField Width="200px" HeaderText="生产厂家">
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Width="200px" Text='<%#  GetSuplierName(Eval("SupplyID").ToString()) %>'></asp:Label>
                                    </ItemTemplate>
                                </f:TemplateField>
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
            Width="600px" Height="560px" OnClose="Window1_Close">
        </f:Window>
        <f:Window ID="Window2" CloseAction="Hide" runat="server" IsModal="true" Hidden="true"
            Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank"
            Width="800px" Height="700px" OnClose="Window2_Close">
        </f:Window>
    </form>
    <script type="text/javascript">

        function onPrint() {
            var textbox1ID = '<%= hdNO.ClientID %>';
            var url = 'ContractDoorPrint.aspx?printNO=' + F(textbox1ID).getValue();
            window.open(url);
        }

    </script>
</body>
</html>
