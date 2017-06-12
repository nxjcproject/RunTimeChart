<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Monitor_EquipmentIdleAlarm.aspx.cs" Inherits="RuntimeChart.Web.UI_EnergyRealtimeChart.Monitor_EquipmentIdleAlarm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/gray/easyui.css" />
    <link rel="stylesheet" type="text/css" href="/lib/ealib/themes/icon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtIcon.css" />
    <link rel="stylesheet" type="text/css" href="/lib/extlib/themes/syExtCss.css" />

    <link type="text/css" rel="stylesheet" href="/css/common/NormalPage.css" />
    <link type="text/css" rel="stylesheet" href="css/page/Monitor_EquipmentIdleAlarm.css" />

    <script type="text/javascript" src="/lib/ealib/jquery.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/jquery.easyui.min.js" charset="utf-8"></script>
    <script type="text/javascript" src="/lib/ealib/easyui-lang-zh_CN.js" charset="utf-8"></script>

    <!--[if gt IE 8]><script type="text/javascript" src="/lib/ealib/extend/easyUI.WindowsOverrange.js" charset="utf-8"></script>-->
    <!--[if !IE]>
    <script type="text/javascript" src="/lib/ealib/extend/easyUI.WindowsOverrange.js" charset="utf-8"></script>
    <!--<![endif]-->
    <script type="text/javascript" src="/js/common/format/DateTimeFormat.js" charset="utf-8"></script>

    <script type="text/javascript" src="/js/common/PrintFile.js" charset="utf-8"></script>
    <script type="text/javascript" src="js/page/Monitor_EquipmentIdleAlarm.js" charset="utf-8"></script>
</head>
<body>
    <div id="Mainlayout" class="easyui-layout" data-options="fit:true,border:false">
        <div class="easyui-panel" data-options="region:'north',border:false" style="height: 35px; padding-top: 1px; overflow: hidden; background-color: #dddddd;">
            <table id="QueryTable" style="background-color: #dddddd; border: 1px solid #bbbbbb;">
                <tr>
                    <td style="width: 80px; height: 30px; text-align: center;">选择生产区域</td>
                    <td style="width: 180px; text-align: left;">
                        <ul id="ComboTree_OrganizationIdF" class="easyui-combotree" style="width: 170px;">
                        </ul>
                    </td>
                    <td></td>
                </tr>
            </table>
        </div>
        <div class="easyui-panel" data-options="region:'center',border:false" style="overflow: auto; padding-left: 10px; padding-top: 10px;">
            <table>
                <tr>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                   <td style="width: 280px;">
                        <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan ="4" style ="height:20px;"></td>
                </tr>
                 <tr>
                    <td style="width: 280px;">
                        <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                   <td style="width: 280px;">
                        <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan ="4" style ="height:20px;"></td>
                </tr>
                 <tr>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                   <td style="width: 280px;">
                        <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 280px;">
                         <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 280px;">
                        <table style="font-size: 10pt;">
                            <tr>
                                <td style="width: 80px; height: 16px; text-align: left; font-weight: bold; padding-left: 6px;background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">1#水泥磨
                                </td>
                                <td style="width: 130px; height: 16px; text-align: left; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');" colspan ="2">产量:121.21
                                </td>
                                <td style="width: 50px; height: 16px; text-align: center; Color:red; font-weight: bold; background-image:url('../images/page/EquipmentIdleAlarmTitle.png');">空转
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 86px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">名称</td>
                                <td style="width: 80px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电量</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">电流</td>
                                <td style="width: 50px; height: 16px; text-align: center; font-weight: bold; color:white; background-color: #578cf4; border: 1px solid #cccccc;">运行</td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">磨主电机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                 <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">动辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">145.15</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">47.15</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentRun.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">定辊</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                            <tr>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">循环风机</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">0</td>
                                <td style="height: 16px; text-align: center; border: 1px solid #cccccc;">25</td>
                                <td style="height: 16px; text-align: center; background-image:url('../images/page/EquipmentStop.png'); border: 1px solid #cccccc;"></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <form id="form1" runat="server">
        <div>
        </div>
    </form>
</body>
</html>
