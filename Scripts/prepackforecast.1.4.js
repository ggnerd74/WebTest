
var primarygridheight = $(document).height() * 0.75;


function RefreshGrid() {
    //clear previous kendoGrid if any
    var grid = $('#maingrid').data("kendoGrid");
    if (grid !== undefined)
        grid.destroy();
    $('#maingrid').empty();

    initGrid();
}

var editmode = true;

var currentmodel;
var currentcell;

function forecastvaluetemplate(dataItem) {
    return "<div class='editcelldiv'>" + dataItem.forecastvalue + "</div>"
}

function actualvaluetemplate(dataItem) {
    return "<div class='editcelldiv'>" + dataItem.actualvalue + "</div>"
}

function startingqohtemplate(dataItem) {
    return "<div class='editcelldiv'>" + dataItem.startingqoh + "</div>"
}

function initGrid() {
    $('#maingrid').kendoGrid({
        columns: [
            { field: "productcode", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>Product</span><br/><span>Code</span>", title: "Product Code", width: "115px", locked: true, stickable: true},
            { field: "um", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, title: "UM", width: "75px", locked: true, stickable: true },
            { field: "ProductDescription", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>Product</span><br/><span>Description</span>", title: "description", width: "290px", locked: true, stickable: true },
            { field: "startingqoh", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>Starting</span><br/><span>QOH</span>", title: "startingqoh", width: "120px", template: startingqohtemplate, stickable: true },
            { field: "forecastvalue", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>Planned</span><br/><span>Production</span>", title: "forecastvalue", width: "120px", template: forecastvaluetemplate, stickable: true },
            { field: "actualvalue", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>Actual</span><br/><span>Production</span>", title: "actualvalue", width: "120px", template: actualvaluetemplate, stickable: true },
            { field: "lastmodified", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>Last</span><br/><span>Modified</span><br/><span>Date</span>", title: "lastmodified", width: "115px", format: "{0:MM-dd-yy}", stickable: true  },
            { field: "username", headerAttributes: { style: "text-align: center;" }, attributes: { style: "text-align: center;" }, headerTemplate: "<span>User</span><br/><span>Name</span>", title: "username", width: "115px", stickable: true }
        ],
        height: primarygridheight,
        scrollable: true,
        sortable: true,
        pageable: true,
        groupable: true,
        columnMenu: {
            columns: false
        },
        filterable: true,
        reorderable: true,
        resizable: true,
        dataSource: {
            transport: {
                read: {
                    url: "/IntraCo/GetPrepackForecastGridSet",
                    dataType: "json",
                    cache: false
                },
                update: {
                    url: "/IntraCo/PrepackForecastUpdate",
                    dataType: "json",
                    type: "post",
                    cache: false
                },
                create: {
                    url: "/IntraCo/PrepackForecastUpdate",
                    dataType: "json",
                    type: "post",
                    cache: false
                },
                parameterMap: function (data, t) {

                    if (t == "read") {

                        return {
                            page: data.page,
                            pageSize: data.pageSize,
                            skip: data.skip,
                            take: data.take,
                            forecastdate: $('#forecastdate').val()
                        };
                    }

                    if (data.forecastdate) {
                        var datestring_i = kendo.toString(data.forecastdate, "MM-dd-yyyy h:mm:ss tt");
                        data.forecastdate = datestring_i;
                    }

                    if (data.lastmodified) {
                        var datestring_lr = kendo.toString(data.lastmodified, "MM-dd-yyyy h:mm:ss tt");
                        data.lastmodified = datestring_lr;
                    }

                    return data;

                }
            },
            requestEnd: function (e) {
                var response = e.response;
                var type = e.type;
                if (type === "create" || type === "update") {
                    if (response.update) {
                        currentmodel.rid = response.rid;
                    }
                }
            },
            batch: false,
            autoSync: true,
            serverPaging: true,
            pageSize: 50,
            schema: {
                data: "items",
                total: "total",
                model: {
                    id: "rid",
                    fields: {
                        rid: {
                            type: "number",
                            editable: true
                        },
                        forecastdate: {
                            type: "date",
                            editable: false
                        },
                        productcode: {
                            type: "string",
                            editable: false
                        },
                        um: {
                            type: "string",
                            editable: false
                        },
                        productDescription: {
                            type: "string",
                            editable: false
                        },
                        forecastvalue: {
                            type: "number",
                            editable: true
                        },
                        actualvalue: {
                            type: "number",
                            editable: true
                        },
                        lastmodified: {
                            type: "date",
                            editable: false
                        },
                        username: {
                            type: "string",
                            editable: false
                        },
                        startingqoh: {
                            type: "number",
                            editable: true
                        }

                    }
                }
            }
        },
        editable: true,
        cancel: RefreshGrid,
        edit: function (e) {
            currentmodel = e.model;
            currentcell = e.container;
        }

    });
}


$(function () {
   
    $('.k-dialog').css("border", "2px solid black");
    
    $('#toptoolbar').kendoToolBar({
        items: [
            {
                template: '<div><label>Forecast Date:</label> <input id="forecastdate" type="text" /></div>', overflow: "never"
            },
            {
                template: '<div style="margin-left: 10px;"></div>'
            }
        ]
    });

    var todayDate = kendo.toString(kendo.parseDate(new Date()), 'MM-dd-yyyy');

    $('#forecastdate').kendoDatePicker({
        value: todayDate,
        format: "MM-dd-yyyy",
        change: function () {
            RefreshGrid();
        }
    });

    RefreshGrid(); 

});

