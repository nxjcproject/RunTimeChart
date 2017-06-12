var EquipmentParameters = null;
var IntervalRuntimeCycle = 1000;
var IntervalMachineHaltCycle = 300000;
var RuntimeData = null;
var MachinHaltData = null;
var BlinkFlag = true;
$(function () {
    var m_EquipmentCommonId = ['RawMaterialsGrind', 'CoalGrind', 'RotaryKiln', 'CementGrind', 'Generator'];
    loadMachineHaltlDialog();                    //初始化停机记录
    GetEquipmentParameters(m_EquipmentCommonId);
});

function GetEquipmentParameters(myEquipmentCommonId) {
    var m_EquipmentCommonIdString = "";
    for (var i = 0; i < myEquipmentCommonId.length; i++) {
        if (i == 0) {
            m_EquipmentCommonIdString = myEquipmentCommonId[i];
        }
        else {
            m_EquipmentCommonIdString = m_EquipmentCommonIdString + "," + myEquipmentCommonId[i];
        }
    }
    $.ajax({
        type: "POST",
        url: "Monitor_MainMachineRuntimeStatus.aspx/GetEquipmentParameters",
        data: "{myEquipmentCommonIds:'" + m_EquipmentCommonIdString + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData != null && m_MsgData != undefined) {
                EquipmentParameters = m_MsgData.rows;
                SetEquipmentHtml();
            }
        },
        error: function (msg) {
        }
    });
}
function GetNodeInfo()
{   
    var m_NodeInfo = {};
    if (EquipmentParameters != null && EquipmentParameters != undefined)
    {
        var m_CompanyName = "";
        for (var i = 0; i < EquipmentParameters.length; i++)
        {
            if(m_CompanyName != EquipmentParameters[i]["CompanyName"])
            {
                m_CompanyName = EquipmentParameters[i]["CompanyName"];
                m_NodeInfo[m_CompanyName] = { "clinker": {},"cement":[],"cogeneration":[]};
            }
            if(EquipmentParameters[i]["ProductionLineType"] == "熟料")
            {
                if( m_NodeInfo[m_CompanyName]["clinker"][EquipmentParameters[i]["ProductionLineName"]] == undefined)
                {
                    m_NodeInfo[m_CompanyName]["clinker"][EquipmentParameters[i]["ProductionLineName"]] = {};
                }
                if(m_NodeInfo[m_CompanyName]["clinker"][EquipmentParameters[i]["ProductionLineName"]][EquipmentParameters[i]["EquipmentCommonId"]]  == undefined)
                {
                    m_NodeInfo[m_CompanyName]["clinker"][EquipmentParameters[i]["ProductionLineName"]][EquipmentParameters[i]["EquipmentCommonId"]] = [];
                }
                m_NodeInfo[m_CompanyName]["clinker"][EquipmentParameters[i]["ProductionLineName"]][EquipmentParameters[i]["EquipmentCommonId"]].push(
                    {
                        "ItemId": EquipmentParameters[i]["ItemId"], "EquipmentId": EquipmentParameters[i]["EquipmentId"], "EquipmentName": EquipmentParameters[i]["EquipmentName"]
                        ,"FactoryOrganizationId":EquipmentParameters[i]["FactoryOrganizationId"],"VariableName":EquipmentParameters[i]["VariableName"],"ValidValues":EquipmentParameters[i]["ValidValues"]});
            }
            else if(EquipmentParameters[i]["ProductionLineType"] == "水泥磨")
            {
                m_NodeInfo[m_CompanyName]["cement"].push(
                    {"ItemId":EquipmentParameters[i]["ItemId"],"EquipmentId":EquipmentParameters[i]["EquipmentId"],"EquipmentName":EquipmentParameters[i]["EquipmentName"]
                        ,"FactoryOrganizationId":EquipmentParameters[i]["FactoryOrganizationId"],"VariableName":EquipmentParameters[i]["VariableName"],"ValidValues":EquipmentParameters[i]["ValidValues"]});
            }
            else if (EquipmentParameters[i]["ProductionLineType"] == "余热发电") {
                m_NodeInfo[m_CompanyName]["cogeneration"].push(
                    {
                        "ItemId": EquipmentParameters[i]["ItemId"], "EquipmentId": EquipmentParameters[i]["EquipmentId"], "EquipmentName": EquipmentParameters[i]["EquipmentName"]
                        , "FactoryOrganizationId": EquipmentParameters[i]["FactoryOrganizationId"], "VariableName": EquipmentParameters[i]["VariableName"], "ValidValues": EquipmentParameters[i]["ValidValues"]
                    });
            }
        }
    }
    return m_NodeInfo;
}
function SetEquipmentHtml() {
    var m_NodeInfo = GetNodeInfo();
    var m_FirstRowFlag = true;
    $.each(m_NodeInfo, function (key, value) {
        var m_ClinkerColumnCount = 2;
        var m_CementColumnCount = 3;
        var m_CogenerationColumnCount = 2;
        var m_CompanyRowsHtml = "";
        var m_DataRowHtml = "";
        var m_ClinkerRowHtml = [];
        var m_CementRowHtml = [];
        var m_CogenerationRowHtml = [];
        $.each(value['clinker'], function (SubKey, SubValue) {
            var m_RawMaterialsGrindHtml = "";
            var m_CoalGrindHtml = "";
            var m_RotaryKilnHtml = "";

            if (SubValue["RawMaterialsGrind"] != undefined) {
                if (SubValue["RawMaterialsGrind"].length == 1) {
                    m_RawMaterialsGrindHtml = m_RawMaterialsGrindHtml + '<img id = "' + SubValue["RawMaterialsGrind"][0]["ItemId"] + '" class = "SingleTag"'
                                                  + ' src="images/page/GrayButton.png" title="' + SubValue["RawMaterialsGrind"][0]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + SubValue["RawMaterialsGrind"][0]["EquipmentId"]
                                                  + '","EquipmentName":"' + SubValue["RawMaterialsGrind"][0]["EquipmentName"]
                                                  + '","FactoryOrganizationId":"' + SubValue["RawMaterialsGrind"][0]["FactoryOrganizationId"]
                                                  + '","VariableName":"' + SubValue["RawMaterialsGrind"][0]["VariableName"]
                                                  + '","ValidValues":"' + SubValue["RawMaterialsGrind"][0]["ValidValues"]
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';

                }
                else {
                    for (var i = 0; i < SubValue["RawMaterialsGrind"].length; i++) {
                        m_RawMaterialsGrindHtml = m_RawMaterialsGrindHtml + '<img id = "' + SubValue["RawMaterialsGrind"][i]["ItemId"] + '" class = "MultiTags"'
                                                  + ' src="images/page/GrayButton.png" title="' + SubValue["RawMaterialsGrind"][i]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + SubValue["RawMaterialsGrind"][i]["EquipmentId"]
                                                  + '","EquipmentName":"' + SubValue["RawMaterialsGrind"][i]["EquipmentName"]
                                                  + '","FactoryOrganizationId":"' + SubValue["RawMaterialsGrind"][i]["FactoryOrganizationId"]
                                                  + '","VariableName":"' + SubValue["RawMaterialsGrind"][i]["VariableName"]
                                                  + '","ValidValues":"' + SubValue["RawMaterialsGrind"][i]["ValidValues"]
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
                    }
                }
            }
            if (SubValue["CoalGrind"] != undefined) {
                if (SubValue["CoalGrind"].length == 1) {
                    m_CoalGrindHtml = m_CoalGrindHtml + '<img id = "' + SubValue["CoalGrind"][0]["ItemId"] + '" class = "SingleTag"'
                                                  + ' src="images/page/GrayButton.png" title="' + SubValue["CoalGrind"][0]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + SubValue["CoalGrind"][0]["EquipmentId"]
                                                  + '","EquipmentName":"' + SubValue["CoalGrind"][0]["EquipmentName"] 
                                                  + '","FactoryOrganizationId":"' + SubValue["CoalGrind"][0]["FactoryOrganizationId"] 
                                                  + '","VariableName":"' + SubValue["CoalGrind"][0]["VariableName"] 
                                                  + '","ValidValues":"' + SubValue["CoalGrind"][0]["ValidValues"] 
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
                }
                else {
                    for (var i = 0; i < SubValue["CoalGrind"].length; i++) {
                        m_CoalGrindHtml = m_CoalGrindHtml + '<img id = "' + SubValue["CoalGrind"][i]["ItemId"] + '" class = "MultiTags"'
                                                      + ' src="images/page/GrayButton.png" title="' + SubValue["CoalGrind"][i]["EquipmentName"]
                                                      + '" data-options=\'{"EquipmentId":"' + SubValue["CoalGrind"][i]["EquipmentId"]
                                                      + '","EquipmentName":"' + SubValue["CoalGrind"][i]["EquipmentName"]
                                                      + '","FactoryOrganizationId":"' + SubValue["CoalGrind"][i]["FactoryOrganizationId"]
                                                      + '","VariableName":"' + SubValue["CoalGrind"][i]["VariableName"]
                                                      + '","ValidValues":"' + SubValue["CoalGrind"][i]["ValidValues"]
                                                      + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
                    }
                }
            }
            if (SubValue["RotaryKiln"] != undefined) {
                if (SubValue["RotaryKiln"].length == 1) {
                    m_RotaryKilnHtml = m_RotaryKilnHtml + '<img id = "' + SubValue["RotaryKiln"][0]["ItemId"] + '" class = "SingleTag"'
                                                  + ' src="images/page/GrayButton.png" title="' + SubValue["RotaryKiln"][0]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + SubValue["RotaryKiln"][0]["EquipmentId"]
                                                  + '","EquipmentName":"' + SubValue["RotaryKiln"][0]["EquipmentName"]
                                                  + '","FactoryOrganizationId":"' + SubValue["RotaryKiln"][0]["FactoryOrganizationId"]
                                                  + '","VariableName":"' + SubValue["RotaryKiln"][0]["VariableName"]
                                                  + '","ValidValues":"' + SubValue["RotaryKiln"][0]["ValidValues"]
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
                }
                else {
                    for (var i = 0; i < SubValue["RotaryKiln"].length; i++) {
                        m_RotaryKilnHtml = m_RotaryKilnHtml + '<img id = "' + SubValue["RotaryKiln"][i]["ItemId"] + '" class = "MultiTags"'
                                                  + ' src="images/page/GrayButton.png" title="' + SubValue["RotaryKiln"][i]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + SubValue["RotaryKiln"][i]["EquipmentId"]
                                                  + '","EquipmentName":"' + SubValue["RotaryKiln"][i]["EquipmentName"]
                                                  + '","FactoryOrganizationId":"' + SubValue["RotaryKiln"][i]["FactoryOrganizationId"]
                                                  + '","VariableName":"' + SubValue["RotaryKiln"][i]["VariableName"]
                                                  + '","ValidValues":"' + SubValue["RotaryKiln"][i]["ValidValues"]
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
                    }
                }
            }
            m_ClinkerRowHtml.push('<td class="SubTitleTd">' + SubKey + '</td>'
                        + '<td class="StatusTd">' + m_RawMaterialsGrindHtml + '</td>'
                        + '<td class="StatusTd">' + m_CoalGrindHtml + '</td>'
                        + '<td class="StatusTd">' + m_RotaryKilnHtml + '</td>');
        });
        for (var i = 0; i < value["cement"].length; i++) {
            var m_CementGrindHtml = '<img id = "' + value["cement"][i]["ItemId"] + '" class = "SingleTag"'
                                                  + ' src="images/page/GrayButton.png" title="' + value["cement"][i]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + value["cement"][i]["EquipmentId"]
                                                  + '","EquipmentName":"' + value["cement"][i]["EquipmentName"]
                                                  + '","FactoryOrganizationId":"' + value["cement"][i]["FactoryOrganizationId"]
                                                  + '","VariableName":"' + value["cement"][i]["VariableName"]
                                                  + '","ValidValues":"' + value["cement"][i]["ValidValues"]
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
            m_CementRowHtml.push('<td class="SubTitleTd">' + value["cement"][i]['EquipmentName'].replace('水泥磨', '') + '</td><td class="StatusTd">' + m_CementGrindHtml + '</td>');
        }
        for (var i = 0; i < value["cogeneration"].length; i++) {
            var m_CogenerationGrindHtml = '<img id = "' + value["cogeneration"][i]["ItemId"] + '" class = "SingleTag"'
                                                  + ' src="images/page/GrayButton.png" title="' + value["cogeneration"][i]["EquipmentName"]
                                                  + '" data-options=\'{"EquipmentId":"' + value["cogeneration"][i]["EquipmentId"]
                                                  + '","EquipmentName":"' + value["cogeneration"][i]["EquipmentName"]
                                                  + '","FactoryOrganizationId":"' + value["cogeneration"][i]["FactoryOrganizationId"]
                                                  + '","VariableName":"' + value["cogeneration"][i]["VariableName"]
                                                  + '","ValidValues":"' + value["cogeneration"][i]["ValidValues"]
                                                  + '"}\' onclick=\'GetEquipmentInfo(this,"' + key + '");\' />';
            m_CogenerationRowHtml.push('<td class="SubTitleTd">' + value["cogeneration"][i]['EquipmentName'].replace('余热','') + '</td><td class="StatusTd">' + m_CogenerationGrindHtml + '</td>');
        }
        ///////////////生成第一行Html////////////////
        if (m_FirstRowFlag == true) {
            m_FirstRowFlag = false;
            m_CompanyRowsHtml = '<tr><td colspan="' + (1 + 4 * m_ClinkerColumnCount + 2 * m_CogenerationColumnCount + 2 * m_CementColumnCount) + '" class="MainTitleRow">重点设备运行状态监控</td></tr>';
            m_CompanyRowsHtml = m_CompanyRowsHtml + '<tr><td class="CompanyNameTitleTd"></td>';
            for (var i = 0; i < m_ClinkerColumnCount; i++) {
                m_CompanyRowsHtml = m_CompanyRowsHtml
                    + '<td class="SubTitleTd">产线</td>'
                    + '<td class="SubTitleTd">生料磨</td>'
                    + '<td class="SubTitleTd">煤磨</td>'
                    + '<td class="SubTitleTd">回转窑</td>';
            }
            m_CompanyRowsHtml = m_CompanyRowsHtml + '<td colspan="' + (m_CogenerationColumnCount * 2) + '" class="CogenerationProductionLineNameTd">余热发电</td>';
            m_CompanyRowsHtml = m_CompanyRowsHtml + '<td colspan="' + (m_CementColumnCount * 2) + '" class="CementProductionLineNameTd">水泥磨</td></tr>';
        }
        else {
            m_CompanyRowsHtml = '<tr><td colspan="' + (1 + 4 * m_ClinkerColumnCount + 2 * m_CogenerationColumnCount + 2 * m_CementColumnCount) + '" class="BlankRow"></td></tr>';
        }
        //m_CompanyRowsHtml = m_CompanyRowsHtml + '<tr><td rowspan="' + (m_MargeRowCount + 1) + '" class="CompanyNameTd">' + key + '</td>';

        /////////////////////合并数据行html/////////////////////
        var m_MargeRowCount = m_ClinkerRowHtml.length / m_ClinkerColumnCount > m_CementRowHtml.length / m_CementColumnCount ? Math.ceil(m_ClinkerRowHtml.length / m_ClinkerColumnCount) : Math.ceil(m_CementRowHtml.length / m_CementColumnCount);
        for (var i = 0; i < m_MargeRowCount; i++) {
            m_DataRowHtml = m_DataRowHtml + '<tr>';
            for (var j = 0; j < m_ClinkerColumnCount; j++) {
                if (i * m_ClinkerColumnCount + j == 0) {
                    m_DataRowHtml = m_DataRowHtml + '<td class="CompanyNameTd" rowspan = "' + m_MargeRowCount + '">' + key + '</td>';
                }
                if (i * m_ClinkerColumnCount + j < m_ClinkerRowHtml.length) {
                    m_DataRowHtml = m_DataRowHtml + m_ClinkerRowHtml[i * m_ClinkerColumnCount + j];
                }
                else {
                    m_DataRowHtml = m_DataRowHtml + '<td class="SubTitleTd"></td>'
                        + '<td class="StatusTd"></td>'
                        + '<td class="StatusTd"></td>'
                        + '<td class="StatusTd"></td>';
                }
            }
            for (var j = 0; j < m_CogenerationColumnCount; j++) {
                if (i * m_CogenerationColumnCount + j < m_CogenerationRowHtml.length) {
                    m_DataRowHtml = m_DataRowHtml + m_CogenerationRowHtml[i * m_CogenerationColumnCount + j];
                }
                else {
                    m_DataRowHtml = m_DataRowHtml + '<td class="SubTitleTd"></td><td class="StatusTd"></td>';
                }
            }
            for (var j = 0; j < m_CementColumnCount; j++) {
                if (i * m_CementColumnCount + j < m_CementRowHtml.length) {
                    m_DataRowHtml = m_DataRowHtml + m_CementRowHtml[i * m_CementColumnCount + j];
                }
                else {
                    m_DataRowHtml = m_DataRowHtml + '<td class="SubTitleTd"></td><td class="StatusTd"></td>';
                }
            }
            m_DataRowHtml = m_DataRowHtml + '</tr>';
        }

        
        ///////////////////////////////
        $("#ChartContentTable").append(m_CompanyRowsHtml);
        $("#ChartContentTable").append(m_DataRowHtml);
    });
    GetEquipmentRuntimeStatus();
    GetEquipmentHaltStatus();
    setInterval('SetEquipmentStatus()', 1000);         //状态每秒刷新
}
function GetEquipmentRuntimeStatus() {
    var m_Tags = "";
    if (EquipmentParameters != null) {
        for (var i = 0; i < EquipmentParameters.length; i++) {
            if (i == 0) {
                m_Tags = EquipmentParameters[i]["FactoryOrganizationId"] + "," + EquipmentParameters[i]["VariableName"];
            }
            else {
                m_Tags = m_Tags + ";" + EquipmentParameters[i]["FactoryOrganizationId"] + "," + EquipmentParameters[i]["VariableName"];
            }
        }
        $.ajax({
            type: "POST",
            url: "Monitor_MainMachineRuntimeStatus.aspx/GetEquipmentRuntimeStatus",
            data: "{myTags:'" + m_Tags + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_MsgData = jQuery.parseJSON(msg.d);
                if (m_MsgData != null && m_MsgData != undefined) {
                    RuntimeData = null;
                    RuntimeData = m_MsgData;

                }
                setTimeout("GetEquipmentRuntimeStatus()", IntervalRuntimeCycle);
            },
            error: function (msg) {
                setTimeout("GetEquipmentRuntimeStatus()", IntervalRuntimeCycle);
            }
        });
    }
    else {
        setTimeout("GetEquipmentRuntimeStatus()", IntervalRuntimeCycle);
    }
}
function GetEquipmentHaltStatus() {
    var m_Tags = "";
    if (EquipmentParameters != null) {
        for (var i = 0; i < EquipmentParameters.length; i++) {
            if (m_Tags == "") {
                if (EquipmentParameters[i]["EquipmentCommonId"] == "RotaryKiln") {
                    m_Tags = EquipmentParameters[i]["OrganizationId"] + "," + EquipmentParameters[i]["EquipmentId"] + "," + EquipmentParameters[i]["ItemId"];
                }
            }
            else {
                if (EquipmentParameters[i]["EquipmentCommonId"] == "RotaryKiln") {
                    m_Tags = m_Tags + ";" + EquipmentParameters[i]["OrganizationId"] + "," + EquipmentParameters[i]["EquipmentId"] + "," + EquipmentParameters[i]["ItemId"];
                }
            }
        }
        $.ajax({
            type: "POST",
            url: "Monitor_MainMachineRuntimeStatus.aspx/GetEquipmentHaltStatus",
            data: "{myTags:'" + m_Tags + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var m_MsgData = jQuery.parseJSON(msg.d);
                if (m_MsgData != null && m_MsgData != undefined) {
                    MachinHaltData = null;
                    MachinHaltData = m_MsgData;

                }
                setTimeout("GetEquipmentHaltStatus()", IntervalMachineHaltCycle);
            },
            error: function (msg) {
                setTimeout("GetEquipmentHaltStatus()", IntervalMachineHaltCycle);
            }
        });
    }
    else {
        setTimeout("GetEquipmentHaltStatus()", IntervalMachineHaltCycle);
    }
}
function SetEquipmentStatus() {
    if (BlinkFlag == true) {
        BlinkFlag = false;
        if (RuntimeData != null) {
            $.each(RuntimeData, function (Key, Value) {
                var m_Obj = $(document.getElementById(Key));
                if (m_Obj != undefined && m_Obj != null) {
                    if (m_Obj.data("options").ValidValues.toLowerCase() != Value.toLowerCase()) {           //判断信号是否运行
                        if (m_Obj.attr("src") != "images/page/GreenButton.png") {
                            m_Obj.attr("src", "images/page/GreenButton.png");
                        }
                    }
                    else {
                        if (m_Obj.attr("src") != "images/page/RedButton.png") {
                            m_Obj.attr("src", "images/page/RedButton.png");
                        }
                    }
                }
            });
        }
    }
    else {
        BlinkFlag = true;
        if (RuntimeData != null && MachinHaltData != null) {
            $.each(MachinHaltData, function (Key, Value) {
                var m_Obj = $(document.getElementById(Key));
                if (m_Obj != undefined && m_Obj != null) {
                    if (m_Obj.data("options").ValidValues.toLowerCase() != RuntimeData[Key].toLowerCase()) {           //判断信号是否运行
                        m_Obj.attr("src", "images/page/GrayButton.png");
                    }
                }
            });
        }
    }
}
function GetEquipmentInfo(myObj, myCompanyName) {
    //alert($(myObj).data("options").EquipmentId + "|||" + $(myObj).data("options").FactoryOrganizationId);
    $('#dlg_MachineHaltRecord').dialog('setTitle', myCompanyName + ">>" + $(myObj).data("options").EquipmentName);
    $.ajax({
        type: "POST",
        url: "Monitor_MainMachineRuntimeStatus.aspx/GetMachineHaltRecord",
        data: "{myOrganizationId:'" + $(myObj).data("options").FactoryOrganizationId + "',myEquipmentId:'" + $(myObj).data("options").EquipmentId + "'}",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            var m_MsgData = jQuery.parseJSON(msg.d);
            if (m_MsgData != null && m_MsgData != undefined) {
                $('#grid_MachineHaltRecord').datagrid('loadData', m_MsgData);
                $('#dlg_MachineHaltRecord').dialog('open');
            }
            setTimeout("GetEquipmentRuntimeStatus()", IntervalRuntimeCycle);
        },
        error: function (msg) {
            setTimeout("GetEquipmentRuntimeStatus()", IntervalRuntimeCycle);
        }
    });
}

function loadMachineHaltlDialog() {
    $('#dlg_MachineHaltRecord').dialog({
        title: '停机记录',
        width: 520,
        height: 320,
        left: 300,
        top: 20,
        closed: true,
        cache: false,
        modal: true,
        iconCls: 'icon-search',
        resizable: false
    });
}
function SetDivPosization(myWidth) {
    var m_ContentDivWidth = $('#TableContentDiv').width() + 30;
    if (myWidth > m_ContentDivWidth) {           //当窗口比div大
        $('#TableContentDiv').css('margin-left', parseInt((myWidth - m_ContentDivWidth) / 2));
    }
    else {
        $('#TableContentDiv').css('margin-left', 0);
    }
}