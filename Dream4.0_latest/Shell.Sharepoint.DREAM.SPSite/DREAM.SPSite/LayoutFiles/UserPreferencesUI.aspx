<!--
//All rights are reserved. Reproduction or transmission in whole or in part, 
//in any form or by any means, electronic, mechanical or otherwise, 
//is prohibited without the prior consent of the copyright owner
//
//Filename: UserPreferencesUI.aspx
//Project : DREAM Portal Integration
//Version : 0.5
//Owner   : Wipro Technologies
-->

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPreferencesUI.aspx.cs" Inherits="Shell.SharePoint.DREAM.Site.UI.UserPreferencesUI" %>

<%@ Register Src="~/_CONTROLTEMPLATES/DREAM/UserPreferencesUI.ascx" TagName="UserPreferencesUI" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>User Preferences</title>
    <script type="text/javascript" src="/_Layouts/DREAM/Javascript/DREAMJavaScriptFunctionsRel2_1.js"></script>
    <link href="/_LAYOUTS/DREAM/styles/DREAMStyleSheetRel2_1.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:UserPreferencesUI ID="UserPreferencesUI1" runat="server" />
    </div>
    </form>
</body>
</html>
