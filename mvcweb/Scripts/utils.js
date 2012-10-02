var utils = (function() {
	var typing_timer;
	var done_typing_interval = 2000;

	function init_alert_dialog(id) {
		$(id).dialog({
			autoOpen : false,
			modal : true,
			buttons : {
				OK : function() {
					$(this).dialog("close");
				}
			}
		});
	}

	function init_server_error_dialog() {
		$(document).ajaxError(function(evt, jqXHR, ajaxOptions, errorThrown) {
			show_error_dialog(jqXHR.responseText);
		});
		$("#error-dialog").dialog({
			autoOpen : false,
			modal : true,
			width : 700,
			height : 500,
			buttons : {
				OK : function() {
					$(this).dialog("close");
				}
			}
		});
	}

	function init_confirm_delete(id) {
		$(id).dialog({
			autoOpen : false,
			modal : true
		});
	}

	function item_exist(item, arr) {
		var i = $.inArray(item, arr);
		return (i >= 0 ? true : false);
	}

	function remove_dialog(id) {
		$(id).dialog('destroy');
		$(id).remove();
	}

	function clear_dialogs() {
		remove_dialog("div[id^='dialog-message']");
		remove_dialog("div[id^='list-panel']");
		remove_dialog("div[id^='confirm-delete']");
		remove_dialog("div[id^='dialog']");
	}

	function cancel_form() {
		$("#left_box").hide();
	}

	function cancel_dialog(level, dialog_id) {
		var i = level - 1;
		var prefix = (i < 1 ? '' : '_' + i);
		$(dialog_id + prefix).dialog('close');
	}

	function show_dialog(arg, msg) {
		if (arg == 1)
			$("#dialog_msg").html(msg);

		else
			$("#dialog_msg").text(msg);

		$("#dialog-message").dialog('open');
	}

	function show_error_dialog(msg) {
		$("#error_dialog").html(msg);
		$("#error_dialog style").remove();
		$("#error-dialog").dialog('open');
	}

	function bind_hover(selector) {
		selector.hover(function() {
			$(this).toggleClass('ui-state-hover');
		}, function() {
			$(this).toggleClass('ui-state-hover');
		});
	}

	function get_form_level(id) {
		var i = id.indexOf('_');
		if (i < 0)
			return 0;

		var s = id.substr(i + 1);
		return parseInt(s, 10);
	}

	function get_form_scope(id) {
		var i = id.lastIndexOf('-');
		var j = id.lastIndexOf('_');
		if (i < 5)
			return '';

		else
			return id.substring(i + 1, j);
	}

	function get_prev_form_level(id) {
		var i = get_form_level(id);
		return (i > 0 ? i - 1 : 0);
	}

	function get_next_form_level(id) {
		var i = get_form_level(id);
		return i + 1;
	}

	function get_prefix(id) {
		var p = get_form_level(id);
		return (p == 0 ? '' : '_' + p);
	}

	function set_data(selector, key, arr) {
		selector.removeData(key);
		selector.data(key, arr);
	}

	function countdown_filter(func) {
		stop_filter_timer();
		typing_timer = setTimeout(func, done_typing_interval);
	}

	function stop_filter_timer() {
		clearTimeout(typing_timer);
	}

	function add_drag_css(evt, ui) {
		$(".ui-draggable-dragging").addClass("ui-state-hover");
	}

	function remove_drag_css(evt, ui) {
		$(".ui-draggable-dragging").removeClass("ui-state-hover");
	}

	function get_itemid(arg) {
		var i = arg.indexOf('_');
		if (i >= 0) {
			var s = arg.substr(i + 1);
			return parseInt(s, 10);
		}

		return -1;
	}

	function get_errors(errors) {
		var data = {
			errors : errors
		};
		var h = new EJS({
			url : '/springmvc/resources/tpl/form_error.html',
			ext : '.html'
		}).render(data);
		return h;
	}

	function get_menu_attr() {
		var menu = $("#menu");
		var menu_width = menu.width();
		menu_width = parseInt(menu_width) + 5;
		var menu_offset = menu.offset();
		var menu_left = menu_offset.left;
		var menu_top = menu_offset.top;
		menu_left -= 5;

		return {
			width : menu_width,
			left : menu_left,
			top : menu_top
		};
	}

	return {
		init_alert_dialog : init_alert_dialog,
		init_server_error_dialog : init_server_error_dialog,
		init_confirm_delete : init_confirm_delete,
		item_exist : item_exist,
		remove_dialog : remove_dialog,
		clear_dialogs : clear_dialogs,
		cancel_form : cancel_form,
		cancel_dialog : cancel_dialog,
		show_dialog : show_dialog,
		show_error_dialog : show_error_dialog,
		bind_hover : bind_hover,
		get_form_level : get_form_level,
		get_form_scope : get_form_scope,
		get_next_form_level : get_next_form_level,
		get_prefix : get_prefix,
		set_data : set_data,
		typing_timer : typing_timer,
		done_typing_interval : done_typing_interval,
		countdown_filter : countdown_filter,
		stop_filter_timer : stop_filter_timer,
		add_drag_css : add_drag_css,
		remove_drag_css : remove_drag_css,
		get_itemid : get_itemid,
		get_errors : get_errors
	};
}());

$.ajaxSetup({
	cache : false
});