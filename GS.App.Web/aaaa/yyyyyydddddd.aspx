<!--include=true-->
<!--浏览地址http://localhost/aaaa/aaaa.aspx-->
<!--图片资源文件访问/aaaa/image/-->
<!--icon 站点图标/aaaa/icon.png-->
<!--jQuery 引用地址<script src="/Scripts/jquery-1.10.2.js"></script>-->
<!--所有文件引用请用绝对路径-->
<!--修改时间2017-12-18 01:10-->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="GS.App.Web.WebForm1" %>
      <run data-load="deptoption" />
<div class="am-u-sm-12 am-u-md-12 am-u-lg-12">
	<div class="widget am-cf">
		<div class="widget-head am-cf">
			<div class="widget-title am-fl">部门</div>
			<div class="widget-function am-fr">
				<a href="javascript:;" class="am-icon-cog"></a>
			</div>
		</div>
		<div class="widget-body am-fr">
			<form data-form class="am-form tpl-form-line-form" onsubmit="return app.valid()">
				<div class="am-form-group">
					<label for="user-phone" class="am-u-sm-3 am-form-label">上级部门 <span class="tpl-form-line-small-title">Parent Department</span></label>
					<div class="am-u-sm-9">
						<select id="parent" style="color:#000000">
							
						</select>
					</div>
				</div>
				<div class="am-form-group">
					<label for="user-name" class="am-u-sm-3 am-form-label">部门编号 <span class="tpl-form-line-small-title">Department Code</span></label>
					<div class="am-u-sm-9">
						<input type="text" class="tpl-form-input" name="code" id="code" required oninvalid="setCustomValidity('请输入部门编号！');" oninput="setCustomValidity('');" placeholder="请输入部门编号">
						<small>请输入部门编号。</small>
					</div>
				</div>
				<div class="am-form-group">
					<label for="user-name" class="am-u-sm-3 am-form-label">部门名称 <span class="tpl-form-line-small-title">Department</span></label>
					<div class="am-u-sm-9">
						<input type="text" class="tpl-form-input" name="name" id="name" required oninvalid="setCustomValidity('请输入部门名称！');" oninput="setCustomValidity('');" placeholder="请输入部门名称">
						<small>请输入部门名称。</small>
					</div>
				</div>
				<input type="hidden" name="id" id="id">
				<div class="am-form-group">
					<div class="am-u-sm-9 am-u-sm-push-3">
						<button type="submit"  class="am-btn am-btn-primary tpl-btn-bg-color-success ">提交</button>
					</div>
				</div>
			</form>
		</div>
	</div>
</div>
