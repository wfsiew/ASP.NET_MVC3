var nav_list = (function() {
	var config = {
		list_url : '',
		list_func : null
	};

	function show_list() {
		$("#id_display").data('pgnum', 1);
		update_list();
	}

	function update_list() {
		var d = $("#id_display").val();
		var pgnum = $("#id_display").data('pgnum');
		var s = get_search_query();
		$("#right_box").load(
				config.list_url + "?pgnum=" + pgnum + "&pgsize=" + d + s, null,
				function() {
					init_navigate();
				});
	}

	function go_prev() {
		var val = $("#id_pg").val();
		var arr = val.split(',');
		var d = $("#id_display").val();
		$("#id_display").data('pgnum', arr[2]);
		update_list();
	}

	function go_next() {
		var val = $("#id_pg").val();
		var arr = val.split(',');
		var d = $("#id_display").val();
		$("#id_display").data('pgnum', arr[3]);
		update_list();
	}

	function query_keypress(evt) {
		if (evt.keyCode == '13') {
			evt.preventDefault();
			evt.stopPropagation();
			show_list();
		}
	}

	function query_keyup() {
		utils.countdown_filter(show_list);
	}

	function set_item_msg(arg) {
		$(".item_display").text(arg);
	}

	function init_navigate() {
		$("#id_display").data('pgnum', 1);
		if ($.isFunction(config.list_func))
			config.list_func();

		var val = $("#id_pg").val();
		var arr = val.split(',');
		if (arr[0] == '0') {
			set_disabled("#id_prev", 1, null);
		}

		else {
			set_disabled("#id_prev", 0, go_prev);
		}

		if (arr[1] == '0') {
			set_disabled("#id_next", 1, null);
		}

		else {
			set_disabled("#id_next", 0, go_next);
		}

		set_item_msg(arr[4]);
		utils.bind_hover($(".list_button"));
	}

	function set_disabled(id, arg, handler) {
		var o = $(id);
		o.unbind("click");
		o.unbind("mouseenter");
		o.unbind("mouseleave");
		if (arg == 1) {
			o.attr("disabled", "disabled");
			o.removeClass("hover");
			o.addClass("ui-state-disabled");
		}

		else {
			o.removeAttr("disabled");
			o.removeClass("ui-state-disabled");
			o.addClass("hover");
			o.click(handler);
			utils.bind_hover(o);
		}
	}

	function get_search_query() {
		var id_selection = $("#id_selection");
		var search_by_qry = "&find=" + id_selection.val();
		var keyword = $("#id_query").val();
		var s = (search_by_qry == '&find=0' && keyword == "" ? ""
				: search_by_qry + "&keyword=" + encodeURIComponent(keyword));
		return s;
	}

	function init() {
		init_navigate();
	}

	return {
		init : init,
		config : config,
		show_list : show_list,
		update_list : update_list,
		query_keypress : query_keypress,
		query_keyup : query_keyup,
		set_item_msg : set_item_msg
	};
}());