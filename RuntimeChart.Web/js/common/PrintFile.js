
function doPrint(myTableString) {

    var m_Div = $('<div></div');
    var m_TableObj = $(myTableString);
    m_TableObj.appendTo(m_Div);
    //m_Div.append(myTableString);

    //m_PrintDialog.document.write(tableString);
    
    //var m_Options = { mode: 'popup', popClose: false};
    //m_Div.printArea();
    m_Div.jqprint();
}

function PrintHtml(myDataTable) {
    var m_TableString = myDataTable;
    doPrint(m_TableString);
}

function ChangeToTable(printDatagrid) {
    var tableString = '<table cellspacing="0" class="pb">';
    var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // 得到frozenColumns对象
    var columns = printDatagrid.datagrid("options").columns;    // 得到columns对象
    var nameList = new Array();

    // 载入title
    if (typeof columns != 'undefined' && columns != '') {
        $(columns).each(function (index) {
            tableString += '\n<tr>';
            if (typeof frozenColumns != 'undefined' && typeof frozenColumns[index] != 'undefined') {
                for (var i = 0; i < frozenColumns[index].length; ++i) {
                    if (!frozenColumns[index][i].hidden) {
                        tableString += '\n<th width="' + frozenColumns[index][i].width + '"';
                        if (typeof frozenColumns[index][i].rowspan != 'undefined' && frozenColumns[index][i].rowspan > 1) {
                            tableString += ' rowspan="' + frozenColumns[index][i].rowspan + '"';
                        }
                        if (typeof frozenColumns[index][i].colspan != 'undefined' && frozenColumns[index][i].colspan > 1) {
                            tableString += ' colspan="' + frozenColumns[index][i].colspan + '"';
                        }
                        if (typeof frozenColumns[index][i].field != 'undefined' && frozenColumns[index][i].field != '') {
                            nameList.push(frozenColumns[index][i]);
                        }
                        tableString += '>' + frozenColumns[0][i].title + '</th>';
                    }
                }
            }
            for (var i = 0; i < columns[index].length; ++i) {
                if (!columns[index][i].hidden) {
                    tableString += '\n<th width="' + columns[index][i].width + '"';
                    if (typeof columns[index][i].rowspan != 'undefined' && columns[index][i].rowspan > 1) {
                        tableString += ' rowspan="' + columns[index][i].rowspan + '"';
                    }
                    if (typeof columns[index][i].colspan != 'undefined' && columns[index][i].colspan > 1) {
                        tableString += ' colspan="' + columns[index][i].colspan + '"';
                    }
                    if (typeof columns[index][i].field != 'undefined' && columns[index][i].field != '') {
                        nameList.push(columns[index][i]);
                    }
                    tableString += '>' + columns[index][i].title + '</th>';
                }
            }
            tableString += '\n</tr>';
        });
    }
    // 载入内容
    var rows = printDatagrid.datagrid("getRows"); // 这段代码是获取当前页的所有行
    for (var i = 0; i < rows.length; ++i) {
        tableString += '\n<tr>';
        for (var j = 0; j < nameList.length; ++j) {
            var e = nameList[j].field.lastIndexOf('_0');

            tableString += '\n<td';
            if (nameList[j].align != 'undefined' && nameList[j].align != '') {
                tableString += ' style="text-align:' + nameList[j].align + ';"';
            }
            tableString += '>';
            if (e + 2 == nameList[j].field.length) {
                tableString += rows[i][nameList[j].field.substring(0, e)];
            }
            else
                tableString += rows[i][nameList[j].field];
            tableString += '</td>';
        }
        tableString += '\n</tr>';
    }
    tableString += '\n</table>';
    return tableString;
}

