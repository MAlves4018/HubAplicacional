
import { Modal } from 'bootstrap';


var MenuTree = function (options) {

    /* ******************************************
     * private members */
    var member = {
        container: null,
        nodeRenderCallback: function (node) {
            var txt = "<p><strong>" + JSON.stringify($(node).data("item")) + "</strong></p>";
            $(node).children('div').first().children('.tree-node-header-label').first().html(txt);
            return true; /* false would prevent this node to be added to the tree*/
        },
        nodeDblclickCallback: function (node) { },
        nodeDeleteCallback: function (node) { return true },/* false would prevent this node from being removed */
        nodeReceiveCallback: function (node, target) { return true },
    };

    var that = this;


    function construct(options) {
        $.extend(member, options);

        if (member.container != null)
            member.container = (member.container instanceof $) ? member.container : $(member.container);
    };

    /**
     * create a new DOM item using "nodeData" and append to node parent
     * 
     * @param {any} i
     * @param {any} nodeData
     * @param {any} parent
     */
    function appendNode(i, nodeData, parent) {

        var li = $("<li>").addClass("tree-node");
        $(li).data("item",nodeData["Node"]);

        var title = $("<div>")
            .addClass("tree-node-header") 
            .append("<span class='expandable open'><i class='fa-solid fa-angle-right'></i></span>")
            //.append("<span class='expandable' style='display: none;'><i class='fa-solid fa-angle-right'></i></span>")
            .append('<a><i class="fa-solid fa-up-down-left-right"></i></a>')
            .append('<button type="button" class="del-item close text-danger" >&times;</button></span><div class="tree-node-header-label"></div>');

        var ul = $("<ul>").addClass("space");
        $(li).append($(title));
        $(li).append($(ul));

        if (member.nodeRenderCallback($(li))) {
            parent.append($(li));
            $(nodeData["Children"]).each(function (j, perm) { appendNode(j, perm, ul); });
        }
    }


    /**
     * 
     * toggle the expand button checking if each node has children nodes
     */
    function checkExpandableState() { 
        member.container.find(".expandable").show();
        member.container.find("ul:empty").each(function (index, element) { $(this).prev().children().first().hide(); });
    }

    /* recursively calculate the Width all titles */
    function updateTreeWidth(obj) {

        var titles = $(obj).siblings('.space').children('.tree-node').children('.tree-node-header');
        var pTitleWidth = parseInt($(obj).css('width'));
        var leftOffset = parseInt($(obj).siblings('.space').css('margin-left'));
        var newWidth = pTitleWidth - leftOffset;

        $(titles).each(function (index, element) {
            $(element).css({
                'width': newWidth,
            });
            updateTreeWidth(element);
        });
    }

 

    function enableNodeEvents() {
        //member.container.disableSelection(); 
        member.container.find(".space").each(function () {

            $(this).sortable({
                connectWith: '.space',
                handle:'.tree-node-header a',
                // placeholder: ....,
                //tolerance: 'intersect',
                placeholder: "ui-sortable-placeholder",
                revert: 200,
                receive: function (event, ui) {
                    member.nodeReceiveCallback(ui.item, $(event.target).parent());
                    that.updateTreeWidth();
                    checkExpandableState();
                },
            });
        }); 
        /* enable toggle child items */
        member.container.find('.expandable').unbind('click').click(function () { 
            $(this).parent().next().toggle();
            $(this).parent().find('.expandable').toggleClass("open");
        });

        /* enable delete items */
        member.container.find('.del-item').unbind('click').click(function () {
            var item = $(this).parent().parent();

            if (member.nodeDeleteCallback(item)) {
                $(item).remove();
                checkExpandableState();
            }
        });

        member.container.find(".tree-node-header").unbind('dblclick').dblclick(function () {
            member.nodeDblclickCallback($(this).parent());
        });
    }

    function getNodeData(node) {
        var Children = [];
        $(node).children('ul').first().children('.tree-node').each(function (i, child) { Children.push(getNodeData(child)); });
        return { 'Node': $(node).data("item"), 'Children': Children };
    }

    /* ******************************************
     * public members*/


    /** 
     * Append a new node to the root node, using the given data
     * @param {any} data
     */
    this.appendNode = function (data) {

        appendNode(0, data, member.container.children('.tree').first());
        enableNodeEvents();
        checkExpandableState();
        that.updateTreeWidth();
    };

    this.renderNode = function (node) {
        member.nodeRenderCallback(node)
    };

    this.enableEvents = function () {
        window.onresize = function (event) {
            that.updateTreeWidth();
        };
        enableNodeEvents();
    };

    this.updateTreeWidth = function () {
        updateTreeWidth(member.container.children(".tree-header").first());
    };

    this.getTreeData = function () {
        var rootDataObject = getNodeData(member.container);
        return rootDataObject['Children'];
    };

    /* call constructor */
    construct(options);
};


