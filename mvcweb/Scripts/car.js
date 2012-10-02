var car = (function () {
    var add_url = "/ajax/cars/add";
    var edit_url = "/ajax/cars/edit/";
    var delete_url = "/ajax/cars/delete/";
    var list_url = "/ajax/cars/list";

    function show_form() {
        $("#left_box").load(
				add_url,
				null,
				function () {
				    $("#left_box").css('width', '300px');
				    $(".save_button.save").click(func_save);
				    $(".save_button.cancel,.form_title div.close").click(
							utils.cancel_form);
				    utils.bind_hover($(".save_button,.form_title div.close"));
				    $("#left_box").show();
				});

        return false;
    }

    function save_success() {
        utils.cancel_form();
        nav_list.show_list();
    }

    function edit_helper(o) {
        var arg = $(o.self).parent().attr("id");
        $("#left_box").load(
				edit_url + arg,
				null,
				function () {
				    $("#left_box").css('width', '300px');
				    $(".save_button.save").click(function () {
				        return func_update(arg);
				    });
				    $(".save_button.cancel,.form_title div.close").click(
							utils.cancel_form);
				    utils.bind_hover($(".save_button,.form_title div.close"));
				    $("#left_box").show();
				});
    }

    function func_save() {
        var data = get_data();
        $('input').next().remove();
        $.post(add_url, data, function (result) {
            if (result.success == 1)
                save_success();

            else if (result.error == 1) {
                for (var e in result.errors) {
                    var d = $('#error_' + e).get(0);
                    if (!d) {
                        var o = {
                            field: e,
                            msg: result.errors[e]
                        };
                        var h = new EJS({
                            url: '/Content/tpl/label_error.html',
                            ext: '.html'
                        }).render(o);
                        $("input[name='" + e + "']").after(h);
                    }
                }
            }

            else
                utils.show_dialog(2, result);
        });
        return false;
    }

    function func_edit() {
        var self = this;
        edit_helper({
            self: self
        });
    }

    function func_update(arg) {
        var data = get_data();
        $('input').next().remove();
        $.post(edit_url + arg, data, function (result) {
            if (result.success == 1)
                save_success();

            else if (result.error == 1) {
                for (var e in result.errors) {
                    var d = $('#error_' + e).get(0);
                    if (!d) {
                        var o = {
                            field: e,
                            msg: result.errors[e]
                        };
                        var h = new EJS({
                            url: '/Content/tpl/label_error.html',
                            ext: '.html'
                        }).render(o);
                        $("input[name='" + e + "']").after(h);
                    }
                }
            }

            else
                utils.show_dialog(2, result);
        });
        return false;
    }

    function func_delete() {
        var item = $(this).parent();
        var arg = item.attr("id");
        var val = $("#id_pg").val();
        var arr = val.split(',');
        var currpg = parseInt(arr[3], 10);
        --currpg;
        var pgsize = $("#id_display").val();
        var search_by = $("#id_selection").val();
        var keyword = $("#id_query").val();
        var data = {
            id: arg,
            pgnum: currpg,
            pgsize: pgsize,
            find: search_by,
            keyword: keyword
        };

        $.post(delete_url, data, function (result) {
            if (result.success == 1) {
                nav_list.set_item_msg(result.itemscount);
                var tr = item.parent();
                tr.remove();
                delete tr;
            }
        });
    }

    function get_data() {
        var data = {
            make: $("#id_make").val(),
            model: $("#id_model").val(),
            year: $("#id_year").val(),
            doors: $("#id_doors").val(),
            colour: $("#id_colour").val(),
            price: $("#id_price").val()
        };
        return data;
    }

    function init_list() {
        $(".list_button.edit").click(func_edit);
        $(".list_button.delete").click(func_delete);
    }

    function init() {
        utils.init_server_error_dialog();
        $("#left_box").hide();
        $("#id_add").click(show_form);
        $("#id_find").click(nav_list.show_list);
        $("#id_display,#id_selection").change(nav_list.show_list);
        $("#id_query").keypress(nav_list.query_keypress);
        $("#id_query").keyup(nav_list.query_keyup);
        utils.init_alert_dialog("#dialog-message");
        utils.bind_hover($("#id_add,#id_find"));
        nav_list.config.list_url = list_url;
        nav_list.config.list_func = init_list;
        nav_list.init();
    }

    return {
        init: init
    };
} ());

$(document).ready(car.init);