///////////////////////获得DataGrid的Html//////////////////////
function GetDataGridTableHtml(myDataGridId, myTitleName, mySelectDatetime)
{
    var m_HiddenFieldColumn = [];
    var m_DataGridFrozenColumnsObj = $('#' + myDataGridId).datagrid("options")["frozenColumns"].length != 0 ? $('#' + myDataGridId).datagrid("options")["frozenColumns"] : [];
    var m_DataGridColumnsObj = $('#' + myDataGridId).datagrid("options")["columns"].length != 0 ? $('#' + myDataGridId).datagrid("options")["columns"] : [];

    var m_DataGridTableHtml = "";
    var m_frozenColumnsCount = 0;
    var m_ColumnsCount = 0;
    var m_TitleSpan = 0;
    var m_ColumnNamesHtml = "<tr>";
    //////////////////////////字段名////////////////////
    var m_ColumnRowSize = 0;
    if (m_DataGridFrozenColumnsObj.length > 0) {
        m_ColumnRowSize = m_DataGridFrozenColumnsObj.length;
    }
    else if (m_DataGridColumnsObj.length > 0) {
        m_ColumnRowSize = m_DataGridColumnsObj.length;
    }
    for (var i = 0; i < m_ColumnRowSize; i++) {
        if (m_DataGridFrozenColumnsObj.length!=0) {
            for (var j = 0; j < m_DataGridFrozenColumnsObj[i].length; j++) {
                if (m_DataGridFrozenColumnsObj[i][j]["hidden"] != true) {    //只导出显示出来的数据
                    var m_RowSpan = m_DataGridFrozenColumnsObj[i][j]["rowspan"] == undefined ? 1 : m_DataGridFrozenColumnsObj[i][j]["rowspan"];
                    var m_ColSpan = m_DataGridFrozenColumnsObj[i][j]["colspan"] == undefined ? 1 : m_DataGridFrozenColumnsObj[i][j]["colspan"];
                    if (i == 0) {                    //记录冻结列一共多少列
                        m_frozenColumnsCount = m_frozenColumnsCount + m_ColSpan;
                    }
                    var m_CellItem = '<td';
                    if (m_ColSpan > 1) {
                        m_CellItem = m_CellItem + ' colspan=' + m_ColSpan;
                    }
                    if (m_RowSpan > 1) {
                        m_CellItem = m_CellItem + ' rowspan=' + m_RowSpan;
                    }
                    m_CellItem = m_CellItem + ' style = "border:0.1pt solid black; text-align:center;">';
                    m_ColumnNamesHtml = m_ColumnNamesHtml + m_CellItem + m_DataGridFrozenColumnsObj[i][j]["title"] + '</td>';
                }
                else {
                    if (m_DataGridFrozenColumnsObj[i][j]["field"] != undefined && m_DataGridFrozenColumnsObj[i][j]["field"] != "") {
                        m_HiddenFieldColumn.push(m_DataGridFrozenColumnsObj[i][j]["field"]);
                    }
                }
            }
        }
        if (m_DataGridColumnsObj.length!=0) {
            for (var j = 0; j < m_DataGridColumnsObj[i].length; j++) {
                if (m_DataGridColumnsObj[i][j]["hidden"] != true) {

                    var m_RowSpan = m_DataGridColumnsObj[i][j]["rowspan"] == undefined ? 1 : m_DataGridColumnsObj[i][j]["rowspan"];
                    var m_ColSpan = m_DataGridColumnsObj[i][j]["colspan"] == undefined ? 1 : m_DataGridColumnsObj[i][j]["colspan"];
                    if (i == 0) {                    //记录冻结列一共多少列
                        m_ColumnsCount = m_ColumnsCount + m_ColSpan;
                    }
                    var m_CellItem = '<td';
                    if (m_ColSpan > 1) {
                        m_CellItem = m_CellItem + ' colspan=' + m_ColSpan;
                    }
                    if (m_RowSpan > 1) {
                        m_CellItem = m_CellItem + ' rowspan=' + m_RowSpan;
                    }
                    m_CellItem = m_CellItem + ' style = "border:0.1pt solid black; text-align:center;">';
                    m_ColumnNamesHtml = m_ColumnNamesHtml + m_CellItem + m_DataGridColumnsObj[i][j]["title"] + '</td>';
                }
                else {
                    if (m_DataGridColumnsObj[i][j]["field"] != undefined && m_DataGridColumnsObj[i][j]["field"] != "") {
                        m_HiddenFieldColumn.push(m_DataGridColumnsObj[i][j]["field"]);
                    }
                }
            }
        }
        m_ColumnNamesHtml = m_ColumnNamesHtml + "</tr>";
    }

    ////////////////////////行数据//////////////////////
    //var m_DataGridRowsObj = $('#' + myDataGridId).datagrid("options")["columns"].length != 0 ? $('#' + myDataGridId).datagrid("options")["columns"] : []; 
    var m_DataGridDataObj = $('#' + myDataGridId).datagrid("getRows");
    for (var i = 0; i < m_DataGridDataObj.length; i++) {
        m_ColumnNamesHtml = m_ColumnNamesHtml + "<tr>";

        for (key in m_DataGridDataObj[i]) {
            if (!CheckHiddenColumn(key, m_HiddenFieldColumn)) {
                m_ColumnNamesHtml = m_ColumnNamesHtml + '<td style = "border:0.1pt solid black; text-align:center;">' + m_DataGridDataObj[i][key] + '</td>';
            }
        }
        m_ColumnNamesHtml = m_ColumnNamesHtml + "</tr>";
    }
    ///////////////////////////////////////////////////////获得excel表的title
    m_TitleSpan = m_frozenColumnsCount + m_ColumnsCount;
    var m_TitleHtml = '<tr><td colspan = ' + m_TitleSpan + ' style = "font-size:18pt; text-align:center; font-weight:bold;">' + myTitleName + '</td></tr>';
    m_TitleHtml = m_TitleHtml + '<tr><td colspan = ' + m_TitleSpan + ' style = "text-align:center; "> 统计日期: ' + mySelectDatetime + '</td></tr>';

    return m_DataGridTableHtml = '<table style = "border:0px;margin:0px;border-collapse:collapse;border-spacing:0px;padding:0px;">' + m_TitleHtml + m_ColumnNamesHtml + '</table>';

}

