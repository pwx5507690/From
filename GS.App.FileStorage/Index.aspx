﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="GS.App.FileStorage.Index" %>

<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="content-type" content="text/html; charset=utf-8" />
	<title>文件管理</title>
	<link rel="stylesheet" type="text/css" href="styles/reset.css" />
	<link rel="stylesheet" type="text/css" href="scripts/jquery.filetree/jqueryFileTree.css" />
	<link rel="stylesheet" type="text/css" href="scripts/jquery.contextmenu/jquery.contextMenu-1.01.css" />
	<link rel="stylesheet" type="text/css" href="scripts/custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css" />

	<link href="Assets/css/amazeui.min.css" rel="stylesheet" />
	<link href="Assets/css/admin.css" rel="stylesheet" />
	<link href="Assets/css/app.css" rel="stylesheet" />
	<style type="text/css">
		#loading-wrap {
			position: fixed;
			height: 100%;
			width: 100%;
			overflow: hidden;
			top: 0;
			left: 0;
			display: block;
			background: white url(./images/wait30trans.gif) no-repeat center center;
			z-index: 999;
		}
	</style>
	<!-- CSS dynamically added using 'config.options.theme' defined in config file -->
</head>
<body style="background-color: #ffffff;">
	<div id="loading-wrap">
		<!-- loading wrapper / removed when loaded -->
	</div>
	<div>
		<form id="uploader" method="post">
			<%if (string.IsNullOrEmpty(SelectType))
				{ %>
			<h1 style="margin-top: 10px"></h1>
			<div id="uploadresponse"></div>
			<%}
				else {
					%>
			<div style="display:none" id="uploadresponseType"></div>
			    <%
				} %>
			<div class="am-form-group" style='float: right'>
				<div class="am-btn-toolbar">
					<%if (string.IsNullOrEmpty(SelectType))
						{ %>
					<div class="am-btn-group am-form-group-sm">
						<input type="text" style="height: 28px;" value="" placeholder="请输入名称名称" class="am-form-field" name="q" id="q" />
					</div>
					<div class="am-btn-group am-btn-group-xs">
						<button id="level-up" name="level-up" type="button" value="LevelUp" class="am-btn am-btn-secondary"><span class="am-icon-mail-reply"></span></button>
					</div>
					<div class="am-btn-group am-btn-group-xs">
						<button id="home" name="home" type="button" class="am-btn am-btn-secondary" value="Home"><span class="am-icon-home"></span></button>
					</div>
					<%} %>
					<%if (!string.IsNullOrEmpty(SelectType))
						{ %>
					<div class="am-btn-group am-btn-group-xs">
						<button id="selected" name="selected" type="button"
							class="am-btn am-btn-success" value="确定">
							确定选择
						</button>

					</div>
					<%} %>
					<div class="am-btn-group am-btn-group-xs">
						<input id="mode" name="mode" type="hidden" value="add" />
						<input id="currentpath" name="currentpath" type="hidden" />
						<div id="file-input-container">
							<div id="alt-fileinput">
								<input id="filepath" name="filepath" type="text" />
								<button id="browse" name="browse" type="button" value="Browse"></button>
							</div>
							<input id="newfile" name="newfile" type="file" />
						</div>
						<button id="upload" name="upload" type="submit" value="Upload" class="am-btn am-btn-default am-btn-success">
							<span></span>
						</button>

						<button id="newfolder" name="newfolder" type="button" value="New Folder" class="am-btn am-btn-default am-btn-success">
							<i class="am-icon-folder-open"></i>
						</button>


					</div>
					<%--<div class="am-btn-group am-btn-group-xs">
						<button id="grid" class="am-btn am-btn-secondary" type="button">
							<i class="am-icon-th"></i> 
						</button>
						<button id="list" class="am-btn am-btn-secondary" type="button">
							<i class="am-icon-list"></i> 
						</button>
					</div>--%>
				</div>
			</div>
			<hr data-am-widget="divider" style="" class="am-divider am-divider-default" />
		</form>

		<div id="splitter">
			<%if (string.IsNullOrEmpty(SelectType))
				{ %>
			<div id="filetree"></div>
			<%} %>
			<div id="fileinfo">
				<h1></h1>
			</div>
		</div>

		<div id="footer">
			<a href="" id="link-to-project"></a>
			<div id="folder-info"></div>
		</div>

		<ul id="itemOptions" class="contextMenu">
			<li class="select"><a href="#select"></a></li>
			<li class="download"><a href="#download"></a></li>
			<li class="rename"><a href="#rename"></a></li>
			<li class="move"><a href="#move"></a></li>
			<li class="replace"><a href="#replace"></a></li>
			<li class="delete separator"><a href="#delete"></a></li>
		</ul>
		<script>
			var selectedType = "<%=SelectType%>";

		</script>
		<script type="text/javascript" src="scripts/jquery-1.11.3.min.js"></script>
		<script type="text/javascript" src="scripts/jquery-browser.js"></script>
		<script type="text/javascript" src="scripts/jquery.form-3.24.js"></script>
		<script type="text/javascript" src="scripts/jquery.splitter/jquery.splitter-1.5.1.js"></script>
		<script type="text/javascript" src="scripts/jquery.filetree/jqueryFileTree.js"></script>
		<script type="text/javascript" src="scripts/jquery.contextmenu/jquery.contextMenu-1.01.js"></script>
		<script type="text/javascript" src="scripts/jquery.impromptu-3.2.min.js"></script>
		<script type="text/javascript" src="scripts/jquery.tablesorter-2.7.2.min.js"></script>
		<script type="text/javascript" src="scripts/filemanager.js"></script>
		<script type="text/javascript" src="Assets/js/amazeui.min.js"></script>

	</div>
	<div class="am-modal am-modal-no-btn" tabindex="-5" id="modal">
		<div class="am-modal-dialog">
			<div class="am-modal-hd">
				<h4 id="modalTitle"></h4>
				<a href="javascript: void(0)" class="am-close am-close-spin" data-am-modal-close>&times;</a>
			</div>
			<div class="am-modal-bd">
				<div id="modalContent">
				</div>
			</div>
		</div>
	</div>
</body>
<script>
	$(function () {
		if (selectedType) { $("#newfolder,.vsplitbar").hide(); }
	});
</script>
</html>
