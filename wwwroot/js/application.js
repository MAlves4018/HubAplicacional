
$(document).ready(function () {
    $('table').DataTable({
        "language": {
            "url": "../json/datatable-pt-pt.json"
        },
        "columnDefs": [
            { "width": "15%", "targets": -1 }
        ],
        dom: 'Blfrtip',
        buttons: [
            'copyHtml5',
            'excelHtml5',
            'csvHtml5'
        ],
        responsive: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "Todos"]]
    });
    $('select').select2({
        theme: "bootstrap-5"
    });

    // Destruir select2 para filtros
    $('#id_objetivo_estrategico').select2('destroy');
    $('#id_dimensao').select2('destroy');
    $('#id_indicador').select2('destroy');

    $('#centrosDepositos').select2({
        dropdownCssClass: "centros-depositos-sel",
        theme: "bootstrap-5",
        selectionCssClass: "select2--large", // For Select2 v4.1
        dropdownCssClass: "select2--large",
        templateResult: function (data, container) {
            if (data.element) {
                $(container).attr('data-style', $(data.element).attr("data-style"));
            }
            return data.text;
        }
    }).on('select2:selecting', function (e) {
        var $select = $(this);
        var pid = e.params.args.data.element.attributes[0].value;
        if (e.params.args.data.id == '') {
            e.preventDefault();
            var childIds = $.grep(e.target.options, function (option) {
                return $(option).data('pid') == pid;
            });
            childIds = $.map(childIds, function (option) {
                return option.value;
            });
            childIds = $select.select2('val').concat(childIds);
            $select.val(childIds);
            $select.trigger('change');
            $select.select2('close');
        }
    });

    $('#rolesUsers').select2({
        dropdownCssClass: "roles-users-sel",
        theme: "bootstrap-5",
        selectionCssClass: "select2--large", // For Select2 v4.1
        dropdownCssClass: "select2--large",
        templateResult: function (data, container) {
            if (data.element) {
                $(container).attr('data-style', $(data.element).attr("data-style"));
            }
            return data.text;
        }
    }).on('select2:selecting', function (e) {
        var $select = $(this);
        var pid = e.params.args.data.element.attributes[0].value;
        if (e.params.args.data.id == '') {
            e.preventDefault();
            var childIds = $.grep(e.target.options, function (option) {
                return $(option).data('pid') == pid;
            });
            childIds = $.map(childIds, function (option) {
                return option.value;
            });
            childIds = $select.select2('val').concat(childIds);
            $select.val(childIds);
            $select.trigger('change');
            $select.select2('close');
        }
    })
});