///////////////////////获得DataGrid的Html//////////////////////
function GetDataGridTableHtmlSplitColumn(myDataGridId, myTitleName, mySelectDatetime, mySplitWord, myFirstDisplayColumn)
{

    var m_HiddenFieldColumn = [];
    var m_DataGridDataObj = jQuery.extend(true, {}, $('#' + myDataGridId).datagrid("getRows"));
    
    /////////////////////增加一段程序在导出时判断第一列是否有'>>'如果有则拆分列////////////////////
    var m_RowNameArray = [];
    var m_MaxRowNameSize = 0;
    $.each(m_DataGridDataObj, function (Index, value) {                  //找出存在>>的第一个字段，分割后存入数组
        var m_RowName = value[myFirstDisplayColumn];
        var m_ExtendColumn = m_RowName.split(mySplitWord)
        if (m_ExtendColumn.length > 0) {
            m_RowNameArray.push(m_ExtendColumn);
            m_HiddenFieldColumn.push(myFirstDisplayColumn);
        }
    });
    $.each(m_RowNameArray, function (Index, value) {                  //找出存在>>的第一个字段，分割后存入数组
        if (m_MaxRowNameSize < value.length) {                        //找出最多的列
            m_MaxRowNameSize = value.length;
        }
    });

    /////////////////////////////////////////////////////////////////////////////////////////////


    var m_DataGridFrozenColumnsObj = $('#' + myDataGridId).datagrid("options")["frozenColumns"].length != 0 ? $('#' + myDataGridId).datagrid("options")["frozenColumns"] : [];
    var m_DataGridColumnsObj = $('#' + myDataGridId).datagrid("options")["columns"].length != 0 ? $('#' + myDataGridId).datagrid("options")["columns"] : [];

    var m_DataGridTableHtml = "";
    var m_frozenColumnsCount = 0;
    var m_ColumnsCount = 0;
    var m_TitleSpan = 0;
    var m_ColumnNamesHtml = "<tr>";
    //////////////////////////字段名////////////////////
    var m_ColumnRowSize = 0;
    if (m_DataGridFrozenColumnsObj.length > 0) {
        m_ColumnRowSize = m_DataGridFrozenColumnsObj.length;
    }
    else if (m_DataGridColumnsObj.length > 0) {
        m_ColumnRowSize = m_DataGridColumnsObj.length;
    }
    for (var i = 0; i < m_ColumnRowSize; i++) {
        var m_RowSpanCount = 0;
        var m_IsFirstColumn = true;             //是否是每行的第一列
        //var m_RowNameExtend = '<td style = "border:0.1pt solid black; text-align:center;"></td>';
        if (m_DataGridFrozenColumnsObj.length > 0) {
            for (var j = 0; j < m_DataGridFrozenColumnsObj[i].length; j++) {
                if (m_DataGridFrozenColumnsObj[i][j]["hidden"] != true) {    //只导出显示出来的数据
                    var m_RowSpan = m_DataGridFrozenColumnsObj[i][j]["rowspan"] == undefined ? 1 : m_DataGridFrozenColumnsObj[i][j]["rowspan"];
                    var m_ColSpan = m_DataGridFrozenColumnsObj[i][j]["colspan"] == undefined ? 1 : m_DataGridFrozenColumnsObj[i][j]["colspan"];
                    if (i == 0) {                    //记录冻结列一共多少列
                        m_frozenColumnsCount = m_frozenColumnsCount + m_ColSpan;
                    }
                    var m_CellItem = '<td';
                    if (m_IsFirstColumn == true) {                       //当第一列的情况
                        if (m_ColSpan > 1) {                             //有合并单元可
                            m_CellItem = m_CellItem + ' colspan=' + m_ColSpan + m_MaxRowNameSize;
                        }
                        else {
                            m_CellItem = m_CellItem + ' colspan=' + m_MaxRowNameSize;
                        }

                        if (m_RowSpan > 1) {
                            m_CellItem = m_CellItem + ' rowspan=' + m_RowSpan;
                            if (m_RowSpanCount == i)                        //当时合并行的时候如果不相等，则说明该列需要跳过
                            {
                                m_RowSpanCount = m_RowSpanCount + m_RowSpan;
                            }
                        }
                        else {
                            m_RowSpanCount = m_RowSpanCount + 1;
                        }
                        m_IsFirstColumn = false;
                    }
                    else {
                        if (m_ColSpan > 1) {
                            m_CellItem = m_CellItem + ' colspan=' + m_ColSpan;
                        }
                        if (m_RowSpan > 1) {
                            m_CellItem = m_CellItem + ' rowspan=' + m_RowSpan;
                        }
                    }
                    m_CellItem = m_CellItem + ' style = "border:0.1pt solid black; text-align:center;">';
                    m_ColumnNamesHtml = m_ColumnNamesHtml + m_CellItem + m_DataGridFrozenColumnsObj[i][j]["title"] + '</td>';
                }
                else {
                    if (m_DataGridFrozenColumnsObj[i][j]["field"] != undefined && m_DataGridFrozenColumnsObj[i][j]["field"] != "") {
                        m_HiddenFieldColumn.push(m_DataGridFrozenColumnsObj[i][j]["field"]);
                    }
                }
            }
        }
        if (m_DataGridColumnsObj.length > 0) {
            for (var j = 0; j < m_DataGridColumnsObj[i].length; j++) {
                if (m_DataGridColumnsObj[i][j]["hidden"] != true) {

                    var m_RowSpan = m_DataGridColumnsObj[i][j]["rowspan"] == undefined ? 1 : m_DataGridColumnsObj[i][j]["rowspan"];
                    var m_ColSpan = m_DataGridColumnsObj[i][j]["colspan"] == undefined ? 1 : m_DataGridColumnsObj[i][j]["colspan"];
                    if (i == 0) {                    //记录冻结列一共多少列
                        m_ColumnsCount = m_ColumnsCount + m_ColSpan;
                    }
                    var m_CellItem = '<td';
                    if (m_IsFirstColumn == true) {                       //当第一列的情况
                        if (m_ColSpan > 1) {                             //有合并单元可
                            m_CellItem = m_CellItem + ' colspan=' + m_ColSpan + m_MaxRowNameSize;
                        }
                        else {
                            m_CellItem = m_CellItem + ' colspan=' + m_MaxRowNameSize;
                        }

                        if (m_RowSpan > 1) {
                            m_CellItem = m_CellItem + ' rowspan=' + m_RowSpan;
                            if (m_RowSpanCount == i)                        //当时合并行的时候如果不相等，则说明该列需要跳过
                            {
                                m_RowSpanCount = m_RowSpanCount + m_RowSpan;
                            }
                        }
                        else {
                            m_RowSpanCount = m_RowSpanCount + 1;
                        }
                        m_IsFirstColumn = false;
                    }
                    else {
                        if (m_ColSpan > 1) {
                            m_CellItem = m_CellItem + ' colspan=' + m_ColSpan;
                        }
                        if (m_RowSpan > 1) {
                            m_CellItem = m_CellItem + ' rowspan=' + m_RowSpan;
                        }
                    }
                    m_CellItem = m_CellItem + ' style = "border:0.1pt solid black; text-align:center;">';
                    m_ColumnNamesHtml = m_ColumnNamesHtml + m_CellItem + m_DataGridColumnsObj[i][j]["title"] + '</td>';
                }
                else {
                    if (m_DataGridColumnsObj[i][j]["field"] != undefined && m_DataGridColumnsObj[i][j]["field"] != "") {
                        m_HiddenFieldColumn.push(m_DataGridColumnsObj[i][j]["field"]);
                    }
                }
            }
        }
        m_ColumnNamesHtml = m_ColumnNamesHtml + "</tr>";
    }

    ////////////////////////行数据//////////////////////
    //var m_DataGridRowsObj = $('#' + myDataGridId).datagrid("options")["columns"].length != 0 ? $('#' + myDataGridId).datagrid("options")["columns"] : []; 

    $.each(m_DataGridDataObj, function (i, value) {
        m_ColumnNamesHtml = m_ColumnNamesHtml + "<tr>";
        var m_ExtendTd = "";
        for (var jd = 0; jd < m_MaxRowNameSize; jd++) {
            if (jd < m_RowNameArray[i].length) {
                m_ExtendTd = m_ExtendTd + '<td style = "border:0.1pt solid black; text-align:center;">' + m_RowNameArray[i][jd] + '</td>';
            }
            else {
                m_ExtendTd = m_ExtendTd + '<td style = "border:0.1pt solid black; text-align:center;"></td>';
            }
        }
        m_ColumnNamesHtml = m_ColumnNamesHtml + m_ExtendTd;
        for (key in m_DataGridDataObj[i]) {
            if (!CheckHiddenColumn(key, m_HiddenFieldColumn)) {
                m_ColumnNamesHtml = m_ColumnNamesHtml + '<td style = "border:0.1pt solid black; text-align:center;">' + m_DataGridDataObj[i][key] + '</td>';
            }
        }
        m_ColumnNamesHtml = m_ColumnNamesHtml + "</tr>";
    });
    ///////////////////////////////////////////////////////获得excel表的title
    if (m_MaxRowNameSize != 0) {
        m_TitleSpan = m_frozenColumnsCount + m_ColumnsCount + m_MaxRowNameSize - 1;
    }
    else {
        m_TitleSpan = m_frozenColumnsCount + m_ColumnsCount;
    }
    var m_TitleHtml = '<tr><td colspan = ' + m_TitleSpan + ' style = "font-size:18pt; text-align:center; font-weight:bold;">' + myTitleName + '</td></tr>';
    m_TitleHtml = m_TitleHtml + '<tr><td colspan = ' + m_TitleSpan + ' style = "text-align:center; "> 统计日期: ' + mySelectDatetime + '</td></tr>';

    return m_DataGridTableHtml = '<table style = "border:0px;margin:0px;border-collapse:collapse;border-spacing:0px;padding:0px;">' + m_TitleHtml + m_ColumnNamesHtml + '</table>';

}
function CheckHiddenColumn(myFieldName, myHiddenFieldNames) {
    var m_Finded = false;
    for (var i = 0; i < myHiddenFieldNames.length; i++) {
        if (myHiddenFieldNames[i] == myFieldName) {
            m_Finded = true;
            break;
        }
    }
    return m_Finded;
}


