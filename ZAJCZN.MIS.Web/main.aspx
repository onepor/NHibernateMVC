﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="main.aspx.cs" Inherits="ZAJCZN.MIS.Web.main" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>首页</title>
    <link href="res/css/main.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="regionPanel" runat="server" />
        <f:Panel ID="regionPanel" Layout="Region" CssClass="mainpanel" ShowBorder="false" ShowHeader="false" runat="server">
            <Items>
                <f:ContentPanel ID="topPanel" CssClass="topregion bgpanel" ShowBorder="false" ShowHeader="false" RegionPosition="Top" runat="server">
                    <div id="header" class="f-widget-header f-mainheader">
                        <table>
                            <tr>
                                <td>
                                    <f:Button runat="server" CssClass="icononlyaction" ID="Button1" ToolTip="家喜林门"
                                        IconAlign="Top" Text="家喜林门管理系统"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false">
                                    </f:Button>
                                </td>
                                <td style="text-align: right;">
                                    <f:Label runat="server" ID="lblTime" ShowLabel="false"></f:Label>
                                    <f:Button runat="server" ID="btnUserName" CssClass="userpicaction" Text="" IconAlign="Left"
                                        EnablePostBack="false" EnableDefaultState="false" EnableDefaultCorner="false">
                                        <Menu runat="server">
                                            <f:MenuButton ID="btnHelp" EnablePostBack="false" Icon="Help" Text="帮助" runat="server">
                                            </f:MenuButton>
                                            <f:MenuSeparator runat="server">
                                            </f:MenuSeparator>
                                            <f:MenuButton runat="server" Text="安全退出" ConfirmText="确定退出系统？" OnClick="btnExit_Click">
                                            </f:MenuButton>
                                        </Menu>
                                    </f:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </f:ContentPanel>
                <f:Panel ID="leftPanel" CssClass="leftregion bgpanel"
                    EnableCollapse="true" Width="200px" RegionSplit="true" RegionSplitIcon="true" RegionSplitWidth="8px"
                    ShowHeader="false" Title="系统菜单" Layout="Fit" RegionPosition="Left" runat="server">
                </f:Panel> 
                <f:TabStrip ID="mainTabStrip" CssClass="centerregion" RegionPosition="Center" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                    <Tabs>
                        <f:Tab ID="Tab1" Title="首页" IFrameName="main" EnableIFrame="true" IFrameUrl="~/admin/default.aspx"
                            Icon="House" runat="server">
                        </f:Tab>
                    </Tabs>
                    <Tools>
                        <f:Tool runat="server" EnablePostBack="false" IconFont="_Refresh" CssClass="tabtool" ToolTip="刷新本页" ID="toolRefresh">
                            <Listeners>
                                <f:Listener Event="click" Handler="onToolRefreshClick" />
                            </Listeners>
                        </f:Tool>
                        <f:Tool runat="server" EnablePostBack="false" IconFont="_Maximize" CssClass="tabtool" ToolTip="最大化" ID="toolMaximize">
                            <Listeners>
                                <f:Listener Event="click" Handler="onToolMaximizeClick" />
                            </Listeners>
                        </f:Tool>
                    </Tools>
                </f:TabStrip>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" EnableIFrame="true"
            EnableResize="true" EnableMaximize="true" IFrameUrl="about:blank" Width="800px"
            Height="500px">
        </f:Window>
    </form>
    <script>

        var mainTabStripClientID = '<%= mainTabStrip.ClientID %>';
        var topPanelClientID = '<%= topPanel.ClientID %>';
        var leftPanelClientID = '<%= leftPanel.ClientID %>';
        var tab = '<%= Tab1.ClientID %>';

        function OnTest(url) {
            var y = F(mainTabStripClientID).getActiveTab();
            if (y.f_dynamic_added_tab) {
                window.open(url, 'main');
            }
            else {
                window.open(url, 'main');
            }
        }

        // 点击标题栏工具图标 - 刷新
        function onToolRefreshClick(event) {
            var mainTabStrip = F(mainTabStripClientID);
            var activeTab = mainTabStrip.getActiveTab();
            if (activeTab.iframe) {
                var iframeWnd = activeTab.getIFrameWindow();
                iframeWnd.location.reload();
            }
        }

        // 点击标题栏工具图标 - 最大化
        function onToolMaximizeClick(event) {
            var topPanel = F(topPanelClientID);
            var leftPanel = F(leftPanelClientID);

            //if (this.name != 'fullscreen') {
            //    window.open(location.href, 'fullscreen', 'fullscreen,scrollbars')
            //}

            var currentTool = this;
            F.noAnimation(function () {
                if (currentTool.iconFont === 'f-iconfont-maximize') {
                    currentTool.setIconFont('f-iconfont-restore');

                    leftPanel.collapse();
                    topPanel.collapse();
                } else {
                    currentTool.setIconFont('f-iconfont-maximize');

                    leftPanel.expand();
                    topPanel.expand();
                }
            });
        }


        // 添加示例标签页
        // id： 选项卡ID
        // iframeUrl: 选项卡IFrame地址 
        // title： 选项卡标题
        // icon： 选项卡图标
        // createToolbar： 创建选项卡前的回调函数（接受tabOptions参数）
        // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame
        // iconFont： 选项卡图标字体
        function addExampleTab(tabOptions) {

            if (typeof (tabOptions) === 'string') {
                tabOptions = {
                    id: arguments[0],
                    iframeUrl: arguments[1],
                    title: arguments[2],
                    icon: arguments[3],
                    createToolbar: arguments[4],
                    refreshWhenExist: arguments[5],
                    iconFont: arguments[6]
                };
            }

            F.addMainTab(F(mainTabStripClientID), tabOptions);
        }


        // 移除选中标签页
        function removeActiveTab() {
            var mainTabStrip = F(mainTabStripClientID);

            var activeTab = mainTabStrip.getActiveTab();
            activeTab.hide();
        }


        F.ready(function () {

            var mainTabStrip = F(mainTabStripClientID);
            var leftPanel = F(leftPanelClientID);
            var treeMenu = leftPanel.getItem(0);

            // 初始化主框架中的树(或者Accordion+Tree)和选项卡互动，以及地址栏的更新
            // treeMenu： 主框架中的树控件实例，或者内嵌树控件的手风琴控件实例
            // mainTabStrip： 选项卡实例
            // updateHash: 切换Tab时，是否更新地址栏Hash值（默认值：true）
            // refreshWhenExist： 添加选项卡时，如果选项卡已经存在，是否刷新内部IFrame（默认值：false）
            // refreshWhenTabChange: 切换选项卡时，是否刷新内部IFrame（默认值：false）
            // maxTabCount: 最大允许打开的选项卡数量
            // maxTabMessage: 超过最大允许打开选项卡数量时的提示信息
            F.initTreeTabStrip(treeMenu, mainTabStrip, {
                maxTabCount: 10,
                maxTabMessage: '请先关闭一些选项卡（最多允许打开 10 个）！'
            });


        });

    </script>
</body>
</html>
