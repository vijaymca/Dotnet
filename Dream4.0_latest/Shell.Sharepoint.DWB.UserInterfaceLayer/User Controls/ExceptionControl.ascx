<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ExceptionControl.ascx.cs" Inherits="Shell.SharePoint.DWB.UserInterfaceLayer.ExceptionControl" %>

<link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />  


<div style="width:100%;height:100%;border:double 1px #CCCCCC;
	padding:5px;
	BACKGROUND: url("/images/left_navigation_background.jpg") repeat-y;">	
	<table class="tableAdvSrchBorder" cellspacing="0" cellpadding="4" width="100%">
    <tr>
    <td colspan="4" style="font-family:Verdana;
	font-size:11px;
	background-color:#E0ECF0;
	font-weight:bold;
	vertical-align:middle;
	background-image: url(/_layouts/DREAM/images/breadcrumb_bg.gif);
	height:21px">
            <b>Exception</b>
    </td>    
    </tr>
    <tr><td>
        <span id="spnException" style="font-family:Verdana;
	font-size:11px;
	color:red;
	text-align:left;" runat ="server" ></span>
	</td>
    </tr>
    </table>
</div>