//////////////////////获得树形结构的Html///////////////////////

var MaxDepth = 0;
//TreeGrid的DomId,导出日报标题名称,树形节点名称的字段,选择的组织机构名称,选择的时间
function GetTreeTableHtml(myTreeGridDomId, myTitleName, myTreeTitleColumn, mySelectOrganizationName, mySelectDatetime) {
    var m_ColumnNamesHtml = "";

    var ValueColumns = GetTreeGridColumns(myTreeGridDomId);
    var m_NewTreeDateObj = GetTreeRootNode(myTreeGridDomId);
    var TreeHtml = GetTreeHtml(m_NewTreeDateObj, "", ValueColumns, myTreeTitleColumn);
    ////////////////////以下是标题以及列名///////////////////
    var m_ColumnNamesSpan = MaxDepth + 1;
    var m_TitleSpan = MaxDepth + 1 + ValueColumns.length;
    var m_TitleHtml = '<tr><td colspan = ' + m_TitleSpan + ' style = "font-size:18pt; text-align:center; font-weight:bold;">' + mySelectOrganizationName + myTitleName + '</td></tr>';
    m_TitleHtml = m_TitleHtml + '<tr><td colspan = ' + m_TitleSpan + ' style = "text-align:center; "> 统计日期: ' + mySelectDatetime + '</td></tr>';
    for (var i = 0; i < ValueColumns.length + 1; i++) {
        if (i == 0) {
            m_ColumnNamesHtml = '<tr><td colspan = ' + m_ColumnNamesSpan + ' style = "border:0.1pt solid black; text-align:center;">区域</td>';
        }
        else {
            m_ColumnNamesHtml = m_ColumnNamesHtml + '<td style = "border:0.1pt solid black; text-align:center;">' + ValueColumns[i - 1]["title"] + '</td>';
        }
    }
    TreeHtml = m_ColumnNamesHtml + "</tr>" + TreeHtml;
    TreeHtml = '<table style = "border:0px;margin:0px;border-collapse:collapse;border-spacing:0px;padding:0px;">' + m_TitleHtml + TreeHtml + '</table>';
    return TreeHtml;
}
function GetTreeRootNode(myTreeGridDomId) {
    var m_OldTreeDateObj = $('#' + myTreeGridDomId).treegrid("getData");
    var m_NewTreeDateObj = jQuery.extend(true, {}, m_OldTreeDateObj);   //深度克隆对象
    if (m_NewTreeDateObj != null && m_NewTreeDateObj != undefined && m_NewTreeDateObj.length != 0) {
        $.each(m_NewTreeDateObj, function (i, value) {
            //this;      //this指向当前元素
            //i;         //i表示Array当前下标
            //value;     //value表示Array当前元素
            value["Depth"] = 0;         //深度
            value["AllChildrenCount"] = GetSubTreeNode(0, value);     //所有的孩子总数，包括所有子节点以及子节点的孩子节点
        });
    }
    return m_NewTreeDateObj;
}
function GetSubTreeNode(myDepth, myParentTreeNode) {
    var m_Nodes = myParentTreeNode["children"];
    var m_NodesCount = 0;
    if (m_Nodes != null && m_Nodes != undefined && m_Nodes.length != 0) {
        myParentTreeNode["IsLeaf"] = false;
        m_NodesCount = m_Nodes.length;
        $.each(m_Nodes, function (i, value) {
            value["Depth"] = myDepth + 1;         //深度
            value["AllChildrenCount"] = GetSubTreeNode(myDepth + 1, value);     //所有的孩子总数，包括所有子节点以及子节点的孩子节点
            m_NodesCount = m_NodesCount + value["AllChildrenCount"];
        });
    }
    else {
        myParentTreeNode["IsLeaf"] = true;
        if (myParentTreeNode["Depth"] > MaxDepth) {
            MaxDepth = myParentTreeNode["Depth"];
        }
    }
    return m_NodesCount;
}