$(function () {

    function createUuid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }

    var tree = new MenuTree({
        container: '#tree-container',
        nodeRenderCallback: function (node) {
            var nodeData = $(node).data("item");
            var IsExternal = nodeData["IsExternal"];
            $(node).attr("IsExternal", IsExternal);
            $(node).attr("Visible", nodeData["Visible"]);
            var txt = "<div><strong>" + nodeData['Name'] + "</strong></div>";
            if (IsExternal) {
                txt += "<div><strong><em>Link externo para : </em></strong>" + nodeData["ExternalUrl"] + "</div>";
            }
            else {
                //txt += "<div><strong><em>Área: </em></strong>" + nodeData["Area"]
                //    + "</div><div><strong><em>Controlador: </em></strong>" + nodeData["ControllerName"]
                //    + "</div><div><strong><em>Comando: </em></strong>" + nodeData["ActionName"] + "</div>";
                txt += "</div><div><strong><em>Controlador: </em></strong>" + nodeData["ControllerName"]
                    + "</div><div><strong><em>Comando: </em></strong>" + nodeData["ActionName"] + "</div>";
            }
            $(node).children('div').first().children('.tree-node-header-label').first().html(txt);
            return true;
        },
        nodeDblclickCallback: function (node) {
            $('#edt-dlg-box').data("item", node);
            //$('#edt-dlg-box').modal('show');  /* true false 'toggle' 'hide' */
            let myModalEl = document.getElementById('edt-dlg-box');
            var myModal = new Modal(myModalEl);
            myModal.show();
        },
        nodeDeleteCallback: function (node) { return confirm("Quer mesmo apagar o item \n " + $(node).data("item")['Name']); },
    });

    tree.enableEvents();

    /* enable create items */
    $('#new-node-btn').click(function () {
        var uuid = createUuid();
        var permission = {
            "Node": {
                "Id": uuid, "Name": "new", "ParentMenuId": "",  
                "ControllerName": "", "ActionName": "", "IsExternal": false, "ExternalUrl": "",
                "DisplayOrder": 0, "Permitted": false, "Visible": false
            }, "Children": []
        };

        tree.appendNode(permission);
        Toast.create("Novo registo", "Foi adicionado um novo registo ao final da lista", TOAST_STATUS.SUCCESS, 2000);
    });

    var tableData = JSON.parse($("#Menus").attr('value'));
    $(tableData).each(function (i, data) { tree.appendNode(data); });

    ///* callback foe when the edit box is about to be shown */
    //$("#edt-dlg-box").on('show.bs.modal', function (e) {  /*  shown.bs.modal show.bs.modal hide.bs.modal hidden.bs.modal  */
    //    var item = $(this).data("item").data("item"); 
    //    $("#edt-Name").val(item["Name"]);
    //    $("#edt-Area").val(item["Area"]);
    //    $("#edt-ControllerName").val(item["ControllerName"]);
    //    $("#edt-ActionName").val(item["ActionName"]);
    //    $("#edt-ExternalUrl").val(item["ExternalUrl"]);
    //    $("#edt-Visible").prop("checked", item["Visible"]); //$('#textbox1').val(this.checked)
    //    $("#edt-IsExternal").prop("checked", item["IsExternal"]);
    //});

    document.getElementById('edt-dlg-box').addEventListener('show.bs.modal', function (e) {
        var item = $(this).data("item").data("item"); 
        $("#edt-Name").val(item["Name"]);
        //$("#edt-Area").val(item["Area"]);
        $("#edt-ControllerName").val(item["ControllerName"]);
        $("#edt-ActionName").val(item["ActionName"]);
        $("#edt-ExternalUrl").val(item["ExternalUrl"]);
        $("#edt-Visible").prop("checked", item["Visible"]); //$('#textbox1').val(this.checked)
        $("#edt-IsExternal").prop("checked", item["IsExternal"]);
        $("#edt-NotAnActionOrController").prop("checked", item["NotAnActionOrController"]);
    })


    $("#edt-btn-ok").click(function () {
        var item = $("#edt-dlg-box").data("item");
        var itemdata = $(item).data("item");

        itemdata["Name"] = $("#edt-Name").val();
        //itemdata["Area"] = $("#edt-Area").val();
        itemdata["ControllerName"] = $("#edt-ControllerName").val();
        itemdata["ActionName"] = $("#edt-ActionName").val();
        itemdata["ExternalUrl"] = $("#edt-ExternalUrl").val();
        itemdata["Visible"] = $("#edt-Visible").is(':checked');
        itemdata["IsExternal"] = $("#edt-IsExternal").is(':checked');
        itemdata["NotAnActionOrController"] = $("#edt-NotAnActionOrController").is(':checked');

        tree.renderNode(item);
    });

    /* enable create items */
    $('#btn-submit').click(function () {
        if (!confirm("Quer guardar as alterações efectuadas ?"))
            return false;

        var rootList = tree.getTreeData();

        function fixData(parentId, children) {
            var list = [];
            children.forEach(function (child, index) {
                child['Node']['ParentMenuId'] = parentId;
                child['Node']['DisplayOrder'] = index;
                list.push({ 'Node': child['Node'], 'Children': fixData(child['Node']['Id'], child['Children']) });
            });
            return list;
        }

        rootList = fixData("", rootList);

        var jsonvalue = JSON.stringify(rootList);
        $("#Menus").attr('value', jsonvalue);
         

        return true;
    });
 

});