function GetTreeHtml(myTreeDateObj, myHtmlString, myValueColumns, myTreeTitleColumns) {
    var m_HtmlString = "";
    if (myTreeDateObj != null && myTreeDateObj != undefined && myTreeDateObj.length != 0) {
        $.each(myTreeDateObj, function (i, value) {
            //this;      //this指向当前元素
            //i;         //i表示Array当前下标
            //value;     //value表示Array当前元素
            if (value["IsLeaf"] != true) {
                m_HtmlString = m_HtmlString + "<tr>" + GetNodeTreeData(value, myTreeTitleColumns, myValueColumns) + "</tr>";
                m_HtmlString = m_HtmlString + GetTreeHtml(value["children"], m_HtmlString, myValueColumns, myTreeTitleColumns);
            }
            else {
                m_HtmlString = m_HtmlString + "<tr>" + GetLeafTreeData(value, myTreeTitleColumns, myValueColumns) + "</tr>";
            }
        });
    }
    return m_HtmlString;
}
function GetNodeTreeData(myNode, myTitleColumnName, myValueColumns) {
    var m_ColumnString = "";
    var m_AllChildrenCount = myNode["AllChildrenCount"] + 1;
    for (var i = myNode["Depth"]; i <= MaxDepth; i++) {
        if (i == myNode["Depth"]) {
            m_ColumnString = m_ColumnString + '<td rowspan = ' + m_AllChildrenCount + ' style = "border:0.1pt solid black;">' + myNode[myTitleColumnName] + '</td>';
        }
        else {
            m_ColumnString = m_ColumnString + '<td style = "border:0.1pt solid black;"></td>';
        }
    }
    for (var i = 0; i < myValueColumns.length; i++) {
        m_ColumnString = m_ColumnString + '<td style = "border:0.1pt solid black;">' + myNode[myValueColumns[i]["field"]] + '</td>';
    }
    return m_ColumnString;
}
function GetLeafTreeData(myNode, myTitleColumnName, myValueColumns) {
    var m_ColumnString = "";
    for (var i = myNode["Depth"]; i <= MaxDepth; i++) {
        if (i == myNode["Depth"]) {
            m_ColumnString = m_ColumnString + '<td style = "border:0.1pt solid black;">' + myNode[myTitleColumnName] + '</td>';
        }
        else {
            m_ColumnString = m_ColumnString + '<td style = "border:0.1pt solid black;"></td>';
        }
    }
    for (var i = 0; i < myValueColumns.length; i++) {
        m_ColumnString = m_ColumnString + '<td style = "border:0.1pt solid black;">' + myNode[myValueColumns[i]["field"]] + '</td>';
    }
    return m_ColumnString;
}
function GetTreeGridColumns(myTreeGridDomId) {
    var m_OldTreeDateObj = $('#' + myTreeGridDomId).treegrid("options")["columns"];
    var m_ValueCloumns = [];
    $.each(m_OldTreeDateObj[0], function (i, value) {
        //this;      //this指向当前元素
        //i;         //i表示Array当前下标
        //value;     //value表示Array当前元素
        if (value["hidden"] != true) {
            m_ValueCloumns.push({ "field": value["field"], "title": value["title"] });
        }
    });
    return m_ValueCloumns;
}
